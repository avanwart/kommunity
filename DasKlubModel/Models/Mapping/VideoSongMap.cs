using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoSongMap : EntityTypeConfiguration<VideoSong>
    {
        public VideoSongMap()
        {
            // Primary Key
            this.HasKey(t => new { t.videoID, t.songID });

            // Properties
            this.Property(t => t.videoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.songID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VideoSong");
            this.Property(t => t.videoID).HasColumnName("videoID");
            this.Property(t => t.songID).HasColumnName("songID");
            this.Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            this.HasRequired(t => t.Song)
                .WithMany(t => t.VideoSongs)
                .HasForeignKey(d => d.songID);
            this.HasRequired(t => t.Video)
                .WithMany(t => t.VideoSongs)
                .HasForeignKey(d => d.videoID);

        }
    }
}
