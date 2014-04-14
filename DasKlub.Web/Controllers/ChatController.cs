using System.Text;
using System.Web.Mvc;
using DasKlub.Lib.BOL;

namespace DasKlub.Web.Controllers
{
    public class ChatController : Controller
    {
        public JsonResult RecentChatMessages()
        {
            var crs = new ChatRooms();

            crs.GetRecentChatMessages();

            var sb = new StringBuilder();

            foreach (var cnt in crs)
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

            foreach (var cnt in crus)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
                {
                    ChattingUsers = sb.ToString()
                });
        }


    }
}