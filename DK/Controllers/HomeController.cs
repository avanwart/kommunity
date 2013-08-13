﻿//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.AppSpec.DasKlub.BOL.DomainConnection;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Configs;
using BootBaronLib.Operational;
using BootBaronLib.Values;
using DasKlub.Web.Models;
using DasKlub.Web.Models.Models;
using Google.GData.Client;
using Google.YouTube;
using HttpUtility = System.Web.HttpUtility;
using Utilities = BootBaronLib.Operational.Utilities;
using Video = BootBaronLib.AppSpec.DasKlub.BOL.Video;

namespace DasKlub.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IForumCategoryRepository _forumcategoryRepository;
        private readonly MembershipUser _mu;


        public HomeController()
            : this(new ForumCategoryRepository())
        {
            _mu = Membership.GetUser();
        }

        private HomeController(IForumCategoryRepository forumcategoryRepository)
        {
            _mu = Membership.GetUser();
            _forumcategoryRepository = forumcategoryRepository;
        }

        #region Actions

        [HttpGet]
        public ActionResult SiteContent(string brandType)
        {
            var siteBrand =
                (SiteEnums.SiteBrandType) Enum.Parse(typeof (SiteEnums.SiteBrandType), brandType);

            ViewBag.ContentMessage = SiteDomain.GetSiteDomainValue(siteBrand, Utilities.GetCurrentLanguageCode());


            return View();
        }

        [HttpPost]
        public ActionResult VideoSubmit(string video, string videoType, string personType,
                                        string footageType, string band, string song, string contestID)
        {
            var devkey = GeneralConfigs.YouTubeDevKey;

            if (string.IsNullOrWhiteSpace(video))
            {
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();
            }
            var vir = new VideoRequest {RequestURL = video};

            string vidKey = string.Empty;

            vir.RequestURL = vir.RequestURL.Replace("https", "http");

            if (vir.RequestURL.Contains("http://youtu.be/"))
            {
                vir.VideoKey = vir.RequestURL.Replace("http://youtu.be/", string.Empty);
            }
            else if (vir.RequestURL.Contains("http://www.youtube.com/watch?"))
            {
                var nvcKey = HttpUtility.ParseQueryString(vir.RequestURL.Replace("http://www.youtube.com/watch?", string.Empty));

                vidKey = nvcKey["v"].Replace("#", string.Empty).Replace("!", string.Empty);
                vir.VideoKey = vidKey;
            }
            else
            {
                // invalid 
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();
            }

            if (string.IsNullOrWhiteSpace(videoType) ||
                string.IsNullOrWhiteSpace(personType) ||
                string.IsNullOrWhiteSpace(footageType) ||
                string.IsNullOrWhiteSpace(band) ||
                string.IsNullOrWhiteSpace(song))
            {
                // invalid 
                vir.StatusType = 'P';
                Response.Redirect("~/videosubmission.aspx?statustype=P");
                return new EmptyResult();
            }

            var vid = new Video("YT", vidKey) {ProviderCode = "YT"};


            try
            {
                var yousettings = new YouTubeRequestSettings("Das Klub", devkey);

                var yourequest = new YouTubeRequest(yousettings);
                var entryUri = new Uri(string.Format("http://gdata.youtube.com/feeds/api/videos/{0}", vidKey));

                var video2 = yourequest.Retrieve<Google.YouTube.Video>(entryUri);
                vid.Duration = (float) Convert.ToDouble(video2.YouTubeEntry.Duration.Seconds);
                vid.ProviderUserKey = video2.Uploader;
                vid.PublishDate = video2.YouTubeEntry.Published;
            }
            catch (GDataRequestException gdx)
            {
                vid.IsEnabled = false;
                vid.Update();

                // invalid 
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                Utilities.LogError("invalid link", gdx);
                return new EmptyResult();
            }
            catch (ClientFeedException cfe)
            {
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                Utilities.LogError("invalid link", cfe);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                Utilities.LogError("invalid link", ex);
                return new EmptyResult();
            }


            vid.VolumeLevel = 5;

            if (string.IsNullOrWhiteSpace(vid.ProviderKey))
            {
                // invalid 
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();
            }

            if (vid.VideoID == 0)
            {
                vid.IsHidden = false;
                vid.IsEnabled = true;
                vid.Create();
            }
            else
            {
                vid.Update();
            }


            // if there is a contest, add it now since there is an id
            int subContestID;
            if (!string.IsNullOrWhiteSpace(contestID) && int.TryParse(contestID, out subContestID) &&
                subContestID > 0)
            {
                //TODO: check if it already is in the contest

                ContestVideo.DeleteVideoFromAllContests(vid.VideoID);

                var cv = new ContestVideo {ContestID = subContestID, VideoID = vid.VideoID};

                cv.Create();
            }
            else
            {
                // TODO: JUST REMOVE FROM CURRENT CONTEST, NOT ALL
                ContestVideo.DeleteVideoFromAllContests(vid.VideoID);
            }

            // vid type

            var propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            var mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
            mp.RemoveCache();
            MultiPropertyVideo.AddMultiPropertyVideo(Convert.ToInt32(videoType), vid.VideoID);

            // human

            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
            mp.RemoveCache();
            MultiPropertyVideo.AddMultiPropertyVideo(Convert.ToInt32(personType), vid.VideoID);


            // footage

            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
            mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
            mp.RemoveCache();
            MultiPropertyVideo.AddMultiPropertyVideo(Convert.ToInt32(footageType), vid.VideoID);


            // song 1

            var artst = new Artist(band.Trim());

            if (artst.ArtistID == 0)
            {
                artst.GetArtistByAltname(band.Trim());
            }

            if (artst.ArtistID == 0)
            {
                artst.Name = band.Trim();
                artst.AltName = FromString.URLKey(artst.Name);
                artst.Create();
            }


            var sng = new Song(artst.ArtistID, song.Trim());


            if (sng.SongID == 0)
            {
                sng.Name = sng.Name.Trim();
                sng.SongKey = FromString.URLKey(sng.Name);
                sng.Create();
            }

            VideoSong.AddVideoSong(sng.SongID, vid.VideoID, 1);

            if (vid.VideoID > 0)
            {
                Response.Redirect(vid.VideoURL); // just send them to it
            }


            return new EmptyResult();
        }

        public ActionResult Index()
        {

            var newestVideos = new Videos();
            newestVideos.GetMostRecentVideos();

            if (newestVideos.Count > 0)
            {
                ViewBag.NewestVideo = newestVideos[0].ProviderKey;
            }

            UserAccount ua = null;

            if (_mu != null)
            {
                ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));
            }

            using (var context = new DasKlubDBContext())
            {
                var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
                var mostPopularThisWeek =
                    context.ForumPost
                           .Where(x => x.CreateDate > oneWeekAgo)
                           .GroupBy(x => x.ForumSubCategoryID)
                           .OrderByDescending(y => y.Count())
                           .ToList().FirstOrDefault();

                if (mostPopularThisWeek != null)
                {
                    var topForumThreadOfTheWeek =
                        context.ForumSubCategory.FirstOrDefault(x => x.ForumSubCategoryID == mostPopularThisWeek.Key);
                    var topFeedItem = new ForumFeedModel {ForumSubCategory = topForumThreadOfTheWeek};
                    topFeedItem.ForumCategory =
                        context.ForumCategory.FirstOrDefault(
                            x => x.ForumCategoryID == topFeedItem.ForumSubCategory.ForumCategoryID);
                    var mostRecentPostToTopThread = context.ForumPost.OrderByDescending(x => x.CreateDate)
                                                           .FirstOrDefault(
                                                               x =>
                                                               x.ForumSubCategoryID ==
                                                               topFeedItem.ForumSubCategory.ForumSubCategoryID);
                    if (mostRecentPostToTopThread != null)
                        topFeedItem.LastPosted = mostRecentPostToTopThread.CreateDate;

                    if (ua != null)
                    {
                        var isNew =
                            context.ForumPostNotification.FirstOrDefault(
                                x =>
                                x.ForumSubCategoryID == topForumThreadOfTheWeek.ForumSubCategoryID &&
                                x.UserAccountID == ua.UserAccountID);

                        if (isNew != null && !isNew.IsRead)
                        {
                            topFeedItem.IsNewPost = true;

                            var forumSubPostCount =
                                context.ForumPost.Count(
                                    x => x.ForumSubCategoryID == topFeedItem.ForumSubCategory.ForumSubCategoryID);

                            var pageCount = (forumSubPostCount + ForumController.PageSize - 1)/ForumController.PageSize;

                            if (mostRecentPostToTopThread != null)
                                topFeedItem.URLTo =
                                    new Uri(topFeedItem.ForumSubCategory.SubForumURL + "/" +
                                            ((pageCount > 1)
                                                 ? pageCount.ToString(CultureInfo.InvariantCulture)
                                                 : string.Empty) + "#" +
                                            mostRecentPostToTopThread.ForumPostID.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    ViewBag.TopThreadOfTheWeek = topFeedItem;
                }


                var newestThreads = context.ForumSubCategory
                                           .OrderByDescending(x => x.CreateDate)
                                           .Where(x => x.ForumSubCategoryID != mostPopularThisWeek.Key)
                                           .Take(20);
                var subItems = new Dictionary<int, DateTime>();
                var forumFeed = new List<ForumFeedModel>();

                foreach (var post in newestThreads)
                {
                    if (!subItems.ContainsKey(post.ForumSubCategoryID))
                    {
                        subItems.Add(post.ForumSubCategoryID, post.CreateDate);
                    }
                }

                var newestPosts = context.ForumPost.GroupBy(x => x.ForumSubCategoryID)
                                         .Select(y => y.OrderByDescending(x => x.CreateDate).FirstOrDefault())
                                         .Where(x => x.ForumSubCategoryID != mostPopularThisWeek.Key)
                                         .OrderByDescending(i => i.CreateDate)
                                         .Take(10);

                foreach (var post in newestPosts)
                {
                    if (!subItems.ContainsKey(post.ForumSubCategoryID))
                    {
                        subItems.Add(post.ForumSubCategoryID, post.CreateDate);
                    }
                    else if (subItems[post.ForumSubCategoryID] < post.CreateDate)
                    {
                        subItems[post.ForumSubCategoryID] = post.CreateDate;
                    }
                }

                foreach (var forumFeedItem in subItems.Select(post => new ForumFeedModel
                    {
                        ForumSubCategory =
                            context.ForumSubCategory.FirstOrDefault(x => x.ForumSubCategoryID == post.Key),
                        LastPosted = post.Value
                    }))
                {
                    forumFeedItem.ForumCategory =
                        context.ForumCategory.FirstOrDefault(
                            x => x.ForumCategoryID == forumFeedItem.ForumSubCategory.ForumCategoryID);

                    if (ua != null)
                    {
                        var isNew =
                            context.ForumPostNotification.FirstOrDefault(
                                x =>
                                x.ForumSubCategoryID == forumFeedItem.ForumSubCategory.ForumSubCategoryID &&
                                x.UserAccountID == ua.UserAccountID);

                        if (isNew != null && !isNew.IsRead)
                        {
                            forumFeedItem.IsNewPost = true;

                            var forumSubPostCount =
                                context.ForumPost.Count(
                                    x => x.ForumSubCategoryID == forumFeedItem.ForumSubCategory.ForumSubCategoryID);

                            var pageCount = (forumSubPostCount + ForumController.PageSize - 1)/ForumController.PageSize;

                            var mostRecentPostToTopThread = context.ForumPost.OrderByDescending(x => x.CreateDate)
                                                                   .FirstOrDefault(
                                                                       x =>
                                                                       x.ForumSubCategoryID ==
                                                                       forumFeedItem.ForumSubCategory.ForumSubCategoryID);

                            if (mostRecentPostToTopThread != null)
                                forumFeedItem.URLTo =
                                    new Uri(forumFeedItem.ForumSubCategory.SubForumURL + "/" +
                                            ((pageCount > 1)
                                                 ? pageCount.ToString(CultureInfo.InvariantCulture)
                                                 : string.Empty) + "#" +
                                            mostRecentPostToTopThread.ForumPostID.ToString(CultureInfo.InvariantCulture));
                        }
                    }

                    forumFeed.Add(forumFeedItem);
                }

                forumFeed = forumFeed.OrderByDescending(x => x.LastPosted).Take(9).ToList();
                ViewBag.ForumFeed = forumFeed;
                ViewBag.MostRecentThreads = newestThreads;

                var last30Days = DateTime.UtcNow.AddDays(-30);
                var mostPostsInForum =
                    (from b in context.ForumPost
                    where b.CreateDate < DateTime.UtcNow && b.CreateDate > last30Days
                    group b by b.CreatedByUserID
                    into grp
                    orderby grp.Count() descending 
                    select  grp.Key).Take(7).ToList();

                var topForumUsers = new UserAccounts();

                topForumUsers.AddRange(mostPostsInForum.Select(topForumUser => new UserAccount(topForumUser)));

                ViewBag.TopForumUsersOfTheMonth = topForumUsers;
            }

            var cnts = new Contents();
            cnts.GetContentPageWiseReleaseAll(1, 5);

            ViewBag.RecentArticles = cnts;

            var topUsers = new UserAccounts();
            topUsers.GetMostApplaudedLast30Days();

            ViewBag.TopUsersOfTheMonth = topUsers;

            return View();
        }


        public ActionResult Contact()
        {
            return View();
        }

        #endregion
    }
}