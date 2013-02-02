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
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Operational;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Venue : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private int _venueID = 0;

        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private decimal _latitude = 0;

        public decimal Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        private decimal _longitude = 0;

        public decimal Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        private string _venueName = string.Empty;

        public string VenueName
        {
            get { return _venueName; }
            set { _venueName = value; }
        }

        private string _addressLine1 = string.Empty;

        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }

        private string _addressLine2 = string.Empty;

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }

        private string _city = string.Empty;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _region = string.Empty;

        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        private string _postalCode = string.Empty;

        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        private string _countryISO = string.Empty;

        public string CountryISO
        {
            get { return _countryISO; }
            set { _countryISO = value; }
        }

        private string _venueURL = string.Empty;

    
        public string VenueURL
        {
            get { return _venueURL; }
            set { _venueURL = value; }
        }

        private bool _isEnabled = false;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }


        private string _phoneNumber = string.Empty;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }


        private char _venueType = char.MinValue;

        public char VenueType
        {
            get { return _venueType; }
            set { _venueType = value; }
        }

        #endregion

        public string VenueTypeIcon
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);


                switch (VenueType)
                {
                    case 'C':
                    case 'F':
                    case 'S':
                    case 'V':
                    case 'W':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/map/{0}.png", VenueType)));
                        break;
                    default:
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            "~/content/images/map/U.png"));
                        break;
                }

                return sb.ToString();
            }
        }
        
        #region constructors

        public Venue(int venueID)
        {
            Get(venueID);
        }

        public Venue(DataRow dr)
        {
            Get(dr);
        }

        public Venue()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}", this.GetType().FullName, this.VenueID.ToString());
            }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }

        #endregion


        public override void Get(int venueID)
        {
            this.VenueID = venueID;

            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();

                // set the stored procedure name
                comm.CommandText = "up_GetVenueByID";

                ADOExtenstion.AddParameter(comm, "venueID", venueID);
                
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    Get(dt.Rows[0]);

                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }

        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVenue";

            ADOExtenstion.AddParameter(comm, "createdByUserID",   CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "venueName", VenueName);
            ADOExtenstion.AddParameter(comm, "addressLine1", AddressLine1);
            ADOExtenstion.AddParameter(comm, "addressLine2", AddressLine2);
            ADOExtenstion.AddParameter(comm, "city",  City);
            ADOExtenstion.AddParameter(comm, "region",  Region);
            ADOExtenstion.AddParameter(comm, "postalCode", PostalCode);
            ADOExtenstion.AddParameter(comm, "countryISO",  CountryISO);
            ADOExtenstion.AddParameter(comm, "venueURL",  VenueURL);
            ADOExtenstion.AddParameter(comm, "isEnabled", IsEnabled);
            ADOExtenstion.AddParameter(comm, "latitude", Latitude);
            ADOExtenstion.AddParameter(comm, "longitude", Longitude);
            ADOExtenstion.AddParameter(comm, "phoneNumber",  PhoneNumber);
            ADOExtenstion.AddParameter(comm, "venueType",  VenueType);
            ADOExtenstion.AddParameter(comm, "description", Description);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            else
            {
                Venues venus = new Venues();
                venus.RemoveCache();

                this.VenueID = Convert.ToInt32(result);

                return this.VenueID;
            }
        }

        public override bool Update()
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateVenue";


            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "venueName", VenueName);
            ADOExtenstion.AddParameter(comm, "addressLine1",AddressLine1);
            ADOExtenstion.AddParameter(comm, "addressLine2", AddressLine2);
            ADOExtenstion.AddParameter(comm, "city",  City);
            ADOExtenstion.AddParameter(comm, "region",  Region);
            ADOExtenstion.AddParameter(comm, "postalCode", PostalCode);
            ADOExtenstion.AddParameter(comm, "countryISO", CountryISO);
            ADOExtenstion.AddParameter(comm, "venueURL", VenueURL);
            ADOExtenstion.AddParameter(comm, "isEnabled", IsEnabled);
            ADOExtenstion.AddParameter(comm, "latitude",  Latitude);
            ADOExtenstion.AddParameter(comm, "longitude", Longitude);
            ADOExtenstion.AddParameter(comm, "phoneNumber",PhoneNumber);
            ADOExtenstion.AddParameter(comm, "venueType", VenueType);
            ADOExtenstion.AddParameter(comm, "description", Description);
            ADOExtenstion.AddParameter(comm, "venueID",  VenueID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            Venues venus = new Venues();
            venus.RemoveCache();

            return (result != -1);
 
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.AddressLine1 = FromObj.StringFromObj(dr["addressLine1"]);
                this.VenueID = FromObj.IntFromObj(dr["venueID"]);
                this.VenueName = FromObj.StringFromObj(dr["venueName"]);
                this.AddressLine1 = FromObj.StringFromObj(dr["addressLine1"]);
                this.AddressLine2 = FromObj.StringFromObj(dr["addressLine2"]);
                this.City = FromObj.StringFromObj(dr["city"]);
                this.Region = FromObj.StringFromObj(dr["region"]);
                this.PostalCode = FromObj.StringFromObj(dr["postalCode"]);
                this.CountryISO = FromObj.StringFromObj(dr["countryISO"]);
                this.VenueURL = FromObj.StringFromObj(dr["venueURL"]);
                this.IsEnabled = FromObj.BoolFromObj(dr["isEnabled"]);
                this.Latitude = FromObj.DecimalFromObj(dr["latitude"]);
                this.Longitude = FromObj.DecimalFromObj(dr["longitude"]);
                this.PhoneNumber = FromObj.StringFromObj(dr["phoneNumber"]);
                this.VenueType = FromObj.CharFromObj(dr["venueType"]);
                this.Description = FromObj.StringFromObj(dr["description"]);

            }
            catch //(Exception ex)
            {
                //Utilities.LogError(ex);
            }
        }


        public string MapText
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<div class=""venue_icon"">");
                sb.Append(this.ToString());
                
                if (!string.IsNullOrEmpty(this.VenueURL))
                {
                    sb.Append("<br />");
                    sb.AppendFormat("{0}: ", Messages.Venue);
                    Uri uri = new Uri(this.VenueURL);
                    sb.Append(@"<a target=""_blank"" href=""");
                    sb.Append(this.VenueURL);
                    sb.Append(@""">");
                    sb.Append(uri.Host);
                    sb.Append("</a>");
                }

                if (!string.IsNullOrEmpty(this.PhoneNumber))
                {
                    sb.Append("<br />");
                    sb.AppendFormat("{0}: ", Messages.Phone);
                    sb.Append(this.PhoneNumber);
                }

                //if (!string.IsNullOrEmpty(this.Description))
                //{
                //    sb.Append("<br />");
                //    sb.AppendFormat("{0}: ", Messages.Details);
                //    sb.Append(Utilities.MakeLink(this.Description));
                //}

                sb.Append("</div>");

                return sb.ToString();

            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(100);

            if (!string.IsNullOrEmpty(this.VenueName))
            {
                sb.Append(this.VenueName);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.AddressLine1))
            {
                sb.Append(this.AddressLine1);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.AddressLine2))
            {
                sb.Append(this.AddressLine2);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.City))
            {
                sb.Append(this.City);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.Region))
            {
                sb.Append(this.Region);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.PostalCode))
            {
                sb.Append(this.PostalCode);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(this.CountryISO))
            {
                sb.Append(this.CountryISO);
            }

            return sb.ToString();
        }
    }


    public class Venues : List<Venue>, ICacheName
    {

        #region IGetAll Members

        public void GetAll()
        {
            if (HttpContext.Current.Cache[this.CacheName] == null)
            {

                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllVenues";

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    Venue ven = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        ven = new Venue(dr);
                        this.Add(ven);
                    }

                    HttpContext.Current.Cache.AddObjToCache(this, this.CacheName);
                }
            }
            else
            {
                Venues uads = (Venues)HttpContext.Current.Cache[this.CacheName];

                foreach (Venue ven in uads) this.Add(ven);
            }
        }

        #endregion

        public static DataTable GetDateVenues()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDateVenues";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            return dt;
        }

        public static CityRegionCountries GetDistinctLocations()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctLocations";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            CityRegionCountries crcs = new CityRegionCountries();

            CityRegionCountry crc = null;

            foreach (DataRow dr in dt.Rows)
            {
                crc = new CityRegionCountry(dr);
                crcs.Add(crc);
            }

            return crcs;
        }



        #region ICacheName Members

        public string CacheName
        {
            get
            {
                return this.GetType().FullName + "all";
            }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }

        #endregion
    }
}
