using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ChatRoomMap : EntityTypeConfiguration<ChatRoom>
    {
        public ChatRoomMap()
        {
            // Primary Key
            HasKey(t => t.chatRoomID);

            // Properties
            Property(t => t.userName)
                .HasMaxLength(25);

            Property(t => t.ipAddress)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("ChatRoom");
            Property(t => t.chatRoomID).HasColumnName("chatRoomID");
            Property(t => t.userName).HasColumnName("userName");
            Property(t => t.chatMessage).HasColumnName("chatMessage");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
            Property(t => t.roomID).HasColumnName("roomID");
        }
    }
}