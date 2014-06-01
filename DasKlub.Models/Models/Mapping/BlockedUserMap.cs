using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class BlockedUserMap : EntityTypeConfiguration<BlockedUser>
    {
        public BlockedUserMap()
        {
            // Primary Key
            HasKey(t => t.blockedUserID);

            // Properties
            // Table & Column Mappings
            ToTable("BlockedUser");
            Property(t => t.blockedUserID).HasColumnName("blockedUserID");
            Property(t => t.userAccountIDBlocking).HasColumnName("userAccountIDBlocking");
            Property(t => t.userAccountIDBlocked).HasColumnName("userAccountIDBlocked");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");

            // Relationships
            HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.BlockedUsers)
                .HasForeignKey(d => d.userAccountIDBlocking);
        }
    }
}