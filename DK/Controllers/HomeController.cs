//  Copyright 2013 
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
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.AppSpec.DasKlub.BOL.DomainConnection;
using BootBaronLib.AppSpec.DasKlub.BOL.Logging;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Configs;
using BootBaronLib.Operational;
using BootBaronLib.Values;
using Google.GData.Client;
using Google.YouTube;
using HttpUtility = System.Web.HttpUtility;
using Utilities = BootBaronLib.Operational.Utilities;
using Video = BootBaronLib.AppSpec.DasKlub.BOL.Video;

namespace DasKlub.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly string _devkey = GeneralConfigs.YouTubeDevKey;
        private readonly string _password = GeneralConfigs.YouTubeDevPass;
        private readonly string _username = GeneralConfigs.YouTubeDevUser;

        #region Variables

        #endregion

        #region Json

        public JsonResult Download(char link)
        {
            if (Request.Url != null)
            {
                var cl = new ClickLog
                    {
                        CurrentURL = Request.Url.ToString(),
                        ClickType = link,
                        IpAddress = Request.UserHostAddress
                    };

                var mu = Membership.GetUser();

                if (mu != null)
                {
                    cl.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                }

                cl.Create();
            }

            return Json(new
                {
                    Result = 1
                });
        }

        #endregion

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
            if (string.IsNullOrWhiteSpace(video))
            {
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();
            }
            var vir = new VideoRequest {RequestURL = video};

            var vidKey = string.Empty;

            vir.RequestURL = vir.RequestURL.Replace("https", "http");

            if (vir.RequestURL.Contains("http://youtu.be/"))
            {
                vir.VideoKey = vir.RequestURL.Replace("http://youtu.be/", string.Empty);
            }
            else if (vir.RequestURL.Contains("http://www.youtube.com/watch?"))
            {
                var nvcKey =
                    HttpUtility.ParseQueryString(vir.RequestURL.Replace("http://www.youtube.com/watch?", string.Empty));

                vir.VideoKey = nvcKey["v"];
                vidKey = nvcKey["v"];
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
                var yousettings = new YouTubeRequestSettings("You Manager", _devkey, _username, _password);

                var yourequest = new YouTubeRequest(yousettings);
                var entryUri = new Uri(string.Format("http://gdata.youtube.com/feeds/api/videos/{0}", vidKey));

                Google.YouTube.Video video2 = yourequest.Retrieve<Google.YouTube.Video>(entryUri);
                vid.Duration = (float) Convert.ToDouble(video2.YouTubeEntry.Duration.Seconds);
                vid.ProviderUserKey = video2.Uploader;
                vid.PublishDate = video2.YouTubeEntry.Published;
            }
            catch (GDataRequestException)
            {
                vid.IsEnabled = false;
                vid.Update();
                //litVideo.Text = string.Empty;
                // return;

                // invalid 
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();
            }
            catch (ClientFeedException)
            {
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();

            }
            catch
            {
                vir.StatusType = 'I';
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();
            }


            vid.VolumeLevel = 5;
                // vid.HumanType = personType;


                //    t(string video, string videoType, string personType,
                //string footageType, string band, string song, string contestID)


                //  vid.VideoType = videoType;


                //vid.Duration = (float)Convert.ToDouble(txtDuration.Text);
                //vid.Intro = (float)Convert.ToDouble(txtSecondsIn.Text);
                //vid.LengthFromStart = (float)Convert.ToDouble(txtElasedEnd.Text);
                //vid.ProviderCode = ddlVideoProvider.SelectedValue;
                //vid.ProviderUserKey = txtUserName.Text;
                //vid.VolumeLevel = Convert.ToInt32(ddlVolumeLevel.SelectedValue);
                //vid.IsEnabled = chkEnabled.Checked;
                //// vid.IsHidden = chkHidden.Checked;
                //vid.EnableTrim = chkEnabled.Checked;

                ///// publish date 
                //YouTubeRequestSettings yousettings =
                //    new YouTubeRequestSettings("You Manager", devkey, username, password);
                //YouTubeRequest yourequest;
                //Uri Url;

                //yourequest = new YouTubeRequest(yousettings);
                //Url = new Uri("http://gdata.youtube.com/feeds/api/videos/" + vid.ProviderKey);
                //video = new Google.YouTube.Video();
                //video = yourequest.Retrieve<Google.YouTube.Video>(Url);
                //vid.PublishDate = video.YouTubeEntry.Published;

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


                //// guitar
                //if (!string.IsNullOrWhiteSpace(this.ddlGuitarType.SelectedValue)
                //    && this.ddlGuitarType.SelectedValue != selectText)
                //{
                //    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.GUITR);
                //    mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                //    MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
                //    mp.RemoveCache();
                //    MultiPropertyVideo.AddMultiPropertyVideo(
                //        Convert.ToInt32(ddlGuitarType.SelectedValue), vid.VideoID);
                //}

                //// Language
                //if (!string.IsNullOrWhiteSpace(this.ddlLanguage.SelectedValue)
                //    && this.ddlLanguage.SelectedValue != selectText)
                //{
                //    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.LANGE);
                //    mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                //    MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
                //    mp.RemoveCache();
                //    MultiPropertyVideo.AddMultiPropertyVideo(
                //        Convert.ToInt32(ddlLanguage.SelectedValue), vid.VideoID);
                //}


                //// genre
                //if (!string.IsNullOrWhiteSpace(this.ddlGenre.SelectedValue)
                //    && this.ddlGenre.SelectedValue != selectText)
                //{
                //    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.GENRE);
                //    mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                //    MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
                //    mp.RemoveCache();
                //    MultiPropertyVideo.AddMultiPropertyVideo(
                //        Convert.ToInt32(ddlGenre.SelectedValue), vid.VideoID);
                //}

                //// difficulty
                //if (!string.IsNullOrWhiteSpace(this.ddlDifficultyLevel.SelectedValue)
                //    && this.ddlDifficultyLevel.SelectedValue != selectText)
                //{
                //    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.DIFFC);
                //    mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                //    MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
                //    mp.RemoveCache();
                //    MultiPropertyVideo.AddMultiPropertyVideo(
                //        Convert.ToInt32(this.ddlDifficultyLevel.SelectedValue), vid.VideoID);
                //}

                //VideoSong.DeleteSongsForVideo(vid.VideoID);

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

                //  RefreshLists();

                //                lblStatus.Text = "OK";

                if (vid.VideoID > 0)
                {
                    Response.Redirect(vid.VideoURL); // just send them to it
                }
            
           
                //              lblStatus.Text = ex.Message;

                //{
        
                //}
           

            //Video v1 = new Video();

            //if (!string.IsNullOrEmpty(vir.VideoKey))
            //{
            //    v1 = new Video("YT", vir.VideoKey);
            //}

            //if (v1.VideoID > 0 && v1.IsEnabled)
            //{
            //     Response.Redirect(v1.VideoURL);// just send them to it
            //     return new EmptyResult();
            //}

            //vir.GetVideoRequest();

            //if (vir.StatusType == 'W')
            //{
            //    Response.Redirect("~/videosubmission.aspx?statustype=W");
            //    return new EmptyResult();
            //}
            //else if (vir.StatusType == 'R')
            //{
            //     Response.Redirect("~/videosubmission.aspx?statustype=R");
            //     return new EmptyResult();
            //}

            //vir.StatusType = 'W';
            //vir.Create();
            // Response.Redirect("~/videosubmission.aspx?statustype=W");
            // return new EmptyResult();

            //if (vir.StatusType == 'W')
            //{
            //    Response.Redirect("~/videosubmission.aspx?statustype=W");
            //    return new EmptyResult();
            //}
            //else if (vir.StatusType == 'R')
            //{
            //    Response.Redirect("~/videosubmission.aspx?statustype=R");
            //    return new EmptyResult();
            //}
            //else
            //{
            //    v1 = new Video("YT", vir.VideoKey);

            //    if (v1.VideoID > 0 && v1.IsEnabled)
            //    {
            //        Response.Redirect(v1.VideoURL);// just send them to it
            //    }
            //    else
            //    {
            //        vir.StatusType = 'W';
            //        vir.Create();
            //        Response.Redirect("~/videosubmission.aspx?statustype=W");
            //        return new EmptyResult();
            //    }
            //}


            return new EmptyResult();
        }


        public ActionResult Index()
        {
            // CONTESTS



            //// 
            //var pitms = new PhotoItems {UseThumb = true, ShowTitle = false};
            //pitms.GetPhotoItemsPageWise(1, 4);

            //ViewBag.PhotoList = pitms.ToUnorderdList;

            //var cnts = new Contents();
            //cnts.GetContentPageWiseAll(1, 3);

            //ViewBag.RecentArticles = cnts.ToUnorderdList;

            //var uas = new UserAccounts();
            //uas.GetNewestUsers();
            //ViewBag.NewestUsers = uas.ToUnorderdList;


            //var newestVideos = new Videos();
            //newestVideos.GetMostRecentVideos();
            //var newSongs = new SongRecords();
            //newSongs.AddRange(newestVideos.Select(v1 => new SongRecord(v1)));

            //ViewBag.NewestVideos = newSongs.VideosList();


            //var vid = new Video(Video.RandomVideoIDVideo());

            //ViewBag.RandomVideoKey = vid.ProviderKey;



            var cndss = Contest.GetCurrentContest();
            var cvids = new ContestVideos();


            var vidsInContest = new Videos();
            vidsInContest.AddRange(cvids.Select(cv1 => new Video(cv1.VideoID)));

            vidsInContest.Sort((p1, p2) => p2.PublishDate.CompareTo(p1.PublishDate));

            var sngrcds3 = new SongRecords();
            sngrcds3.AddRange(vidsInContest.Select(v1 => new SongRecord(v1)));

            ViewBag.ContestVideoList = sngrcds3.VideosList();
            ViewBag.CurrentContest = cndss;

            // video typesa
            var propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            var mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort((p1, p2) => String.Compare(p1.DisplayName, p2.DisplayName, StringComparison.Ordinal));


            ViewBag.VideoTypes = mps;

            // person types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort((p1, p2) => String.Compare(p1.DisplayName, p2.DisplayName, StringComparison.Ordinal));

            ViewBag.PersonTypes = mps;

            //// footage types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort((p1, p2) => String.Compare(p1.DisplayName, p2.DisplayName, StringComparison.Ordinal));


            ViewBag.FootageTypes = mps;


            return View();
        }

     

        public ActionResult Contact()
        {
            return View();
        }

        #endregion
    }
}