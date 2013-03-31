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
using System.Web.UI;
using System.Web.UI.WebControls;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.Operational;

namespace DasKlub.m.auth
{
    public partial class ArtistTD : Page
    {
        #region variables

        private const string unknownValue = "-UNKNOWN-";

        #endregion

        #region page events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlTourDate.DataSource = Venues.GetDateVenues();
                ddlTourDate.DataTextField = "datevenue";
                ddlTourDate.DataValueField = "eventID";
                ddlTourDate.DataBind();
                ddlTourDate.Items.Insert(0, new ListItem(unknownValue));
                //   Utilities.General.SortDropDownList(ddlTourDate);

                ///
                var arts = new Artists();
                arts.GetAll();

                // artists 1
                ddlArtist1.DataSource = arts;
                ddlArtist1.DataTextField = "name";
                ddlArtist1.DataValueField = "name";
                ddlArtist1.DataBind();
                ddlArtist1.Items.Insert(0, new ListItem(unknownValue));
                Utilities.General.SortDropDownList(ddlArtist1);

                // artists 2
                ddlArtist2.DataSource = arts;
                ddlArtist2.DataTextField = "name";
                ddlArtist2.DataValueField = "name";
                ddlArtist2.DataBind();
                ddlArtist2.Items.Insert(0, new ListItem(unknownValue));
                Utilities.General.SortDropDownList(ddlArtist2);

                // artists 3
                ddlArtist3.DataSource = arts;
                ddlArtist3.DataTextField = "name";
                ddlArtist3.DataValueField = "name";
                ddlArtist3.DataBind();
                ddlArtist3.Items.Insert(0, new ListItem(unknownValue));
                Utilities.General.SortDropDownList(ddlArtist3);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var atd = new ArtistEvent();

            atd.EventID = Convert.ToInt32(ddlTourDate.SelectedValue);

            var art = new Artist();

            if (ddlArtist1.SelectedValue != unknownValue && !string.IsNullOrEmpty(ddlArtist1.SelectedValue))
            {
                art = new Artist(ddlArtist1.SelectedValue);
                atd.ArtistID = art.ArtistID;
                atd.RankOrder = 1;
                atd.Create();

                if (ddlArtist2.SelectedValue != unknownValue && !string.IsNullOrEmpty(ddlArtist2.SelectedValue))
                {
                    art = new Artist(ddlArtist2.SelectedValue);
                    atd.ArtistID = art.ArtistID;
                    atd.RankOrder = 2;
                    atd.Create();

                    if (ddlArtist3.SelectedValue != unknownValue && !string.IsNullOrEmpty(ddlArtist3.SelectedValue))
                    {
                        art = new Artist(ddlArtist3.SelectedValue);
                        atd.ArtistID = art.ArtistID;
                        atd.ArtistID = Convert.ToInt32(ddlArtist3.SelectedValue);
                        atd.RankOrder = 3;
                        atd.Create();
                    }
                }
            }
        }

        #endregion

        protected void gvwEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}