using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SizeMap : EntityTypeConfiguration<Size>
    {
        public SizeMap()
        {
            // Primary Key
            this.HasKey(t => t.sizeID);

            // Properties
            this.Property(t => t.sizeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Size");
            this.Property(t => t.sizeID).HasColumnName("sizeID");
            this.Property(t => t.sizeName).HasColumnName("sizeName");
            this.Property(t => t.sizeTypeID).HasColumnName("sizeTypeID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            this.HasOptional(t => t.SizeType)
                .WithMany(t => t.Sizes)
                .HasForeignKey(d => d.sizeTypeID);

        }
    }
}
