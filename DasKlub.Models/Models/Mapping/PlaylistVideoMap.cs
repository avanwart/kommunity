using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PlaylistVideoMap : EntityTypeConfiguration<PlaylistVideo>
    {
        public PlaylistVideoMap()
        {
            // Primary Key
            this.HasKey(t => t.playlistVideoID);

            // Properties
            // Table & Column Mappings
            this.ToTable("PlaylistVideo");
            this.Property(t => t.playlistVideoID).HasColumnName("playlistVideoID");
            this.Property(t => t.playlistID).HasColumnName("playlistID");
            this.Property(t => t.videoID).HasColumnName("videoID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            this.HasRequired(t => t.Playlist)
                .WithMany(t => t.PlaylistVideos)
                .HasForeignKey(d => d.playlistID);
            this.HasRequired(t => t.Video)
                .WithMany(t => t.PlaylistVideos)
                .HasForeignKey(d => d.videoID);

        }
    }
}
