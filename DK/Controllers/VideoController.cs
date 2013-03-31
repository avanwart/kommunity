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
using System.Data;
using System.Text;
using System.Web.Mvc;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Operational;
using BootBaronLib.Values;
using IntrepidStudios;

namespace DasKlub.Controllers
{
    public class VideoController : Controller
    {

        #region Variables

        char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        char chosen = ' ';
        int pageSize = 50;
        Videos toShow = new Videos();
        Contest contest = null;
        int videoPageNumber = 1;

        #endregion

        [HttpGet]
        public ActionResult Contests()
        {
            BootBaronLib.AppSpec.DasKlub.BOL.VideoContest.Contests contests = new Contests();

            contests.GetAll();

            return View(contests);
            
        }


        [HttpGet]
        public ActionResult Contest(string key)
        {
            contest = new Contest();

            contest.GetContestByKey(key);

            ContestVideos convids = new ContestVideos();

            convids.GetContestVideosForContest(contest.ContestID);

            ViewBag.ContestName = contest.Name;



            SongRecords sngrcs = new SongRecords();
            SongRecord sngrcd = null;
            BootBaronLib.AppSpec.DasKlub.BOL.Video vidCon = null;

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.VideoContest.ContestVideo vi in convids)
            {
                vidCon = new BootBaronLib.AppSpec.DasKlub.BOL.Video(vi.VideoID);

                sngrcd = new SongRecord(vidCon);

                sngrcs.Add(sngrcd);
            }



            sngrcs.Sort(delegate(BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent.SongRecord p1,
            BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent.SongRecord p2)
            {
                return p2.VideoID.CompareTo(p1.VideoID);
            });

            return View(sngrcs);
        }

        private void LoadUserBandViewBag()
        {
            char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            ////// BANDS

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<div class=""letter_group""><ul>");

            foreach (char ch2 in letters)
            {
                sb.Append("<li>");


                sb.AppendFormat(@"<a href=""{0}"">{1}</a>", System.Web.VirtualPathUtility.ToAbsolute(
    "~/video/bands/" + Convert.ToChar(ch2.ToString().ToLower())), Convert.ToChar(ch2.ToString()));



                sb.Append("</li>");
            }

            sb.Append("</ul></div>");

            ViewBag.LetterOfBands = sb.ToString();

            //// USERS 

            sb = new StringBuilder();

            sb.Append(@"<div class=""letter_group""><ul>");

            foreach (char ch2 in letters)
            {
                sb.Append("<li>");

                sb.AppendFormat(@"<a href=""{0}"">{1}</a>", System.Web.VirtualPathUtility.ToAbsolute(
                    "~/video/users/" + Convert.ToChar(ch2.ToString().ToLower())), Convert.ToChar(ch2.ToString()));

                sb.Append("</li>");
            }

            sb.Append("</ul></div>");

            ViewBag.LetterOfUsers = sb.ToString();

        }

        [HttpGet]
        public ActionResult Detail(string videoContext)
        {
            ViewBag.VideoHeight = (Request.Browser.IsMobileDevice) ? 200 : 400;
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ? 225 : 600;

            return View();
        }

        public JsonResult Items(int pageNumber)
        {
            videoPageNumber = pageNumber;
            toShow = new BootBaronLib.AppSpec.DasKlub.BOL.Videos();

            LoadFilteredVideos(true);

            SongRecords sngrcs = new SongRecords();
            SongRecord sngrcd = null;

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video vi in toShow)
            {
                sngrcd = new SongRecord(vi);
                sngrcs.Add(sngrcd);
            }

            sngrcs.IncludeStateAndEndTag = false;

