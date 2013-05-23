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

namespace DasKlub.Web.Web.m.auth
{
    public partial class NewTourDate : Page
    {
        #region variables

        private const string unknownValue = "-UNKNOWN-";
        private Event evnt;
        private Events evnts;

        #endregion

        #region page events

        protected void btnDeleteEvent_Click(object sender, EventArgs e)
        {
            if (gvwEvents.SelectedDataKey == null || string.IsNullOrEmpty(gvwEvents.SelectedDataKey.Value.ToString()))
                return;

            evnt = new Event(Convert.ToInt32(gvwEvents.SelectedDataKey.Value));

            if (evnt.Delete())
            {
                //?
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVenueList();
                LoadEventCycles();
                LoadGrid();
            }
        }


        protected void gvwAllEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfEventID.Value = gvwEvents.SelectedDataKey.Value.ToString();

            ClearInput();

            evnt = new Event(Convert.ToInt32(gvwEvents.SelectedDataKey.Value));

            txtName.Text = evnt.Name;
            hfEventID.Value = evnt.EventID.ToString();
            txtEventDetailURL.Text = evnt.EventDetailURL;
            txtNotes.Text = evnt.Notes;
            txtRSVPURL.Text = evnt.RsvpURL;
            txtTicketURL.Text = evnt.TicketURL;
            chkIsEnabled.Checked = evnt.IsEnabled;
            chkIsReoccuring.Checked = evnt.IsReoccuring;
            ddlYear.SelectedValue = evnt.LocalTimeBegin.Year.ToString();
            ddlMonth.SelectedValue = evnt.LocalTimeBegin.Month.ToString();
            ddlDay.SelectedValue = evnt.LocalTimeBegin.Day.ToString();
            ddlVenues.SelectedValue = evnt.VenueID.ToString();
            ddlEventCycle.SelectedValue = evnt.EventCycleID.ToString();
        }


        protected void btnNewEvent_Click(object sender, EventArgs e)
        {
            evnt = new Event();

            evnt.Name = txtName.Text;
            evnt.LocalTimeBegin = Convert.ToDateTime(ddlYear.SelectedValue + "-" +
                                                     ddlMonth.SelectedValue + "-" + ddlDay.SelectedValue);
            evnt.VenueID = Convert.ToInt32(ddlVenues.SelectedValue);
            evnt.EventDetailURL = txtEventDetailURL.Text;
            evnt.Notes = txtNotes.Text;
            evnt.RsvpURL = txtRSVPURL.Text;
            evnt.TicketURL = txtTicketURL.Text;
            evnt.IsEnabled = chkIsEnabled.Checked;
            evnt.IsReoccuring = chkIsReoccuring.Checked;
            evnt.EventCycleID = Convert.ToInt32(ddlEventCycle.SelectedValue);

            evnt.Create();

            LoadGrid();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfEventID.Value))
            {
                evnt = new Event(Convert.ToInt32(hfEventID.Value));
            }
            else
            {
                return;
            }

            evnt.Name = txtName.Text;
            evnt.LocalTimeBegin = Convert.ToDateTime(ddlYear.SelectedValue + "-" +
                                                     ddlMonth.SelectedValue + "-" + ddlDay.SelectedValue);
            evnt.VenueID = Convert.ToInt32(ddlVenues.SelectedValue);
            evnt.EventDetailURL = txtEventDetailURL.Text;
            evnt.Notes = txtNotes.Text;
            evnt.RsvpURL = txtRSVPURL.Text;
            evnt.TicketURL = txtTicketURL.Text;
            evnt.IsEnabled = chkIsEnabled.Checked;
            evnt.IsReoccuring = chkIsReoccuring.Checked;
            evnt.EventCycleID = Convert.ToInt32(ddlEventCycle.SelectedValue);

            evnt.Update();

            LoadGrid();
        }

        #endregion

        #region methods

        private void LoadVenueList()
        {
            var vnues = new Venues();

            vnues.GetAll();

            vnues.Sort(delegate(Venue p1, Venue p2) { return p1.VenueName.CompareTo(p2.VenueName); });

            ddlVenues.DataSource = vnues;
            ddlVenues.DataTextField = "venueName";
            ddlVenues.DataValueField = "venueID";
            ddlVenues.DataBind();
            ddlVenues.Items.Insert(0, new ListItem(unknownValue));
            // Utilities.General.SortDropDownList(ddlVenues);
        }

        private void LoadEventCycles()
        {
            var envtcyc = new EventCycles();

            envtcyc.GetAll();

            envtcyc.Sort(delegate(EventCycle p1, EventCycle p2) { return p1.CycleName.CompareTo(p2.CycleName); });

            ddlEventCycle.DataSource = envtcyc;
            ddlEventCycle.DataTextField = "cycleName";
            ddlEventCycle.DataValueField = "eventCycleID";
            ddlEventCycle.DataBind();
            ddlEventCycle.Items.Insert(0, new ListItem(unknownValue));
            // Utilities.General.SortDropDownList(ddlVenues);
        }


        private void ClearInput()
        {
            txtName.Text = string.Empty;
            txtEventDetailURL.Text = string.Empty;
            txtNotes.Text = string.Empty;
            txtRSVPURL.Text = string.Empty;
            txtTicketURL.Text = string.Empty;
            chkIsEnabled.Checked = false;
            chkIsReoccuring.Checked = false;
        }

        public void LoadGrid()
        {
            evnts = new Events();
            evnts.GetAll();
            gvwEvents.DataSource = evnts;
            gvwEvents.DataBind();
        }

        #endregion
    }
}