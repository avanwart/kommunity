using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SizeTypeMap : EntityTypeConfiguration<SizeType>
    {
        public SizeTypeMap()
        {
            // Primary Key
            HasKey(t => t.sizeTypeID);

            // Properties
            Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("SizeType");
            Property(t => t.sizeTypeID).HasColumnName("sizeTypeID");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
        }
    }
}