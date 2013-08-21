using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class PhotoItemMap : EntityTypeConfiguration<PhotoItem>
    {
        public PhotoItemMap()
        {
            // Primary Key
            this.HasKey(t => t.photoItemID);

            // Properties
            this.Property(t => t.title)
                .HasMaxLength(100);

            this.Property(t => t.filePathRaw)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.filePathThumb)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.filePathStandard)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("PhotoItem");
            this.Property(t => t.photoItemID).HasColumnName("photoItemID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.filePathRaw).HasColumnName("filePathRaw");
            this.Property(t => t.filePathThumb).HasColumnName("filePathThumb");
            this.Property(t => t.filePathStandard).HasColumnName("filePathStandard");

            // Relationships
            this.HasOptional(t => t.UserAccount)
                .WithMany(t => t.PhotoItems)
                .HasForeignKey(d => d.createdByUserID);

        }
    }
}
