using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ChatRoomUser
    {
        [Key]
        public int chatRoomUserID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string ipAddress { get; set; }
        public int? roomID { get; set; }
        public string connectionCode { get; set; }
    }
}