using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using Microsoft.AspNet.SignalR;
using SignalRChat.Common;

namespace DasKlub.Web.Common
{
    public class MessageDetail
    {
        public string UserName { get; set; }

        public string Message { get; set; }
    }
}

namespace DasKlub.Web.Common
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }
}

namespace DasKlub.Web.Controllers
{
    [Authorize]
    public class ChatHub : Hub
    {
        #region Data Members

        private static readonly List<UserDetail> ConnectedUsers = new List<UserDetail>();
        private static readonly List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        #endregion

        #region Methods

        public void Connect(string userName)
        {
            MembershipUser mu = Membership.GetUser();
            string id = Context.ConnectionId;

            if (ConnectedUsers.Count(x => x.ConnectionId == id) != 0) return;
            ConnectedUsers.Add(new UserDetail {ConnectionId = id, UserName = userName});

            if (mu != null)
            {
                var enterRoom = new ChatRoomUser();

                enterRoom.GetChatRoomUserByUserAccountID(Convert.ToInt32(mu.ProviderUserKey));

                if (enterRoom.RoomID == 0)
                {
                    enterRoom = new ChatRoomUser
                        {
                            CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey),
                            ConnectionCode = id
                        };
                    enterRoom.Create();
                }
                else
                {
                    enterRoom.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                    enterRoom.ConnectionCode = id;
                    enterRoom.Update();
                }
            }

            // send to caller
            Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

            // send to all except caller client
            Clients.AllExcept(id).onNewUserConnected(id, userName);
        }

        public void SendMessageToAll(string userName, string message)
        {
            message = HttpUtility.HtmlEncode(message);

            MembershipUser mu = Membership.GetUser();

            if (mu != null)
            {
                var chatMessage = new ChatRoom
                    {
                        CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey),
                        ChatMessage = message
                    };
                chatMessage.Create();
            }

            AddMessageinCache(userName, message);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {
            message = HttpUtility.HtmlEncode(message);

            string fromUserId = Context.ConnectionId;

            UserDetail toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            UserDetail fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser == null || fromUser == null) return;
            // send to 
            Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

            // send to caller user
            Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
        }

        public override Task OnDisconnected()
        {
            UserDetail item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                string id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

                var exitRoom = new ChatRoomUser();
                exitRoom.GetChatRoomUserByConnection(id);
                exitRoom.DeleteChatRoomUser();
            }


            return base.OnDisconnected();
        }

        #endregion

        #region private Messages

        // store last 100 messages in cache
        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        private static void AddMessageinCache(string userName, string message)
        {
            CurrentMessage.Add(new MessageDetail {UserName = userName, Message = message});

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }

        #endregion
    }

    //public class Chat : Hub 
    //{
    //    public Task Connect()
    //    {
    //        var ua = new UserAccount(Context.User.Identity.Name);

    //        var cru = new ChatRoomUser();

    //        cru.GetChatRoomUserByUserAccountID(ua.UserAccountID);

    //        if (cru.ChatRoomUserID == 0)
    //        {
    //            cru.CreatedByUserID = ua.UserAccountID;
    //            cru.ConnectionCode = Context.ConnectionId;
    //            cru.Create();
    //        }
    //        else
    //        {
    //            cru.ConnectionCode = Context.ConnectionId;
    //            cru.UpdatedByUserID = ua.UserAccountID;
    //            cru.Update();
    //        }

    //        Send(@"<i style=""color:yellow;font-size:10px;font-style: italic;"">CONNECTION OPENED</i>", ua.UserAccountID);

    //        return Clients.All(Context.ConnectionId, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
    //    }

    //    public Task Reconnect(IEnumerable<string> groups)
    //    {
    //        var ua = new UserAccount(Context.User.Identity.Name);

    //        var cru = new ChatRoomUser();

    //        cru.GetChatRoomUserByUserAccountID(ua.UserAccountID);

    //        if (cru.ChatRoomUserID > 0)
    //        {
    //            cru.ConnectionCode = Context.ConnectionId;
    //            cru.UpdatedByUserID = ua.UserAccountID;
    //            cru.Update();
    //        }

    //        Send(@"<i style=""color:purple;font-size:10px;font-style: italic;"">RECONNECT</i>", ua.UserAccountID);

    //        return null;
    //    }

    //    public Task Disconnect()
    //    {
    //        var cru = new ChatRoomUser();

    //        cru.GetChatRoomUserByConnection(Context.ConnectionId);

    //        var ua = new UserAccount(cru.CreatedByUserID);

    //        cru.DeleteChatRoomUser();

    //        Send(@"<i style=""color:red;font-size:10px;font-style: italic;"">CONNECTION CLOSED</i>", ua.UserAccountID);

    //        return Clients.Caller(Context.ConnectionId, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
    //    }

    //    private void Send(string message, int userAccountID)
    //    {
    //        if (string.IsNullOrWhiteSpace(message) || (userAccountID == 0)) return;

    //        var cr = new ChatRoom {ChatMessage = message, CreatedByUserID = userAccountID};

    //        cr.Create();

    //        var uad = new UserAccountDetail();
    //        uad.GetUserAccountDeailForUser(userAccountID);
    //        string userFace = string.Format(@"<div class=""user_face"">{0}</div>", uad.UserFace);

    //        if (!message.Contains("CONNECTION OPENED") && !message.Contains("CONNECTION CLOSED") &&
    //            !message.Contains("RECONNECT"))
    //        {
    //            message = HttpUtility.HtmlEncode(message);
    //            message = Video.IFrameVideo(message);
    //        }

    //        // Call the addMessage method on all clients
    //        Clients.Caller(
    //            string.Format(
    //                @"{2} <span style=""font-size:10px"">{0}</span> <span style=""font-size:14px;color:#FFF;font-family: ‘Lucida Sans Unicode’, ‘Lucida Grande’, sans-serif;"">{1}</span>",
    //                DateTime.UtcNow.ToString("u"), message, userFace));
    //    }
    //}
}