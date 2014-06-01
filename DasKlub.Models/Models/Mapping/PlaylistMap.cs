using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PlaylistMap : EntityTypeConfiguration<Playlist>
    {
        public PlaylistMap()
        {
            // Primary Key
            HasKey(t => t.playlistID);

            // Properties
            Property(t => t.playListName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Playlist");
            Property(t => t.playlistID).HasColumnName("playlistID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.playlistBegin).HasColumnName("playlistBegin");
            Property(t => t.playListName).HasColumnName("playListName");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.autoPlay).HasColumnName("autoPlay");

            // Relationships
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.Playlists)
                .HasForeignKey(d => d.userAccountID);
        }
    }
}