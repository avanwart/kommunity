using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusUpdateMap : EntityTypeConfiguration<StatusUpdate>
    {
        public StatusUpdateMap()
        {
            // Primary Key
            HasKey(t => t.statusUpdateID);

            // Properties
            Property(t => t.message)
                .IsRequired();

            Property(t => t.statusType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("StatusUpdate");
            Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.message).HasColumnName("message");
            Property(t => t.statusType).HasColumnName("statusType");
            Property(t => t.photoItemID).HasColumnName("photoItemID");
            Property(t => t.zoneID).HasColumnName("zoneID");
            Property(t => t.isMobile).HasColumnName("isMobile");

            // Relationships
            HasOptional(t => t.PhotoItem)
                .WithMany(t => t.StatusUpdates)
                .HasForeignKey(d => d.photoItemID);
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.StatusUpdates)
                .HasForeignKey(d => d.userAccountID);
            HasOptional(t => t.Zone)
                .WithMany(t => t.StatusUpdates)
                .HasForeignKey(d => d.zoneID);
        }
    }
}