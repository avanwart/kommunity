﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL
{
    public class StatusComment : BaseIUserLogCrud, ICacheName, IUnorderdListItem
    {
        #region properties

        private string _message = string.Empty;
        private char _statusType = char.MinValue;
        public int StatusCommentID { get; set; }

        public int StatusUpdateID { get; set; }

        public int UserAccountID { get; set; }

        public char StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        public string Message
        {
            get { return _message == null ? _message : _message.Trim(); }
            set { _message = value; }
        }

        #endregion

        #region constructors

        public StatusComment()
        {
        }

        public StatusComment(int statusCommentID)
        {
            Get(statusCommentID);
        }

        public StatusComment(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        #region methods

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddStatusComment";

            comm.AddParameter("statusUpdateID", StatusUpdateID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("statusType", StatusType);
            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("message", Message);

            // the result is their ID
            // execute the stored procedure
            string result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            StatusCommentID = Convert.ToInt32(result);

            return StatusCommentID;
        }

        public override bool Delete()
        {
            if (StatusCommentID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusComment";

            comm.AddParameter("statusCommentID", StatusCommentID);

            //RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                Message = FromObj.StringFromObj(dr["message"]);
                StatusType = FromObj.CharFromObj(dr["statusType"]);
                StatusUpdateID = FromObj.IntFromObj(dr["statusUpdateID"]);
                UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                StatusCommentID = FromObj.IntFromObj(dr["statusCommentID"]);
            }
            catch
            {
            }
        }

        public override void Get(int uniqueID)
        {
            StatusCommentID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetStatusComment";

            comm.AddParameter("statusCommentID", StatusCommentID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        #endregion

        public string StatusCommentAcknowledgementsOptions
        {
            get
            {
                var sb = new StringBuilder(100);

                MembershipUser mu = Membership.GetUser();

                if (mu == null) return string.Empty;

                if (StatusCommentAcknowledgement.IsUserCommentAcknowledgement(StatusCommentID,
                    Convert.ToInt32(mu.ProviderUserKey)))
                {
                    sb.Append(@"<div class=""left_float"">");

                    sb.Append(@"<span class=""status_comment_count_applaud"">");
                    sb.Append(StatusCommentAcknowledgements.GetCommentAcknowledgementCount(StatusCommentID,
                        Convert.ToChar(
                            SiteEnums.AcknowledgementType
                                .A.ToString())));
                    sb.Append(@"</span>");

                    var ack = new StatusCommentAcknowledgement();
                    ack.GetCommentAcknowledgement(StatusCommentID, Convert.ToInt32(mu.ProviderUserKey));

                    if (ack.StatusCommentAcknowledgementID > 0 &&
                        ack.AcknowledgementType == Convert.ToChar(SiteEnums.AcknowledgementType.A.ToString()))
                    {
                        sb.AppendFormat(@"<button title=""{0}"" name=""status_comment_update_id_applaud""",
                            Messages.YouResponded);

                        sb.Append(@"  class=""applaud_status_comment_complete""  type=""button"" value=""");
                        sb.Append(StatusCommentID.ToString());
                        sb.AppendFormat(@""">{0}</button>", Messages.Applaud);
                    }
                    else
                    {
                        sb.AppendFormat(@"<button title=""{0}"" name=""status_comment_update_id_applaud""",
                            Messages.YouResponded);

                        sb.Append(@"   class=""applaud_status_comment""  type=""button"" value=""");
                        sb.Append(StatusCommentID.ToString());
                        sb.AppendFormat(@""">{0}</button>", Messages.Applaud);
                    }

                    sb.Append(@"</div>");

                    sb.Append(@"<div class=""left_float"">");


                    sb.Append(@"</div>");
                }
                else
                {
                    sb.Append(@"<div class=""left_float"">");

                    sb.Append(@"<span class=""status_comment_count_applaud"">");
                    sb.Append(StatusCommentAcknowledgements.GetCommentAcknowledgementCount(StatusCommentID,
                        Convert.ToChar(
                            SiteEnums.AcknowledgementType
                                .A.ToString())));
                    sb.Append(@"</span>");
                    sb.AppendFormat(@"<button title=""{0}"" name=""status_comment_update_id_applaud""", Messages.Applaud);
                    sb.Append(@" class=""applaud_status_comment"" type=""button"" value=""");
                    sb.Append(StatusCommentID.ToString());
                    sb.AppendFormat(@""">{0}</button>", Messages.Applaud);

                    sb.Append(@"</div>");

                    sb.Append(@"<div class=""left_float"">");

                    sb.Append(@"</div>");
                }

                return sb.ToString();
            }
        }

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public string ToUnorderdListItem
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(@"<li class=""status_com"" ");
                sb.Append(@" id=""status_com_id_");
                sb.Append(StatusCommentID.ToString());
                sb.Append(@""">");
                sb.Append(@"<div class=""inner_status_com"">");

                var uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(UserAccountID);

                sb.Append(@"<div>");

                sb.AppendFormat(@"<div class=""user_account_thumb"">{0}</div>", uad.SmallUserIcon);


                // comment acknowledgements
                sb.AppendFormat(
                    @"<div class=""acknowlege_options left_float""><div id=""status_comment_ack_{0}"">{1}</div></div>",
                    StatusCommentID, StatusCommentAcknowledgementsOptions);

                sb.Append(@"</div>");

                sb.Append(@"<div class=""clear""></div>");

                sb.AppendFormat(@"<i title=""{1}"">{0}</i>", Utilities.TimeElapsedMessage(CreateDate),
                    CreateDate.ToString("o"));

                sb.Append("<br />");

                sb.Append(Utilities.MakeLink(FromString.ReplaceNewLineSingleWithHtml(Message), true));

                var currentUser = new UserAccount(HttpContext.Current.User.Identity.Name);

                if (currentUser.UserAccountID != 0 && UserAccountID == currentUser.UserAccountID)
                {
                    sb.Append("<br />");

                    sb.AppendFormat(@"<button title=""{0}"" name=""delete_status_comment_id""", Messages.Delete);
                    sb.Append(@" class=""btn btn-mini btn-danger delete_icon_small"" type=""button"" value=""");
                    sb.Append(StatusCommentID.ToString());
                    sb.AppendFormat(@""">{0}</button>", Messages.Delete);
                }

                sb.Append(@"<div class=""clear""></div>");

                sb.Append(@"</div>");


                sb.Append(@"</li>");

                return sb.ToString();
            }
        }

        public void GetStatusCommentMessage()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetStatusCommentMessage";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateID), StatusUpdateID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Message), Message);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }
    }

    public class StatusComments : List<StatusComment>, IUnorderdList
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
                if (Count == 0) return string.Empty;

                var sb = new StringBuilder(100);

                if (IncludeStartAndEndTags) sb.Append("<ul>");

                foreach (StatusComment stcom in this)
                {
                    sb.Append(stcom.ToUnorderdListItem);
                }

                if (IncludeStartAndEndTags) sb.Append("</ul>");

                return sb.ToString();
            }
        }

        public static int GetMostCommentedOnStatus(DateTime beginDate)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostCommentedOnStatus";

            comm.AddParameter("beginDate", beginDate);

            // execute the stored procedure
            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str)) return 0;
            
            return
                FromObj.IntFromObj(str);
        }


        public static int GetStatusCommentCount(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetStatusCommentCount";

            comm.AddParameter("statusUpdateID", statusUpdateID);

            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str)) return 0;

            return Convert.ToInt32(str) > 0 ? Convert.ToInt32(str) : 0;
        }

        public void GetAllStatusCommentsForUpdate(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllStatusCommentsForUpdate";

            comm.AddParameter("statusUpdateID", statusUpdateID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (StatusComment statusCom in from DataRow dr in dt.Rows select new StatusComment(dr))
            {
                Add(statusCom);
            }
        }

        public static bool DeleteStatusComments(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusComments";

            comm.AddParameter("statusUpdateID", statusUpdateID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public static bool DeleteStatusCommentsForUser(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusCommentsForUser";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}