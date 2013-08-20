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
using System.Text;
using System.Web;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.AppSpec.DasKlub.BOL
{
    public class CalendarItem : ICalendarEvent, IUnorderdListItem
    {
        #region constructors

        public CalendarItem()
        {
        }

        public CalendarItem(Event td)
        {
            _startDate = td.StartDate;
            _endDate = td.EndDate;
            _eventDescription = td.EventDescription;
            _eventDetailURL = td.EventDetailURL;
            //_eventType = td.EventType;
            _ticketDetailURL = td.TicketDetailURL;
            _venueDetail = td.VenueDetail;
            _rsvpURL = td.RsvpURL;

            var ven = new Venue(td.VenueID);

            _venueURL = ven.VenueURL;
        }

        #endregion

        #region private properties

        private readonly DateTime _endDate = DateTime.MinValue;

        private readonly string _eventDescription = string.Empty;

        private readonly string _eventDetailURL = string.Empty;

        private readonly string _eventType = string.Empty;

        private readonly string _rsvpURL = string.Empty;
        private readonly DateTime _startDate = DateTime.MinValue;
        private readonly string _ticketDetailURL = string.Empty;
        private readonly string _venueDetail = string.Empty;

        private readonly string _venueURL = string.Empty;

        #endregion

        #region ICalendarEvent Members

        public string EventDetailURL
        {
            get { return _eventDetailURL; }
        }

        public string EventType
        {
            get { return _eventType; }
        }

        public string RSVPURL
        {
            get { return _rsvpURL; }
        }


        public DateTime StartDate
        {
            get { return _startDate; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
        }

        public string VenueDetail
        {
            get { return _venueDetail; }
        }

        public string EventDescription
        {
            get { return _eventDescription; }
        }

        public string TicketDetailURL
        {
            get { return _ticketDetailURL; }
        }


        public string VenueURL
        {
            get { return _venueURL; }
        }

        #endregion

        public string ToUnorderdListItem
        {
            get
            {
                var sb = new StringBuilder(100);

                sb.Append(@"<li>");
                sb.Append(EventDescription);
                sb.Append(@" | <a target=""_blank"" href=""http://maps.google.com?daddr=");
                sb.Append(HttpUtility.UrlEncode(VenueDetail));
                sb.Append(@""">MAP</a>");

                if (!string.IsNullOrEmpty(VenueURL))
                {
                    sb.Append(@" | <a target=""_blank"" href=""");
                    sb.Append(VenueURL);
                    sb.Append(@""">VENUE</a>");
                }


                if (!string.IsNullOrEmpty(TicketDetailURL))
                {
                    sb.Append(@" | <a target=""_blank"" href=""");
                    sb.Append(TicketDetailURL);
                    sb.Append(@""">TICKET</a>");
                }

                if (!string.IsNullOrEmpty(EventDetailURL))
                {
                    sb.Append(@" | <a target=""_blank"" href=""");
                    sb.Append(EventDetailURL);
                    sb.Append(@""">DETAILS</a>");
                }

                if (!string.IsNullOrEmpty(RSVPURL))
                {
                    sb.Append(@" | <a target=""_blank"" href=""");
                    sb.Append(RSVPURL);
                    sb.Append(@""">RSVP</a>");
                }


                sb.Append(@"</li>");

                return sb.ToString();
            }
        }
    }

    public class CalendarItems : List<CalendarItem>
    {
        public string ToJSON(DateTime dtBegin)
        {
            if (Count == 0) return string.Empty;

            var sb = new StringBuilder();

            sb.Append(@"<div class=""event_listings"">");

            sb.Append(@"<ul>");

            foreach (CalendarItem citm in this)
            {
                sb.Append(citm.ToUnorderdListItem);
            }

            sb.Append(@"</ul>");


            sb.Append(@"<div class=""clear""></div>");

            sb.Append(@"</div>");

            if (this[0] != null)
            {
                return @"{""EventsToday"": """ + HttpUtility.HtmlEncode(sb.ToString()) + @""",
                ""ISODate"": """ + FromDate.DateToYYYY_MM_DD(dtBegin) + @"""}";
            }
            else
            {
                return @"{""EventsToday"": """ + HttpUtility.HtmlEncode(sb.ToString()) + @""",
                ""ISODate"": """ + string.Empty + @"""}";
            }
        }
    }
}