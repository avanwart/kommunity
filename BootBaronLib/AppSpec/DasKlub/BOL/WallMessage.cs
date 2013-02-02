//  Copyright 2012 
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
using BootBaronLib.Operational.Converters;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class WallMessage : BaseIUserLogCRUD, IUnorderdListItem
    {
        #region constructors

        public WallMessage() { }

        public WallMessage(DataRow dr) { Get(dr); }

        #endregion

        #region properties

        private int _wallMessageID = 0;

        public int WallMessageID
        {
            get { return _wallMessageID; }
            set { _wallMessageID = value; }
        }

        private string _message = string.Empty;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private bool _isRead = false;

        public bool IsRead
        {
            get { return _isRead; }
            set { _isRead = value; }
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

        #endregion

        #region methods

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            this.FromUserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FromUserAccountID)]);
            this.IsRead = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.IsRead)]);
            this.Message = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Message)]);
            this.ToUserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ToUserAccountID)]);
            this.WallMessageID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.WallMessageID)]);
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddWallMessage";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Message), Message);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IsRead), IsRead);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ToUserAccountID), ToUserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FromUserAccountID), FromUserAccountID);

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
                this.WallMessageID = Convert.ToInt32(result);

                return this.WallMessageID;
            }
        }


        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteWallMessage";

            ADOExtenstion.AddParameter(comm, "wallMessageID", this.WallMessageID);

            //RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        #endregion


        public string ToUnorderdListItem
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                UserAccount ua = new UserAccount(this.FromUserAccountID);
                bool isUsersPost = false;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    UserAccount currentUser = new UserAccount(HttpContext.Current.User.Identity.Name);

                    if (this.ToUserAccountID == currentUser.UserAccountID || this.FromUserAccountID == currentUser.UserAccountID)
                    {
                        isUsersPost = true;
                    }
                }


                UserAccountDetail uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);

                sb.Append(@"<li class=""status_post"">");

                sb.AppendFormat(@"<div class=""user_account_thumb"">{0}</div>", uad.SmallUserIcon);


                string timeElapsed = Utilities.TimeElapsedMessage(CreateDate);


                sb.AppendFormat(@"<i title=""{1}"">{0}</i>", timeElapsed, CreateDate.ToString("o"));


                if (!string.IsNullOrWhiteSpace(this.Message))
                {
                    sb.Append("<br />");
                }
                sb.Append("<br />");


                sb.Append(this.Message);

                sb.Append("<br />");
                sb.Append(@"<div class=""clear""></div>");

                if (isUsersPost || this.IsUsersWall)
                {
                    sb.AppendFormat(@"<a class=""delete_icon btn btn-danger btn-mini"" href=""{0}"">{1}</a>", 
                        System.Web.VirtualPathUtility.ToAbsolute("~/" + ua.UserName + "/deletewallitem/" + this.WallMessageID.ToString()), 
                        Messages.Delete); 
                }

                sb.Append(@"<div class=""clear""></div>");

                sb.Append("</li>");

                return sb.ToString();

            }
        }

        private bool _isUsersWall = false;

        public bool IsUsersWall
        {
            get { return _isUsersWall; }
            set { _isUsersWall = value; }
        }
    }

    public class WallMessages : List<WallMessage>, IUnorderdList
    {
        public WallMessages() { }

        public static bool DeleteUserWall(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserWall";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public string ToUnorderdList
        {
            get
            {
                if (this.Count == 0) return string.Empty;

                StringBuilder sb = new StringBuilder(100);

                if (IncludeStartAndEndTags) sb.Append(@"<ul id=""status_update_list_items"">");

                foreach (WallMessage su in this)
                {
                    sb.Append(su.ToUnorderdListItem);
                }

                if (IncludeStartAndEndTags) sb.Append(@"</ul>");

                return sb.ToString();

            }
        }



        public int GetWallMessagessPageWise(int pageIndex, int pageSize, int toUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetWallMessagessPageWise";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);
            ADOExtenstion.AddParameter(comm, "toUserAccountID", toUserAccountID);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds.Tables[0].Rows.Count > 0)
            {
                WallMessage statup = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    statup = new WallMessage(dr);
                    this.Add(statup);
                }
            }

            return recordCount;
        }


        public int GetWallMessagesUserCountUnread(int toUserAccountID)
        {
           
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetWallMessagesUserCountUnread";
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


        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }


        private bool _isUsersWall = false;

        public bool IsUsersWall
        {
            get { return _isUsersWall; }
            set { _isUsersWall = value; }
        }

    

       
    }
}
