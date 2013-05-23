﻿//  Copyright 2013 
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

using System.Text;
using System.Web.Mvc;
using BootBaronLib.AppSpec.DasKlub.BOL;

namespace DasKlub.Web.Web.Controllers
{
    public class ChatController : Controller
    {
        public JsonResult RecentChatMessages()
        {
            var crs = new ChatRooms();

            crs.GetRecentChatMessages();

            var sb = new StringBuilder();

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
            var crus = new ChatRoomUsers();

            crus.GetChattingUsers();

            if (crus.Count == 0) return null;

            var sb = new StringBuilder();

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