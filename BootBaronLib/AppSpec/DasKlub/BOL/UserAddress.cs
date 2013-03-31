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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Values;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserAddress : BaseIUserLogCRUD, ISet
    {
        #region properties 

        private string _addressLine1 = string.Empty;
        private string _addressLine2 = string.Empty;
        private string _addressLine3 = string.Empty;
        private char _addressStatus = char.MinValue;
        private string _choice1 = string.Empty;
        private string _choice2 = string.Empty;
        private string _city = string.Empty;
        private string _countryISO = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;

        private string _middleName = string.Empty;
        private string _postalCode = string.Empty;
        private string _region = string.Empty;
        public int UserAddressID { get; set; }

        public string FirstName
        {
            get
            {
                if (_firstName == null) return string.Empty;
                return _firstName;
            }
            set { _firstName = value; }
        }

        public string MiddleName
        {
            get
            {
                if (_middleName == null) return string.Empty;
                return _middleName;
            }
            set { _middleName = value; }
        }

        public string LastName
        {
            get
            {
                if (_lastName == null) return string.Empty;
                return _lastName;
            }
            set { _lastName = value; }
        }

        public string AddressLine1
        {
            get
            {
                if (_addressLine1 == null) return string.Empty;
                return _addressLine1.Trim();
            }
            set { _addressLine1 = value; }
        }

        public string AddressLine2
        {
            get
            {
                if (_addressLine2 == null) return string.Empty;
                return _addressLine2.Trim();
            }
            set { _addressLine2 = value; }
        }

        public string AddressLine3
        {
            get
            {
                if (_addressLine3 == null) return string.Empty;
                return _addressLine3.Trim();
            }
            set { _addressLine3 = value; }
        }

        public string City
        {
            get
            {
                if (_city == null) return string.Empty;
                return _city;
            }
            set { _city = value; }
        }

        public string Region
        {
            get
            {
                if (_region == null) return string.Empty;
                return _region;
            }
            set { _region = value; }
        }

        public string PostalCode
        {
            get
            {
                if (_postalCode == null) return string.Empty;
                return _postalCode;
            }
            set { _postalCode = value; }
        }

        public string CountryISO
        {
            get
            {
                if (_countryISO == null) return string.Empty;
                return _countryISO;
            }
            set { _countryISO = value; }
        }

        public int UserAccountID { get; set; }


        /// <summary>
        ///     N = nothing wanted
        ///     S = sticker
        ///     K = kit
        ///     T = t-shirt
        ///     U = unprocessed
        /// </summary>
        public char AddressStatus
        {
            get { return _addressStatus; }
            set { _addressStatus = value; }
        }

        public string Choice1
        {
            get { return _choice1; }
            set { _choice1 = value; }
        }

        public string Choice2
        {
            get { return _choice2; }
            set { _choice2 = value; }
        }

        #endregion

        public UserAddress()
        {
        }

        public UserAddress(DataRow dr)
        {
            Get(dr);
        }

        public UserAddress(int userAddressID)
        {
            Get(userAddressID);
        }

        #region methods

        public static bool IsBlank(int userAccountID)
        {
            var uadd = new UserAddress();
            uadd.GetUserAddress(userAccountID);

            return uadd.UserAddressID == 0;
        }

        public override void Get(int userAddressID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAddressByID";

            comm.AddParameter("userAddressID", userAddressID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserAddress";

            comm.AddParameter("firstName", FirstName);
            comm.AddParameter("middleName", MiddleName);
            comm.AddParameter("lastName", LastName);
            comm.AddParameter("addressLine1", AddressLine1);
            comm.AddParameter("addressLine2", AddressLine2);
            comm.AddParameter("addressLine3", AddressLine3);
            comm.AddParameter("city", City);
            comm.AddParameter("region", Region);
            comm.AddParameter("postalCode", PostalCode);
            comm.AddParameter("countryISO", CountryISO);
            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("addressStatus", AddressStatus);
            comm.AddParameter("choice1", Choice1);
            comm.AddParameter("choice2", Choice2);

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
                UserAddressID = Convert.ToInt32(result);

                return UserAddressID;
            }
        }


        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserAddress";

            comm.AddParameter("firstName", FirstName);
            comm.AddParameter("middleName", MiddleName);
            comm.AddParameter("lastName", LastName);
            comm.AddParameter("addressLine1", AddressLine1);
            comm.AddParameter("addressLine2", AddressLine2);
            comm.AddParameter("addressLine3", AddressLine3);
            comm.AddParameter("city", City);
            comm.AddParameter("region", Region);
            comm.AddParameter("postalCode", PostalCode);
            comm.AddParameter("countryISO", CountryISO);
            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("addressStatus", AddressStatus);
            comm.AddParameter("choice1", Choice1);
            comm.AddParameter("choice2", Choice2);
            comm.AddParameter("userAddressID", UserAddressID);

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            return result;
        }

        public void GetUserAddress(int userAccountID)
        {
            UserAccountID = userAccountID;

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAddress";

            comm.AddParameter("userAccountID", UserAccountID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                UserAddressID = FromObj.IntFromObj(dr["userAddressID"]);
                FirstName = FromObj.StringFromObj(dr["firstName"]);
                MiddleName = FromObj.StringFromObj(dr["middleName"]);
                LastName = FromObj.StringFromObj(dr["lastName"]);
                AddressLine1 = FromObj.StringFromObj(dr["addressLine1"]);
                AddressLine2 = FromObj.StringFromObj(dr["addressLine2"]);
                AddressLine3 = FromObj.StringFromObj(dr["addressLine3"]);
                City = FromObj.StringFromObj(dr["city"]);
                Region = FromObj.StringFromObj(dr["region"]);
                PostalCode = FromObj.StringFromObj(dr["postalCode"]);
                CountryISO = FromObj.StringFromObj(dr["countryISO"]);
                UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                AddressStatus = FromObj.CharFromObj(dr["addressStatus"]);
                Choice1 = FromObj.StringFromObj(dr["choice1"]);
                Choice2 = FromObj.StringFromObj(dr["choice2"]);

                base.Get(dr);
            }
            catch
            {
            }
        }

        public override bool Delete()
        {
            if (UserAccountID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserAddress";

            comm.AddParameter("userAccountID", UserAccountID);


            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        #endregion

        public string FormattedAddress
        {
            get
            {
                var sb = new StringBuilder(100);

                if (!string.IsNullOrEmpty(FirstName))
                {
                    sb.Append(FirstName);
                }

                if (!string.IsNullOrEmpty(MiddleName))
                {
                    sb.Append(" ");
                    sb.Append(MiddleName);
                }

                if (!string.IsNullOrEmpty(LastName))
                {
                    sb.Append(" ");
                    sb.Append(LastName);
                }

                sb.Append("<br />");

                if (!string.IsNullOrEmpty(AddressLine1))
                {
                    sb.Append(AddressLine1);
                }

                if (!string.IsNullOrEmpty(AddressLine2))
                {
                    sb.Append("<br />");
                    sb.Append(AddressLine2);
                }

                if (!string.IsNullOrEmpty(AddressLine3))
                {
                    sb.Append("<br />");
                    sb.Append(AddressLine3);
                }

                sb.Append("<br />");

                if (!string.IsNullOrEmpty(City))
                {
                    sb.Append(City);
                }

                if (!string.IsNullOrEmpty(Region))
                {
                    sb.Append(", ");
                    sb.Append(Region);
                }

                if (!string.IsNullOrEmpty(PostalCode))
                {
                    sb.Append(" ");
                    sb.Append(PostalCode);
                }


                sb.Append("<br />");


                if (!string.IsNullOrEmpty(CountryISO))
                {
                    SiteEnums.CountryCodeISO coiso = GeoData.GetCountryISOForCountryCode(CountryISO);
                    sb.Append(Utilities.GetEnumDescription(coiso));
                }


                return sb.ToString();
            }
        }

        public bool Set()
        {
            if (UserAddressID == 0) return Create() > 0;
            else return Update();
        }
    }


    public class UserAddresses : List<UserAddress>
    {
        public void GetUserAddressesByStatus(char addressStatus)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAddressesByStatus";


            comm.AddParameter("addressStatus", addressStatus);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAddress uadd = null;
                foreach (DataRow dr in dt.Rows)
                {
                    uadd = new UserAddress(dr);

                    Add(uadd);
                }
            }
        }
    }
}