using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;

namespace DasKlub.Lib.BOL
{
    public class WallMessage : BaseIUserLogCrud, IUnorderdListItem
    {
        #region constructors

        public WallMessage()
        {
        }

        public WallMessage(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        #region properties

        private string _message = string.Empty;
        public int WallMessageID { get; set; }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public bool IsRead { get; set; }

        public int FromUserAccountID { get; set; }

        public int ToUserAccountID { get; set; }

        #endregion

        #region methods

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            FromUserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => FromUserAccountID)]);
            IsRead = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => IsRead)]);
            Message = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Message)]);
            ToUserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ToUserAccountID)]);
            WallMessageID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => WallMessageID)]);
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddWallMessage";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Message), Message);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsRead), IsRead);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ToUserAccountID), ToUserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FromUserAccountID), FromUserAccountID);

            // the result is their ID
            string result = string.Empty;

            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            WallMessageID = Convert.ToInt32(result);

            return WallMessageID;
        }


        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteWallMessage";

            comm.AddParameter("wallMessageID", WallMessageID);

            //RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        #endregion

        public bool IsUsersWall { get; set; }

        public string ToUnorderdListItem
        {
            get
            {
                var sb = new StringBuilder(100);

                var ua = new UserAccount(FromUserAccountID);
                bool isUsersPost = false;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    var currentUser = new UserAccount(HttpContext.Current.User.Identity.Name);

                    if (ToUserAccountID == currentUser.UserAccountID || FromUserAccountID == currentUser.UserAccountID)
                    {
                        isUsersPost = true;
                    }
                }


                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);

                sb.Append(@"<li class=""status_post"">");

                sb.AppendFormat(@"<div class=""user_account_thumb"">{0}</div>", uad.SmallUserIcon);


                string timeElapsed = Utilities.TimeElapsedMessage(CreateDate);


                sb.AppendFormat(@"<i title=""{1}"">{0}</i>", timeElapsed, CreateDate.ToString("o"));


                if (!string.IsNullOrWhiteSpace(Message))
                {
                    sb.Append("<br />");
                }
                sb.Append("<br />");


                sb.Append(Message);

                sb.Append("<br />");
                sb.Append(@"<div class=""clear""></div>");

                if (isUsersPost || IsUsersWall)
                {
                    sb.AppendFormat(@"<a class=""delete_icon btn btn-danger btn-mini"" href=""{0}"">{1}</a>",
                        VirtualPathUtility.ToAbsolute(string.Format("~/{0}/deletewallitem/{1}", ua.UserNameLower, WallMessageID)),
                        Messages.Delete);
                }

                sb.Append(@"<div class=""clear""></div>");

                sb.Append("</li>");

                return sb.ToString();
            }
        }
    }

    public class WallMessages : List<WallMessage>, IUnorderdList
    {
        private bool _includeStartAndEndTags = true;
        public bool IsUsersWall { get; set; }

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

                if (IncludeStartAndEndTags) sb.Append(@"<ul id=""status_update_list_items"">");

                foreach (WallMessage su in this)
                {
                    sb.Append(su.ToUnorderdListItem);
                }

                if (IncludeStartAndEndTags) sb.Append(@"</ul>");

                return sb.ToString();
            }
        }

        public static bool DeleteUserWall(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserWall";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
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

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);
            comm.AddParameter("toUserAccountID", toUserAccountID);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds.Tables[0].Rows.Count > 0)
            {
                WallMessage statup = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    statup = new WallMessage(dr);
                    Add(statup);
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

            comm.AddParameter("toUserAccountID", toUserAccountID);

            // execute the stored procedure
            string rslt = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(rslt))
            {
                return Convert.ToInt32(rslt);
            }
            return 0;
        }
    }
}