using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ChatRoomMap : EntityTypeConfiguration<ChatRoom>
    {
        public ChatRoomMap()
        {
            // Primary Key
            this.HasKey(t => t.chatRoomID);

            // Properties
            this.Property(t => t.userName)
                .HasMaxLength(25);

            this.Property(t => t.ipAddress)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ChatRoom");
            this.Property(t => t.chatRoomID).HasColumnName("chatRoomID");
            this.Property(t => t.userName).HasColumnName("userName");
            this.Property(t => t.chatMessage).HasColumnName("chatMessage");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
            this.Property(t => t.roomID).HasColumnName("roomID");
        }
    }
}
