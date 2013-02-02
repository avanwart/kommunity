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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BootBaronLib.AppSpec.DasKlub.BOL;
using System.Web;
using System.Xml;
using BootBaronLib.Enums;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Resources;
using System;

//http://code.google.com/web/ajaxcrawling/docs/getting-started.html

namespace BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class SongRecord : IJSONResponse
    {
        #region properties



        private bool _getRelatedVideos = true;

        public bool GetRelatedVideos
        {
            get { return _getRelatedVideos; }
            set { _getRelatedVideos = value; }
        }




        public string UserAccountURL
        {
            get
            {
                return string.Format(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(string.Format("~/{0}", this.UserAccount), this.UserAccount));

            }
        }

        private string _providerKey = string.Empty;

        public string ProviderKey
        {
            get { return _providerKey; }
            set { _providerKey = value; }
        }

        private string _providerCode = string.Empty;

        public string ProviderCode
        {
            get { return _providerCode; }
            set { _providerCode = value; }
        }

        private float _totalSeconds = 0;

        public float TotalSeconds
        {
            get { return _totalSeconds; }
            set { _totalSeconds = value; }
        }

        private float _startTime = 0;

        public float StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private float _endTime = 0;

        public float EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }


        private string _songDisplay = string.Empty;

        public string SongDisplay
        {
            get { return _songDisplay; }
            set { _songDisplay = value; }
        }

        private string _human = string.Empty;

        public string Human
        {
            get { return _human; }
            set { _human = value; }
        }

        private int _volumeLevel = 0;

        public int VolumeLevel
        {
            get { return _volumeLevel; }
            set { _volumeLevel = value; }
        }


        private string _userAccount = string.Empty;

        public string UserAccount
        {
            get { return _userAccount; }
            set { _userAccount = value; }
        }


        private int _videoID = 0;

        public int VideoID
        {
            get { return _videoID; }
            set { _videoID = value; }
        }


        public ArrayList songs = null;

        #endregion

        #region constructor

        public SongRecord() { }

        public SongRecord(Video vid)
        {

            this.VideoID = vid.VideoID;
            this.UserAccount = vid.ProviderUserKey;
            this.VolumeLevel = vid.VolumeLevel;
            this.EndTime = vid.LengthFromStart;
            this.StartTime = vid.Intro;
            this.TotalSeconds = vid.Duration;
            PropertyType pt = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            MultiProperty mp = new MultiProperty(vid.VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            this.Human = mp.Name;
            this.ProviderCode = vid.ProviderCode;
            this.ProviderKey = vid.ProviderKey;

            Songs sngs = new Songs();
            sngs.GetSongsForVideo(vid.VideoID);

            Artist art = null;

            if (sngs.Count > 1)
            {
                // sort oldest 1st 
                sngs.Sort(delegate(Song p1, Song p2)
                {
                    return p1.RankOrder.CompareTo(p2.RankOrder);
                });

                int cnt = 1;

                songs = new ArrayList();

                foreach (Song sng in sngs)
                {
                    songs.Add(sng.SongID);

                    art = new Artist(sng.ArtistID);
                    this.SongDisplay += HttpUtility.HtmlEncode(
                        string.Format(@"{0} : <a href=""{1}/{2}"">{3}</a> - {4}<br />",
                        cnt.ToString(), 
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

                this.SongDisplay +=
                    HttpUtility.HtmlEncode(
                    string.Format(@"<a href=""{0}/{1}"">{2}</a> - {3}",
                    Utilities.URLAuthority(), art.AltName,   art.Name,  sngs[0].Name));

            }


        }


        #endregion

        private string _songDisplayNoLink = string.Empty;

        public string SongDisplayNoLink
        {
            set { _songDisplayNoLink = value; }

            get
            {
                Video vid = new Video(this.VideoID);

                Songs sngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);

                Artist art = null;

                if (sngs.Count > 1)
                {
                    // sort oldest 1st 
                    sngs.Sort(delegate(Song p1, Song p2)
                    {
                        return p1.RankOrder.CompareTo(p2.RankOrder);
                    });

                    int cnt = 1;

                    songs = new ArrayList();

                    foreach (Song sng in sngs)
                    {
                        songs.Add(sng.SongID);

                        art = new Artist(sng.ArtistID);
                        this._songDisplayNoLink += HttpUtility.HtmlEncode(
                            string.Format(@"{0}: <b>{1}</b> - {2} <br />", cnt.ToString(),  art.Name , sng.Name ));
                        cnt++;
                    }
                }
                else if (sngs.Count == 1)
                {
                    art = new Artist(sngs[0].ArtistID);
                    //this.SongDisplay += art.Name + " - " + sngs[0].Name;// +"<br />";// +HttpUtility.HtmlEncode(" | ");
                    this._songDisplayNoLink += HttpUtility.HtmlEncode(string.Format(@"<b>{0}</b> - {1}", art.Name, sngs[0].Name));
                }
                return _songDisplayNoLink;
            }
        }

        private string _songDisplayNoLink2 = string.Empty;

        public string SongDisplayNoLink2
        {
            set { _songDisplayNoLink2 = value; }

            get
            {
                Video vid = new Video(this.VideoID);

                Songs sngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);

                Artist art = null;

                if (sngs.Count > 1)
                {
                    // sort oldest 1st 
                    sngs.Sort(delegate(Song p1, Song p2)
                    {
                        return p1.RankOrder.CompareTo(p2.RankOrder);
                    });

                    int cnt = 1;

                    songs = new ArrayList();

                    foreach (Song sng in sngs)
                    {
                        songs.Add(sng.SongID);

                        art = new Artist(sng.ArtistID);
                        this._songDisplayNoLink2 += HttpUtility.HtmlEncode(
                            cnt.ToString() + ": " + art.Name + " - " + sng.Name);
                        cnt++;
                    }
                }
                else if (sngs.Count == 1)
                {
                    art = new Artist(sngs[0].ArtistID);
                    //this.SongDisplay += art.Name + " - " + sngs[0].Name;// +"<br />";// +HttpUtility.HtmlEncode(" | ");
                    this._songDisplayNoLink2 += HttpUtility.HtmlEncode(art.Name + " - " + sngs[0].Name);
                }
                return _songDisplayNoLink2;
            }
        }

        public string SongDisplayNoLink3
        {
            set { _songDisplayNoLink = value; }

            get
            {
                Video vid = new Video(this.VideoID);

                Songs sngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);

                Artist art = null;

                if (sngs.Count > 1)
                {
                    // sort oldest 1st 
                    sngs.Sort(delegate(Song p1, Song p2)
                    {
                        return p1.RankOrder.CompareTo(p2.RankOrder);
                    });

                    int cnt = 1;

                    songs = new ArrayList();

                    foreach (Song sng in sngs)
                    {
                        songs.Add(sng.SongID);

                        art = new Artist(sng.ArtistID);
                        //this._songDisplayNoLink += HttpUtility.HtmlEncode(
                        this._songDisplayNoLink +=
                            string.Format("{0}: {1} - {2} <br />",
                        cnt.ToString(),art.Name , sng.Name);
                        cnt++;
                    }
                }
                else if (sngs.Count == 1)
                {
                    art = new Artist(sngs[0].ArtistID);
                    this._songDisplayNoLink += string.Format( @"<a href=""{0}"">{1}</a> - {2}",art.UrlTo , art.Name,sngs[0].Name);
                }
                return _songDisplayNoLink;
            }
        }

        public string JSONResponse
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);


                sb.Append(@"{""EndTime"": """);
                sb.Append(this.EndTime.ToString());
                sb.Append(@""", 
            ""Human"": """);
                sb.Append(this.Human);
                sb.Append(@""", 
            ""ProviderCode"": """);
                sb.Append(this.ProviderCode);
                sb.Append(@""", 
            ""ProviderKey"": """);
                sb.Append(this.ProviderKey);
                sb.Append(@""", 
            ""SongDisplay"": """);
                sb.Append(this.SongDisplay);
                sb.Append(@""", 
            ""StartTime"": """);
                sb.Append(this.StartTime.ToString());
                sb.Append(@""", 
            ""TotalSeconds"": """);
                sb.Append(this.TotalSeconds.ToString());
                sb.Append(@""", 
            ""UserAccount"": """);
                sb.Append(this.UserAccount);
                sb.Append(@""", 
            ""RelatedVids"": """);
                sb.Append(((this.GetRelatedVideos) ? Video.GetRelatedVideosList(VideoID) : string.Empty));
                sb.Append(@""", 
            ""ITunesLink"": """);
                sb.Append(SongProperty.GetPropertyTypeLink(VideoID));
                sb.Append(@""", 
            ""VolumeLevel"": """);
                sb.Append(this.VolumeLevel.ToString());
                sb.Append(@"""}");

                return sb.ToString();

            }
        }
    }

    public class SongRecords : List<SongRecord>
    {
        private Videos _vids = null;

        public Videos Vids
        {
            get { return _vids; }
            set { _vids = value; }
        }




        public XmlDocument JSONoptions()
        {

            // vb.GetAll();

            SongRecord sngrcd = null;
            PropertyType pt = null;
            MultiProperty mp = null;
            Songs sngs = null;
            Artist art = null;

            Vids = new Videos();

            Vids.GetAll();

            Videos theVids = new Videos();

            foreach (Video v1 in Vids)
            {
                if (v1.VideoType.ToLower() == "dance")
                {
                    theVids.Add(v1);
                }
            }

            Vids = null;
            Vids = theVids;


            // sort oldest 1st 
            Vids.Sort(delegate(Video p1, Video p2)
            {
                return p2.CreateDate.CompareTo(p1.CreateDate);
            });


            foreach (Video vid in Vids)
            {
                if (vid.IsHidden) continue;

                sngrcd = new SongRecord();

                sngrcd.VolumeLevel = vid.VolumeLevel;
                sngrcd.EndTime = vid.LengthFromStart;
                sngrcd.StartTime = vid.Intro;
                sngrcd.TotalSeconds = vid.Duration;
                pt = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
                mp = new MultiProperty(vid.VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                sngrcd.Human = mp.PropertyContent;
                sngrcd.ProviderCode = vid.ProviderCode;
                sngrcd.ProviderKey = vid.ProviderKey;

                sngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);

                sngs.Sort(delegate(Song p1, Song p2)
                {
                    return p2.RankOrder.CompareTo(p1.RankOrder);
                });

                if (sngs.Count > 1)
                {
                    art = new Artist(sngs[0].ArtistID);
                    sngrcd.SongDisplay += art.Name + " - " + sngs[0].Name;// +HttpUtility.HtmlEncode(" | ");
                }
                else
                {
                    int cnt = 1;

                    foreach (Song sng in sngs)
                    {
                        art = new Artist(sng.ArtistID);
                        sngrcd.SongDisplay += HttpUtility.HtmlEncode(cnt.ToString() + ": " + art.Name + " - " + sng.Name + "<br />");
                    }
                }

                if (sngs.Count > 0 && !string.IsNullOrEmpty(vid.ProviderKey))
                    this.Add(sngrcd);
            }


            return FromObj.ConvertObjectToXML(this, true);





        }


        public XmlDocument JSONVideo(string videoKey)
        {
            //       Videos vids = new Videos();

            //     vids.GetAll();

            SongRecord sngrcd = null;
            PropertyType pt = null;
            MultiProperty mp = null;
            Songs sngs = null;
            Artist art = null;


            //// sort oldest 1st 
            //vids.Sort(delegate(Video p1, Video p2)
            //{
            //    return p2.CreateDate.CompareTo(p1.CreateDate);
            //});

            Video vid = new Video("YT", videoKey.Substring(3, videoKey.Length - 3));

            //foreach (Video vid in vids)
            //{
            if (vid.IsHidden) return null;

            sngrcd = new SongRecord();

            sngrcd.VolumeLevel = vid.VolumeLevel;
            sngrcd.EndTime = vid.LengthFromStart;
            sngrcd.StartTime = vid.Intro;
            sngrcd.TotalSeconds = vid.Duration;
            pt = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            mp = new MultiProperty(vid.VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            sngrcd.Human = mp.PropertyContent;
            sngrcd.ProviderCode = vid.ProviderCode;
            sngrcd.ProviderKey = vid.ProviderKey;

            sngs = new Songs();
            sngs.GetSongsForVideo(vid.VideoID);

            foreach (Song sng in sngs)
            {
                art = new Artist(sng.ArtistID);
                sngrcd.SongDisplay += art.Name + " - " + sng.Name + HttpUtility.HtmlEncode(" | ");
            }

            if (sngs.Count > 0 && !string.IsNullOrEmpty(vid.ProviderKey))
                this.Add(sngrcd);


            return FromObj.ConvertObjectToXML(this, true);





        }



        public void GetMostRecentVideos()
        {
            Videos vids = new Videos();

            vids.GetAll();

            SongRecord sngrcd = null;

            // sort oldest 1st 
            vids.Sort(delegate(Video p1, Video p2)
            {
                return p2.CreateDate.CompareTo(p1.CreateDate);
            });

            int totalCount = 1;

            foreach (Video vid in vids)
            {
                if (vid.IsHidden) continue;

                sngrcd = new SongRecord(vid);

                if (!string.IsNullOrEmpty(sngrcd.SongDisplay) && !string.IsNullOrEmpty(sngrcd.ProviderKey))
                    this.Add(sngrcd);

                if (totalCount == 5) break;
                else totalCount++;
            }
        }

        private bool _isUserSelected = false;

        public bool IsUserSelected
        {
            get { return _isUserSelected; }
            set { _isUserSelected = value; }
        }



        public string ListOfVideos(string humanType, string videoType)
        {
            if (this.Count == 0) return string.Empty;


            StringBuilder sb = new StringBuilder();

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

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent.SongRecord v in this)
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


        private bool _enableChangeOrder = true;

        public bool EnableChangeOrder
        {
            get { return _enableChangeOrder; }
            set { _enableChangeOrder = value; }
        }


        public string VideoPlaylist()
        {
            if (Count == 0) return null;

            StringBuilder sb = new StringBuilder(100);

            sb.Append("<ul>");

            int i = 1;

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent.SongRecord v in this)
            {

                sb.Append(@"<li class=""playlist_mod"">");
                sb.Append(@"<ul>");

                sb.Append(@"<li class=""playlist_vid"">");
                sb.Append(@"<div class=""image"">");


                sb.AppendFormat(
@"<a href=""{0}/{1}#!{1}"">", VirtualPathUtility.ToAbsolute("~/video"), v.ProviderCode, v.ProviderKey);




                sb.AppendFormat(@"<img longdesc=""{0}"" alt=""{1}/{2}#!"" class=""preview_thmb mini"" style=""width: 100px;height: 75px;"" 
   src=""http://i3.ytimg.com/vi/{2}/2.jpg""    />", v.SongDisplayNoLink, VirtualPathUtility.ToAbsolute("~/video"), v.ProviderKey, v.ProviderCode);

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
                   @"<button title=""{1}"" name=""video_up_id"" class=""bring_up"" type=""submit"" value=""{0}"">{1}</button>", v.VideoID.ToString(), Messages.BringUp);

                    }

                    if (i != this.Count)
                    {
                        sb.AppendFormat(@"<button title=""{1}"" name=""video_down_id"" class=""bring_down"" type=""submit"" value=""{0}"">{1}</button>", v.VideoID.ToString(), Messages.BringDown);
                    }

                }

                sb.Append(@"</td>");

                sb.Append(@"<td>");

                sb.AppendFormat(
 @"<button title=""{1}""
name=""video_delete_id"" class=""btn btn-danger"" type=""submit""
  value=""{0}"">{1}</button>", v.VideoID.ToString(), Messages.RemoveFromList);

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


        private bool _includeStateAndEndTag = true;

        public bool IncludeStateAndEndTag
        {
            get { return _includeStateAndEndTag; }
            set { _includeStateAndEndTag = value; }
        }

        public string VideosList()
        {
            if (this.Count == 0) return string.Empty;


            StringBuilder sb = new StringBuilder(100);

            if (IncludeStateAndEndTag)
            {
                sb.Append("<ul>");
            }



            foreach (BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent.SongRecord v in this)
            {
                sb.Append(@"<li class=""vid_preview"">");
                sb.Append(@"<div class=""image"">");
                sb.Append(@"<a href=""/video/" + v.ProviderCode + @"#!" + v.ProviderKey + @""">");
                sb.Append(@"<img longdesc=""" + v.SongDisplayNoLink + @"""   alt=""/video/YT#!" + v.ProviderKey +
                    @""" class=""preview_thmb mini"" style=""width: 100px;height: 75px;"" src=""http://i3.ytimg.com/vi/" + v.ProviderKey + @"/2.jpg""    />");
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
            if (this.Count == 0) return string.Empty;


            StringBuilder sb = new StringBuilder(100);

            if (IncludeStateAndEndTag)
            {
                sb.Append(@"<ul>");
            }

            Video vid = null;
         

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent.SongRecord v in this)
            {
                vid = new Video(v.VideoID);

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
