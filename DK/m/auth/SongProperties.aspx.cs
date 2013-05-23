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
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.Operational;

namespace DasKlub.Web.Web.m.auth
{
    public partial class SongProperties : Page
    {
        private const string unknownValue = "-UNKNOWN-";
        private Songs artsngs;
        private Artist artst;
        private Song sng;
        private string videoKey = string.Empty;

        ///
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var arts = new Artists();
                arts.GetAll();

                // artists 1
                ddlArtist1.DataSource = arts;
                ddlArtist1.DataTextField = "name";
                ddlArtist1.DataValueField = "name";
                ddlArtist1.DataBind();
                ddlArtist1.Items.Insert(0, new ListItem(unknownValue));
                Utilities.General.SortDropDownList(ddlArtist1);
            }


            artst = new Artist(ddlArtist1.SelectedValue);
            sng = new Song(artst.ArtistID, ddlArtistSongs1.SelectedValue);
        }

        //private void RefreshLists()
        //{
        //    Artists arts = new Artists();
        //    arts.GetAll();

        //    ddlArtist1.DataSource = arts;
        //    ddlArtist1.DataTextField = "name";
        //    ddlArtist1.DataValueField = "name";
        //    ddlArtist1.DataBind();
        //    General.SortDropDownList(ddlArtist1);

        //    //
        //    ddlArtist2.DataSource = arts;
        //    ddlArtist2.DataTextField = "name";
        //    ddlArtist2.DataValueField = "name";
        //    ddlArtist2.DataBind();
        //    General.SortDropDownList(ddlArtist2);

        //    //
        //    ddlArtist3.DataSource = arts;
        //    ddlArtist3.DataTextField = "name";
        //    ddlArtist3.DataValueField = "name";
        //    ddlArtist3.DataBind();
        //    General.SortDropDownList(ddlArtist3);

        //    Statuses stus = new Statuses();

        //    stus.GetAll();

        //    ddlVideoStatus.DataSource = stus;
        //    ddlVideoStatus.DataTextField = "statusDescription";
        //    ddlVideoStatus.DataValueField = "statusID";
        //    ddlVideoStatus.DataBind();


        //    ///
        //    PropertyType propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
        //    MultiProperties mps = new MultiProperties(propTyp.PropertyTypeID);

        //    ddlHumanType.DataSource = mps;
        //    ddlHumanType.DataTextField = "name";
        //    ddlHumanType.DataValueField = "multiPropertyID";
        //    ddlHumanType.DataBind();

        //    ///

        //    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.FOOTG);
        //    mps = new MultiProperties(propTyp.PropertyTypeID);

        //    ddlFootageType.DataSource = mps;
        //    ddlFootageType.DataTextField = "name";
        //    ddlFootageType.DataValueField = "multiPropertyID";
        //    ddlFootageType.DataBind();
        //    ///
        //    propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
        //    mps = new MultiProperties(propTyp.PropertyTypeID);


        //    ddlVideoType.DataSource = mps;
        //    ddlVideoType.DataTextField = "name";
        //    ddlVideoType.DataValueField = "multiPropertyID";
        //    ddlVideoType.DataBind();

        //}


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    HttpContext.Current.Cache.Remove(enumerator.Key.ToString());
                }


                //  RefreshLists();

                lblStatus.Text = "OK";
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }


            // amazon
            var sp1 = new SongProperty();

            sp1.SongID = sng.SongID;
            sp1.PropertyContent = txtAmazonLink.Text;
            sp1.PropertyType = SongProperty.SPropType.AM.ToString();

            if (!string.IsNullOrEmpty(sp1.PropertyContent))
            {
                sp1.Create();
            }

            // itunes
            sp1 = new SongProperty();

            sp1.SongID = sng.SongID;
            sp1.PropertyContent = txtiTunesLink.Text;
            sp1.PropertyType = SongProperty.SPropType.IT.ToString();

            if (!string.IsNullOrEmpty(sp1.PropertyContent))
            {
                sp1.Create();
            }
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


            txtAmazonLink.Text = string.Empty;
            txtiTunesLink.Text = string.Empty;
        }

        protected void ddlArtistSongs1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sp1 = new SongProperty();

            //am
            sp1.GetSongPropertySongIDTypeID(
                sng.SongID,
                SongProperty.SPropType.AM.ToString());

            txtAmazonLink.Text = sp1.PropertyContent;


            sp1 = new SongProperty();

            //it
            sp1.GetSongPropertySongIDTypeID(
                sng.SongID,
                SongProperty.SPropType.IT.ToString());

            txtiTunesLink.Text = sp1.PropertyContent;
        }
    }
}