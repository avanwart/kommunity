using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ArtistEventMap : EntityTypeConfiguration<ArtistEvent>
    {
        public ArtistEventMap()
        {
            // Primary Key
            HasKey(t => new {t.artistID, t.eventID});

            // Properties
            Property(t => t.artistID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.eventID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("ArtistEvent");
            Property(t => t.artistID).HasColumnName("artistID");
            Property(t => t.eventID).HasColumnName("eventID");
            Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            HasRequired(t => t.Artist)
                .WithMany(t => t.ArtistEvents)
                .HasForeignKey(d => d.artistID);
            HasRequired(t => t.Event)
                .WithMany(t => t.ArtistEvents)
                .HasForeignKey(d => d.eventID);
        }
    }
}