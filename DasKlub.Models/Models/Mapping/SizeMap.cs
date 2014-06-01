using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SizeMap : EntityTypeConfiguration<Size>
    {
        public SizeMap()
        {
            // Primary Key
            HasKey(t => t.sizeID);

            // Properties
            Property(t => t.sizeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Size");
            Property(t => t.sizeID).HasColumnName("sizeID");
            Property(t => t.sizeName).HasColumnName("sizeName");
            Property(t => t.sizeTypeID).HasColumnName("sizeTypeID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            HasOptional(t => t.SizeType)
                .WithMany(t => t.Sizes)
                .HasForeignKey(d => d.sizeTypeID);
        }
    }
}