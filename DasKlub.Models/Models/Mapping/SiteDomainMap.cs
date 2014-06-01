using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SiteDomainMap : EntityTypeConfiguration<SiteDomain>
    {
        public SiteDomainMap()
        {
            // Primary Key
            HasKey(t => t.siteDomainID);

            // Properties
            Property(t => t.propertyType)
                .IsFixedLength()
                .HasMaxLength(5);

            Property(t => t.language)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("SiteDomain");
            Property(t => t.siteDomainID).HasColumnName("siteDomainID");
            Property(t => t.propertyType).HasColumnName("propertyType");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.language).HasColumnName("language");
            Property(t => t.description).HasColumnName("description");
        }
    }
}