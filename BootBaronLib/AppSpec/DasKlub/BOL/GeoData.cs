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
using System.Linq;
using BootBaronLib.DAL;
using BootBaronLib.Operational;
using BootBaronLib.Values;


namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    /// <summary>
    /// Location based data useful for getting the lat/long of a postal code
    /// </summary>
    /// <see cref="http://beta.codeproject.com/KB/webservices/geonamestosql.aspx"/>
    public class GeoData
    {
        #region constructors

        public GeoData()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion

        #region public static methods


        public static SiteStructs.LatLong GetLatLongForCountryPostal(string countryCode, string postalCode)
        {
            SiteStructs.LatLong latlong = new SiteStructs.LatLong();

            if (string.IsNullOrWhiteSpace(countryCode) || string.IsNullOrWhiteSpace(postalCode)) return latlong;

            if (countryCode == SiteEnums.CountryCodeISO.UK.ToString())
            {
                // they call it GB not UK or the other ones made for flags

                string cocode = "GB";

                switch ((SiteEnums.CountryCodeISO)Enum.Parse(typeof(SiteEnums.CountryCodeISO), countryCode))
                {
                    case SiteEnums.CountryCodeISO.UK:
                        countryCode = countryCode.Replace(SiteEnums.CountryCodeISO.UK.ToString(), cocode);
                        break;
                    default:
                        break;
                }

                if (postalCode.Length > 3)
                {
                    // they use only the beginning
                    postalCode = postalCode.Substring(0, 4);
                }
            }
            else if (countryCode == SiteEnums.CountryCodeISO.CR.ToString())
            {
                // they don't have the postal codes in Costa Rica
                postalCode = string.Empty;
            }
            else if (countryCode == SiteEnums.CountryCodeISO.NL.ToString())
            {
                if (postalCode.Length > 3)
                {
                    // they use only the beginning
                    postalCode = postalCode.Substring(0, 4);
                }
            }
            else if (countryCode == SiteEnums.CountryCodeISO.CA.ToString())
            {
                if (postalCode.Length > 2)
                {
                    // they use only the beginning
                    postalCode = postalCode.Substring(0, 3);
                }
            }
            else if (countryCode == SiteEnums.CountryCodeISO.JP.ToString())
            {
                postalCode = postalCode.Replace("-", string.Empty);
            }
            else if (countryCode == SiteEnums.CountryCodeISO.BR.ToString())
            {
                postalCode = postalCode.Replace(" ", string.Empty);

                if (postalCode.Contains("-"))
                {
                    postalCode = postalCode.Split('-')[0];
                    postalCode = postalCode + "-000";
                }
                //if (postalCode.Length == 8)
                //{
                //    // they need a dash
                //    postalCode = postalCode.Insert(5, "-");
                //}
            }
            else
            {
                // not sure why
            }

               // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetLatLongForCountryPostal";

            ADOExtenstion.AddParameter(comm, "postalCode",  postalCode.Replace(" ", string.Empty));
            ADOExtenstion.AddParameter(comm, "countryCode", countryCode);


 
            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                latlong.latitude = FromObj.DoubleFromObj(dt.Rows[0]["latitude"]);
                latlong.longitude = FromObj.DoubleFromObj(dt.Rows[0]["longitude"]);
            }

            return latlong;
        }

        public static void AddGeoData(
            string countrycode, string postalcode,
            string placename, string state,
            string county, string community,
            string latitude, string longitude, string accuracy)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddGeoData";

            ADOExtenstion.AddParameter(comm, "postalcode",  postalcode);
            ADOExtenstion.AddParameter(comm, "countrycode", countrycode);
            ADOExtenstion.AddParameter(comm, "placename",  placename);
            ADOExtenstion.AddParameter(comm, "state",  state);
            ADOExtenstion.AddParameter(comm, "county",  county);
            ADOExtenstion.AddParameter(comm, "community",  community);
            ADOExtenstion.AddParameter(comm, "latitude",  latitude);
            ADOExtenstion.AddParameter(comm, "longitude", longitude);
            ADOExtenstion.AddParameter(comm, "accuracy",  accuracy);

            // execute the stored procedure
            DbAct.ExecuteScalar(comm);
        }


        public static SiteStructs.CityRegion GetCityRegionForPostalCodeCountry(
            string postalCode, SiteEnums.CountryCodeISO countryCode)
        {
            SiteStructs.CityRegion cr = new SiteStructs.CityRegion();

            cr.CityName = GetCityForCountryPostalCode(postalCode, countryCode);
            cr.Region = GetStateForPostalCode(postalCode, countryCode, true);

            return cr;
        }

        /// <summary>
        /// Check to see if this postal code exists for the country, if it is in GB however, 
        /// the list is not currently updated to validate against
        /// </summary>
        /// <param name="postalCode"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        /// <see cref=">http://www.geonames.org/"/>
        public static bool IsValidPostalCode(string postalCode, SiteEnums.CountryCodeISO countryCode)
        {
            postalCode = postalCode.Trim();

            if (string.IsNullOrEmpty(postalCode) ||
                countryCode == SiteEnums.CountryCodeISO.U0) return false;
            //else if (countryCode != SiteEnums.CountryCodeISO.US) return true; // incomplete list for GB, CA and others

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsValidPostalCode";
            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@postalcode";
            //param.Value = postalCode.Trim();


            if (countryCode == SiteEnums.CountryCodeISO.US)
            {
                param.Value = (postalCode.Length >= 5) ? postalCode.Substring(0, 5) : string.Empty; // it has to be this way for US
            }
            else if (countryCode == SiteEnums.CountryCodeISO.CA)
            {
                param.Value = (postalCode.Length >= 3) ? postalCode.Substring(0, 3) : string.Empty; // it has to be this way for CA
            }
            else if (countryCode == SiteEnums.CountryCodeISO.NZ)
            {
                param.Value = (postalCode.Length >= 4) ? postalCode.Substring(0, 4) : string.Empty; // it has to be this way for NZ
            }
            else if (countryCode.ToString() == "GB" || countryCode == SiteEnums.CountryCodeISO.UK)
            {
                param.Value = (postalCode.Length >= 3) ? postalCode.Substring(0, 3) : string.Empty; // it has to be this way for GB and UK
            }
            else if (countryCode == SiteEnums.CountryCodeISO.JP)
            {
                param.Value = postalCode.Replace("-", string.Empty);
            }
            else
            {
                param.Value = postalCode;
            }


            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@countrycode";
            param.Value = countryCode.ToString();
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            return DbAct.ExecuteScalar(comm) == "1";
        }

        /// <summary>
        /// For the given country and postal code, get the city
        /// </summary>
        /// <param name="postalCode"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public static string GetCityForCountryPostalCode(
            string postalCode,
            SiteEnums.CountryCodeISO countryCode)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCityForCountryPostalCode";
            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@postalcode";
            
            if (countryCode == SiteEnums.CountryCodeISO.US)
            {
                param.Value = (postalCode.Length >= 5) ? postalCode.Substring(0, 5) : string.Empty; // it has to be this way for US
            }
            else if (countryCode == SiteEnums.CountryCodeISO.CA)
            {
                param.Value = (postalCode.Length >= 3) ? postalCode.Substring(0, 3) : string.Empty; // it has to be this way for CA
            }
            else if (countryCode == SiteEnums.CountryCodeISO.NZ)
            {
                param.Value = (postalCode.Length >= 4) ? postalCode.Substring(0, 4) : string.Empty; // it has to be this way for NZ
            }
            else if (countryCode.ToString() == "GB"|| countryCode == SiteEnums.CountryCodeISO.UK)
            {
                param.Value = (postalCode.Length >= 3) ? postalCode.Substring(0, 3) : string.Empty; // it has to be this way for GB and UK
            }
            else
            {
              
                param.Value = postalCode;
            }
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@countrycode";

            if (countryCode == SiteEnums.CountryCodeISO.UK)
            {
                param.Value = "GB"; // it has to be GB for UK
            }
            else
            {
                param.Value = countryCode.ToString();
            }
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm);
        }


        /// <summary>
        /// Get the state for the postal code, country
        /// </summary>
        /// <param name="postalCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="returnStateCode"></param>
        /// <returns></returns>
        public static string GetStateForPostalCode
            (
                                string postalCode,
                                SiteEnums.CountryCodeISO countryCode,
                                bool returnStateCode
            )
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetStateForPostalCode";

            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@";
            param.Value = (postalCode.Length >= 5) ? postalCode.Substring(0, 5) : string.Empty;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@countrycode";
            param.Value = countryCode.ToString();
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            // execute the stored procedure
            string rsult = DbAct.ExecuteScalar(comm);

            if (returnStateCode)
            {
                return GetStateCodeForStateName(rsult);
            }
            else
            {
                return rsult;
            }
        }

        /// <summary>
        /// Get a JSON City State for ZIP
        /// </summary>
        /// <param name="countyCode"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        /// <see cref=">http://www.aspcode.net/JQuery-and-ASPNET-returning-classes-with-JSON.aspx"/>
        public static string JSONCityStateForZip(string countryCode, string zipCode)
        {

            if (string.IsNullOrEmpty(zipCode) || string.IsNullOrEmpty(countryCode))
                return string.Empty;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCityStateForCountryZip";
            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@postalcode";
            param.Value = (zipCode.Length >= 5) ? zipCode.Substring(0, 5) : string.Empty;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@countrycode";
            param.Value = countryCode;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);


            if (dt != null && dt.Rows.Count > 0)
            {

                string city = FromObj.StringFromObj(dt.Rows[0]["placename"]);
                string state = FromObj.StringFromObj(dt.Rows[0]["state"]);

                return @"{""City"":""" + city + @""",""State"":""" + GetStateCodeForStateName(state) + @"""}"; //JSON String
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Given the region name (ex: California), return the region code (ex: CA)
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public static string GetStateCodeForStateName(string stateName)
        {

            switch (stateName)
            {

                // US
                case "Alabama": return "AL";
                case "Alaska": return "AK";
                case "American Samoa": return "AS";
                case "Arizona": return "AZ";
                case "Arkansas": return "AR";
                case "California": return "CA";
                case "Colorado": return "CO";
                case "Connecticut": return "CT";
                case "Delaware": return "DE";
                case "District of Columbia": return "DC";
                case "Florida": return "FL";
                case "Georgia": return "GA";
                case "Guam": return "GU";
                case "Hawaii": return "HI";
                case "Idaho": return "ID";
                case "Illinois": return "IL";
                case "Indiana": return "IN";
                case "Iowa": return "IA";
                case "Kansas": return "KS";
                case "Kentucky": return "KY";
                case "Louisiana": return "LA";
                case "Maine": return "ME";
                case "Marshall Islands": return "MH";
                case "Maryland": return "MD";
                case "Massachusetts": return "MA";
                case "Michigan": return "MI";
                case "Minnesota": return "MN";
                case "Mississippi": return "MS";
                case "Missouri": return "MO";
                case "Montana": return "MT";
                case "Nebraska": return "NE";
                case "Nevada": return "NV";
                case "New Hampshire": return "NH";
                case "New Jersey": return "NJ";
                case "New Mexico": return "NM";
                case "New York": return "NY";
                case "North Carolina": return "NC";
                case "North Dakota": return "ND";
                case "Northern Mariana Islands": return "MP";
                case "Ohio": return "OH";
                case "Oklahoma": return "OK";
                case "Oregon": return "OR";
                case "Palau": return "PW";
                case "Pennsylvania": return "PA";
                case "Puerto Rico": return "PR";
                case "Rhode Island": return "RI";
                case "South Carolina": return "SC";
                case "South Dakota": return "SD";
                case "Tennessee": return "TN";
                case "Texas": return "TX";
                case "Utah": return "UT";
                case "Vermont": return "VT";
                case "Virgin Islands": return "VI";
                case "Virginia": return "VA";
                case "Washington": return "WA";
                case "West Virginia": return "WV";
                case "Wisconsin": return "WI";
                case "Wyoming": return "WY";
                case "ARMED FORCES (AA)": return "AA";
                case "ARMED FORCES (AE)": return "AE";
                case "ARMED FORCES (AP)": return "AP";

                // CA
                case "Albert": return "AB";
                case "Manitoba": return "MB";
                case "New Brunswick": return "NB";
                case "Newfoundland & Labrador": return "NL";
                case "Nova Scotia": return "NS";
                case "Northwest Territories": return "NT";
                case "Nunavut": return "NU";
                case "Ontario": return "ON";
                case "Prince Edward Island": return "PE";
                case "Quebec": return "QC";
                case "Saskatchewan": return "SK";
                case "Yukon Territory": return "YT";


                // AU
                case "Australian Antarctic Territory": return "AAT";
                case "Australian Capital Territory": return "ACT";
                case "Northern Territory": return "YT";
                case "New South Wales": return "NT";
                case "Queensland": return "QLD";
                case "South Australia": return "SA";
                case "Tasmania": return "TAS";
                case "Victoria": return "VIC";
                case "Western Australia": return "WA";

                // UK
                case "Avon": return "AVON";
                case "Bedfordshire": return "BEDS";
                case "Berkshire": return "BERKS";
                case "Buckinghamshire": return "BUCKS";
                case "Cambridgeshire": return "CAMBS";
                case "Cheshire": return "CHESH";
                case "Cleveland": return "CLEVE";
                case "Cornwall": return "CORN";
                case "Cumbria": return "CUMB";
                case "Derbyshire": return "DERBY";
                case "Devon": return "DEVON";
                case "Dorset": return "DORSET";
                case "Durham": return "DURHAM";
                case "Essex": return "ESSEX";
                case "Gloucestershire": return "GLOUS";
                case "Greater London": return "GLONDON";
                case "Greater Manchester": return "GMANCH";
                case "Hampshire": return "HANTS";
                case "Hereford & Worcestershire": return "HERWOR";
                case "Hertfordshire": return "HERTS";
                case "Humberside": return "HUMBER";
                case "Isle of Man": return "IOM";
                case "Isle of Wight": return "IOW";
                case "Kent": return "KENT";
                case "Lancashire": return "LANCS";
                case "Leicestershire": return "LEICS";
                case "Lincolnshire": return "LINCS";
                case "Merseyside": return "MERSEY";
                case "Norfolk": return "NORF";
                case "Northamptonshire": return "NHANTS";
                case "Northumberland": return "NTHUMB";
                case "Nottinghamshire": return "NOTTS";
                case "Oxfordshire": return "OXON";
                case "Shropshire": return "SHROPS";
                case "Somerset": return "SOM";
                case "Staffordshire": return "STAFFS";
                case "Suffolk": return "SUFF";
                case "Surrey": return "SURREY";
                case "Sussex": return "SUSS";
                case "Warwickshire": return "WARKS";
                case "West Midlands": return "WMID";
                case "Wiltshire": return "WILTS";
                case "Yorkshire": return "YORK";


                default:
                    return string.Empty;
            }
        }

        public static SiteEnums.CountryCodeISO GetCountryISOForCountryCode(string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode))
                return SiteEnums.CountryCodeISO.U0;

            SiteEnums.CountryCodeISO theCO = SiteEnums.CountryCodeISO.U0;

            if (Enum.TryParse(countryCode, out theCO))
            {
                return (SiteEnums.CountryCodeISO)Enum.Parse(typeof(SiteEnums.CountryCodeISO), countryCode);
            }
            else
            {
                return SiteEnums.CountryCodeISO.U0;
            }
        }


        public static Dictionary<string, string> GetAllCountries()
        {
            //http://dotnetperls.com/dictionary-keys
            var d = new Dictionary<string, string>();

            foreach (SiteEnums.CountryCodeISO countryISO in Enum.GetValues(typeof(SiteEnums.CountryCodeISO)))
            {
                if (countryISO != SiteEnums.CountryCodeISO.U0 &&
                    countryISO != SiteEnums.CountryCodeISO.RD)
                {
                  //  d.Add(Utilities.GetEnumDescription(countryISO), countryISO.ToString());
                    d.Add( countryISO.ToString(), Utilities.GetEnumDescription(countryISO));
                }
            }

            var items = from k in d.Keys
                        orderby d[k] ascending
                        select k;

            Dictionary<string, string> theList = new Dictionary<string, string>();

            foreach (string k in items)
            {
                theList.Add(k, d[k]);
            }
            return theList;
        }


        public static Dictionary<string, string> GetAllCountriesSorted(string pipedList)
        {
            var d = new Dictionary<string, string>();

            string[] values = pipedList.Split('|');

            SiteEnums.CountryCodeISO countryCode = SiteEnums.CountryCodeISO.U0;

            foreach (string s in values)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    countryCode = (SiteEnums.CountryCodeISO)Enum.Parse(typeof(SiteEnums.CountryCodeISO), s);

                    if (countryCode != SiteEnums.CountryCodeISO.U0 && countryCode != 
                        SiteEnums.CountryCodeISO.RD)
                    {
                        //d.Add(Utilities.GetEnumDescription(countryCode), countryCode.ToString());
                        d.Add(countryCode.ToString(), Utilities.GetEnumDescription(countryCode));
                    }
                }
            }

            var items = from k in d.Keys
                        orderby d[k] ascending
                        select k;

            Dictionary<string, string> theList = new Dictionary<string, string>();

            foreach (string k in items)
            {
                theList.Add(k, d[k]);
            }
            return theList;
        }
        



        #endregion

    }
}