//  Copyright 2012 
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
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.AppSpec.DasKlub.BOL.DomainConnection;
using BootBaronLib.AppSpec.DasKlub.BOL.Logging;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Enums;
using BootBaronLib.Operational.Converters;
using Google.GData.Client;
using Google.YouTube;


namespace DasKlub.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        string devkey = BootBaronLib.Configs.GeneralConfigs.YouTubeDevKey;
        string username = BootBaronLib.Configs.GeneralConfigs.YouTubeDevUser;
        string password = BootBaronLib.Configs.GeneralConfigs.YouTubeDevPass;

        #region Variables

        char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        Videos toShow = new Videos();


        #endregion

        #region Json

        public JsonResult Download(char link)
        {
            ClickLog cl = new ClickLog();

            cl.CurrentURL = Request.Url.ToString();
            cl.ClickType = link;
            cl.IpAddress = Request.UserHostAddress;

            MembershipUser mu = Membership.GetUser();

            if (mu != null)
            {
                cl.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
            }

            cl.Create();

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


            SiteEnums.SiteBrandType siteBrand =
(SiteEnums.SiteBrandType)Enum.Parse(typeof(SiteEnums.SiteBrandType), brandType);

            ViewBag.ContentMessage = SiteDomain.GetSiteDomainValue(siteBrand, BootBaronLib.Operational.Utilities.GetCurrentLanguageCode());


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
            VideoRequest vir = new VideoRequest();

            vir.RequestURL = video;

            string vidKey = string.Empty;

            vir.RequestURL = vir.RequestURL.Replace("https", "http");

            if (vir.RequestURL.Contains("http://youtu.be/"))
            {
                vir.VideoKey = vir.RequestURL.Replace("http://youtu.be/", string.Empty);
            }
            else if (vir.RequestURL.Contains("http://www.youtube.com/watch?"))
            {
                NameValueCollection nvcKey = System.Web.HttpUtility.ParseQueryString(vir.RequestURL.Replace("http://www.youtube.com/watch?", string.Empty));

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




            try
            {

                //IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();

                //while (enumerator.MoveNext())
                //{

                //    HttpContext.Current.Cache.Remove(enumerator.Key.ToString());

                //}

                //Artists allartsis = new Artists();
                //allartsis.RemoveCache();

                //if (gvwRequestedVideos.SelectedDataKey != null)
                //{
                //    vidreq = new VideoRequest(Convert.ToInt32(gvwRequestedVideos.SelectedDataKey.Value));
                //    vidreq.StatusType = 'A';
                //    vidreq.Update();
                //}

                BootBaronLib.AppSpec.DasKlub.BOL.Video vid = new BootBaronLib.AppSpec.DasKlub.BOL.Video("YT", vidKey);


                vid.ProviderCode = "YT";

                Google.YouTube.Video video2;
                video2 = new Google.YouTube.Video();

                try
                {
                    YouTubeRequestSettings yousettings = new YouTubeRequestSettings("You Manager", devkey, username, password);
                    YouTubeRequest yourequest;
                    Uri Url;

                    yourequest = new YouTubeRequest(yousettings);
                    Url = new Uri("http://gdata.youtube.com/feeds/api/videos/" + vidKey);

                    video2 = yourequest.Retrieve<Google.YouTube.Video>(Url);
                    vid.Duration = (float)Convert.ToDouble(video2.YouTubeEntry.Duration.Seconds);
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
                int subContestID = 0;
                if (!string.IsNullOrWhiteSpace(contestID) && int.TryParse(contestID, out subContestID) && subContestID > 0)
                {
                    //TODO: check if it already is in the contest

                    ContestVideo.DeleteVideoFromAllContests(vid.VideoID);

                    ContestVideo cv = new ContestVideo();

                    cv.ContestID = subContestID;
                    cv.VideoID = vid.VideoID;
                    cv.Create();
                }
                else
                {
                    // TODO: JUST REMOVE FROM CURRENT CONTEST, NOT ALL
                    ContestVideo.DeleteVideoFromAllContests(vid.VideoID);
                }

                PropertyType propTyp = null;
                MultiProperty mp = null;

                // vid type

                propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
                mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
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

                Artist artst = new Artist(band.Trim());

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


                Song sng = new Song(artst.ArtistID, song.Trim());


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
                    Response.Redirect(vid.VideoURL);// just send them to it
                }
            }
            catch
            {
                //              lblStatus.Text = ex.Message;

                //{
                Response.Redirect("~/videosubmission.aspx?statustype=I");
                return new EmptyResult();
                //}

            }











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


            Contest cndss = Contest.GetCurrentContest();
            ContestVideos cvids = new ContestVideos();


            Videos vidsInContest = new Videos();
            BootBaronLib.AppSpec.DasKlub.BOL.Video vid2 = null;

            foreach (ContestVideo cv1 in cvids)
            {
                vid2 = new BootBaronLib.AppSpec.DasKlub.BOL.Video(cv1.VideoID);
                vidsInContest.Add(vid2);
            }

            vidsInContest.Sort(delegate(BootBaronLib.AppSpec.DasKlub.BOL.Video p1, BootBaronLib.AppSpec.DasKlub.BOL.Video p2)
            {
                return p2.PublishDate.CompareTo(p1.PublishDate);
            });

            SongRecords sngrcds3 = new SongRecords();
            SongRecord sng3 = new SongRecord();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video v1 in vidsInContest)
            {
                sng3 = new SongRecord(v1);

                sngrcds3.Add(sng3);
            }

            ViewBag.ContestVideoList = sngrcds3.VideosList();
            ViewBag.CurrentContest = cndss;


            // 
            PhotoItems pitms = new PhotoItems();
            pitms.UseThumb = true;
            pitms.ShowTitle = false;
            pitms.GetPhotoItemsPageWise(1, 4);

            ViewBag.PhotoList = pitms.ToUnorderdList;

            Contents cnts = new Contents();
            cnts.GetContentPageWiseAll(1, 3);

            ViewBag.RecentArticles = cnts.ToUnorderdList;

            UserAccounts uas = new UserAccounts();
            uas.GetNewestUsers();
            ViewBag.NewestUsers = uas.ToUnorderdList;


            Videos newestVideos = new Videos();
            newestVideos.GetMostRecentVideos();
            SongRecords newSongs = new SongRecords();
            SongRecord sng1 = null;
            foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video v1 in newestVideos)
            {
                sng1 = new SongRecord(v1);
                newSongs.Add(sng1);
            }

            ViewBag.NewestVideos = newSongs.VideosList();



            BootBaronLib.AppSpec.DasKlub.BOL.Video vid = new BootBaronLib.AppSpec.DasKlub.BOL.Video(BootBaronLib.AppSpec.DasKlub.BOL.Video.RandomVideoIDVideo());

            ViewBag.RandomVideoKey = vid.ProviderKey;


            ///video submit
            MultiProperties addList = null;
            PropertyType propTyp = null;
            MultiProperties mps = null;

            // video typesa
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort(delegate(MultiProperty p1, MultiProperty p2)
            {
                return p1.DisplayName.CompareTo(p2.DisplayName);
            });


            ViewBag.VideoTypes = mps;

            // person types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort(delegate(MultiProperty p1, MultiProperty p2)
            {
                return p1.DisplayName.CompareTo(p2.DisplayName);
            });

            ViewBag.PersonTypes = mps;

            //// footage types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort(delegate(MultiProperty p1, MultiProperty p2)
            {
                return p1.DisplayName.CompareTo(p2.DisplayName);
            });
            addList = new MultiProperties();


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
