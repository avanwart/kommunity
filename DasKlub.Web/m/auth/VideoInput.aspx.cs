﻿using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.ArtistContent;
using DasKlub.Lib.BOL.VideoContest;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using Google.GData.Client;
using Google.YouTube;
using HttpUtility = System.Web.HttpUtility;
using Utilities = DasKlub.Lib.Operational.Utilities;
using Video = DasKlub.Lib.BOL.Video;

namespace DasKlub.Web.m.auth
{
    public partial class VideoInput : Page
    {
        #region variables

        private const string unknownValue = "-UNKNOWN-";
        private const string selectText = "- SELECT -";

        private readonly string devkey = GeneralConfigs.YouTubeDevKey;
        private readonly string password = GeneralConfigs.YouTubeDevPass;
        private readonly string username = GeneralConfigs.YouTubeDevUser;
        private Songs artsngs;
        private Artist artst;
        private MultiProperty mp;

        ///
        private MultiProperties mps;

        private PropertyType propTyp;
        private Song sng;
        private Video vid;
        private string videoKey = string.Empty;

        private VideoRequest vidreq;

        #endregion

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["vidid"]) && !IsPostBack)
            {
                videoKey = Request.QueryString["vidid"];
                LoadVideo(videoKey);
            }


            if (!IsPostBack)
            {
                // RefreshLists();
                LoadGrid();

                LoadContests();
            }
        }

        private void LoadContests()
        {
            //
            var contests = new Contests();

            contests.GetAll();

            ddlContest.DataSource = contests;
            ddlContest.DataTextField = "name";
            ddlContest.DataValueField = "contestID";
            ddlContest.DataBind();

            ddlContest.Items.Add(new ListItem(unknownValue));

            ddlContest.SelectedValue = unknownValue;
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var allartsis = new Artists();
                allartsis.RemoveCache();

                if (gvwRequestedVideos.SelectedDataKey != null)
                {
                    vidreq = new VideoRequest(Convert.ToInt32(gvwRequestedVideos.SelectedDataKey.Value))
                    {
                        StatusType = 'A'
                    };
                    vidreq.Update();
                }

                vid = new Video("YT", txtVideoKey.Text)
                {
                    Duration = (float) Convert.ToDouble(txtDuration.Text),
                    Intro = (float) Convert.ToDouble(txtSecondsIn.Text),
                    LengthFromStart = (float) Convert.ToDouble(txtElasedEnd.Text),
                    ProviderCode = ddlVideoProvider.SelectedValue,
                    ProviderUserKey = txtUserName.Text,
                    VolumeLevel = Convert.ToInt32(ddlVolumeLevel.SelectedValue),
                    IsEnabled = chkEnabled.Checked,
                    EnableTrim = chkEnabled.Checked
                };

                // vid.IsHidden = chkHidden.Checked;

                /// publish date 
                var yousettings = new YouTubeRequestSettings("Das Klub", devkey);
                var yourequest = new YouTubeRequest(yousettings);
                var Url = new Uri("http://gdata.youtube.com/feeds/api/videos/" + vid.ProviderKey);
                var video = new Google.YouTube.Video();
                video = yourequest.Retrieve<Google.YouTube.Video>(Url);
                vid.PublishDate = video.YouTubeEntry.Published;

                if (vid.VideoID == 0)
                {
                    vid.Create();
                }
                else
                    vid.Update();

                // if there is a contest, add it now since there is an id
                if (ddlContest.SelectedValue != unknownValue)
                {
                    //TODO: check if it already is in the contest

                    ContestVideo.DeleteVideoFromAllContests(vid.VideoID);

                    var cv = new ContestVideo();

                    cv.ContestID = Convert.ToInt32(ddlContest.SelectedValue);
                    cv.VideoID = vid.VideoID;
                    cv.Create();
                }
                else
                {
                    // TODO: JUST REMOVE FROM CURRENT CONTEST, NOT ALL
                    ContestVideo.DeleteVideoFromAllContests(vid.VideoID);
                }


                // vid type
                if (!string.IsNullOrWhiteSpace(ddlVideoType.SelectedValue)
                    && ddlVideoType.SelectedValue != selectText)
                {
                    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
                    mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                    MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
                    mp.RemoveCache();
                    MultiPropertyVideo.AddMultiPropertyVideo(
                        Convert.ToInt32(
                            ddlVideoType.SelectedValue), vid.VideoID);
                }

                // human
                if (!string.IsNullOrWhiteSpace(ddlHumanType.SelectedValue)
                    && ddlHumanType.SelectedValue != selectText)
                {
                    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
                    mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                    MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
                    mp.RemoveCache();
                    MultiPropertyVideo.AddMultiPropertyVideo(
                        Convert.ToInt32(
                            ddlHumanType.SelectedValue), vid.VideoID);
                }


                // footage
                if (!string.IsNullOrWhiteSpace(ddlFootageType.SelectedValue)
                    && ddlFootageType.SelectedValue != selectText)
                {
                    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
                    mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                    MultiPropertyVideo.DeleteMultiPropertyVideo(mp.MultiPropertyID, vid.VideoID);
                    mp.RemoveCache();
                    MultiPropertyVideo.AddMultiPropertyVideo(
                        Convert.ToInt32(
                            ddlFootageType.SelectedValue), vid.VideoID);
                }


                VideoSong.DeleteSongsForVideo(vid.VideoID);

                // song 1

                artst = string.IsNullOrEmpty(txtArtist1.Text.Trim())
                    ? new Artist(ddlArtist1.SelectedValue)
                    : new Artist(txtArtist1.Text);

                if (artst.ArtistID == 0)
                {
                    artst.AltName = FromString.UrlKey(artst.Name);
                    artst.Create();
                }

                if (string.IsNullOrEmpty(txtArtistSong1.Text))
                {
                    sng = new Song(artst.ArtistID, ddlArtistSongs1.SelectedValue);
                }
                else
                {
                    sng = new Song(artst.ArtistID, txtArtistSong1.Text);
                }

                if (sng.SongID == 0)
                {
                    sng.SongKey = FromString.UrlKey(sng.Name);
                    sng.Create();
                }

                VideoSong.AddVideoSong(sng.SongID, vid.VideoID, 1);

                // song 2

                if ((ddlArtist2.SelectedValue != unknownValue && !string.IsNullOrEmpty(ddlArtist2.SelectedValue)) ||
                    !string.IsNullOrEmpty(txtArtist2.Text))
                {
                    artst = null;
                    sng = null;

                    if (string.IsNullOrEmpty(txtArtist2.Text.Trim()))
                    {
                        artst = new Artist(ddlArtist2.SelectedValue);
                    }
                    else
                    {
                        artst = new Artist(txtArtist2.Text);
                    }

                    if (artst.ArtistID == 0)
                    {
                        artst.AltName = FromString.UrlKey(artst.Name);
                        artst.Create();
                    }

                    if (string.IsNullOrEmpty(txtArtistSong2.Text))
                    {
                        sng = new Song(artst.ArtistID, ddlArtistSongs2.SelectedValue);
                    }
                    else
                    {
                        sng = new Song(artst.ArtistID, txtArtistSong2.Text);
                    }

                    if (sng.SongID == 0)
                    {
                        sng.SongKey = FromString.UrlKey(sng.Name);
                        sng.Create();
                    }

                    VideoSong.AddVideoSong(sng.SongID, vid.VideoID, 2);


                    if ((ddlArtist3.SelectedValue != unknownValue && !string.IsNullOrEmpty(ddlArtist3.SelectedValue)) ||
                        !string.IsNullOrEmpty(txtArtist3.Text))
                    {
                        // song 3

                        artst = null;
                        sng = null;

                        if (string.IsNullOrEmpty(txtArtist3.Text))
                        {
                            artst = new Artist(ddlArtist3.SelectedValue);
                        }
                        else
                        {
                            artst = new Artist(txtArtist3.Text);
                        }

                        if (artst.ArtistID == 0)
                        {
                            artst.AltName = FromString.UrlKey(artst.Name);
                            artst.Create();
                        }

                        if (string.IsNullOrEmpty(txtArtistSong3.Text))
                        {
                            sng = new Song(artst.ArtistID, ddlArtistSongs3.SelectedValue);
                        }
                        else
                        {
                            sng = new Song(artst.ArtistID, txtArtistSong3.Text);
                        }

                        if (sng.SongID == 0)
                        {
                            sng.SongKey = FromString.UrlKey(sng.Name);
                            sng.Create();
                        }

                        VideoSong.AddVideoSong(sng.SongID, vid.VideoID, 3);


                        if ((ddlArtist4.SelectedValue != unknownValue && !string.IsNullOrEmpty(ddlArtist4.SelectedValue)) ||
                            !string.IsNullOrEmpty(txtArtist4.Text))
                        {
                            // song 4

                            artst = null;
                            sng = null;

                            if (string.IsNullOrEmpty(txtArtist4.Text))
                            {
                                artst = new Artist(ddlArtist4.SelectedValue);
                            }
                            else
                            {
                                artst = new Artist(txtArtist4.Text);
                            }

                            if (artst.ArtistID == 0)
                            {
                                artst.AltName = FromString.UrlKey(artst.Name);
                                artst.Create();
                            }

                            if (string.IsNullOrEmpty(txtArtistSong4.Text))
                            {
                                sng = new Song(artst.ArtistID, ddlArtistSongs4.SelectedValue);
                            }
                            else
                            {
                                sng = new Song(artst.ArtistID, txtArtistSong4.Text);
                            }

                            if (sng.SongID == 0)
                            {
                                sng.SongKey = FromString.UrlKey(sng.Name);
                                sng.Create();
                            }

                            VideoSong.AddVideoSong(sng.SongID, vid.VideoID, 4);


                            if ((ddlArtist5.SelectedValue != unknownValue &&
                                 !string.IsNullOrEmpty(ddlArtist5.SelectedValue)) ||
                                !string.IsNullOrEmpty(txtArtist5.Text))
                            {
                                // song 5

                                artst = null;
                                sng = null;

                                if (string.IsNullOrEmpty(txtArtist5.Text))
                                {
                                    artst = new Artist(ddlArtist5.SelectedValue);
                                }
                                else
                                {
                                    artst = new Artist(txtArtist5.Text);
                                }

                                if (artst.ArtistID == 0)
                                {
                                    artst.AltName = FromString.UrlKey(artst.Name);
                                    artst.Create();
                                }

                                if (string.IsNullOrEmpty(txtArtistSong5.Text))
                                {
                                    sng = new Song(artst.ArtistID, ddlArtistSongs5.SelectedValue);
                                }
                                else
                                {
                                    sng = new Song(artst.ArtistID, txtArtistSong5.Text);
                                }

                                if (sng.SongID == 0)
                                {
                                    sng.SongKey = FromString.UrlKey(sng.Name);
                                    sng.Create();
                                }

                                VideoSong.AddVideoSong(sng.SongID, vid.VideoID, 5);


                                if ((ddlArtist6.SelectedValue != unknownValue &&
                                     !string.IsNullOrEmpty(ddlArtist6.SelectedValue)) ||
                                    !string.IsNullOrEmpty(txtArtist6.Text))
                                {
                                    // song 6

                                    artst = null;
                                    sng = null;

                                    if (string.IsNullOrEmpty(txtArtist6.Text))
                                    {
                                        artst = new Artist(ddlArtist6.SelectedValue);
                                    }
                                    else
                                    {
                                        artst = new Artist(txtArtist6.Text);
                                    }

                                    if (artst.ArtistID == 0)
                                    {
                                        artst.AltName = FromString.UrlKey(artst.Name);
                                        artst.Create();
                                    }

                                    if (string.IsNullOrEmpty(txtArtistSong6.Text))
                                    {
                                        sng = new Song(artst.ArtistID, ddlArtistSongs6.SelectedValue);
                                    }
                                    else
                                    {
                                        sng = new Song(artst.ArtistID, txtArtistSong6.Text);
                                    }

                                    if (sng.SongID == 0)
                                    {
                                        sng.SongKey = FromString.UrlKey(sng.Name);
                                        sng.Create();
                                    }

                                    VideoSong.AddVideoSong(sng.SongID, vid.VideoID, 6);
                                }
                            }
                        }
                    }
                }

                //  RefreshLists();

                lblStatus.Text = "OK";
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }

            LoadGrid();
        }


        protected void ddlArtist1_SelectedIndexChanged(object sender, EventArgs e)
        {
            artsngs = new Songs();
            artst = new Artist(ddlArtist1.SelectedValue);
            artsngs.GetSongsForArtist(artst.ArtistID);
            ddlArtistSongs1.DataSource = artsngs;
            ddlArtistSongs1.DataTextField = "name";
            ddlArtistSongs1.DataValueField = "name";
            ddlArtistSongs1.DataBind();
            ddlArtistSongs1.Items.Insert(0, new ListItem(unknownValue));
            Utilities.General.SortDropDownList(ddlArtistSongs1);
        }

        protected void ddlArtist2_SelectedIndexChanged(object sender, EventArgs e)
        {
            artsngs = new Songs();
            artst = new Artist(ddlArtist2.SelectedValue);
            artsngs.GetSongsForArtist(artst.ArtistID);
            ddlArtistSongs2.DataSource = artsngs;
            ddlArtistSongs2.DataTextField = "name";
            ddlArtistSongs2.DataValueField = "name";
            ddlArtistSongs2.DataBind();
            ddlArtistSongs2.Items.Insert(0, new ListItem(unknownValue));
            Utilities.General.SortDropDownList(ddlArtistSongs2);
        }

        protected void ddlArtist3_SelectedIndexChanged(object sender, EventArgs e)
        {
            artsngs = new Songs();
            artst = new Artist(ddlArtist3.SelectedValue);
            artsngs.GetSongsForArtist(artst.ArtistID);
            ddlArtistSongs3.DataSource = artsngs;
            ddlArtistSongs3.DataTextField = "name";
            ddlArtistSongs3.DataValueField = "name";
            ddlArtistSongs3.DataBind();
            ddlArtistSongs3.Items.Insert(0, new ListItem(unknownValue));
            Utilities.General.SortDropDownList(ddlArtistSongs3);
        }


        protected void ddlArtist4_SelectedIndexChanged(object sender, EventArgs e)
        {
            artsngs = new Songs();
            artst = new Artist(ddlArtist4.SelectedValue);
            artsngs.GetSongsForArtist(artst.ArtistID);
            ddlArtistSongs4.DataSource = artsngs;
            ddlArtistSongs4.DataTextField = "name";
            ddlArtistSongs4.DataValueField = "name";
            ddlArtistSongs4.DataBind();
            ddlArtistSongs4.Items.Insert(0, new ListItem(unknownValue));
            Utilities.General.SortDropDownList(ddlArtistSongs4);
        }


        protected void ddlArtist5_SelectedIndexChanged(object sender, EventArgs e)
        {
            artsngs = new Songs();
            artst = new Artist(ddlArtist5.SelectedValue);
            artsngs.GetSongsForArtist(artst.ArtistID);
            ddlArtistSongs5.DataSource = artsngs;
            ddlArtistSongs5.DataTextField = "name";
            ddlArtistSongs5.DataValueField = "name";
            ddlArtistSongs5.DataBind();
            ddlArtistSongs5.Items.Insert(0, new ListItem(unknownValue));
            Utilities.General.SortDropDownList(ddlArtistSongs5);
        }


        protected void ddlArtist6_SelectedIndexChanged(object sender, EventArgs e)
        {
            artsngs = new Songs();
            artst = new Artist(ddlArtist5.SelectedValue);
            artsngs.GetSongsForArtist(artst.ArtistID);
            ddlArtistSongs6.DataSource = artsngs;
            ddlArtistSongs6.DataTextField = "name";
            ddlArtistSongs6.DataValueField = "name";
            ddlArtistSongs6.DataBind();
            ddlArtistSongs6.Items.Insert(0, new ListItem(unknownValue));
            Utilities.General.SortDropDownList(ddlArtistSongs6);
        }

        protected void btnFetchURL_Click(object sender, EventArgs e)
        {
            LoadVideoFromURL(txtURL.Text);
        }

        protected void gvwRequestedVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = Convert.ToInt32(gvwRequestedVideos.SelectedDataKey.Value);

            if (selected > 0)
            {
                btnReject.Enabled = true;
                hfVideoRequestID.Value = selected.ToString();
                vidreq = new VideoRequest(selected);
                txtURL.Text = vidreq.RequestURL;
                LoadVideoFromURL(vidreq.RequestURL);
            }
        }

        #endregion

        #region methods

        private void LoadVideoFromURL(string url)
        {
            if (url.Contains("http://youtu.be/"))
            {
                LoadVideo(url.Replace("http://youtu.be/", string.Empty));
            }
            else
            {
                var ul = new Uri(url.Trim());

                NameValueCollection nvc = HttpUtility.ParseQueryString(ul.Query);

                LoadVideo(nvc["v"]);
            }
        }

        private void LoadGrid()
        {
            // from reqeursts 
            // throw new NotImplementedException();
            var vreqs = new VideoRequests();
            vreqs.GetUnprocessedVideos();

            gvwRequestedVideos.DataSource = vreqs;
            gvwRequestedVideos.DataBind();
            gvwRequestedVideos.SelectedIndex = -1;
        }


        private void ClearInput()
        {
            txtAlbumName1.Text = string.Empty;

            txtArtist1.Text = string.Empty;
            txtArtist2.Text = string.Empty;

            txtArtistSong1.Text = string.Empty;
            txtArtistSong2.Text = string.Empty;
        }

        private void LoadVideo(string videoKey)
        {
            ClearInput();

            try
            {
                vid = new Video("YT", videoKey);

                litVideo.Text =
                    string.Format(
                        @"<iframe width=""425"" height=""349"" src=""http://www.youtube.com/embed/{0}"" frameborder=""0"" allowfullscreen></iframe>",
                        vid.ProviderKey);

                txtSecondsIn.Text = vid.Intro.ToString();
                txtElasedEnd.Text = vid.LengthFromStart.ToString();
                ddlVideoProvider.SelectedValue = vid.ProviderCode;
                chkEnabled.Checked = vid.IsEnabled;
                ddlVolumeLevel.SelectedValue = vid.VolumeLevel.ToString();
                lblVideoID.Text = vid.VideoID.ToString();


                if (vid.VolumeLevel == 0)
                {
                    ddlVolumeLevel.SelectedValue = "5";
                    chkEnabled.Checked = true;
                }


                var video = new Google.YouTube.Video();

                try
                {
                    var yousettings = new YouTubeRequestSettings("Das Klub", devkey);

                    var yourequest = new YouTubeRequest(yousettings);
                    var url = new Uri("http://gdata.youtube.com/feeds/api/videos/" + videoKey);

                    video = yourequest.Retrieve<Google.YouTube.Video>(url);
                    txtDuration.Text = video.YouTubeEntry.Duration.Seconds;
                }
                catch (GDataRequestException)
                {
                    vid.IsEnabled = false;
                    vid.Update();
                    litVideo.Text = string.Empty;
                    return;
                }

                if (vid.LengthFromStart == 0)
                {
                    txtElasedEnd.Text = video.YouTubeEntry.Duration.Seconds;
                }

                if (vid.LengthFromStart == 0)
                {
                    txtDuration.Text = video.YouTubeEntry.Duration.Seconds;
                }

                txtUserName.Text = video.Uploader;
                lblVideoID.Text = vid.VideoID.ToString();
                txtVideoKey.Text = video.VideoId;

                // vid type
                propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
                mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                mps = new MultiProperties(propTyp.PropertyTypeID);
                mps.Sort(delegate(MultiProperty p1, MultiProperty p2) { return p1.Name.CompareTo(p2.Name); });
                ddlVideoType.DataSource = mps;
                ddlVideoType.DataTextField = "name";
                ddlVideoType.DataValueField = "multiPropertyID";
                ddlVideoType.DataBind();
                ddlVideoType.Items.Insert(0, new ListItem(selectText));
                if (mp.MultiPropertyID != 0)
                    ddlVideoType.SelectedValue = mp.MultiPropertyID.ToString();

                // human
                propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
                mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                mps = new MultiProperties(propTyp.PropertyTypeID);
                mps.Sort(delegate(MultiProperty p1, MultiProperty p2) { return p1.Name.CompareTo(p2.Name); });
                ddlHumanType.DataSource = mps;
                ddlHumanType.DataTextField = "name";
                ddlHumanType.DataValueField = "multiPropertyID";
                ddlHumanType.DataBind();
                ddlHumanType.Items.Insert(0, new ListItem(selectText));
                if (mp.MultiPropertyID != 0)
                    ddlHumanType.SelectedValue = mp.MultiPropertyID.ToString();

                // footage
                propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
                mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
                mps = new MultiProperties(propTyp.PropertyTypeID);
                mps.Sort(delegate(MultiProperty p1, MultiProperty p2) { return p1.Name.CompareTo(p2.Name); });
                ddlFootageType.DataSource = mps;
                ddlFootageType.DataTextField = "name";
                ddlFootageType.DataValueField = "multiPropertyID";
                ddlFootageType.DataBind();
                ddlFootageType.Items.Insert(0, new ListItem(selectText));
                if (mp.MultiPropertyID != 0)
                    ddlFootageType.SelectedValue = mp.MultiPropertyID.ToString();


                // contest

                var vidInContest = new ContestVideo();

                vidInContest.GetContestVideo(vid.VideoID);

                if (vidInContest.ContestVideoID != 0)
                {
                    ddlContest.SelectedValue = vidInContest.ContestID.ToString();
                }
                else
                    ddlContest.SelectedValue = unknownValue;

                var sngs = new Songs();
                artsngs = new Songs();
                sngs.GetSongsForVideo(vid.VideoID);
                Artist art = null;

                var arts = new Artists();
                arts.GetAll();

                // artists 1
                ddlArtist1.DataSource = arts;
                ddlArtist1.DataTextField = "name";
                ddlArtist1.DataValueField = "name";
                ddlArtist1.DataBind();
                Utilities.General.SortDropDownList(ddlArtist1);
                ddlArtist1.Items.Insert(0, new ListItem(unknownValue));

                // artists 2
                ddlArtist2.DataSource = arts;
                ddlArtist2.DataTextField = "name";
                ddlArtist2.DataValueField = "name";
                ddlArtist2.DataBind();
                Utilities.General.SortDropDownList(ddlArtist2);
                ddlArtist2.Items.Insert(0, new ListItem(unknownValue));

                // artists 3
                ddlArtist3.DataSource = arts;
                ddlArtist3.DataTextField = "name";
                ddlArtist3.DataValueField = "name";
                ddlArtist3.DataBind();
                Utilities.General.SortDropDownList(ddlArtist3);
                ddlArtist3.Items.Insert(0, new ListItem(unknownValue));

                // artists 4
                ddlArtist4.DataSource = arts;
                ddlArtist4.DataTextField = "name";
                ddlArtist4.DataValueField = "name";
                ddlArtist4.DataBind();
                Utilities.General.SortDropDownList(ddlArtist4);
                ddlArtist4.Items.Insert(0, new ListItem(unknownValue));

                // artists 5
                ddlArtist5.DataSource = arts;
                ddlArtist5.DataTextField = "name";
                ddlArtist5.DataValueField = "name";
                ddlArtist5.DataBind();
                Utilities.General.SortDropDownList(ddlArtist5);
                ddlArtist5.Items.Insert(0, new ListItem(unknownValue));


                // artists 6
                ddlArtist6.DataSource = arts;
                ddlArtist6.DataTextField = "name";
                ddlArtist6.DataValueField = "name";
                ddlArtist6.DataBind();
                Utilities.General.SortDropDownList(ddlArtist6);
                ddlArtist6.Items.Insert(0, new ListItem(unknownValue));


                foreach (Song sng in sngs)
                {
                    if (sng.Name == unknownValue || string.IsNullOrEmpty(sng.Name)) continue;

                    // sngrcd.SongDisplay += art.Name + " - " + sng.Name + " " ;

                    if (sng.RankOrder == 0 || sng.RankOrder == 1)
                    {
                        // song 1
                        art = new Artist(sng.ArtistID);
                        ddlArtist1.SelectedValue = art.Name;

                        artsngs = new Songs();
                        artsngs.GetSongsForArtist(art.ArtistID);
                        ddlArtistSongs1.DataSource = artsngs;
                        ddlArtistSongs1.DataTextField = "name";
                        ddlArtistSongs1.DataValueField = "name";
                        ddlArtistSongs1.DataBind();
                        ddlArtistSongs1.Items.Insert(0, new ListItem(unknownValue));
                        Utilities.General.SortDropDownList(ddlArtistSongs1);

                        ddlArtistSongs1.SelectedValue = sng.Name;
                    }
                    else if (sng.RankOrder == 2)
                    {
                        // song 2
                        art = new Artist(sng.ArtistID);
                        ddlArtist2.SelectedValue = art.Name;

                        artsngs = new Songs();
                        artsngs.GetSongsForArtist(art.ArtistID);
                        ddlArtistSongs2.DataSource = artsngs;
                        ddlArtistSongs2.DataTextField = "name";
                        ddlArtistSongs2.DataValueField = "name";
                        ddlArtistSongs2.DataBind();
                        ddlArtistSongs2.Items.Insert(0, new ListItem(unknownValue));
                        Utilities.General.SortDropDownList(ddlArtistSongs2);

                        ddlArtistSongs2.SelectedValue = sng.Name;
                    }
                    else if (sng.RankOrder == 3)
                    {
                        // song 3
                        art = new Artist(sng.ArtistID);
                        ddlArtist3.SelectedValue = art.Name;

                        artsngs = new Songs();
                        artsngs.GetSongsForArtist(art.ArtistID);
                        ddlArtistSongs3.DataSource = artsngs;
                        ddlArtistSongs3.DataTextField = "name";
                        ddlArtistSongs3.DataValueField = "name";
                        ddlArtistSongs3.DataBind();
                        ddlArtistSongs3.Items.Insert(0, new ListItem(unknownValue));
                        Utilities.General.SortDropDownList(ddlArtistSongs3);

                        ddlArtistSongs3.SelectedValue = sng.Name;
                    }
                    else if (sng.RankOrder == 4)
                    {
                        // song 4
                        art = new Artist(sng.ArtistID);
                        ddlArtist4.SelectedValue = art.Name;

                        artsngs = new Songs();
                        artsngs.GetSongsForArtist(art.ArtistID);
                        ddlArtistSongs4.DataSource = artsngs;
                        ddlArtistSongs4.DataTextField = "name";
                        ddlArtistSongs4.DataValueField = "name";
                        ddlArtistSongs4.DataBind();
                        ddlArtistSongs4.Items.Insert(0, new ListItem(unknownValue));
                        Utilities.General.SortDropDownList(ddlArtistSongs4);

                        ddlArtistSongs4.SelectedValue = sng.Name;
                    }
                    else if (sng.RankOrder == 5)
                    {
                        // song 5
                        art = new Artist(sng.ArtistID);
                        ddlArtist5.SelectedValue = art.Name;

                        artsngs = new Songs();
                        artsngs.GetSongsForArtist(art.ArtistID);
                        ddlArtistSongs5.DataSource = artsngs;
                        ddlArtistSongs5.DataTextField = "name";
                        ddlArtistSongs5.DataValueField = "name";
                        ddlArtistSongs5.DataBind();
                        ddlArtistSongs5.Items.Insert(0, new ListItem(unknownValue));
                        Utilities.General.SortDropDownList(ddlArtistSongs5);

                        ddlArtistSongs5.SelectedValue = sng.Name;
                    }
                    else if (sng.RankOrder == 6)
                    {
                        // song 6
                        art = new Artist(sng.ArtistID);
                        ddlArtist6.SelectedValue = art.Name;

                        artsngs = new Songs();
                        artsngs.GetSongsForArtist(art.ArtistID);
                        ddlArtistSongs6.DataSource = artsngs;
                        ddlArtistSongs6.DataTextField = "name";
                        ddlArtistSongs6.DataValueField = "name";
                        ddlArtistSongs6.DataBind();
                        ddlArtistSongs6.Items.Insert(0, new ListItem(unknownValue));
                        Utilities.General.SortDropDownList(ddlArtistSongs6);

                        ddlArtistSongs6.SelectedValue = sng.Name;
                    }
                }


                lblStatus.Text = "OK";
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        #endregion

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfVideoRequestID.Value))
            {
                vidreq = new VideoRequest(Convert.ToInt32(hfVideoRequestID.Value));
                vidreq.StatusType = 'R';
                vidreq.Update();
                lblStatus.Text = "Video rejected";

                LoadGrid();
            }
        }
    }
}