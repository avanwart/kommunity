using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class ChatRoomUser : BaseIUserLogCrud, IUnorderdListItem
    {
        #region properties

        private string _connectionCode = string.Empty;
        private string _ipAddress = string.Empty;
        public int ChatRoomUserID { get; set; }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public int RoomID { get; set; }


        public string ConnectionCode
        {
            get { return _connectionCode; }
            set { _connectionCode = value; }
        }

        #endregion

        public ChatRoomUser()
        {
        }

        public ChatRoomUser(DataRow dr)
        {
            Get(dr);
        }

        #region methods

        public void GetChatRoomUserByConnection(string connectionCode)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetChatRoomUserByConnection";

            comm.AddParameter("connectionCode", connectionCode);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
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
            comm.CommandText = "up_AddChatRoomUser";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("ipAddress", IpAddress);
            comm.AddParameter("roomID", RoomID);
            comm.AddParameter("connectionCode", ConnectionCode);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            ChatRoomUserID = Convert.ToInt32(result);

            return ChatRoomUserID;
        }


        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateChatRoomUser";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UpdatedByUserID), UpdatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IpAddress), IpAddress);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => RoomID), RoomID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ConnectionCode), ConnectionCode);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            //RemoveCache();

            return (result != -1);
        }


        public bool DeleteChatRoomUser()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteChatRoomUser";

            comm.AddParameter("connectionCode", ConnectionCode);

            //RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override sealed void Get(DataRow dr)
        {
            base.Get(dr);

            ChatRoomUserID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ChatRoomUserID)]);
            ConnectionCode = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ConnectionCode)]);
            IpAddress = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => IpAddress)]);
            RoomID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => RoomID)]);
        }

        #endregion

        public string ToUnorderdListItem
        {
            get
            {
                if (CreatedByUserID == 0) return string.Empty;

                var uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(CreatedByUserID);

                var sb = new StringBuilder(100);

                sb.AppendFormat(@"<li>{0}</li>", uad.SmallUserIcon);

                return sb.ToString();
            }
        }

        public void GetChatRoomUserByUserAccountID(int createdByUserID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetChatRoomUserByUserAccountID";

            comm.AddParameter("createdByUserID", createdByUserID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }
    }

    public class ChatRoomUsers : List<ChatRoomUser>
    {
        public static int GetChattingUserCount()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetChattingUserCount";

            // execute the stored procedure
            var rslt = DbAct.ExecuteScalar(comm);

            return !string.IsNullOrEmpty(rslt) ? Convert.ToInt32(rslt) : 0;
        }

        public void GetChattingUsers()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetChattingUsers";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var cru in from DataRow dr in dt.Rows select new ChatRoomUser(dr))
            {
                Add(cru);
            }

            Sort((x, y) => (x.CreateDate.CompareTo(y.CreateDate)));
        }
    }
}