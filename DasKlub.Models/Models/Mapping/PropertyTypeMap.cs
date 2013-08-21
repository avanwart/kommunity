using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PropertyTypeMap : EntityTypeConfiguration<PropertyType>
    {
        public PropertyTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.propertyTypeID);

            // Properties
            this.Property(t => t.propertyTypeCode)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.propertyTypeName)
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("PropertyType");
            this.Property(t => t.propertyTypeID).HasColumnName("propertyTypeID");
            this.Property(t => t.propertyTypeCode).HasColumnName("propertyTypeCode");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.propertyTypeName).HasColumnName("propertyTypeName");
        }
    }
}
