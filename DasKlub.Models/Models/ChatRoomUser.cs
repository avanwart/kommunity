using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class ChatRoomUser
    {
        [Key]
        public int chatRoomUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string ipAddress { get; set; }
        public Nullable<int> roomID { get; set; }
        public string connectionCode { get; set; }
    }
}
