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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class ChatRoomUser : BaseIUserLogCRUD, IUnorderdListItem
    {
        #region properties

        private int _chatRoomUserID = 0;

        public int ChatRoomUserID
        {
            get { return _chatRoomUserID; }
            set { _chatRoomUserID = value; }
        }

        private string _ipAddress = string.Empty;

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        private int _roomID = 0;

        public int RoomID
        {
            get { return _roomID; }
            set { _roomID = value; }
        }

        private string _connectionCode = string.Empty;
        
      

        public string ConnectionCode
        {
            get { return _connectionCode; }
            set { _connectionCode = value; }
        }

        #endregion

        public ChatRoomUser() { }

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

            ADOExtenstion.AddParameter(comm, "connectionCode", connectionCode);

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

            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "ipAddress", IpAddress);
            ADOExtenstion.AddParameter(comm, "roomID", RoomID);
            ADOExtenstion.AddParameter(comm, "connectionCode", ConnectionCode);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.ChatRoomUserID = Convert.ToInt32(result);

            return this.ChatRoomUserID;
        }


        public override bool Update()
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateChatRoomUser";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IpAddress), IpAddress);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.RoomID), RoomID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ConnectionCode), ConnectionCode);

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

            ADOExtenstion.AddParameter(comm, "connectionCode", this.ConnectionCode);

            //RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;

        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            this.ChatRoomUserID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ChatRoomUserID)]);
            this.ConnectionCode = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ConnectionCode)]);
            this.IpAddress = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.IpAddress)]);
            this.RoomID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.RoomID)]);
        }

        #endregion

        public void GetChatRoomUserByUserAccountID(int createdByUserID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetChatRoomUserByUserAccountID";

            ADOExtenstion.AddParameter(comm, "createdByUserID", createdByUserID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public string ToUnorderdListItem
        {
            get {

                if (this.CreatedByUserID == 0) return string.Empty;

                UserAccountDetail uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(this.CreatedByUserID);

                StringBuilder sb = new StringBuilder(100);

                sb.AppendFormat(@"<li>{0}</li>", uad.SmallUserIcon);

                return sb.ToString();

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
            string rslt = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(rslt))
            {
                return Convert.ToInt32(rslt);
            }
            else return 0;
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
            if (dt != null && dt.Rows.Count > 0)
            {
                ChatRoomUser cru = null;
                foreach (DataRow dr in dt.Rows)
                {
                    cru = new ChatRoomUser(dr);
                    this.Add(cru);
                }

                this.Sort((ChatRoomUser x, ChatRoomUser y) => (x.CreateDate.CompareTo(y.CreateDate)));
            }
        }
    }
}
