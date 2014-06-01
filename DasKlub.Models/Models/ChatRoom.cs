using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ChatRoom
    {
        [Key]
        public int chatRoomID { get; set; }

        public string userName { get; set; }
        public string chatMessage { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string ipAddress { get; set; }
        public int? roomID { get; set; }
    }
}