using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusUpdateNotificationMap : EntityTypeConfiguration<StatusUpdateNotification>
    {
        public StatusUpdateNotificationMap()
        {
            // Primary Key
            this.HasKey(t => t.statusUpdateNotificationID);

            // Properties
            this.Property(t => t.responseType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("StatusUpdateNotification");
            this.Property(t => t.statusUpdateNotificationID).HasColumnName("statusUpdateNotificationID");
            this.Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.isRead).HasColumnName("isRead");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.responseType).HasColumnName("responseType");

            // Relationships
            this.HasRequired(t => t.StatusUpdate)
                .WithMany(t => t.StatusUpdateNotifications)
                .HasForeignKey(d => d.statusUpdateID);

        }
    }
}
