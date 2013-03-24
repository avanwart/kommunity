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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational.Converters;
using System.Text;
using BootBaronLib.Operational;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserConnection : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private int _userConnectionID = 0;

        public int UserConnectionID
        {
            get { return _userConnectionID; }
            set { _userConnectionID = value; }
        }

        private int _fromUserAccountID = 0;

        public int FromUserAccountID
        {
            get { return _fromUserAccountID; }
            set { _fromUserAccountID = value; }
        }

        private int _toUserAccountID = 0;

        public int ToUserAccountID
        {
            get { return _toUserAccountID; }
            set { _toUserAccountID = value; }
        }


        private bool _isConfirmed = false;

        public bool IsConfirmed
        {
            get { return _isConfirmed; }
            set { _isConfirmed = value; }
        }

        private char _statusType = char.MinValue;


        public UserConnection(DataRow dr)
        {
            Get(dr);
        }

        public UserConnection()
        {
            // TODO: Complete member initialization
        }

        /// <summary>
        /// The type of connection 
        /// C = cyber contact
        /// L = later/ never
        /// R = met in real life
        /// Z = blocked
        /// </summary>
        public char StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        #endregion

        #region extended properties

        public string Name
        {
            get
            {
                switch (this.StatusType)
                {
                    case 'R':
                        return  Messages.RealLifeContacts;
                    case 'C':
                        return Messages.CyberAssociates;
                    case 'Z':
                        return Messages.BlockedUsers;
                    case 'L':
                        return Messages.NotNow;
                    default:
                        return string.Empty;
                }

            }
        }

        public string Icon
        {
            get 
            {
                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<img src=""");

                switch (this.StatusType)
                {
                    case 'R':
                        if (this.IsConfirmed)
                        {
                            sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            @"~/content/images/userstatus/handprint_check.png"));
                        }
                        else
                        {
                            sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            @"~/content/images/userstatus/handprint_hourglass.png"));
                        }
                        break;
                    case 'C':
                        if (this.IsConfirmed)
                        {
                            sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            @"~/content/images/userstatus/keyboard_check.png"));
                        }
                        else
                        {
                            sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            @"~/content/images/userstatus/keyboard_hourglass.png"));
                        }
                        break;
                    case 'Z':
                    case 'L':
                        return string.Empty;
                }

                sb.AppendFormat(@""" alt=""{0}"" title=""{0}"" />", Messages.Contact);

                return sb.ToString();
            }
        }

        #endregion

        #region methods 


        public override void Get(int uniqueID)
        {
            this.UserConnectionID = uniqueID;

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserConnectionByID";

            ADOExtenstion.AddParameter(comm, "userConnectionID", UserConnectionID);

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
            comm.CommandText = "up_AddUserConnection";

            ADOExtenstion.AddParameter(comm, "fromUserAccountID", FromUserAccountID);
            ADOExtenstion.AddParameter(comm, "toUserAccountID", ToUserAccountID);
            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "statusType",  StatusType);
            ADOExtenstion.AddParameter(comm, "isConfirmed",  IsConfirmed);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.UserConnectionID = Convert.ToInt32(result);

            return this.UserConnectionID;
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.IsConfirmed = FromObj.BoolFromObj(dr["isConfirmed"]);
                this.UserConnectionID = FromObj.IntFromObj(dr["userConnectionID"]);
                this.FromUserAccountID = FromObj.IntFromObj(dr["fromUserAccountID"]);
                this.ToUserAccountID = FromObj.IntFromObj(dr["toUserAccountID"]);
                this.StatusType = FromObj.CharFromObj(dr["statusType"]);
            }
            catch
            {

            }
        }

        public override bool Update()
        {
            if (this.UserConnectionID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserConnection";


            ADOExtenstion.AddParameter(comm, "fromUserAccountID",  FromUserAccountID);
            ADOExtenstion.AddParameter(comm, "toUserAccountID", ToUserAccountID);
            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "statusType", StatusType);
            ADOExtenstion.AddParameter(comm, "isConfirmed", IsConfirmed);
            ADOExtenstion.AddParameter(comm, "userConnectionID",  UserConnectionID);

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            RemoveCache();

            return result;
        }


        public   bool Delete(bool deleteAll)
        {
            if (this.UserAccountID == 0) return false;
            if (deleteAll)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_DeleteUserConnection";

                ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);

                RemoveCache();

                // execute the stored procedure
                return DbAct.ExecuteNonQuery(comm) > 0;
            }
            else return Delete();
        }


        public override bool Delete()
        {
            if (this.UserConnectionID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserConnectionByID";

            ADOExtenstion.AddParameter(comm, "userConnectionID", this.UserConnectionID);

            RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public void GetUserToUserConnection(int fromUserAccountID, int toUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserToUserConnection";

            // this is a union on the reverse of this as well
            ADOExtenstion.AddParameter(comm, "fromUserAccountID", fromUserAccountID);
            ADOExtenstion.AddParameter(comm, "toUserAccountID", toUserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        #endregion

        private int _userAccountID = 0;
        
        /// <summary>
        /// regarding this user
        /// </summary>
        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }

        #region constructors 


        public UserConnection(int userConnectionID) { Get(userConnectionID); }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Empty; }
        }

        public void RemoveCache()
        {
            return;
        }

        #endregion
    }

    public class UserConnections : List<UserConnection>, IUnorderdList
    {

        public static int GetCountUnconfirmedConnections(int toUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCountUnconfirmedConnections";
            // this is a union on the reverse of this as well

            ADOExtenstion.AddParameter(comm, "toUserAccountID", toUserAccountID);

            // execute the stored procedure
            string rslt = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(rslt))
            {
                return Convert.ToInt32(rslt);
            }
            else return 0;
        }

        public void GetUserConnections(int fromUserAccountID )
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserConnection"; 
            // this is a union on the reverse of this as well

            ADOExtenstion.AddParameter(comm, "fromUserAccountID", fromUserAccountID);
      
            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserConnection usercon = null;

                foreach (DataRow dr in dt.Rows)
                {
                    usercon = new UserConnection(dr);

                    this.Add(usercon);
                }
            }

        }


        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }

        public string ToUnorderdList
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (IncludeStartAndEndTags) sb.Append(@"<ul>");
                    // sb.Append(@"<ul class=""user_list"">");

                UserAccount ua1 = null;
                UserAccountDetail uad = null;

                foreach (UserConnection uc1 in this)
                {
                    sb.Append(@"<li>");

            
                    ua1 = new UserAccount(uc1.FromUserAccountID);
                    uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(ua1.UserAccountID);

                    sb.Append(@"<div class=""row"">");

                    sb.Append(@"<div class=""span1"">");
                    sb.Append(uad.SmallUserIcon);
                    sb.Append(@"</div>");

                    sb.Append(@"<div class=""span1"">");
                 
                    sb.Append(uc1.Name);
                    sb.Append("<br />");
                    sb.Append(uc1.Icon);

                    sb.Append(@"</div>");


                    sb.Append(@"<div class=""span2"">");

                    sb.AppendFormat(@"<form method=""post"" action=""{0}?rslt=1&username=",  System.Web.VirtualPathUtility.ToAbsolute("~/account/contactrequest/"));
                    sb.Append(ua1.UserName);
                    sb.Append(@"&contacttype=");
                    sb.Append(uc1.StatusType);
                    sb.Append(@""">");
                    sb.AppendFormat(@"<input name=""contact_request"" class=""btn btn-success"" type=""submit"" value=""{0}"" /></form>", Messages.Confirm);
 
                    sb.AppendFormat(@"<form method=""post"" action=""{0}?rslt=0&username=", System.Web.VirtualPathUtility.ToAbsolute("~/account/contactrequest/"));
                    sb.Append(ua1.UserName);
                    sb.Append(@"&contacttype=");
                    sb.Append(uc1.StatusType);
                    sb.Append(@""">");
                    sb.AppendFormat(@"<input name=""contact_request"" class=""btn btn-danger"" type=""submit"" value=""{0}"" /></form>", Messages.NotNow);



                    sb.Append(@"</div>");

                    sb.Append(@"</div>");

                    sb.Append(@"</li>");
                }

                if ( IncludeStartAndEndTags) sb.Append("</ul>");

                return sb.ToString();
            }
        }
    }
}
