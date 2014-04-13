using System.Collections.Generic;
using System.Data;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class CityRegionCountry
    {
        #region properties

        private string _city = string.Empty;

        private string _countryCode = string.Empty;
        private string _region = string.Empty;


        public CityRegionCountry()
        {
        }

        public CityRegionCountry(DataRow dr)
        {
            CountryCode = FromObj.StringFromObj(dr["countryISO"]);
            Region = FromObj.StringFromObj(dr["region"]);
            City = FromObj.StringFromObj(dr["city"]);
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

        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        #endregion
    }

    public class CityRegionCountries : List<CityRegionCountry>
    {
    }
}