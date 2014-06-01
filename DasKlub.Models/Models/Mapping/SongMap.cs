using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SongMap : EntityTypeConfiguration<Song>
    {
        public SongMap()
        {
            // Primary Key
            HasKey(t => t.songID);

            // Properties
            Property(t => t.name)
                .HasMaxLength(150);

            Property(t => t.songKey)
                .HasMaxLength(150);

            // Table & Column Mappings
            ToTable("Song");
            Property(t => t.songID).HasColumnName("songID");
            Property(t => t.artistID).HasColumnName("artistID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.isHidden).HasColumnName("isHidden");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.songKey).HasColumnName("songKey");

            // Relationships
            HasOptional(t => t.Artist)
                .WithMany(t => t.Songs)
                .HasForeignKey(d => d.artistID);
        }
    }
}