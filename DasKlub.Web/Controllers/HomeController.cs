﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.ArtistContent;
using DasKlub.Lib.BOL.DomainConnection;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Lib.BOL.VideoContest;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using DasKlub.Models;
using DasKlub.Models.Forum;
using DasKlub.Models.Models;
using DasKlub.Web.Models;
using Google.GData.Client;
using Google.YouTube;
using HttpUtility = System.Web.HttpUtility;
using Utilities = DasKlub.Lib.Operational.Utilities;
using Video = DasKlub.Lib.BOL.Video;
using System.Text;
using DasKlub.Lib.DAL;
using System.Data;

namespace DasKlub.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IForumCategoryRepository _forumcategoryRepository;
        private readonly MembershipUser _mu;
        private const int AmountOfNewThreadsOnHomepage = 15;
        private const int CountOfNewsItemsOnHomepage = 3;
        private const int AmountOfImagesToShowOnTheHomepage = 12;
        private const string provider = "YT";// YouTube
        private const int ForumThreadsToDisplay = 20;
        private char InvalidStatus = 'I';

        public HomeController()
            : this(new ForumCategoryRepository())
        {
           
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
        public ActionResult VideoSubmit(string video,
                                        string videoType, 
                                        string personType,
                                        string footageType,
                                        string band,
                                        string song,
                                        string contestID)
        {
            var invalidSubmissionLink = string.Concat("~/videosubmission.aspx?statustype=", InvalidStatus.ToString());

            if (string.IsNullOrWhiteSpace(video))
            {
                Response.Redirect(invalidSubmissionLink);
                return new EmptyResult();
            }
            var vir = new VideoRequest {RequestURL = video};

            string vidKey = string.Empty;
            vir.RequestURL = vir.RequestURL;
            vir.VideoKey = Utilities.ExtractYouTubeVideoKey(video);

            if(string.IsNullOrWhiteSpace(vir.VideoKey))
            {
                vir.StatusType = InvalidStatus;
                Response.Redirect(invalidSubmissionLink);
                return new EmptyResult();
            }

            if (string.IsNullOrWhiteSpace(videoType) ||
                string.IsNullOrWhiteSpace(personType) ||
                string.IsNullOrWhiteSpace(footageType) ||
                string.IsNullOrWhiteSpace(band) ||
                string.IsNullOrWhiteSpace(song))
            {
                vir.StatusType = 'P';
                Response.Redirect("~/videosubmission.aspx?statustype=P");
                return new EmptyResult();
            }

            var vid = new Video(provider, vidKey) {ProviderCode = provider};
            
            try
            {
                Google.YouTube.Video youTubeVideo = GetYouTubeVideo(vidKey);
                vid.Duration = (float)Convert.ToDouble(youTubeVideo.YouTubeEntry.Duration.Seconds);
                vid.ProviderUserKey = youTubeVideo.Uploader;
                vid.PublishDate = youTubeVideo.YouTubeEntry.Published;
            }
            catch (GDataRequestException)
            {
                vid.IsEnabled = false;
                vid.Update();
                vir.StatusType = InvalidStatus;
                Response.Redirect(invalidSubmissionLink);
                return new EmptyResult();
            }
            catch (ClientFeedException)
            {
                vir.StatusType = InvalidStatus;
                Response.Redirect(invalidSubmissionLink);
                return new EmptyResult();
            }
            catch (Exception)
            {
                vir.StatusType = InvalidStatus;
                Response.Redirect(invalidSubmissionLink);
                return new EmptyResult();
            }

            vid.VolumeLevel = 5;

            if (string.IsNullOrWhiteSpace(vid.ProviderKey))
            {
                // invalid 
                vir.StatusType = InvalidStatus;
                Response.Redirect(invalidSubmissionLink);
                return new EmptyResult();
            }

            if (vid.VideoID == 0)
            {
                vid.IsHidden = false;
                vid.IsEnabled = true;
                vid.Create();
            }
            else vid.Update();

            // if there is a contest, add it now since there is an id
            int subContestID;
            if (!string.IsNullOrWhiteSpace(contestID) && 
                int.TryParse(contestID, out subContestID) &&
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

        private Google.YouTube.Video GetYouTubeVideo(string vidKey)
        {
            var youTubeSettings = new YouTubeRequestSettings(GeneralConfigs.YouTubeDevApp, GeneralConfigs.YouTubeDevKey);
            var youTubeRequest = new YouTubeRequest(youTubeSettings);
            var entryUri = new Uri(string.Format("http://gdata.youtube.com/feeds/api/videos/{0}", vidKey));
            var video = youTubeRequest.Retrieve<Google.YouTube.Video>(entryUri);
            return video;
        }
 
        public ActionResult Index()
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            UserAccount ua = null;
            
            if (_mu != null)
            {
                ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));
            }

            using (var context = new DasKlubDbContext())
            {
                var mostPopularForumPosts =
                    context.ForumPost
                           .Where(x => x.CreateDate > oneWeekAgo)
                           .GroupBy(x => x.ForumSubCategoryID)
                           .OrderByDescending(y => y.Count())
                           .Take(1)
                           .ToList()
                           .FirstOrDefault();

                if (mostPopularForumPosts != null)
                {
                    const int amountForForm = 7;
                    var mostPopularThread = LoadMostPopularThisWeek(mostPopularForumPosts, context, ua);
                    ViewBag.TopThreadOfTheWeek = mostPopularThread;

                    List<ForumFeedModel> forumFeed;
                    ViewBag.MostRecentThreads = new object();
                    var threadResults = LoadRecentActiveThreads(mostPopularThread.ForumSubCategory.ForumSubCategoryID); 

                    forumFeed = new List<ForumFeedModel>();

                    foreach (int threadId in threadResults)
                    {
                        var feedItem = new ForumFeedModel();
                        
                        if (ua != null) 
                        {
                            var isNew = context.ForumPostNotification
                                            .FirstOrDefault(x => x.ForumSubCategoryID == threadId && x.UserAccountID == ua.UserAccountID);
                            feedItem.IsNewPost = (isNew == null) ? false : !isNew.IsRead;
                        } 
                        
                        var lastPost = context.ForumPost
                                              .Where(x => x.ForumSubCategoryID == threadId)
                                              .OrderByDescending(x => x.CreateDate).FirstOrDefault();
                        feedItem.ForumSubCategory = context.ForumSubCategory
                                                           .Where(x => x.ForumSubCategoryID == threadId)
                                                           .FirstOrDefault();
                        feedItem.ForumCategory = context.ForumCategory
                                                        .Where(x => x.ForumCategoryID == feedItem.ForumSubCategory.ForumCategoryID)
                                                        .FirstOrDefault();
                        feedItem.LastPosted = (lastPost == null) ? feedItem.ForumSubCategory.CreateDate : lastPost.CreateDate;
                        feedItem.PostCount = context.ForumPost.Where(x => x.ForumSubCategoryID == threadId).Count();
                        
                        var pageCount = (feedItem.PostCount + ForumController.PageSizeForumPost - 1) / ForumController.PageSizeForumPost;
                        feedItem.URLTo = (lastPost == null) ? 
                                            feedItem.ForumSubCategory.SubForumURL  :
                                            new Uri(string.Format("{0}/{1}#{2}", 
                                                        feedItem.ForumSubCategory.SubForumURL, 
                                                        ((pageCount > 1)
                                                            ? pageCount.ToString(CultureInfo.InvariantCulture)
                                                            : string.Empty),
                                                        lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture)));

                        feedItem.UserName = (lastPost == null) ? 
                            new UserAccount(feedItem.ForumSubCategory.CreatedByUserID).UserName : 
                            new UserAccount(lastPost.CreatedByUserID).UserName;

                        forumFeed.Add(feedItem);
                    }

                    ViewBag.ForumFeed = forumFeed;
 
                    var mostPostsInForum =
                        (from b in context.ForumPost
                         where b.CreateDate < DateTime.UtcNow && b.CreateDate > oneWeekAgo
                         group b by b.CreatedByUserID
                         into grp
                         orderby grp.Count() descending
                         select grp.Key).Take(amountForForm).ToList();

                    var topForumUsers = LoadTopForumUsers(mostPostsInForum);
                    
                    if (topForumUsers.Count == amountForForm)
                    {
                        ViewBag.TopForumUsersOfTheMonth = topForumUsers;
                    }
                }
            }

            ViewBag.RecentArticles = LoadRecentArticles();
            ViewBag.TopUsersOfTheMonth = LoadTopUsers();

            var recentPhotos = new PhotoItems { UseThumb = true, ShowTitle = false };
            recentPhotos.GetPhotoItemsPageWise(1, AmountOfImagesToShowOnTheHomepage);
            ViewBag.RecentPhotos = recentPhotos;
            
            return View();
        }

        private class ThreadDate 
        {
            public int ForumSubCategoryID   { get; set; }
            public DateTime CreateDate      { get; set; }
        }

        private List<int> LoadRecentActiveThreads(int excludeTopThreadId)
        {
            using (var context = new DasKlubDbContext())
            {
                var mostRecentThreads = 
                         (from      thread in context.ForumSubCategory
                          where     thread.ForumSubCategoryID != excludeTopThreadId
                          orderby   thread.CreateDate descending
                          select    thread).Select(x => new ThreadDate() 
                                                    { 
                                                          ForumSubCategoryID    = x.ForumSubCategoryID, 
                                                          CreateDate            = x.CreateDate
                                                    }).Take(ForumThreadsToDisplay)
                                                      .ToList();

                  var command = DbAct.CreateCommand();
                  command.CommandText = string.Format(@"
                    SELECT TOP {0} [ForumSubCategoryID], [CreateDate]
                    FROM
                    (
                        SELECT [ForumSubCategoryID]
                              ,[CreateDate]
                              ,ROW_NUMBER() OVER (PARTITION BY [ForumSubCategoryID] ORDER BY [CreateDate] DESC) AS Seq
                        FROM  [ForumPosts]
                        WHERE ForumSubCategoryID != {1}
                    ) PartitionedTable
                    WHERE PartitionedTable.Seq = 1
                    ORDER BY [CreateDate] DESC ", ForumThreadsToDisplay,  excludeTopThreadId);
                command.CommandType                 = CommandType.Text;
                var mostRecentPostsToThreads        = DbAct.ExecuteSelectCommand(command);
                var mostRecentPostsToThreadsList    = new List<ThreadDate>();
 
                foreach(System.Data.DataRow item in mostRecentPostsToThreads.Rows)
                {
                    mostRecentPostsToThreadsList.Add
                            (
                                new ThreadDate 
                                    { 
                                      CreateDate            = Convert.ToDateTime(item["CreateDate"]),
                                      ForumSubCategoryID    = Convert.ToInt32(item["ForumSubCategoryID"]) 
                                    }
                            );
                }

                var finalThreadList = new Dictionary<int, DateTime>();
 
                foreach(var thread in mostRecentThreads)
                {
                    finalThreadList.Add(thread.ForumSubCategoryID, thread.CreateDate);
                }

                foreach (var thread in mostRecentPostsToThreadsList)
                {
                    if (finalThreadList.ContainsKey(thread.ForumSubCategoryID))
                        finalThreadList[thread.ForumSubCategoryID] = thread.CreateDate; // updates to more recent date, last post
                    else
                        finalThreadList.Add(thread.ForumSubCategoryID, thread.CreateDate);
                }

                return finalThreadList.OrderByDescending(x => x.Value).Select(x => x.Key).Take(ForumThreadsToDisplay).ToList();
            }
        }

        private UserAccounts LoadTopUsers()
        {
            var topUsers = new UserAccounts();
            topUsers.GetMostApplaudedLastDays();

            return topUsers;
        }

        private Contents LoadRecentArticles()
        {
            var cnts = new Contents();
            cnts.GetContentPageWiseReleaseAll(1, CountOfNewsItemsOnHomepage);

            return cnts;
        }

        private UserAccounts LoadTopForumUsers(IEnumerable<int> mostPostsInForum)
        {
            var topForumUsers = new UserAccounts();

            topForumUsers.AddRange(mostPostsInForum.Select(topForumUser => new UserAccount(topForumUser)));

             return topForumUsers;
        }

        private ForumFeedModel LoadMostPopularThisWeek(
                                IGrouping<int, ForumPost> mostPopularThisWeek,
                                DasKlubDbContext context,
                                UserAccount ua)
        {
            var topForumThreadOfTheWeek =
                context.ForumSubCategory.FirstOrDefault(x => x.ForumSubCategoryID == mostPopularThisWeek.Key);

            if (mostPopularThisWeek == null) return null;

            var topFeedItem = new ForumFeedModel {ForumSubCategory = topForumThreadOfTheWeek};

            topFeedItem.PostCount =
                    context.ForumPost.Count(x => x.ForumSubCategoryID == topFeedItem.ForumSubCategory.ForumSubCategoryID);

            topFeedItem.ForumCategory =
                context.ForumCategory.FirstOrDefault(
                    x => x.ForumCategoryID == topFeedItem.ForumSubCategory.ForumCategoryID);
            var mostRecentPostToTopThread = context.ForumPost.OrderByDescending(x => x.CreateDate)
                                                   .FirstOrDefault(
                                                       x =>
                                                       x.ForumSubCategoryID ==
                                                       topFeedItem.ForumSubCategory.ForumSubCategoryID);
            if (mostRecentPostToTopThread != null)
            {
                topFeedItem.UserName    = new UserAccount(mostRecentPostToTopThread.CreatedByUserID).UserName;
                topFeedItem.LastPosted  = mostRecentPostToTopThread.CreateDate;
            }
                
            if (ua == null) return topFeedItem;

            var isNew =
                context.ForumPostNotification.FirstOrDefault(
                    x =>
                        x.ForumSubCategoryID == topForumThreadOfTheWeek.ForumSubCategoryID &&
                        x.UserAccountID == ua.UserAccountID);

            if (isNew == null || isNew.IsRead) return topFeedItem;

            topFeedItem.IsNewPost = true;

            var forumSubPostCount =
                context.ForumPost.Count(
                    x => x.ForumSubCategoryID == topFeedItem.ForumSubCategory.ForumSubCategoryID);

            var pageCount = (forumSubPostCount + ForumController.PageSizeForumPost - 1)/ForumController.PageSizeForumPost;

            if (mostRecentPostToTopThread != null)
            {
                topFeedItem.URLTo =
                    new Uri(string.Format("{0}/{1}#{2}", topFeedItem.ForumSubCategory.SubForumURL, ((pageCount > 1)
                        ? pageCount.ToString(CultureInfo.InvariantCulture)
                        : string.Empty), mostRecentPostToTopThread.ForumPostID.ToString(CultureInfo.InvariantCulture)));
            }
            return topFeedItem;
        }
     
        public ActionResult Contact()
        {
            return View();
        }

        #endregion
    }
}