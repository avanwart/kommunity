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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class ChatRoom : BaseIUserLogCRUD, IUnorderdListItem
    {
        #region properties 

        private string _chatMessage = string.Empty;
        private string _ipAddress = string.Empty;
        private string _userName = string.Empty;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public int RoomID { get; set; }

        public string ChatMessage
        {
            get { return _chatMessage; }
            set { _chatMessage = value; }
        }

        public int ChatRoomID { get; set; }


        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        #endregion

        public ChatRoom()
        {
        }


        public ChatRoom(DataRow dr)
        {
            Get(dr);
        }

        public string ToUnorderdListItem
        {
            get
            {
                if (CreatedByUserID == 0) return string.Empty;

                var sb = new StringBuilder(100);

                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(CreatedByUserID);

                sb.AppendFormat(@"<li><div class=""user_face"">{0}</div>
                        <span style=""font-size:10px"">{1}</span> <span style=""font-size:14px;color:#FFF;font-family: ‘Lucida Sans Unicode’, ‘Lucida Grande’, sans-serif;"">{2}</span> </li>",
                                uad.UserFace, CreateDate.ToString("u"), Video.IFrameVideo(ChatMessage));

                return sb.ToString();
            }
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            ChatMessage = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ChatMessage)]);
            ChatRoomID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ChatRoomID)]);
            IpAddress = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => IpAddress)]);
            RoomID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => RoomID)]);
            UserName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => UserName)]);
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddChatRoom";
            // create a new parameter
            DbParameter param = null;

            param = comm.CreateParameter();
            param.ParameterName = "@createdByUserID";
            param.Value = CreatedByUserID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@userName";
            param.Value = UserName;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@chatMessage";
            param.Value = ChatMessage;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@ipAddress";
            param.Value = IpAddress;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@roomID";
            param.Value = RoomID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);


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
                ChatRoomID = Convert.ToInt32(result);

                return ChatRoomID;
            }
        }
    }

    public class ChatRooms : List<ChatRoom>
    {
        public void GetRecentChatMessages()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRecentChatMessages";

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ChatRoom content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new ChatRoom(dr);
                    Add(content);
                }

                Sort((ChatRoom x, ChatRoom y) => (x.CreateDate.CompareTo(y.CreateDate)));
            }
        }
    }
}