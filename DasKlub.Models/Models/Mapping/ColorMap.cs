using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ColorMap : EntityTypeConfiguration<Color>
    {
        public ColorMap()
        {
            // Primary Key
            HasKey(t => t.colorID);

            // Properties
            Property(t => t.name)
                .HasMaxLength(25);

            // Table & Column Mappings
            ToTable("Color");
            Property(t => t.colorID).HasColumnName("colorID");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.siteDomainID).HasColumnName("siteDomainID");
        }
    }
}