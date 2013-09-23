using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusUpdateMap : EntityTypeConfiguration<StatusUpdate>
    {
        public StatusUpdateMap()
        {
            // Primary Key
            this.HasKey(t => t.statusUpdateID);

            // Properties
            this.Property(t => t.message)
                .IsRequired();

            this.Property(t => t.statusType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("StatusUpdate");
            this.Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.message).HasColumnName("message");
            this.Property(t => t.statusType).HasColumnName("statusType");
            this.Property(t => t.photoItemID).HasColumnName("photoItemID");
            this.Property(t => t.zoneID).HasColumnName("zoneID");
            this.Property(t => t.isMobile).HasColumnName("isMobile");

            // Relationships
            this.HasOptional(t => t.PhotoItem)
                .WithMany(t => t.StatusUpdates)
                .HasForeignKey(d => d.photoItemID);
            this.HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.StatusUpdates)
                .HasForeignKey(d => d.userAccountID);
            this.HasOptional(t => t.Zone)
                .WithMany(t => t.StatusUpdates)
                .HasForeignKey(d => d.zoneID);

        }
    }
}
