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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using DasKlub.Models;
using SignalR;
using SignalR.Hubs;

namespace DasKlub.Controllers
{

    public class Chat : Hub, IConnected, IDisconnect
    {
        public void Send(string message, int userAccountID)
        {
            if (string.IsNullOrWhiteSpace(message) || (userAccountID == 0)) return;

           
            ChatRoom cr = new ChatRoom();

            cr.ChatMessage = message;
            cr.CreatedByUserID = userAccountID;
            cr.Create();

            BootBaronLib.AppSpec.DasKlub.BOL.UserAccountDetail uad = new BootBaronLib.AppSpec.DasKlub.BOL.UserAccountDetail();
            uad.GetUserAccountDeailForUser(userAccountID);
            string userFace = string.Format(@"<div class=""user_face"">{0}</div>", uad.UserFace);

            if (!message.Contains("CONNECTION OPENED") && !message.Contains("CONNECTION CLOSED") && !message.Contains("RECONNECT"))
            {
                message = HttpUtility.HtmlEncode(message);
                message = Video.IFrameVideo(message);
            }

            // Call the addMessage method on all clients
            Clients.addMessage(string.Format(@"{2} <span style=""font-size:10px"">{0}</span> <span style=""font-size:14px;color:#FFF;font-family: ‘Lucida Sans Unicode’, ‘Lucida Grande’, sans-serif;"">{1}</span>", DateTime.UtcNow.ToString("u"), message, userFace));

        }

        public Task Disconnect()
        {
            ChatRoomUser cru = new ChatRoomUser();

            cru.GetChatRoomUserByConnection(Context.ConnectionId);

            UserAccount ua = new UserAccount(cru.CreatedByUserID);

            cru.DeleteChatRoomUser();

            Send(@"<i style=""color:red;font-size:10px;font-style: italic;"">CONNECTION CLOSED</i>", ua.UserAccountID);

            return Clients.leave(Context.ConnectionId, DateTime.UtcNow.ToString());
        }

        public Task Connect()
        {
            UserAccount ua = new UserAccount(Context.User.Identity.Name);

            ChatRoomUser cru = new ChatRoomUser();

            cru.GetChatRoomUserByUserAccountID(ua.UserAccountID);

            if (cru.ChatRoomUserID == 0)
            {
                cru.CreatedByUserID = ua.UserAccountID;
                cru.ConnectionCode = Context.ConnectionId;
                cru.Create();
            }
            else
            {
                cru.ConnectionCode = Context.ConnectionId;
                cru.UpdatedByUserID = ua.UserAccountID;
                cru.Update();
            }

            Send(@"<i style=""color:yellow;font-size:10px;font-style: italic;"">CONNECTION OPENED</i>", ua.UserAccountID);

            return Clients.joined(Context.ConnectionId, DateTime.UtcNow.ToString());
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            UserAccount ua = new UserAccount(Context.User.Identity.Name);

            ChatRoomUser cru = new ChatRoomUser();

            cru.GetChatRoomUserByUserAccountID(ua.UserAccountID);

            if (cru.ChatRoomUserID > 0)
            {
                cru.ConnectionCode = Context.ConnectionId;
                cru.UpdatedByUserID = ua.UserAccountID;
                cru.Update();
            }

            Send(@"<i style=""color:purple;font-size:10px;font-style: italic;"">RECONNECT</i>", ua.UserAccountID);

            return null;
        }
    }


    public class ChatController : Controller
    {

        public JsonResult RecentChatMessages()
        {
            ChatRooms crs = new ChatRooms();

            crs.GetRecentChatMessages();

            StringBuilder sb = new StringBuilder();

            foreach (ChatRoom cnt in crs)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
            {
                ListItems = sb.ToString()
            });
        }



        public JsonResult GetChattingUsers()
        {
            ChatRoomUsers crus = new ChatRoomUsers();

            crus.GetChattingUsers();

            if (crus.Count == 0) return null;

            StringBuilder sb = new StringBuilder();

            foreach (ChatRoomUser cnt in crus)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
            {
                ChattingUsers = sb.ToString()
            });
        }
         
        public ActionResult Index()
        {
            return View();
        }
    }
}