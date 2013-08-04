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
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Event : BaseIUserLogCRUD, ICacheName, ICalendarEvent
    {
        #region properties

        private string _eventDetailURL = string.Empty;
        private DateTime _localTimeBegin = DateTime.MinValue;
        private DateTime _localTimeEnd = DateTime.MinValue;
        private string _name = string.Empty;
        private string _notes = string.Empty;
        private string _rsvpURL = string.Empty;
        private string _ticketURL = string.Empty;
        public int EventID { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public int EventCycleID { get; set; }

        public int VenueID { get; set; }

        public DateTime LocalTimeBegin
        {
            get { return _localTimeBegin; }
            set { _localTimeBegin = value; }
        }


        public DateTime LocalTimeEnd
        {
            get { return _localTimeEnd; }
            set { _localTimeEnd = value; }
        }


        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }


        public string TicketURL
        {
            get { return _ticketURL; }
            set { _ticketURL = value; }
        }


        public string RsvpURL
        {
            get { return _rsvpURL; }
            set { _rsvpURL = value; }
        }


        public bool IsEnabled { get; set; }


        public bool IsReoccuring { get; set; }


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
            EventID = eventID;

            if (HttpRuntime.Cache[CacheName] == null)
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
                    HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}-{1}", GetType().FullName, EventID.ToString()); }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                EventID = FromObj.IntFromObj(dr["EventID"]);
                VenueID = FromObj.IntFromObj(dr["venueID"]);
                LocalTimeBegin = FromObj.DateFromObj(dr["localTimeBegin"]);
                LocalTimeEnd = FromObj.DateFromObj(dr["localTimeEnd"]);
                Notes = FromObj.StringFromObj(dr["notes"]);
                TicketURL = FromObj.StringFromObj(dr["ticketURL"]);
                EventCycleID = FromObj.IntFromObj(dr["eventCycleID"]);
                RsvpURL = FromObj.StringFromObj(dr["rsvpURL"]);
                Name = FromObj.StringFromObj(dr["name"]);
                IsEnabled = FromObj.BoolFromObj(dr["isEnabled"]);
                IsReoccuring = FromObj.BoolFromObj(dr["isReoccuring"]);
                EventDetailURL = FromObj.StringFromObj(dr["eventDetailURL"]);
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

            if (LocalTimeBegin == DateTime.MinValue)
            {
                LocalTimeBegin = DateTime.UtcNow;
            }

            if (LocalTimeEnd == DateTime.MinValue)
            {
                LocalTimeEnd = DateTime.UtcNow;
            }


            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("name", Name);
            comm.AddParameter("venueID", VenueID);
            comm.AddParameter("localTimeBegin", LocalTimeBegin);
            comm.AddParameter("notes", Notes);
            comm.AddParameter("ticketURL", TicketURL);
            comm.AddParameter("localTimeEnd", LocalTimeEnd);
            comm.AddParameter("eventCycleID", EventCycleID);
            comm.AddParameter("rsvpURL", RsvpURL);
            comm.AddParameter("isReoccuring", IsReoccuring);
            comm.AddParameter("isEnabled", IsEnabled);
            comm.AddParameter("eventDetailURL", EventDetailURL);


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
                VenueID = Convert.ToInt32(result);

                return VenueID;
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
            param.Value = Name;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@venueID ";
            param.Value = VenueID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@updatedByUserID";
            param.Value = UpdatedByUserID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@localTimeBegin";
            param.Value = LocalTimeBegin;
            param.DbType = DbType.DateTime;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@notes";
            param.Value = Notes;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@ticketURL";
            param.Value = TicketURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@localTimeEnd ";
            param.Value = LocalTimeEnd;
            param.DbType = DbType.DateTime;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@eventCycleID";
            param.Value = EventCycleID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@rsvpURL";
            param.Value = RsvpURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@isReoccuring";
            param.Value = IsReoccuring;
            param.DbType = DbType.Boolean;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@isEnabled";
            param.Value = IsEnabled;
            param.DbType = DbType.Boolean;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@eventDetailURL";
            param.Value = EventDetailURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@eventID ";
            param.Value = EventID;
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
            param.Value = EventID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        #region ICalendarEvent Members

        public string VenueURL
        {
            get
            {
                var ven = new Venue(VenueID);

                return ven.VenueURL;
            }
        }

        public DateTime StartDate
        {
            get { return LocalTimeBegin; }
        }

        public DateTime EndDate
        {
            get { return LocalTimeEnd; }
        }

        public string VenueDetail
        {
            get
            {
                var ven = new Venue(VenueID);

                return ven.ToString();
            }
        }

        public string EventDescription
        {
            get
            {
                if (IsReoccuring)
                {
                    return Name;
                }

                var sb = new StringBuilder();

                var artds = new ArtistEvents();

                artds.GetArtistsForEvent(EventID);

                Artist art = null;

                int i = 1;

                if (artds.Count == 1)
                    sb.Append(@"Band: ");
                else if (artds.Count > 1)
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
            get { return TicketURL; }
        }


        public string RSVPURL
        {
            get { return RsvpURL; }
        }

        #endregion
    }

    public class Events : List<Event>
    {
        public void GetEventsForLocation(DateTime beginDate, DateTime endDate, string countryISO, string region,
                                         string city)
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

                    Add(trd);
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
                    Add(td);
                }
            }
        }

        #endregion
    }
}