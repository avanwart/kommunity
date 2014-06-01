using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusUpdateNotificationMap : EntityTypeConfiguration<StatusUpdateNotification>
    {
        public StatusUpdateNotificationMap()
        {
            // Primary Key
            HasKey(t => t.statusUpdateNotificationID);

            // Properties
            Property(t => t.responseType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("StatusUpdateNotification");
            Property(t => t.statusUpdateNotificationID).HasColumnName("statusUpdateNotificationID");
            Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.isRead).HasColumnName("isRead");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.responseType).HasColumnName("responseType");

            // Relationships
            HasRequired(t => t.StatusUpdate)
                .WithMany(t => t.StatusUpdateNotifications)
                .HasForeignKey(d => d.statusUpdateID);
        }
    }
}