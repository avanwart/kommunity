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
using System.Data;
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Operational.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BootBaronLib.Enums;
using BootBaronLib.Operational;
using BootBaronLib.Interfaces;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserAddress: BaseIUserLogCRUD, ISet
    {
        #region properties 

        private int _userAddressID = 0;

        public int UserAddressID
        {
            get { return _userAddressID; }
            set { _userAddressID = value; }
        }

        private string _firstName = string.Empty;

        public string FirstName
        {
            get {
                if (_firstName == null) return string.Empty;
                return _firstName; }
            set { _firstName = value; }
        }

        private string _middleName = string.Empty;

        public string MiddleName
        {
            get {
                if (_middleName == null) return string.Empty;
                return _middleName; }
            set { _middleName = value; }
        }

        private string _lastName = string.Empty;

        public string LastName
        {
            get {
                if (_lastName == null) return string.Empty;
                return _lastName; }
            set { _lastName = value; }
        }

        private string _addressLine1 = string.Empty;

        public string AddressLine1
        {
            get {
                if (_addressLine1 == null) return string.Empty;
                return _addressLine1.Trim(); }
            set { _addressLine1 = value; }
        }

        private string _addressLine2 = string.Empty;

        public string AddressLine2
        {
            get {
                if (_addressLine2 == null) return string.Empty;
                return _addressLine2.Trim();
            }
            set { _addressLine2 = value; }
        }

        private string _addressLine3 = string.Empty;

        public string AddressLine3
        {
            get {
                if (_addressLine3 == null) return string.Empty;
                return _addressLine3.Trim();
            }
            set { _addressLine3 = value; }
        }

        private string _city = string.Empty;

        public string City
        {
            get {
                if (_city == null) return string.Empty;
                return _city; }
            set { _city = value; }
        }

        private string _region = string.Empty;

        public string Region
        {
            get {
                if (_region == null) return string.Empty;
                return _region; }
            set { _region = value; }
        }

        private string _postalCode = string.Empty;

        public string PostalCode
        {
            get {
                if (_postalCode == null) return string.Empty;
                return _postalCode; }
            set { _postalCode = value; }
        }

        private string _countryISO = string.Empty;

        public string CountryISO
        {
            get {
                if (_countryISO == null) return string.Empty;
                return _countryISO; }
            set { _countryISO = value; }
        }

        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }



        private char _addressStatus = char.MinValue;

        /// <summary>
        /// N = nothing wanted
        /// S = sticker
        /// K = kit
        /// T = t-shirt
        /// U = unprocessed
        /// </summary>
        public char AddressStatus
        {
            get { return _addressStatus; }
            set { _addressStatus = value; }
        }

        private string _choice1 = string.Empty;

        public string Choice1
        {
            get { return _choice1; }
            set { _choice1 = value; }
        }

        private string _choice2 = string.Empty;

        public string Choice2
        {
            get { return _choice2; }
            set { _choice2 = value; }
        }


        #endregion


        public string FormattedAddress
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                if (!string.IsNullOrEmpty(this.FirstName))
                {
                    sb.Append(this.FirstName);
                }

                if (!string.IsNullOrEmpty(this.MiddleName))
                {
                    sb.Append(" ");
                    sb.Append(this.MiddleName);
                }

                if (!string.IsNullOrEmpty(this.LastName))
                {
                    sb.Append(" ");
                    sb.Append(this.LastName);
                }

                sb.Append("<br />");

                if (!string.IsNullOrEmpty(this.AddressLine1))
                {
                    sb.Append(this.AddressLine1);
                }

                if (!string.IsNullOrEmpty(this.AddressLine2))
                {
                    sb.Append("<br />");
                    sb.Append(this.AddressLine2);
                }

                if (!string.IsNullOrEmpty(this.AddressLine3))
                {
                    sb.Append("<br />");
                    sb.Append(this.AddressLine3);
                }

                sb.Append("<br />");

                if (!string.IsNullOrEmpty(this.City))
                {
                    sb.Append(this.City);
                }

                if (!string.IsNullOrEmpty(this.Region))
                {
                    sb.Append(", ");
                    sb.Append(this.Region);
                }

                if (!string.IsNullOrEmpty(this.PostalCode))
                {
                    sb.Append(" ");
                    sb.Append(this.PostalCode);
                }


                sb.Append("<br />");


                if (!string.IsNullOrEmpty(this.CountryISO))
                {
                    SiteEnums.CountryCodeISO coiso = GeoData.GetCountryISOForCountryCode(this.CountryISO);
                    sb.Append(Utilities.GetEnumDescription(coiso));
                }


                return sb.ToString();
            }

        }

        public UserAddress() { }

        public UserAddress(DataRow dr) { Get(dr); }

        public UserAddress(int userAddressID) { Get(userAddressID); }

        #region methods


        public static bool IsBlank(int userAccountID)
        {
            UserAddress uadd = new UserAddress();
            uadd.GetUserAddress(userAccountID);

            return uadd.UserAddressID == 0;
        }

        public override void Get(int userAddressID)
        {

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAddressByID";

            ADOExtenstion.AddParameter(comm, "userAddressID", userAddressID);

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

            ADOExtenstion.AddParameter(comm, "firstName", FirstName);
            ADOExtenstion.AddParameter(comm, "middleName", MiddleName);
            ADOExtenstion.AddParameter(comm, "lastName", LastName);
            ADOExtenstion.AddParameter(comm, "addressLine1", AddressLine1);
            ADOExtenstion.AddParameter(comm, "addressLine2", AddressLine2);
            ADOExtenstion.AddParameter(comm, "addressLine3", AddressLine3);
            ADOExtenstion.AddParameter(comm, "city", City);
            ADOExtenstion.AddParameter(comm, "region", Region);
            ADOExtenstion.AddParameter(comm, "postalCode", PostalCode);
            ADOExtenstion.AddParameter(comm, "countryISO", CountryISO);
            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);
            ADOExtenstion.AddParameter(comm, "addressStatus", AddressStatus);
            ADOExtenstion.AddParameter(comm, "choice1", Choice1);
            ADOExtenstion.AddParameter(comm, "choice2", Choice2);

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
                this.UserAddressID = Convert.ToInt32(result);

                return this.UserAddressID;
            }
        }


        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserAddress";

            ADOExtenstion.AddParameter(comm, "firstName", FirstName);
            ADOExtenstion.AddParameter(comm, "middleName", MiddleName);
            ADOExtenstion.AddParameter(comm, "lastName", LastName);
            ADOExtenstion.AddParameter(comm, "addressLine1", AddressLine1);
            ADOExtenstion.AddParameter(comm, "addressLine2", AddressLine2);
            ADOExtenstion.AddParameter(comm, "addressLine3", AddressLine3);
            ADOExtenstion.AddParameter(comm, "city", City);
            ADOExtenstion.AddParameter(comm, "region", Region);
            ADOExtenstion.AddParameter(comm, "postalCode", PostalCode);
            ADOExtenstion.AddParameter(comm, "countryISO", CountryISO);
            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);
            ADOExtenstion.AddParameter(comm, "addressStatus", AddressStatus);
            ADOExtenstion.AddParameter(comm, "choice1", Choice1);
            ADOExtenstion.AddParameter(comm, "choice2", Choice2);
            ADOExtenstion.AddParameter(comm, "userAddressID", UserAddressID);

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            return result;
        }

        public void GetUserAddress(int userAccountID)
        {
            this.UserAccountID = userAccountID;

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAddress";

            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);

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
                this.UserAddressID = FromObj.IntFromObj(dr["userAddressID"]);
                this.FirstName = FromObj.StringFromObj(dr["firstName"]);
                this.MiddleName = FromObj.StringFromObj(dr["middleName"]);
                this.LastName = FromObj.StringFromObj(dr["lastName"]);
                this.AddressLine1 = FromObj.StringFromObj(dr["addressLine1"]);
                this.AddressLine2 = FromObj.StringFromObj(dr["addressLine2"]);
                this.AddressLine3 = FromObj.StringFromObj(dr["addressLine3"]);
                this.City = FromObj.StringFromObj(dr["city"]);
                this.Region = FromObj.StringFromObj(dr["region"]);
                this.PostalCode = FromObj.StringFromObj(dr["postalCode"]);
                this.CountryISO = FromObj.StringFromObj(dr["countryISO"]);
                this.UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                this.AddressStatus = FromObj.CharFromObj(dr["addressStatus"]);
                this.Choice1 = FromObj.StringFromObj(dr["choice1"]);
                this.Choice2 = FromObj.StringFromObj(dr["choice2"]);

                base.Get(dr);
            }
            catch { }
        }

        public override bool Delete()
        {
            if (this.UserAccountID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserAddress";

            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);
 

         
            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        #endregion


        public bool Set()
        {
            if (this.UserAddressID == 0) return this.Create() > 0;
            else return this.Update();
        }
    }


    public class UserAddresses : List<UserAddress>
    {
        public UserAddresses() { }

        public void GetUserAddressesByStatus(char addressStatus)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAddressesByStatus";


            ADOExtenstion.AddParameter(comm, "addressStatus", addressStatus);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAddress uadd = null;
                foreach (DataRow dr in dt.Rows)
                {
                    uadd = new UserAddress(dr);

                    this.Add(uadd);
                }
            }
        }
    }
}

