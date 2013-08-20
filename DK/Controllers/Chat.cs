using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using DasKlub.Lib.AppSpec.DasKlub.BOL;
using DasKlub.Lib.Operational;
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
            var mu = Membership.GetUser();
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
            message = Utilities.MakeLink(message);

            var mu = Membership.GetUser();

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

            var fromUserId = Context.ConnectionId;
            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser == null || fromUser == null) return;
            // send to 
            Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

            // send to caller user
            Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
        }

        public override Task OnDisconnected()
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
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
 
}