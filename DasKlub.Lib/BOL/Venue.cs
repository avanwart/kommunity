using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;

namespace DasKlub.Lib.BOL
{
    public class Venue : BaseIUserLogCrud, ICacheName
    {
        #region properties

        private string _addressLine1 = string.Empty;
        private string _addressLine2 = string.Empty;
        private string _city = string.Empty;
        private string _countryISO = string.Empty;
        private string _description = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _postalCode = string.Empty;
        private string _region = string.Empty;
        private string _venueName = string.Empty;
        private char _venueType = char.MinValue;
        private string _venueURL = string.Empty;
        public int VenueID { get; set; }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string VenueName
        {
            get { return _venueName; }
            set { _venueName = value; }
        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        public string CountryISO
        {
            get { return _countryISO; }
            set { _countryISO = value; }
        }


        public string VenueURL
        {
            get { return _venueURL; }
            set { _venueURL = value; }
        }

        public bool IsEnabled { get; set; }


        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }


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
                var sb = new StringBuilder(100);


                switch (VenueType)
                {
                    case 'C':
                    case 'F':
                    case 'S':
                    case 'V':
                    case 'W':
                        sb.Append(VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/map/{0}.png", VenueType)));
                        break;
                    default:
                        sb.Append(VirtualPathUtility.ToAbsolute(
                            "~/content/images/map/U.png"));
                        break;
                }

                return sb.ToString();
            }
        }

        public string MapText
        {
            get
            {
                var sb = new StringBuilder(100);

                sb.Append(@"<div class=""venue_icon"">");
                sb.Append(ToString());

                if (!string.IsNullOrEmpty(VenueURL))
                {
                    sb.Append("<br />");
                    sb.AppendFormat("{0}: ", Messages.Venue);
                    var uri = new Uri(VenueURL);
                    sb.Append(@"<a target=""_blank"" href=""");
                    sb.Append(VenueURL);
                    sb.Append(@""">");
                    sb.Append(uri.Host);
                    sb.Append("</a>");
                }

                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    sb.Append("<br />");
                    sb.AppendFormat("{0}: ", Messages.Phone);
                    sb.Append(PhoneNumber);
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
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}-{1}", GetType().FullName, VenueID); }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion

        public override void Get(int venueID)
        {
            VenueID = venueID;

            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();

                // set the stored procedure name
                comm.CommandText = "up_GetVenueByID";

                comm.AddParameter("venueID", venueID);

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVenue";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("venueName", VenueName);
            comm.AddParameter("addressLine1", AddressLine1);
            comm.AddParameter("addressLine2", AddressLine2);
            comm.AddParameter("city", City);
            comm.AddParameter("region", Region);
            comm.AddParameter("postalCode", PostalCode);
            comm.AddParameter("countryISO", CountryISO);
            comm.AddParameter("venueURL", VenueURL);
            comm.AddParameter("isEnabled", IsEnabled);
            comm.AddParameter("latitude", Latitude);
            comm.AddParameter("longitude", Longitude);
            comm.AddParameter("phoneNumber", PhoneNumber);
            comm.AddParameter("venueType", VenueType);
            comm.AddParameter("description", Description);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            var venus = new Venues();
            venus.RemoveCache();

            VenueID = Convert.ToInt32(result);

            return VenueID;
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateVenue";


            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("venueName", VenueName);
            comm.AddParameter("addressLine1", AddressLine1);
            comm.AddParameter("addressLine2", AddressLine2);
            comm.AddParameter("city", City);
            comm.AddParameter("region", Region);
            comm.AddParameter("postalCode", PostalCode);
            comm.AddParameter("countryISO", CountryISO);
            comm.AddParameter("venueURL", VenueURL);
            comm.AddParameter("isEnabled", IsEnabled);
            comm.AddParameter("latitude", Latitude);
            comm.AddParameter("longitude", Longitude);
            comm.AddParameter("phoneNumber", PhoneNumber);
            comm.AddParameter("venueType", VenueType);
            comm.AddParameter("description", Description);
            comm.AddParameter("venueID", VenueID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            var venus = new Venues();
            venus.RemoveCache();

            return (result != -1);
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                AddressLine1 = FromObj.StringFromObj(dr["addressLine1"]);
                VenueID = FromObj.IntFromObj(dr["venueID"]);
                VenueName = FromObj.StringFromObj(dr["venueName"]);
                AddressLine1 = FromObj.StringFromObj(dr["addressLine1"]);
                AddressLine2 = FromObj.StringFromObj(dr["addressLine2"]);
                City = FromObj.StringFromObj(dr["city"]);
                Region = FromObj.StringFromObj(dr["region"]);
                PostalCode = FromObj.StringFromObj(dr["postalCode"]);
                CountryISO = FromObj.StringFromObj(dr["countryISO"]);
                VenueURL = FromObj.StringFromObj(dr["venueURL"]);
                IsEnabled = FromObj.BoolFromObj(dr["isEnabled"]);
                Latitude = FromObj.DecimalFromObj(dr["latitude"]);
                Longitude = FromObj.DecimalFromObj(dr["longitude"]);
                PhoneNumber = FromObj.StringFromObj(dr["phoneNumber"]);
                VenueType = FromObj.CharFromObj(dr["venueType"]);
                Description = FromObj.StringFromObj(dr["description"]);
            }
            catch //(Exception ex)
            {
                //Utilities.LogError(ex);
            }
        }


        public override string ToString()
        {
            var sb = new StringBuilder(100);

            if (!string.IsNullOrEmpty(VenueName))
            {
                sb.Append(VenueName);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(AddressLine1))
            {
                sb.Append(AddressLine1);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(AddressLine2))
            {
                sb.Append(AddressLine2);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(City))
            {
                sb.Append(City);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(Region))
            {
                sb.Append(Region);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(PostalCode))
            {
                sb.Append(PostalCode);
                sb.Append(", ");
            }

            if (!string.IsNullOrEmpty(CountryISO))
            {
                sb.Append(CountryISO);
            }

            return sb.ToString();
        }
    }


    public class Venues : List<Venue>, ICacheName
    {
        #region IGetAll Members

        public void GetAll()
        {
            if (HttpRuntime.Cache[CacheName] == null)
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
                        Add(ven);
                    }

                    HttpRuntime.Cache.AddObjToCache(this, CacheName);
                }
            }
            else
            {
                var uads = (Venues) HttpRuntime.Cache[CacheName];

                foreach (Venue ven in uads) Add(ven);
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

            var crcs = new CityRegionCountries();

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
            get { return string.Concat(GetType().FullName, "all"); }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion
    }
}