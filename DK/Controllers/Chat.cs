using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BOL;
using Microsoft.AspNet.SignalR;

namespace DasKlub.Controllers
{
    public class Chat : Hub 
    {
        public Task Connect()
        {
            var ua = new UserAccount(Context.User.Identity.Name);

            var cru = new ChatRoomUser();

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

            return Clients.All(Context.ConnectionId, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            var ua = new UserAccount(Context.User.Identity.Name);

            var cru = new ChatRoomUser();

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

        public Task Disconnect()
        {
            var cru = new ChatRoomUser();

            cru.GetChatRoomUserByConnection(Context.ConnectionId);

            var ua = new UserAccount(cru.CreatedByUserID);

            cru.DeleteChatRoomUser();

            Send(@"<i style=""color:red;font-size:10px;font-style: italic;"">CONNECTION CLOSED</i>", ua.UserAccountID);

            return Clients.Caller(Context.ConnectionId, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
        }

        private void Send(string message, int userAccountID)
        {
            if (string.IsNullOrWhiteSpace(message) || (userAccountID == 0)) return;

            var cr = new ChatRoom {ChatMessage = message, CreatedByUserID = userAccountID};

            cr.Create();

            var uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(userAccountID);
            string userFace = string.Format(@"<div class=""user_face"">{0}</div>", uad.UserFace);

            if (!message.Contains("CONNECTION OPENED") && !message.Contains("CONNECTION CLOSED") &&
                !message.Contains("RECONNECT"))
            {
                message = HttpUtility.HtmlEncode(message);
                message = Video.IFrameVideo(message);
            }

            // Call the addMessage method on all clients
            Clients.Caller(
                string.Format(
                    @"{2} <span style=""font-size:10px"">{0}</span> <span style=""font-size:14px;color:#FFF;font-family: ‘Lucida Sans Unicode’, ‘Lucida Grande’, sans-serif;"">{1}</span>",
                    DateTime.UtcNow.ToString("u"), message, userFace));
        }
    }
}