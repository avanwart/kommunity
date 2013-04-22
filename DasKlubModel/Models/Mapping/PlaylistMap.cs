using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PlaylistMap : EntityTypeConfiguration<Playlist>
    {
        public PlaylistMap()
        {
            // Primary Key
            this.HasKey(t => t.playlistID);

            // Properties
            this.Property(t => t.playListName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Playlist");
            this.Property(t => t.playlistID).HasColumnName("playlistID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.playlistBegin).HasColumnName("playlistBegin");
            this.Property(t => t.playListName).HasColumnName("playListName");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.autoPlay).HasColumnName("autoPlay");

            // Relationships
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.Playlists)
                .HasForeignKey(d => d.userAccountID);

        }
    }
}
