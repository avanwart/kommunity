using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ArtistMap : EntityTypeConfiguration<Artist>
    {
        public ArtistMap()
        {
            // Primary Key
            this.HasKey(t => t.artistID);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.altName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Artist");
            this.Property(t => t.artistID).HasColumnName("artistID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.altName).HasColumnName("altName");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.isHidden).HasColumnName("isHidden");
        }
    }
}
