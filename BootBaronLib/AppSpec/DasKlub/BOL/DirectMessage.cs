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
using System.Web.Security;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class DirectMessage : BaseIUserLogCRUD, ICacheName, IUnorderdListItem
    {
        #region properties

        private int _directMessageID = 0;

        public int DirectMessageID
        {
            get { return _directMessageID; }
            set { _directMessageID = value; }
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

        private bool _isRead = false;

        public bool IsRead
        {
            get { return _isRead; }
            set { _isRead = value; }
        }

        private string _message = string.Empty;

        public string Message
        {
            get {
                if (_message == null) return _message;
                else  return _message.Trim(); 
            
            }
            set { _message = value; }
        }

        private bool _isEnabled = true;

        public DirectMessage(DataRow dr)
        {
            Get(dr);
        }

        public DirectMessage()
        {
            // TODO: Complete member initialization
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }
        
        #endregion
 
        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        #endregion


        private bool _isInbox = true;

        public bool IsInbox
        {
            get { return _isInbox; }
            set { _isInbox = value; }
        }


        #region methods



        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddDirectMessage";

            ADOExtenstion.AddParameter(comm, "createdByUserID",  CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "fromUserAccountID",  FromUserAccountID);
            ADOExtenstion.AddParameter(comm, "toUserAccountID",  ToUserAccountID);
            ADOExtenstion.AddParameter(comm, "isRead",  IsRead);
            ADOExtenstion.AddParameter(comm, "message",Message);
            ADOExtenstion.AddParameter(comm, "isEnabled",  IsEnabled);

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
                this.DirectMessageID = Convert.ToInt32(result);

                return this.DirectMessageID;
            }
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateDirectMessage";

            ADOExtenstion.AddParameter(comm, "directMessageID",  DirectMessageID);
            ADOExtenstion.AddParameter(comm, "updatedByUserID",  UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "fromUserAccountID",  FromUserAccountID);
            ADOExtenstion.AddParameter(comm, "toUserAccountID",  ToUserAccountID);
            ADOExtenstion.AddParameter(comm, "isRead",  IsRead);
            ADOExtenstion.AddParameter(comm, "message",Message);
            ADOExtenstion.AddParameter(comm, "isEnabled",  IsEnabled);

 

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));
            return result;
        }

    
        public override void Get(DataRow dr)
        {
            try
            {
                this.DirectMessageID = FromObj.IntFromObj(dr["directMessageID"]);
                this.FromUserAccountID = FromObj.IntFromObj(dr["fromUserAccountID"]);
                this.ToUserAccountID = FromObj.IntFromObj(dr["toUserAccountID"]);
                this.IsRead = FromObj.BoolFromObj(dr["isRead"]);
                this.Message = FromObj.StringFromObj(dr["message"]);
                this.IsEnabled = FromObj.BoolFromObj(dr["isEnabled"]);

                base.Get(dr);
            }
            catch { }
        }

       

        #endregion

        public string ToUnorderdListItem
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<li class=""inbox_message"">");

                UserAccount ua = null;

                if (IsInbox)
                {
                    ua = new UserAccount(this.FromUserAccountID);
                }
                else
                {
                    ua = new UserAccount(this.ToUserAccountID);
                }

                UserAccountDetail uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);

                sb.Append(uad.SmallUserIcon);

                sb.Append(@"<div class=""content_message"">");

                if (!this.IsRead)
                {
                    if (this.IsInbox)
                        sb.AppendFormat(@" <span class=""label label-warning"">{0}</span> ", Messages.New);
                    else
                        sb.AppendFormat(@" <span class=""label label-warning"">{0}</span> ", Messages.Unread);
                }
                else
                {
                    sb.AppendFormat(@" <span class=""label"">{0}</span> ", Messages.Read);
                }

                sb.AppendFormat(@"<span title=""{0}"" class=""date_message"">", CreateDate.ToString("o"));
                sb.Append(Utilities.TimeElapsedMessage(CreateDate)); ;
                sb.Append(@"</span>");

                if (UpdateDate != null && UpdateDate != DateTime.MinValue) 
                {
                    sb.AppendFormat(@"<span title=""{0}"" class=""date_message""> (", UpdateDate.ToString("o"));
                    sb.Append(Utilities.TimeElapsedMessage(UpdateDate)); ;
                    sb.Append(@")</span>");
                }

                sb.Append(@"<br />");

                sb.Append(FromString.ReplaceNewLineWithHTML( Utilities.MakeLink(this.Message)));


                MembershipUser mu = Membership.GetUser();

                if (mu != null && Convert.ToInt32(mu.ProviderUserKey) != ua.UserAccountID)
                {
                    // TODO: NO NEED FOR THIS WHEN VIEWING MESSAGES TO THE USER WHERE YOU REPLY
                    sb.Append(@"<br />"); 
                    sb.Append(@"<br />");
                    sb.Append(@" <a class=""btn btn-success"" href=""");
                    sb.AppendFormat(System.Web.VirtualPathUtility.ToAbsolute("~/account/reply/"));
                    sb.Append(ua.UserName);
                    sb.Append(@""">");
                    sb.Append(Messages.Reply);
                    sb.Append(@"</a>");
                }


                sb.Append(@"</div>");

                sb.Append(@"</li>");


                return sb.ToString();
            }
        }

        public void GetMostRecentSentMessage(int fromUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostRecentSentMessage";

            ADOExtenstion.AddParameter(comm, "fromUserAccountID", fromUserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);


            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        private bool _isChat = false;

        public bool IsChat
        {
            get { return _isChat; }
            set { _isChat = value; }
        }

    }

    public class DirectMessages : List<DirectMessage>, IUnorderdList
    {
        int messagereturncount = 20;

        private bool _isChat = false;

        public bool IsChat
        {
            get { return _isChat; }
            set { _isChat = value; }
        }

        public int  GetMailPageWiseToUser(int pageIndex, int pageSize, int toUserAccountID, int fromUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMailPageWiseToUser";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);
            ADOExtenstion.AddParameter(comm, "toUserAccountID", toUserAccountID);
            ADOExtenstion.AddParameter(comm, "fromUserAccountID", fromUserAccountID);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DirectMessage content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new DirectMessage(dr);
                    this.Add(content);
                }
            }

            return recordCount;
        }

        public int GetMailPageWise(int pageIndex, int pageSize,  int toUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMailPageWise";

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

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DirectMessage content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new DirectMessage(dr);
                    this.Add(content);
                }
            }

            return recordCount;
        }



        public int GetMailPageWiseFromUser(int pageIndex, int pageSize, int fromUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMailPageWiseSent";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);
            ADOExtenstion.AddParameter(comm, "fromUserAccountID", fromUserAccountID);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DirectMessage content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new DirectMessage(dr);
                    this.Add(content);
                }
            }

            return recordCount;
        }

        public void GetAllMessagesForUser(int toUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDirectMessagesToUser";

            ADOExtenstion.AddParameter(comm, "toUserAccountID",  toUserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                DirectMessage dm = null;
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (messagereturncount > i)
                    {
                        dm = new DirectMessage(dr);
                        this.Add(dm);
                        i++;
                    }
                }
            }
        }

        public static bool DeleteAllDirectMessages(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteAllDirectMessages";

            ADOExtenstion.AddParameter(comm, "userAccountID",  userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public void GetDirectMessagesToFrom(int fromUserAccountID, int toUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDirectMessagesToFrom";

            ADOExtenstion.AddParameter(comm, "fromUserAccountID",  fromUserAccountID);
            ADOExtenstion.AddParameter(comm, "toUserAccountID",  toUserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                DirectMessage dm = null;
                int i = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (messagereturncount > i)
                    {
                        dm = new DirectMessage(dr);
                        this.Add(dm);
                        i++;
                    }
                }
            }
        }



        public void GetDirectMessagesFromUser(int fromUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDirectMessagesFromUser";

            ADOExtenstion.AddParameter(comm, "fromUserAccountID",  fromUserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                DirectMessage dm = null;

                int i = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (messagereturncount > i)
                    {
                        dm = new DirectMessage(dr);
                        this.Add(dm);
                        i++;
                    }
                }
            }
        }



        public static int GetDirectMessagesToUserCount(MembershipUser mu)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDirectMessagesToUserCount";

            ADOExtenstion.AddParameter(comm, "toUserAccountID",  Convert.ToInt32(mu.ProviderUserKey));

            // execute the stored procedure
            string rslt = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(rslt) && Convert.ToInt32(rslt) == 0) return 0;
            else return Convert.ToInt32(rslt);
        }





       
        private bool _allInInbox = true;

        public bool AllInInbox
        {
            get { return _allInInbox; }
            set { _allInInbox = value; }
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
                StringBuilder sb = new StringBuilder(100);

                if ( IncludeStartAndEndTags) sb.Append(@"<ul id=""mail_items"">");

                foreach (DirectMessage dm in this)
                {
                    if (!AllInInbox)
                    {
                        dm.IsInbox = false;
                    }
                    dm.IsChat = this.IsChat;

                    sb.Append(dm.ToUnorderdListItem);
                }

                if (IncludeStartAndEndTags)  sb.Append(@"</ul>");

                return sb.ToString();
            }
        }


     
    }
}
