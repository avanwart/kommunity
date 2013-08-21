using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ColorMap : EntityTypeConfiguration<Color>
    {
        public ColorMap()
        {
            // Primary Key
            this.HasKey(t => t.colorID);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("Color");
            this.Property(t => t.colorID).HasColumnName("colorID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.siteDomainID).HasColumnName("siteDomainID");
        }
    }
}
