using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SiteDomainMap : EntityTypeConfiguration<SiteDomain>
    {
        public SiteDomainMap()
        {
            // Primary Key
            this.HasKey(t => t.siteDomainID);

            // Properties
            this.Property(t => t.propertyType)
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.language)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("SiteDomain");
            this.Property(t => t.siteDomainID).HasColumnName("siteDomainID");
            this.Property(t => t.propertyType).HasColumnName("propertyType");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.language).HasColumnName("language");
            this.Property(t => t.description).HasColumnName("description");
        }
    }
}
