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

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class SongRecord
    {
        #region properties

        public ArrayList Songs = null;
        private bool _getRelatedVideos = true;
        private string _human = string.Empty;
        private string _providerCode = string.Empty;
        private string _providerKey = string.Empty;
        private string _songDisplay = string.Empty;
        private string _userAccount = string.Empty;

        public bool GetRelatedVideos
        {
            get { return _getRelatedVideos; }
            set { _getRelatedVideos = value; }
        }


        public string UserAccountUrl
        {
            get
            {
                return string.Format(@"<a href=""{0}"">{1}</a>",
                                     VirtualPathUtility.ToAbsolute(string.Format("~/{0}", UserAccount)), UserAccount);
            }
        }

        public string ProviderKey
        {
            get { return _providerKey; }
            set { _providerKey = value; }
        }

        public string ProviderCode
        {
            get { return _providerCode; }
            set { _providerCode = value; }
        }

        public float TotalSeconds { get; set; }

        public float StartTime { get; set; }

        public float EndTime { get; set; }


        public string SongDisplay
        {
            get { return _songDisplay; }
            set { _songDisplay = value; }
        }

        public string Human
        {
            get { return _human; }
            set { _human = value; }
        }

        public int VolumeLevel { get; set; }


        public string UserAccount
        {
            get { return _userAccount; }
            set { _userAccount = value; }
        }


        public int VideoID { get; set; }

        #endregion

        #region constructor

        public SongRecord()
        {
        }

        public SongRecord(Video vid)
        {
            VideoID = vid.VideoID;
            UserAccount = vid.ProviderUserKey;
            VolumeLevel = vid.VolumeLevel;
            EndTime = vid.LengthFromStart;
            StartTime = vid.Intro;
            TotalSeconds = vid.Duration;
            var pt = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            var mp = new MultiProperty(vid.VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            Human = mp.Name;
            ProviderCode = vid.ProviderCode;
            ProviderKey = vid.ProviderKey;

            var sngs = new Songs();
            sngs.GetSongsForVideo(vid.VideoID);

            Artist art;

            if (sngs.Count > 1)
            {
                // sort oldest 1st 
                sngs.Sort(delegate(Song p1, Song p2) { return p1.RankOrder.CompareTo(p2.RankOrder); });

                int cnt = 1;

                Songs = new ArrayList();

                foreach (Song sng in sngs)
                {
                    Songs.Add(sng.SongID);

                    art = new Artist(sng.ArtistID);
                    SongDisplay += HttpUtility.HtmlEncode(
                        string.Format(@"{0} : <a href=""{1}/{2}"">{3}</a> - {4}<br />",
                                      cnt.ToString(CultureInfo.InvariantCulture),
                                      Utilities.URLAuthority(),
                                      art.AltName,
                                      art.Name,
                                      sng.Name));

                    cnt++;
                }
            }
            else if (sngs.Count == 1)
            {
                art = new Artist(sngs[0].ArtistID);
                //this.SongDisplay += art.Name + " - " + sngs[0].Name;// +"<br />";// +HttpUtility.HtmlEncode(" | ");

                SongDisplay +=
                    HttpUtility.HtmlEncode(
                        string.Format(@"<a href=""{0}/{1}"">{2}</a> - {3}",
                                      Utilities.URLAuthority(), art.AltName, art.Name, sngs[0].Name));
            }
        }

        #endregion

        private string _songDisplayNoLink = string.Empty;

        private string _songDisplayNoLink2 = string.Empty;

        public string SongDisplayNoLink
        {
            private set { _songDisplayNoLink = value; }

            get
            {
                var vid = new Video(VideoID);

                var sngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);

                Artist art;

                if (sngs.Count > 1)
                {
                    // sort oldest 1st 
                    sngs.Sort((p1, p2) => p1.RankOrder.CompareTo(p2.RankOrder));

                    var cnt = 1;

                    Songs = new ArrayList();

                    foreach (var sng in sngs)
                    {
                        Songs.Add(sng.SongID);

                        art = new Artist(sng.ArtistID);
                        _songDisplayNoLink += HttpUtility.HtmlEncode(
                            string.Format("{0}: <b>{1}</b> - {2} <br />", cnt.ToString(CultureInfo.InvariantCulture),
                                          art.Name, sng.Name));
                        cnt++;
                    }
                }
                else if (sngs.Count == 1)
                {
                    art = new Artist(sngs[0].ArtistID);
                    _songDisplayNoLink += HttpUtility.HtmlEncode(string.Format(@"<b>{0}</b> - {1}", art.Name, sngs[0].Name));
                }
                return _songDisplayNoLink;
            }
        }

        public string SongDisplayNoLink2
        {
            get
            {
                var vid = new Video(VideoID);

                var sngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);

                Artist art;

                if (sngs.Count > 1)
                {
                    sngs.Sort((p1, p2) => p1.RankOrder.CompareTo(p2.RankOrder));

                    var cnt = 1;

                    Songs = new ArrayList();

                    foreach (var sng in sngs)
                    {
                        Songs.Add(sng.SongID);

                        art = new Artist(sng.ArtistID);
                        if (_songDisplayNoLink2 != null)
                            return _songDisplayNoLink2 += HttpUtility.HtmlEncode(
                                cnt.ToString(CultureInfo.InvariantCulture) + ": " + art.Name + " - " + sng.Name);
                        cnt++;
                    }
                }
                else if (sngs.Count == 1)
                {
                    art = new Artist(sngs[0].ArtistID);
                    _songDisplayNoLink2 += HttpUtility.HtmlEncode(art.Name + " - " + sngs[0].Name);
                }
                return _songDisplayNoLink2;
            }
        }

        public string SongDisplayNoLink3
        {
            set { _songDisplayNoLink = value; }

            get
            {
                var vid = new Video(VideoID);

                var sngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);

                Artist art;

                if (sngs.Count > 1)
                {
                    // sort oldest 1st 
                    sngs.Sort((p1, p2) => p1.RankOrder.CompareTo(p2.RankOrder));

                    const int cnt = 1;

                    Songs = new ArrayList();

                    foreach (Song sng in sngs)
                    {
                        Songs.Add(sng.SongID);

                        art = new Artist(sng.ArtistID);

                        return _songDisplayNoLink +=
                               string.Format("{0}: {1} - {2} <br />", arg0: cnt, arg1: art.Name, arg2: sng.Name);
                    }
                }
                else if (sngs.Count == 1)
                {
                    art = new Artist(sngs[0].ArtistID);
                    _songDisplayNoLink += string.Format(@"<a href=""{0}"">{1}</a> - {2}", art.UrlTo, art.Name,
                                                        sngs[0].Name);
                }
                return _songDisplayNoLink;
            }
        }

        public string JSONResponse
        {
            get
            {
                var sb = new StringBuilder(100);


                sb.Append(@"{""EndTime"": """);
                sb.Append(EndTime.ToString(CultureInfo.InvariantCulture));
                sb.Append(@""", 
            ""Human"": """);
                sb.Append(Human);
                sb.Append(@""", 
            ""ProviderCode"": """);
                sb.Append(ProviderCode);
                sb.Append(@""", 
            ""ProviderKey"": """);
                sb.Append(ProviderKey);
                sb.Append(@""", 
            ""SongDisplay"": """);
                sb.Append(SongDisplay);
                sb.Append(@""", 
            ""StartTime"": """);
                sb.Append(StartTime.ToString(CultureInfo.InvariantCulture));
                sb.Append(@""", 
            ""TotalSeconds"": """);
                sb.Append(TotalSeconds.ToString(CultureInfo.InvariantCulture));
                sb.Append(@""", 
            ""UserAccount"": """);
                sb.Append(UserAccount);
                sb.Append(@""", 
            ""RelatedVids"": """);
                sb.Append(((GetRelatedVideos) ? Video.GetRelatedVideosList(VideoID) : string.Empty));
                sb.Append(@""", 
            ""ITunesLink"": """);
                sb.Append(SongProperty.GetPropertyTypeLink(VideoID));
                sb.Append(@""", 
            ""VolumeLevel"": """);
                sb.Append(VolumeLevel.ToString(CultureInfo.InvariantCulture));
                sb.Append(@"""}");

                return sb.ToString();
            }
        }
    }

    public class SongRecords : List<SongRecord>
    {
        private bool _enableChangeOrder = true;
        private bool _includeStateAndEndTag = true;
        public bool IsUserSelected { private get; set; }

        public bool EnableChangeOrder
        {
            private get { return _enableChangeOrder; }
            set { _enableChangeOrder = value; }
        }

        public bool IncludeStateAndEndTag
        {
            private get { return _includeStateAndEndTag; }
            set { _includeStateAndEndTag = value; }
        }


        public string ListOfVideos(string humanType, string videoType)
        {
            if (Count == 0) return string.Empty;


            var sb = new StringBuilder();

            sb.Append(@"<div class=""gallery"">");

            if (IsUserSelected)
            {
            }
            else
            {
                if (string.IsNullOrEmpty(humanType))
                {
                    sb.AppendFormat(@"<b>{0}</b>", Messages.Random);
                }
                else
                {
                    sb.AppendFormat(@"<b>{0}: {1} / {2}</b>", Messages.Random,
                                    Utilities.ResourceValue(humanType),
                                    Utilities.ResourceValue(videoType));
                }
            }


            sb.Append(@"<ul>");

            foreach (SongRecord v in this)
            {
                sb.Append(@"<li class=""vid_preview"">");


                string videoLink = VirtualPathUtility.ToAbsolute("~/video/YT#!") + v.ProviderKey;


                sb.Append(@"<div class=""image"">");

                sb.AppendFormat(@"<a href=""{0}"">", videoLink);

                sb.Append(@"<img longdesc=""" + v.SongDisplayNoLink + @"""  alt=""" + videoLink +
                          @""" class=""preview_thmb mini"" style=""width: 100px;height: 75px;"" src=""http://i3.ytimg.com/vi/" +
                          v.ProviderKey + @"/2.jpg""  />");
                sb.Append(@"</a>");

                sb.Append(@"<div class=""text"">");
                sb.Append(v.UserAccount);
                sb.Append(@"</div>");


                sb.Append(@"</div>");


                sb.Append(@"</li>");
            }

            sb.Append(@"</ul>");
            sb.Append(@"</div>");

            return sb.ToString();
        }


        public string VideoPlaylist()
        {
            if (Count == 0) return null;

            var sb = new StringBuilder(100);

            sb.Append("<ul>");

            int i = 1;

            foreach (SongRecord v in this)
            {
                sb.Append(@"<li class=""playlist_mod"">");
                sb.Append(@"<ul>");

                sb.Append(@"<li class=""playlist_vid"">");
                sb.Append(@"<div class=""image"">");


                sb.AppendFormat(
                    @"<a href=""{0}/{1}#!{2}"">", VirtualPathUtility.ToAbsolute("~/video"), v.ProviderCode,
                    v.ProviderKey);


                sb.AppendFormat(@"<img longdesc=""{0}"" alt=""{1}/{2}#!{3}"" class=""preview_thmb mini"" style=""width: 100px;height: 75px;"" 
   src=""http://i3.ytimg.com/vi/{3}/2.jpg""    />", v.SongDisplayNoLink, VirtualPathUtility.ToAbsolute("~/video"),
                                v.ProviderCode, v.ProviderKey);

                sb.Append(@"</a>");


                sb.Append(@"<div class=""text"">");
                sb.Append(v.UserAccount);
                sb.Append(@"</div>");

                sb.Append(@"</div>");


                sb.Append(@"</li>");

                sb.Append(@"<li class=""playlist_up_down_delete"">");

                sb.Append(@"<table>");
                sb.Append(@"<tr>");


                sb.Append(@"<td>");

                if (EnableChangeOrder)
                {
                    if (i > 1)
                    {
                        sb.AppendFormat(
                            @"<button title=""{1}"" name=""video_up_id"" class=""bring_up"" type=""submit"" value=""{0}"">{1}</button>",
                            v.VideoID.ToString(CultureInfo.InvariantCulture), Messages.BringUp);
                    }

                    if (i != Count)
                    {
                        sb.AppendFormat(
                            @"<button title=""{1}"" name=""video_down_id"" class=""bring_down"" type=""submit"" value=""{0}"">{1}</button>",
                            v.VideoID, Messages.BringDown);
                    }
                }

                sb.Append(@"</td>");

                sb.Append(@"<td>");

                sb.AppendFormat(
                    @"<button title=""{1}""
name=""video_delete_id"" class=""btn btn-danger"" type=""submit""
  value=""{0}"">{1}</button>", v.VideoID, Messages.RemoveFromList);

                sb.Append(@"</td>");

                sb.Append(@"</tr>");
                sb.Append(@"</table>");

                sb.Append(@"</li>");

                sb.Append(@"</ul>");
                sb.Append(@"</li>");

                i++;
            }

            sb.Append("</ul>");

            return sb.ToString();
        }


        public string VideosList()
        {
            if (Count == 0) return string.Empty;


            var sb = new StringBuilder(100);

            if (IncludeStateAndEndTag)
            {
                sb.Append("<ul>");
            }


            foreach (var v in this)
            {
                sb.Append(@"<li class=""vid_preview"">");
                sb.Append(@"<div class=""image"">");
                sb.Append(@"<a href=""/video/" + v.ProviderCode + @"#!" + v.ProviderKey + @""">");
                sb.Append(@"<img longdesc=""" + v.SongDisplayNoLink + @"""   alt=""/video/YT#!" + v.ProviderKey +
                          @""" class=""preview_thmb mini"" style=""width: 100px;height: 75px;"" src=""http://i3.ytimg.com/vi/" +
                          v.ProviderKey + @"/2.jpg""    />");
                sb.Append(@"</a>");

                sb.Append(@"<div class=""text"">");
                sb.Append(v.UserAccount);
                sb.Append(@"</div>");

                sb.Append(@"</div>");
                sb.Append(@"</li>");
            }


            if (IncludeStateAndEndTag)
            {
                sb.Append("</ul>");
            }

            return sb.ToString();
        }


        public string VideosPageList()
        {
            if (Count == 0) return string.Empty;


            var sb = new StringBuilder(100);

            if (IncludeStateAndEndTag)
            {
                sb.Append(@"<ul>");
            }


            foreach (var v in this)
            {
                sb.Append(@"<li class=""video_page_list_item"">");

                sb.AppendFormat(@"<a class=""m_over"" title=""{2}"" href=""{0}{1}"">
                                <img alt=""{2}"" src=""http://i3.ytimg.com/vi/{1}/0.jpg""    /></a>",
                                VirtualPathUtility.ToAbsolute("~/video/YT#!"), v.ProviderKey, v.SongDisplayNoLink2);


                sb.Append(@"</li>");
            }


            if (IncludeStateAndEndTag)
            {
                sb.Append("</ul>");
            }

            return sb.ToString();
        }
    }
}