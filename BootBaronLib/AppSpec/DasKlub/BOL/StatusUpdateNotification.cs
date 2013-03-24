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
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Enums;
using BootBaronLib.Interfaces;
using System.Text;
using BootBaronLib.Resources;


namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class StatusUpdateNotification : BaseIUserLogCRUD,  IUnorderdListItem
    {
        #region properties

        private int _statusUpdateNotificationID = 0;

        public int StatusUpdateNotificationID
        {
            get { return _statusUpdateNotificationID; }
            set { _statusUpdateNotificationID = value; }
        }


        private char _responseType = char.MinValue;

        public char ResponseType
        {
            get { return _responseType; }
            set { _responseType = value; }
        }

        private int _statusUpdateID = 0;

        public int StatusUpdateID
        {
            get { return _statusUpdateID; }
            set { _statusUpdateID = value; }
        }

        private bool _isRead = false;

        public bool IsRead
        {
            get { return _isRead; }
            set { _isRead = value; }
        }

        private int _userAccountID = 0;


        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }
        #endregion

        #region constructors

        public StatusUpdateNotification(DataRow dr)
        {
            Get(dr);
        }

        public StatusUpdateNotification() { }


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
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusUpdateID), this.StatusUpdateID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), this.UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IsRead), this.IsRead);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), this.UserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusUpdateNotificationID), this.StatusUpdateNotificationID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ResponseType), this.ResponseType);

            

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

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusUpdateNotificationID), StatusUpdateNotificationID);

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
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusUpdateID), this.StatusUpdateID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), this.CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IsRead), this.IsRead);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), this.UserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ResponseType), this.ResponseType);

            // result will represent the number of changed rows


            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.StatusUpdateNotificationID = Convert.ToInt32(result);

            return this.StatusUpdateNotificationID;
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            this.StatusUpdateNotificationID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.StatusUpdateNotificationID)]);
            this.StatusUpdateID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.StatusUpdateID)]);
            this.IsRead = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.IsRead)]);
            this.UserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.UserAccountID)]);
            this.ResponseType = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ResponseType)]);

        }

        #endregion

        public void GetStatusUpdateNotificationForUserStatus(int userAccountID, int statusUpdateID, SiteEnums.ResponseType  responseType)
        {
            this.ResponseType = Convert.ToChar(responseType.ToString());
            this.UserAccountID = userAccountID;
            this.StatusUpdateID = statusUpdateID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdateNotificationForUserStatus";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), UserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusUpdateID), StatusUpdateID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ResponseType), ResponseType);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public string ToUnorderdListItem
        {
            get {
                StringBuilder sb = new StringBuilder(100);

                UserAccountDetail uad = new UserAccountDetail( );

                if (this.UpdatedByUserID > 0)
                {
                    uad.GetUserAccountDeailForUser(this.UpdatedByUserID);
                }
                else
                {
                    uad.GetUserAccountDeailForUser(this.CreatedByUserID);
                }

                sb.Append("<li>");

                sb.Append(uad.SmallUserIcon);

                sb.Append("<br />");


                if (this.UpdateDate != DateTime.MinValue && this.UpdateDate != null)
                {
                    sb.AppendFormat(@"<i>{0}</i>", Utilities.TimeElapsedMessage(this.UpdateDate));
                }
                else
                {
                    sb.AppendFormat(@"<i>{0}</i>", Utilities.TimeElapsedMessage(this.CreateDate));
                }

                sb.Append("<br />");

                SiteEnums.ResponseType rtype = 
                (SiteEnums.ResponseType)Enum.Parse(typeof(SiteEnums.ResponseType), this.ResponseType.ToString());

                sb.AppendFormat(@"<a class=""notification_link"" href=""{0}"">", 
                    System.Web.VirtualPathUtility.ToAbsolute("~/account/statusupdate/" + this.StatusUpdateID));

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
    }

     
    

    public class StatusUpdateNotifications : List<StatusUpdateNotification>
    {
        public void GetStatusUpdateNotificationsForUser(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdateNotificationsForUser";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);
            
            StatusUpdateNotification sun = null;

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sun = new StatusUpdateNotification(dr);
                    this.Add(sun);
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

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

            // execute the stored procedure
            string rslt = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(rslt))
            {
                return Convert.ToInt32(rslt);
            }
            else return 0;
            
        }

        public static bool DeleteNotificationsForStatusUpdate(int statusUpdateID)
        {
             // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteNotificationsForStatusUpdate";

            ADOExtenstion.AddParameter(comm, "statusUpdateID", statusUpdateID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
           
        }
    }
}
