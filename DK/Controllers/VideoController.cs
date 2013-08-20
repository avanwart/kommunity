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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DasKlub.Lib.AppSpec.DasKlub.BOL;
using DasKlub.Lib.AppSpec.DasKlub.BOL.ArtistContent;
using DasKlub.Lib.AppSpec.DasKlub.BOL.VideoContest;
using DasKlub.Lib.Values;
using IntrepidStudios;

namespace DasKlub.Web.Controllers
{
    public class VideoController : Controller
    {
        #region Variables

        private const int PageSize = 50;

        private readonly char[] _letters = new[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z'
            };

        private char _chosen = ' ';
        private Contest _contest;
        private Videos _toShow = new Videos();
        private int _videoPageNumber = 1;

        #endregion

        [HttpGet]
        public ActionResult Contests()
        {
            var contests = new Contests();

            contests.GetAll();

            contests.Sort((p1, p2) => p2.BeginDate.CompareTo(p1.BeginDate));

            return View(contests);
        }


        [HttpGet]
        public ActionResult Contest(string key)
        {
            _contest = new Contest();
            _contest.GetContestByKey(key);
            var convids = new ContestVideos();
            convids.GetContestVideosForContest(_contest.ContestID);
            ViewBag.ContestName = _contest.Name;
            var sngrcs = new SongRecords();
            sngrcs.AddRange(convids.Select(vi => new Video(vi.VideoID)).Select(vidCon => new SongRecord(vidCon)));
            sngrcs.Sort((p1, p2) => p2.VideoID.CompareTo(p1.VideoID));

            return View(sngrcs);
        }

        /// <summary>
        /// Gets bands and users
        /// </summary>
        private void LoadUserBandViewBag()
        {
            var sb = new StringBuilder();

            sb.Append(@"<div class=""letter_group""><ul>");

            foreach (var ch2 in _letters)
            {
                sb.Append("<li>");


                sb.AppendFormat(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(
                    "~/video/bands/" + Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture).ToLower())),
                                Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture)));


                sb.Append("</li>");
            }

            sb.Append("</ul></div>");

            ViewBag.LetterOfBands = sb.ToString();

            sb = new StringBuilder();
            sb.Append(@"<div class=""letter_group""><ul>");
            
            foreach (var ch2 in _letters)
            {
                sb.Append("<li>");

                sb.AppendFormat(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(
                    "~/video/users/" + Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture).ToLower())),
                                Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture)));

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
            _videoPageNumber = pageNumber;
            _toShow = new Videos();

            LoadFilteredVideos(true);

            var sngrcs = new SongRecords();
            sngrcs.AddRange(_toShow.Select(vi => new SongRecord(vi)));

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

            var cndss = DasKlub.Lib.AppSpec.DasKlub.BOL.VideoContest.Contest.GetCurrentContest();
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

            _toShow.GetListFilter(_videoPageNumber, PageSize, personType, footageType, videoType);

            if (isAjax) return; //this is ajax

            if (_videoPageNumber == 1)
            {
                var sngrcs = new SongRecords();
                sngrcs.AddRange(_toShow.Select(vi => new SongRecord(vi)));

                ViewBag.VideosFiltered = sngrcs.VideosPageList();
            }

            // video types
            var propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            var mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort((p1, p2) => String.Compare(p1.DisplayName, p2.DisplayName, StringComparison.Ordinal));

            var addList = new MultiProperties();
            addList.AddRange(mps.Where(mp1 => Videos.HasResults(footageType, mp1.MultiPropertyID, personType)));

            ViewBag.VideoTypes = addList;

            // person types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort((p1, p2) => String.Compare(p1.DisplayName, p2.DisplayName, StringComparison.Ordinal));

            addList = new MultiProperties();
            addList.AddRange(mps.Where(mp1 => Videos.HasResults(footageType, videoType, mp1.MultiPropertyID)));

            ViewBag.PersonTypes = addList;

            //// footage types
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
            mps = new MultiProperties(propTyp.PropertyTypeID);
            mps.Sort((p1, p2) => String.Compare(p1.DisplayName, p2.DisplayName, StringComparison.Ordinal));
            addList = new MultiProperties();
            addList.AddRange(mps.Where(mp1 => Videos.HasResults(footageType, mp1.MultiPropertyID, personType)));

            ViewBag.FootageTypes = addList;
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Filter(string firstLetter)
        {
            _chosen = Convert.ToChar(firstLetter);

            ViewBag.FirstLetter = firstLetter.ToUpper();

            var cloud1 = new Cloud
                {
                    DataIDField = @"keyword_id",
                    DataKeywordField = "keyword_value",
                    DataCountField = "keyword_count",
                    DataURLField = "keyword_url"
                };

            var theDs = firstLetter == "0"
                                ? Artists.GetArtistCloudByNonLetter()
                                : Artists.GetArtistCloudByLetter(firstLetter);
            cloud1.DataSource = theDs;

            cloud1.MinFontSize = 14;
            cloud1.MaxFontSize = 30;
            cloud1.FontUnit = "px";

            foreach (char chl in _letters.Where(chl => chl == Convert.ToChar(firstLetter)))
            {
                _chosen = chl;
            }

            var sb = new StringBuilder();

            sb.Append(@"<div class=""letter_group""><ul>");


            foreach (var ch2 in _letters)
            {
                sb.Append("<li>");

                if (Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture).ToLower()) ==
                    Convert.ToChar(_chosen.ToString(CultureInfo.InvariantCulture).ToLower()))
                {
                    sb.Append("<b>");
                    sb.Append(ch2);
                    sb.Append("</b>");
                }
                else
                {
                    sb.AppendFormat(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(
                        "~/video/bands/" + Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture).ToLower())),
                                    Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture)));
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
            _chosen = Convert.ToChar(firstLetter);

            ViewBag.FirstLetter = firstLetter.ToUpper();

            var cloud1 = new Cloud
                {
                    DataIDField = "keyword_id",
                    DataKeywordField = "keyword_value",
                    DataCountField = "keyword_count",
                    DataURLField = "keyword_url"
                };

            var theDs = Videos.GetAccountCloudByLetter(firstLetter);

            cloud1.DataSource = theDs;
            cloud1.MinFontSize = 14;
            cloud1.MaxFontSize = 30;
            cloud1.FontUnit = "px";

            foreach (var chl in _letters.Where(chl => chl == Convert.ToChar(firstLetter)))
            {
                _chosen = chl;
            }

            var sb = new StringBuilder();

            sb.Append(@"<div class=""letter_group""><ul>");


            foreach (var ch2 in _letters)
            {
                sb.Append("<li>");

                if (Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture).ToLower()) ==
                    Convert.ToChar(_chosen.ToString(CultureInfo.InvariantCulture).ToLower()))
                {
                    sb.Append("<b>");
                    sb.Append(ch2);
                    sb.Append("</b>");
                }
                else
                {
                    sb.AppendFormat(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(
                        "~/video/users/" + Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture).ToLower())),
                                    Convert.ToChar(ch2.ToString(CultureInfo.InvariantCulture)));
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