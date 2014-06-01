using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PlaylistVideoMap : EntityTypeConfiguration<PlaylistVideo>
    {
        public PlaylistVideoMap()
        {
            // Primary Key
            HasKey(t => t.playlistVideoID);

            // Properties
            // Table & Column Mappings
            ToTable("PlaylistVideo");
            Property(t => t.playlistVideoID).HasColumnName("playlistVideoID");
            Property(t => t.playlistID).HasColumnName("playlistID");
            Property(t => t.videoID).HasColumnName("videoID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            HasRequired(t => t.Playlist)
                .WithMany(t => t.PlaylistVideos)
                .HasForeignKey(d => d.playlistID);
            HasRequired(t => t.Video)
                .WithMany(t => t.PlaylistVideos)
                .HasForeignKey(d => d.videoID);
        }
    }
}