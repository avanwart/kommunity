using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class EventCycleMap : EntityTypeConfiguration<EventCycle>
    {
        public EventCycleMap()
        {
            // Primary Key
            HasKey(t => t.eventCycleID);

            // Properties
            Property(t => t.cycleName)
                .HasMaxLength(50);

            Property(t => t.eventCode)
                .IsFixedLength()
                .HasMaxLength(3);

            // Table & Column Mappings
            ToTable("EventCycle");
            Property(t => t.eventCycleID).HasColumnName("eventCycleID");
            Property(t => t.cycleName).HasColumnName("cycleName");
            Property(t => t.eventCode).HasColumnName("eventCode");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
        }
    }
}