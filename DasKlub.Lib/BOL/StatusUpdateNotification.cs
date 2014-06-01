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
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL
{
    public class StatusUpdateNotification : BaseIUserLogCrud, IUnorderdListItem
    {
        #region properties

        private char _responseType = char.MinValue;
        public int StatusUpdateNotificationID { get; set; }

        public char ResponseType
        {
            get { return _responseType; }
            set { _responseType = value; }
        }

        public int StatusUpdateID { get; set; }

        public bool IsRead { get; set; }


        public int UserAccountID { get; set; }

        #endregion

        #region constructors

        public StatusUpdateNotification(DataRow dr)
        {
            Get(dr);
        }

        public StatusUpdateNotification()
        {
        }

        #endregion

        #region methods

        public override bool Update()
        {
            if (StatusUpdateNotificationID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateStatusUpdateNotification";

            // create a new parameter
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateID), StatusUpdateID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UpdatedByUserID), UpdatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsRead), IsRead);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateNotificationID),
                StatusUpdateNotificationID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ResponseType), ResponseType);


            // result will represent the number of changed rows

            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            //RemoveCache();

            return result;
        }


        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusUpdateNotification";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateNotificationID),
                StatusUpdateNotificationID);

            //RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddStatusUpdateNotification";

            // create a new parameter
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateID), StatusUpdateID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsRead), IsRead);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ResponseType), ResponseType);

            // result will represent the number of changed rows


            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            StatusUpdateNotificationID = Convert.ToInt32(result);

            return StatusUpdateNotificationID;
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            StatusUpdateNotificationID =
                FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => StatusUpdateNotificationID)]);
            StatusUpdateID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => StatusUpdateID)]);
            IsRead = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => IsRead)]);
            UserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => UserAccountID)]);
            ResponseType = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => ResponseType)]);
        }

        #endregion

        public string ToUnorderdListItem
        {
            get
            {
                var sb = new StringBuilder(100);

                var uad = new UserAccountDetail();

                if (UpdatedByUserID > 0)
                {
                    uad.GetUserAccountDeailForUser(UpdatedByUserID);
                }
                else
                {
                    uad.GetUserAccountDeailForUser(CreatedByUserID);
                }

                sb.Append("<li>");

                sb.Append(uad.SmallUserIcon);

                sb.Append("<br />");


                if (UpdateDate != DateTime.MinValue && UpdateDate != null)
                {
                    sb.AppendFormat(@"<i>{0}</i>", Utilities.TimeElapsedMessage(UpdateDate));
                }
                else
                {
                    sb.AppendFormat(@"<i>{0}</i>", Utilities.TimeElapsedMessage(CreateDate));
                }

                sb.Append("<br />");

                var rtype =
                    (SiteEnums.ResponseType) Enum.Parse(typeof (SiteEnums.ResponseType), ResponseType.ToString());

                sb.AppendFormat(@"<a class=""notification_link"" href=""{0}"">",
                    VirtualPathUtility.ToAbsolute(string.Format("~/account/statusupdate/{0}", StatusUpdateID)));

                switch (rtype)
                {
                    case SiteEnums.ResponseType.C:
                        sb.Append(Messages.Commented);
                        break;
                    case SiteEnums.ResponseType.A:
                        sb.Append(Messages.Applauded);
                        break;
                    case SiteEnums.ResponseType.B:
                        sb.Append(Messages.BeatenDown);
                        break;
                    default:
                        break;
                }

                sb.Append("</a>");


                sb.Append("</li>");

                return sb.ToString();
            }
        }

        public void GetStatusUpdateNotificationForUserStatus(int userAccountID, int statusUpdateID,
            SiteEnums.ResponseType responseType)
        {
            ResponseType = Convert.ToChar(responseType.ToString());
            UserAccountID = userAccountID;
            StatusUpdateID = statusUpdateID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdateNotificationForUserStatus";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateID), StatusUpdateID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ResponseType), ResponseType);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }
    }


    public class StatusUpdateNotifications : List<StatusUpdateNotification>
    {
        public void GetStatusUpdateNotificationsForUser(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdateNotificationsForUser";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            StatusUpdateNotification sun = null;

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sun = new StatusUpdateNotification(dr);
                    Add(sun);
                }
            }
        }

        public static int GetStatusUpdateNotificationCountForUser(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdateNotificationCountForUser";
            // this is a union on the reverse of this as well

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            string rslt = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(rslt))
            {
                return Convert.ToInt32(rslt);
            }
            return 0;
        }

        public static bool DeleteNotificationsForStatusUpdate(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteNotificationsForStatusUpdate";

            comm.AddParameter("statusUpdateID", statusUpdateID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}