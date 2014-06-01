using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            // Primary Key
            HasKey(t => t.eventID);

            // Properties
            Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Event");
            Property(t => t.eventID).HasColumnName("eventID");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.venueID).HasColumnName("venueID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.localTimeBegin).HasColumnName("localTimeBegin");
            Property(t => t.notes).HasColumnName("notes");
            Property(t => t.ticketURL).HasColumnName("ticketURL");
            Property(t => t.localTimeEnd).HasColumnName("localTimeEnd");
            Property(t => t.eventCycleID).HasColumnName("eventCycleID");
            Property(t => t.rsvpURL).HasColumnName("rsvpURL");
            Property(t => t.isReoccuring).HasColumnName("isReoccuring");
            Property(t => t.isEnabled).HasColumnName("isEnabled");
            Property(t => t.eventDetailURL).HasColumnName("eventDetailURL");

            // Relationships
            HasRequired(t => t.EventCycle)
                .WithMany(t => t.Events)
                .HasForeignKey(d => d.eventCycleID);
            HasRequired(t => t.Venue)
                .WithMany(t => t.Events)
                .HasForeignKey(d => d.venueID);
        }
    }
}