using System.Collections.Generic;

namespace DasKlub.Web.Models
{
    public class MapModel
    {
        public IList<MapPoint> MapPoints { get; set; }
    }

    public class MapPoint
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Message { get; set; }

        public string Icon { get; set; }
    }
}