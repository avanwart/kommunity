using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ChatRoomUserMap : EntityTypeConfiguration<ChatRoomUser>
    {
        public ChatRoomUserMap()
        {
            // Primary Key
            this.HasKey(t => t.chatRoomUserID);

            // Properties
            this.Property(t => t.ipAddress)
                .HasMaxLength(50);

            this.Property(t => t.connectionCode)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ChatRoomUser");
            this.Property(t => t.chatRoomUserID).HasColumnName("chatRoomUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
            this.Property(t => t.roomID).HasColumnName("roomID");
            this.Property(t => t.connectionCode).HasColumnName("connectionCode");
        }
    }
}
