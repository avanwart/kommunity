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
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Event : BaseIUserLogCRUD, ICacheName, ICalendarEvent
    {
        #region properties

        private int _EventID = 0;

        public int EventID
        {
            get { return _EventID; }
            set { _EventID = value; }
        }

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private int _eventCycleID = 0;

        public int EventCycleID
        {
            get { return _eventCycleID; }
            set { _eventCycleID = value; }
        }

        private int _venueID = 0;

        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private DateTime _localTimeBegin = DateTime.MinValue;

        public DateTime LocalTimeBegin
        {
            get { return _localTimeBegin; }
            set { _localTimeBegin = value; }
        }


        private DateTime _localTimeEnd = DateTime.MinValue;

        public DateTime LocalTimeEnd
        {
            get { return _localTimeEnd; }
            set { _localTimeEnd = value; }
        }



        private string _notes = string.Empty;

        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
 
    
        private string _ticketURL = string.Empty;

       

        public string TicketURL
        {
            get { return _ticketURL; }
            set { _ticketURL = value; }
        }


        private string _rsvpURL = string.Empty;

        public string RsvpURL
        {
            get { return _rsvpURL; }
            set { _rsvpURL = value; }
        }


        private bool _isEnabled = false;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }


        private bool _isReoccuring = false;

        public bool IsReoccuring
        {
            get { return _isReoccuring; }
            set { _isReoccuring = value; }
        }


        private string _eventDetailURL = string.Empty;
 
        public string EventDetailURL
        {
            get { return _eventDetailURL; }
            set { _eventDetailURL = value; }
        }


        #endregion

        #region constructors

        public Event(DataRow dr)
        {
            Get(dr);
        }

        public Event()
        {
            // TODO: Complete member initialization
        }

        public Event(int eventID)
        {
            this.EventID = eventID;

            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetEventByID";
                // create a new parameter
                DbParameter param = comm.CreateParameter();
                param.ParameterName = "@eventID";
                param.Value = eventID;
                param.DbType = DbType.Int32;
                comm.Parameters.Add(param);

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

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get {
                return string.Format("{0}-{1}", this.GetType().FullName, this.EventID.ToString());
            }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }

        #endregion

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.EventID = FromObj.IntFromObj(dr["EventID"]);
                this.VenueID = FromObj.IntFromObj(dr["venueID"]);
                this.LocalTimeBegin = FromObj.DateFromObj(dr["localTimeBegin"]);
                this.LocalTimeEnd = FromObj.DateFromObj(dr["localTimeEnd"]);
                this.Notes = FromObj.StringFromObj(dr["notes"]);
                this.TicketURL = FromObj.StringFromObj(dr["ticketURL"]);
                this.EventCycleID = FromObj.IntFromObj(dr["eventCycleID"]);
                this.RsvpURL = FromObj.StringFromObj(dr["rsvpURL"]);
                this.Name = FromObj.StringFromObj(dr["name"]);
                this.IsEnabled = FromObj.BoolFromObj(dr["isEnabled"]);
                this.IsReoccuring = FromObj.BoolFromObj(dr["isReoccuring"]);
                this.EventDetailURL = FromObj.StringFromObj(dr["eventDetailURL"]);
            }
            catch // (Exception ex)
            {
                //Utilities.LogError(ex);
            }
        }

        public override int Create()
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddEvent";

            if ( this.LocalTimeBegin == DateTime.MinValue)
            {
                this.LocalTimeBegin = DateTime.UtcNow;
            }

            if ( LocalTimeEnd == DateTime.MinValue)
            {
                LocalTimeEnd = DateTime.UtcNow;
            }


            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "name", Name);
            ADOExtenstion.AddParameter(comm, "venueID", VenueID);
            ADOExtenstion.AddParameter(comm, "localTimeBegin", LocalTimeBegin);
            ADOExtenstion.AddParameter(comm, "notes", Notes);
            ADOExtenstion.AddParameter(comm, "ticketURL", TicketURL);
            ADOExtenstion.AddParameter(comm, "localTimeEnd", LocalTimeEnd);
            ADOExtenstion.AddParameter(comm, "eventCycleID", EventCycleID);
            ADOExtenstion.AddParameter(comm, "rsvpURL", RsvpURL);
            ADOExtenstion.AddParameter(comm, "isReoccuring", IsReoccuring);
            ADOExtenstion.AddParameter(comm, "isEnabled", IsEnabled);
            ADOExtenstion.AddParameter(comm, "eventDetailURL", EventDetailURL);
           
             
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
                this.VenueID = Convert.ToInt32(result);

                return this.VenueID;
            }
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateEvent";
            // create a new parameter
            DbParameter param = null;

            //
            param = comm.CreateParameter();
            param.ParameterName = "@name";
            param.Value = this.Name;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@venueID ";
            param.Value = this.VenueID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@updatedByUserID";
            param.Value = this.UpdatedByUserID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@localTimeBegin";
            param.Value = this.LocalTimeBegin;
            param.DbType = DbType.DateTime;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@notes";
            param.Value = this.Notes;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@ticketURL";
            param.Value = this.TicketURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@localTimeEnd ";
            param.Value = this.LocalTimeEnd;
            param.DbType = DbType.DateTime;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@eventCycleID";
            param.Value = this.EventCycleID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@rsvpURL";
            param.Value = this.RsvpURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@isReoccuring";
            param.Value = this.IsReoccuring;
            param.DbType = DbType.Boolean;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@isEnabled";
            param.Value = this.IsEnabled;
            param.DbType = DbType.Boolean;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@eventDetailURL";
            param.Value = this.EventDetailURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@eventID ";
            param.Value = this.EventID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);

            // the result is their ID
            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            return (result != -1);
 
        }

        public override bool Delete()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteEventByID";
            // create a new parameter
            DbParameter param = null;

            //
            param = comm.CreateParameter();
            param.ParameterName = "@eventID";
            param.Value = this.EventID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        #region ICalendarEvent Members


        public string VenueURL
        {
            get
            {
                Venue ven = new Venue(this.VenueID);

                return ven.VenueURL;
            }
        }

        public DateTime StartDate
        {
            get { return this.LocalTimeBegin; }
        }

        public DateTime EndDate
        {
            get { return this.LocalTimeEnd; }
        }

        public string VenueDetail
        {
            get
            {
                Venue ven = new Venue(this.VenueID);

                return ven.ToString();
            }
        }

        public string EventDescription
        {
            get {
                
                if (this.IsReoccuring)
                {
                    return this.Name;
                }

                StringBuilder sb = new StringBuilder();

                ArtistEvents artds = new ArtistEvents();

                artds.GetArtistsForEvent(this.EventID);

                Artist art = null;

                int i = 1;

                if ( artds.Count == 1)
                    sb.Append(@"Band: ");
                else if ( artds.Count > 1)
                    sb.Append(@"Bands: ");

                foreach (ArtistEvent atd in artds)
                {
                    art = new Artist(atd.ArtistID);

                    sb.Append(@"<a target=""_blank"" href=""");
                    sb.Append(art.FullURLOfArtist);
                    sb.Append(@""">");
                    sb.Append(art.Name);
                    sb.Append(@"</a>");

                    if (i != artds.Count)
                    {
                        sb.Append(", ");
                    }
                
                    i++;
                }

                return sb.ToString();
            }
        }

        

        public string TicketDetailURL
        {
            get { return this.TicketURL; }
        }



        public string RSVPURL
        {
            get { return this.RsvpURL; }
        }

        #endregion

 
    }

    public class Events : List<Event> 
    {
      
        public void GetEventsForLocation(DateTime beginDate, DateTime endDate, string countryISO, string region, string city)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetEventsForLocation";
            // create a new parameter
            DbParameter param = null;

            //
            param = comm.CreateParameter();
            param.ParameterName = "@beginDate";
            param.Value = beginDate;
            param.DbType = DbType.DateTime;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@endDate";
            param.Value = endDate;
            param.DbType = DbType.DateTime;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@countryISO";
            param.Value = countryISO == null ? string.Empty : countryISO;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@region";
            param.Value = region == null ? string.Empty : region;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@city";
            param.Value = city == null ? string.Empty : city;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Event trd = null;

                foreach (DataRow dr in dt.Rows)
                {
                    trd = new Event(dr);

                    this.Add(trd);
                }
            }

        }
 
 

        #region IGetAll Members

        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllEvents";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Event td = null;
                foreach (DataRow dr in dt.Rows)
                {
                    td = new Event(dr);
                    this.Add(td);
                }
            }
        }

        #endregion
    }
}
