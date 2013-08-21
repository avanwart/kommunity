using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ChatRoom
    {
        public int chatRoomID { get; set; }
        public string userName { get; set; }
        public string chatMessage { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string ipAddress { get; set; }
        public Nullable<int> roomID { get; set; }
    }
}
