using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ChatRoomUserMap : EntityTypeConfiguration<ChatRoomUser>
    {
        public ChatRoomUserMap()
        {
            // Primary Key
            HasKey(t => t.chatRoomUserID);

            // Properties
            Property(t => t.ipAddress)
                .HasMaxLength(50);

            Property(t => t.connectionCode)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("ChatRoomUser");
            Property(t => t.chatRoomUserID).HasColumnName("chatRoomUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
            Property(t => t.roomID).HasColumnName("roomID");
            Property(t => t.connectionCode).HasColumnName("connectionCode");
        }
    }
}