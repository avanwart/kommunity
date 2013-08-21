using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SizeTypeMap : EntityTypeConfiguration<SizeType>
    {
        public SizeTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.sizeTypeID);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SizeType");
            this.Property(t => t.sizeTypeID).HasColumnName("sizeTypeID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
        }
    }
}
