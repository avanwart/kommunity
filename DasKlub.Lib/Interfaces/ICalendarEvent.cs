using System;

namespace DasKlub.Lib.Interfaces
{
    public interface ICalendarEvent
    {
        DateTime StartDate { get; }

        DateTime EndDate { get; }

        string VenueDetail { get; }

        string EventDescription { get; }

        string TicketDetailURL { get; }

        string RSVPURL { get; }

        string VenueURL { get; }
    }
}