            return Json(new
            {
                ListItems = sngrcs.VideosPageList()
            });
        }
       
        public ActionResult Index()
        {
            LoadUserBandViewBag();

            LoadFilteredVideos(false);

            return View();
        }

        private void LoadFilteredVideos(bool isAjax)
        {
            int? personType = null;
            int? footageType = null;
            int? videoType = null;
            
  
            if (!string.IsNullOrEmpty(
             Request.QueryString[SiteEnums.QueryStringNames.videoType.ToString()]))
            {
                videoType = Convert.ToInt32(
                    Request.QueryString[SiteEnums.QueryStringNames.videoType.ToString()]);
            }

            if (!string.IsNullOrEmpty(
            Request.QueryString[SiteEnums.QueryStringNames.personType.ToString()]))
            {
                personType = Convert.ToInt32(
                    Request.QueryString[SiteEnums.QueryStringNames.personType.ToString()]);
            }

            if (!string.IsNullOrEmpty(
            Request.QueryString[SiteEnums.QueryStringNames.footageType.ToString()]))
            {
                footageType = Convert.ToInt32(
                    Request.QueryString[SiteEnums.QueryStringNames.footageType.ToString()]);
            }

            toShow.GetListFilter(videoPageNumber, pageSize, personType, footageType, videoType);

            if(isAjax) return;//this is ajax

            if (videoPageNumber == 1)
            {
                SongRecords sngrcs = new SongRecords();
                SongRecord sngrcd = null;

                foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video vi in toShow)
                {
                    sngrcd = new SongRecord(vi);

                    sngrcs.Add(sngrcd);
                }

                ViewBag.VideosFiltered = sngrcs.VideosPageList();
            }

            MultiProperties addList = null;
            PropertyType propTyp = null;
            MultiProperties mps = null;


            // video types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort(delegate(MultiProperty p1, MultiProperty p2)
            {
                return p1.DisplayName.CompareTo(p2.DisplayName);
            });

            addList = new MultiProperties();

            foreach (MultiProperty mp1 in mps)
            {
                if (Videos.HasResults(footageType, mp1.MultiPropertyID, personType))
                {
                    addList.Add(mp1);
                }
            }

            ViewBag.VideoTypes = addList;

            // person types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort(delegate(MultiProperty p1, MultiProperty p2)
            {
                return p1.DisplayName.CompareTo(p2.DisplayName);
            });
           
            addList = new MultiProperties();

            foreach (MultiProperty mp1 in mps)
            {
                if (Videos.HasResults( footageType, videoType, mp1.MultiPropertyID ))
                {
                    addList.Add(mp1);
                }
            }

            ViewBag.PersonTypes = addList;

            //// footage types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort(delegate(MultiProperty p1, MultiProperty p2)
            {
                return p1.DisplayName.CompareTo(p2.DisplayName);
            });
            addList = new MultiProperties();

            foreach (MultiProperty mp1 in mps)
            {
                if (Videos.HasResults(footageType, mp1.MultiPropertyID, personType))
                {
                    addList.Add(mp1);
                }
            }

            ViewBag.FootageTypes = addList;
 
 
 
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Filter(string firstLetter)
        {
            chosen = Convert.ToChar(firstLetter);

            ViewBag.FirstLetter = firstLetter.ToUpper();

            Cloud cloud1 = new Cloud();
            cloud1.DataIDField = "keyword_id";
            cloud1.DataKeywordField = "keyword_value";
            cloud1.DataCountField = "keyword_count";
            cloud1.DataURLField = "keyword_url";
            //cloud1.MinColor = "#000000";
            //cloud1.MaxColor = "#000000";

            DataSet theDS = new DataSet();

            if (firstLetter == "0")
            {
                theDS = Artists.GetArtistCloudByNonLetter();
            }
            else
            {
                theDS = Artists.GetArtistCloudByLetter(firstLetter);
            }
            cloud1.DataSource = theDS;

            cloud1.MinFontSize = 14;
            cloud1.MaxFontSize = 30;
            cloud1.FontUnit = "px";

            foreach (char chl in letters)
            {
                if (chl == Convert.ToChar(firstLetter))
                {
                    chosen = chl;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<div class=""letter_group""><ul>");


            foreach (char ch2 in letters)
            {
                sb.Append("<li>");

                if (Convert.ToChar(ch2.ToString().ToLower()) == Convert.ToChar(chosen.ToString().ToLower()))
                {
                    sb.Append("<b>");
                    sb.Append(ch2);
                    sb.Append("</b>");
                }
                else
                {
                    sb.AppendFormat(@"<a href=""{0}"">{1}</a>", System.Web.VirtualPathUtility.ToAbsolute(
                    "~/video/bands/" + Convert.ToChar(ch2.ToString().ToLower())), Convert.ToChar(ch2.ToString()));
                }

                sb.Append("</li>");
            }

            sb.Append("</ul></div>");

            ViewBag.LetterOfBands = sb.ToString();

            ViewBag.CloudBands = cloud1.HTML();

            return View();
        }

        public ActionResult UserFilter(string firstLetter)
        {
            chosen = Convert.ToChar(firstLetter);

            ViewBag.FirstLetter = firstLetter.ToUpper();

            Cloud cloud1 = new Cloud();
            cloud1.DataIDField = "keyword_id";
            cloud1.DataKeywordField = "keyword_value";
            cloud1.DataCountField = "keyword_count";
            cloud1.DataURLField = "keyword_url";
            //cloud1.MinColor = "#000000";
            //cloud1.MaxColor = "#000000";

            DataSet theDS = new DataSet();


            theDS = Videos.GetAccountCloudByLetter(firstLetter);

            cloud1.DataSource = theDS;
            cloud1.MinFontSize = 14;
            cloud1.MaxFontSize = 30;
            cloud1.FontUnit = "px";

            foreach (char chl in letters)
            {
                if (chl == Convert.ToChar(firstLetter))
                {
                    chosen = chl;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<div class=""letter_group""><ul>");


            foreach (char ch2 in letters)
            {
                sb.Append("<li>");

                if (Convert.ToChar(ch2.ToString().ToLower()) == Convert.ToChar(chosen.ToString().ToLower()))
                {
                    sb.Append("<b>");
                    sb.Append(ch2);
                    sb.Append("</b>");
                }
                else
                {
                    sb.AppendFormat(@"<a href=""{0}"">{1}</a>", System.Web.VirtualPathUtility.ToAbsolute(
    "~/video/users/" + Convert.ToChar(ch2.ToString().ToLower())), Convert.ToChar(ch2.ToString() ));

                }

                sb.Append("</li>");
            }

            sb.Append("</ul></div>");

            ViewBag.LetterOfUsers = sb.ToString();
            
            ViewBag.CloudUsers = cloud1.HTML();

            return View();
        }


    }
 
}
