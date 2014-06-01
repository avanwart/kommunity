using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PhotoItemMap : EntityTypeConfiguration<PhotoItem>
    {
        public PhotoItemMap()
        {
            // Primary Key
            HasKey(t => t.photoItemID);

            // Properties
            Property(t => t.title)
                .HasMaxLength(100);

            Property(t => t.filePathRaw)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.filePathThumb)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.filePathStandard)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("PhotoItem");
            Property(t => t.photoItemID).HasColumnName("photoItemID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.title).HasColumnName("title");
            Property(t => t.filePathRaw).HasColumnName("filePathRaw");
            Property(t => t.filePathThumb).HasColumnName("filePathThumb");
            Property(t => t.filePathStandard).HasColumnName("filePathStandard");

            // Relationships
            HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.PhotoItems)
                .HasForeignKey(d => d.createdByUserID);
        }
    }
}