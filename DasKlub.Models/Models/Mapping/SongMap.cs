using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SongMap : EntityTypeConfiguration<Song>
    {
        public SongMap()
        {
            // Primary Key
            this.HasKey(t => t.songID);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(150);

            this.Property(t => t.songKey)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Song");
            this.Property(t => t.songID).HasColumnName("songID");
            this.Property(t => t.artistID).HasColumnName("artistID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.isHidden).HasColumnName("isHidden");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.songKey).HasColumnName("songKey");

            // Relationships
            this.HasOptional(t => t.Artist)
                .WithMany(t => t.Songs)
                .HasForeignKey(d => d.artistID);

        }
    }
}
