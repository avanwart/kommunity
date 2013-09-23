using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class BlockedUserMap : EntityTypeConfiguration<BlockedUser>
    {
        public BlockedUserMap()
        {
            // Primary Key
            this.HasKey(t => t.blockedUserID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BlockedUser");
            this.Property(t => t.blockedUserID).HasColumnName("blockedUserID");
            this.Property(t => t.userAccountIDBlocking).HasColumnName("userAccountIDBlocking");
            this.Property(t => t.userAccountIDBlocked).HasColumnName("userAccountIDBlocked");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");

            // Relationships
            this.HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.BlockedUsers)
                .HasForeignKey(d => d.userAccountIDBlocking);

        }
    }
}
