using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class EventCycleMap : EntityTypeConfiguration<EventCycle>
    {
        public EventCycleMap()
        {
            // Primary Key
            this.HasKey(t => t.eventCycleID);

            // Properties
            this.Property(t => t.cycleName)
                .HasMaxLength(50);

            this.Property(t => t.eventCode)
                .IsFixedLength()
                .HasMaxLength(3);

            // Table & Column Mappings
            this.ToTable("EventCycle");
            this.Property(t => t.eventCycleID).HasColumnName("eventCycleID");
            this.Property(t => t.cycleName).HasColumnName("cycleName");
            this.Property(t => t.eventCode).HasColumnName("eventCode");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}
