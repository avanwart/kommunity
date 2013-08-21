using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContentMap : EntityTypeConfiguration<Content>
    {
        public ContentMap()
        {
            // Primary Key
            this.HasKey(t => t.contentID);

            // Properties
            this.Property(t => t.contentKey)
                .HasMaxLength(150);

            this.Property(t => t.title)
                .HasMaxLength(150);

            this.Property(t => t.metaDescription)
                .HasMaxLength(500);

            this.Property(t => t.metaKeywords)
                .HasMaxLength(500);

            this.Property(t => t.contentPhotoURL)
                .HasMaxLength(150);

            this.Property(t => t.contentPhotoThumbURL)
                .HasMaxLength(150);

            this.Property(t => t.contentVideoURL)
                .HasMaxLength(150);

            this.Property(t => t.currentStatus)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.language)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.contentVideoURL2)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Content");
            this.Property(t => t.contentID).HasColumnName("contentID");
            this.Property(t => t.siteDomainID).HasColumnName("siteDomainID");
            this.Property(t => t.contentKey).HasColumnName("contentKey");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.detail).HasColumnName("detail");
            this.Property(t => t.metaDescription).HasColumnName("metaDescription");
            this.Property(t => t.metaKeywords).HasColumnName("metaKeywords");
            this.Property(t => t.contentTypeID).HasColumnName("contentTypeID");
            this.Property(t => t.releaseDate).HasColumnName("releaseDate");
            this.Property(t => t.rating).HasColumnName("rating");
            this.Property(t => t.contentPhotoURL).HasColumnName("contentPhotoURL");
            this.Property(t => t.contentPhotoThumbURL).HasColumnName("contentPhotoThumbURL");
            this.Property(t => t.contentVideoURL).HasColumnName("contentVideoURL");
            this.Property(t => t.outboundURL).HasColumnName("outboundURL");
            this.Property(t => t.isEnabled).HasColumnName("isEnabled");
            this.Property(t => t.currentStatus).HasColumnName("currentStatus");
            this.Property(t => t.language).HasColumnName("language");
            this.Property(t => t.contentVideoURL2).HasColumnName("contentVideoURL2");

            // Relationships
            this.HasOptional(t => t.ContentType)
                .WithMany(t => t.Contents)
                .HasForeignKey(d => d.contentTypeID);
            this.HasOptional(t => t.UserAccount)
                .WithMany(t => t.Contents)
                .HasForeignKey(d => d.createdByUserID);

        }
    }
}
