using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            // Primary Key
            this.HasKey(t => t.eventID);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Event");
            this.Property(t => t.eventID).HasColumnName("eventID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.venueID).HasColumnName("venueID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.localTimeBegin).HasColumnName("localTimeBegin");
            this.Property(t => t.notes).HasColumnName("notes");
            this.Property(t => t.ticketURL).HasColumnName("ticketURL");
            this.Property(t => t.localTimeEnd).HasColumnName("localTimeEnd");
            this.Property(t => t.eventCycleID).HasColumnName("eventCycleID");
            this.Property(t => t.rsvpURL).HasColumnName("rsvpURL");
            this.Property(t => t.isReoccuring).HasColumnName("isReoccuring");
            this.Property(t => t.isEnabled).HasColumnName("isEnabled");
            this.Property(t => t.eventDetailURL).HasColumnName("eventDetailURL");

            // Relationships
            this.HasRequired(t => t.EventCycle)
                .WithMany(t => t.Events)
                .HasForeignKey(d => d.eventCycleID);
            this.HasRequired(t => t.Venue)
                .WithMany(t => t.Events)
                .HasForeignKey(d => d.venueID);

        }
    }
}
