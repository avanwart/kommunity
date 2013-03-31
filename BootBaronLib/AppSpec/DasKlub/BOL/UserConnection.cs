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
using System.Web;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserConnection : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private char _statusType = char.MinValue;


        public UserConnection(DataRow dr)
        {
            Get(dr);
        }

        public UserConnection()
        {
            // TODO: Complete member initialization
        }

        public int UserConnectionID { get; set; }

        public int FromUserAccountID { get; set; }

        public int ToUserAccountID { get; set; }


        public bool IsConfirmed { get; set; }

        /// <summary>
        ///     The type of connection
        ///     C = cyber contact
        ///     L = later/ never
        ///     R = met in real life
        ///     Z = blocked
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
                switch (StatusType)
                {
                    case 'R':
                        return Messages.RealLifeContacts;
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
                var sb = new StringBuilder(100);

                sb.Append(@"<img src=""");

                switch (StatusType)
                {
                    case 'R':
                        if (IsConfirmed)
                        {
                            sb.Append(VirtualPathUtility.ToAbsolute(
                                @"~/content/images/userstatus/handprint_check.png"));
                        }
                        else
                        {
                            sb.Append(VirtualPathUtility.ToAbsolute(
                                @"~/content/images/userstatus/handprint_hourglass.png"));
                        }
                        break;
                    case 'C':
                        if (IsConfirmed)
                        {
                            sb.Append(VirtualPathUtility.ToAbsolute(
                                @"~/content/images/userstatus/keyboard_check.png"));
                        }
                        else
                        {
                            sb.Append(VirtualPathUtility.ToAbsolute(
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
            UserConnectionID = uniqueID;

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserConnectionByID";

            comm.AddParameter("userConnectionID", UserConnectionID);

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

            comm.AddParameter("fromUserAccountID", FromUserAccountID);
            comm.AddParameter("toUserAccountID", ToUserAccountID);
            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("statusType", StatusType);
            comm.AddParameter("isConfirmed", IsConfirmed);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            UserConnectionID = Convert.ToInt32(result);

            return UserConnectionID;
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                IsConfirmed = FromObj.BoolFromObj(dr["isConfirmed"]);
                UserConnectionID = FromObj.IntFromObj(dr["userConnectionID"]);
                FromUserAccountID = FromObj.IntFromObj(dr["fromUserAccountID"]);
                ToUserAccountID = FromObj.IntFromObj(dr["toUserAccountID"]);
                StatusType = FromObj.CharFromObj(dr["statusType"]);
            }
            catch
            {
            }
        }

        public override bool Update()
        {
            if (UserConnectionID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserConnection";


            comm.AddParameter("fromUserAccountID", FromUserAccountID);
            comm.AddParameter("toUserAccountID", ToUserAccountID);
            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("statusType", StatusType);
            comm.AddParameter("isConfirmed", IsConfirmed);
            comm.AddParameter("userConnectionID", UserConnectionID);

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            RemoveCache();

            return result;
        }


        public bool Delete(bool deleteAll)
        {
            if (UserAccountID == 0) return false;
            if (deleteAll)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_DeleteUserConnection";

                comm.AddParameter("userAccountID", UserAccountID);

                RemoveCache();

                // execute the stored procedure
                return DbAct.ExecuteNonQuery(comm) > 0;
            }
            else return Delete();
        }


        public override bool Delete()
        {
            if (UserConnectionID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserConnectionByID";

            comm.AddParameter("userConnectionID", UserConnectionID);

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
            comm.AddParameter("fromUserAccountID", fromUserAccountID);
            comm.AddParameter("toUserAccountID", toUserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        #endregion

        /// <summary>
        ///     regarding this user
        /// </summary>
        public int UserAccountID { get; set; }

        #region constructors 

        public UserConnection(int userConnectionID)
        {
            Get(userConnectionID);
        }

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
                var sb = new StringBuilder();

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

                    sb.AppendFormat(@"<form method=""post"" action=""{0}?rslt=1&username=",
                                    VirtualPathUtility.ToAbsolute("~/account/contactrequest/"));
                    sb.Append(ua1.UserName);
                    sb.Append(@"&contacttype=");
                    sb.Append(uc1.StatusType);
                    sb.Append(@""">");
                    sb.AppendFormat(
                        @"<input name=""contact_request"" class=""btn btn-success"" type=""submit"" value=""{0}"" /></form>",
                        Messages.Confirm);

                    sb.AppendFormat(@"<form method=""post"" action=""{0}?rslt=0&username=",
                                    VirtualPathUtility.ToAbsolute("~/account/contactrequest/"));
                    sb.Append(ua1.UserName);
                    sb.Append(@"&contacttype=");
                    sb.Append(uc1.StatusType);
                    sb.Append(@""">");
                    sb.AppendFormat(
                        @"<input name=""contact_request"" class=""btn btn-danger"" type=""submit"" value=""{0}"" /></form>",
                        Messages.NotNow);


                    sb.Append(@"</div>");

                    sb.Append(@"</div>");

                    sb.Append(@"</li>");
                }

                if (IncludeStartAndEndTags) sb.Append("</ul>");

                return sb.ToString();
            }
        }

        public static int GetCountUnconfirmedConnections(int toUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCountUnconfirmedConnections";
            // this is a union on the reverse of this as well

            comm.AddParameter("toUserAccountID", toUserAccountID);

            // execute the stored procedure
            string rslt = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(rslt))
            {
                return Convert.ToInt32(rslt);
            }
            else return 0;
        }

        public void GetUserConnections(int fromUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserConnection";
            // this is a union on the reverse of this as well

            comm.AddParameter("fromUserAccountID", fromUserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserConnection usercon = null;

                foreach (DataRow dr in dt.Rows)
                {
                    usercon = new UserConnection(dr);

                    Add(usercon);
                }
            }
        }
    }
}