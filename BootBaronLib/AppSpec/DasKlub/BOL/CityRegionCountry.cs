﻿//  Copyright 2013 
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
using System.Collections.Generic;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class CityRegionCountry
    {
        #region properties

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

        private string _countryCode = string.Empty;


        public CityRegionCountry() { }

        public CityRegionCountry(System.Data.DataRow dr)
        {
        
            this.CountryCode = FromObj.StringFromObj(dr["countryISO"]);
            this.Region = FromObj.StringFromObj(dr["region"]);
            this.City = FromObj.StringFromObj(dr["city"]);

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
        public CityRegionCountries() { }
    }
}
