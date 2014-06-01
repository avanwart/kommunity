using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PropertyTypeMap : EntityTypeConfiguration<PropertyType>
    {
        public PropertyTypeMap()
        {
            // Primary Key
            HasKey(t => t.propertyTypeID);

            // Properties
            Property(t => t.propertyTypeCode)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(5);

            Property(t => t.propertyTypeName)
                .HasMaxLength(25);

            // Table & Column Mappings
            ToTable("PropertyType");
            Property(t => t.propertyTypeID).HasColumnName("propertyTypeID");
            Property(t => t.propertyTypeCode).HasColumnName("propertyTypeCode");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.propertyTypeName).HasColumnName("propertyTypeName");
        }
    }
}