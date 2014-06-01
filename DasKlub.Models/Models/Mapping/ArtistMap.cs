using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ArtistMap : EntityTypeConfiguration<Artist>
    {
        public ArtistMap()
        {
            // Primary Key
            HasKey(t => t.artistID);

            // Properties
            Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.altName)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Artist");
            Property(t => t.artistID).HasColumnName("artistID");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.altName).HasColumnName("altName");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.isHidden).HasColumnName("isHidden");
        }
    }
}