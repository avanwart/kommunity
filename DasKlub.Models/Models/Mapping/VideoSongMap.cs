using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoSongMap : EntityTypeConfiguration<VideoSong>
    {
        public VideoSongMap()
        {
            // Primary Key
            HasKey(t => new {t.videoID, t.songID});

            // Properties
            Property(t => t.videoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.songID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("VideoSong");
            Property(t => t.videoID).HasColumnName("videoID");
            Property(t => t.songID).HasColumnName("songID");
            Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            HasRequired(t => t.Song)
                .WithMany(t => t.VideoSongs)
                .HasForeignKey(d => d.songID);
            HasRequired(t => t.Video)
                .WithMany(t => t.VideoSongs)
                .HasForeignKey(d => d.videoID);
        }
    }
}