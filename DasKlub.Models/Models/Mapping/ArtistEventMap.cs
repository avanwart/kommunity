using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ArtistEventMap : EntityTypeConfiguration<ArtistEvent>
    {
        public ArtistEventMap()
        {
            // Primary Key
            this.HasKey(t => new { t.artistID, t.eventID });

            // Properties
            this.Property(t => t.artistID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.eventID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ArtistEvent");
            this.Property(t => t.artistID).HasColumnName("artistID");
            this.Property(t => t.eventID).HasColumnName("eventID");
            this.Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            this.HasRequired(t => t.Artist)
                .WithMany(t => t.ArtistEvents)
                .HasForeignKey(d => d.artistID);
            this.HasRequired(t => t.Event)
                .WithMany(t => t.ArtistEvents)
                .HasForeignKey(d => d.eventID);

        }
    }
}
