using System;

namespace DasKlub.Lib.Interfaces
{
    public interface IDatesInRange
    {
        void GetEventsForTimeRange(DateTime beginDate, DateTime endDate);
    }
}