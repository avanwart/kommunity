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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Operational;

namespace DasKlub.m.auth
{
    public partial class ModifyVenue : System.Web.UI.Page
    {
        Venue veu = null;
        const string unknownValue = "-UNKNOWN-";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LoadVenueList();
            }
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            

            if (!string.IsNullOrEmpty(hfVenueID.Value))
            {
                veu = new Venue( Convert.ToInt32(hfVenueID.Value) );
            }
            else
            {
                veu = new Venue();
            }

            veu.IsEnabled = chkEnabled.Enabled;
            veu.AddressLine1 = txtAddressLine1.Text;
            veu.AddressLine2 = txtAddressLine2.Text;
            veu.City = txtCity.Text;
            veu.CountryISO = ddlCountryISO.SelectedValue;
            veu.PostalCode = txtPostalCode.Text;
            veu.Region = txtRegion.Text;
            veu.VenueName = txtVenueName.Text;
            veu.VenueURL = txtVenueURL.Text;
            veu.VenueType = Convert.ToChar( ddlVenueType.SelectedValue);
            veu.PhoneNumber = txtPhoneNumber.Text;
            veu.Description = txtDescription.Text;

            // TODO: DON'T JUST WORK WITH DECIMALS, CUTURE ISSUES
            if (!string.IsNullOrEmpty(txtLongitude.Text.Trim()))
            {
                veu.Longitude = Convert.ToDecimal(txtLongitude.Text);
            }

            if (!string.IsNullOrEmpty(txtLatitude.Text.Trim()))
            {
                veu.Latitude = Convert.ToDecimal(txtLatitude.Text);
            }

            

            if (veu.VenueID == 0)
            {
                veu.Create();
            }
            else veu.Update();

            LoadVenueList();
        }


        protected void ddlVenues_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearInput();

            veu = new Venue(Convert.ToInt32(ddlVenues.SelectedValue));

            txtAddressLine1.Text = veu.AddressLine1;
            txtAddressLine2.Text = veu.AddressLine2;
            txtCity.Text = veu.City;
            txtPostalCode.Text = veu.PostalCode;
            txtRegion.Text = veu.Region;
            txtVenueName.Text = veu.VenueName;
            txtVenueURL.Text = veu.VenueURL;
            ddlCountryISO.SelectedValue = veu.CountryISO;
            chkEnabled.Checked = veu.IsEnabled;
            hfVenueID.Value = veu.VenueID.ToString();
            txtDescription.Text = veu.Description;
            if ( veu.VenueType != char.MinValue) 
                ddlVenueType.SelectedValue = Convert.ToString(veu.VenueType);
            txtPhoneNumber.Text = veu.PhoneNumber;

            if ( veu.Latitude != 0)
                txtLatitude.Text = veu.Latitude.ToString();

            if ( veu.Longitude != 0)
                txtLongitude.Text = veu.Longitude.ToString();
        }


        private void ClearInput()
        {
            txtAddressLine1.Text = string.Empty;
            txtAddressLine2.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtPostalCode.Text = string.Empty;
            txtRegion.Text = string.Empty;
            txtVenueName.Text = string.Empty;
            txtVenueURL.Text = string.Empty;
            chkEnabled.Checked = true;
            hfVenueID.Value = string.Empty;
            txtLongitude.Text = string.Empty;
            txtLatitude.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtDescription.Text = string.Empty;
            
        }

        private void LoadVenueList()
        {
            Venues vnues = new Venues();

            vnues.GetAll();

            vnues.Sort(delegate(Venue p1, Venue p2)
            {
                return p1.VenueName.CompareTo(p2.VenueName);
            });

            ddlVenues.DataSource = vnues;
            ddlVenues.DataTextField = "venueName";
            ddlVenues.DataValueField = "venueID";
            ddlVenues.DataBind();
            ddlVenues.Items.Insert(0, new ListItem(unknownValue));
           // Utilities.General.SortDropDownList(ddlVenues);

        }


    }
}