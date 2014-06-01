using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContentMap : EntityTypeConfiguration<Content>
    {
        public ContentMap()
        {
            // Primary Key
            HasKey(t => t.contentID);

            // Properties
            Property(t => t.contentKey)
                .HasMaxLength(150);

            Property(t => t.title)
                .HasMaxLength(150);

            Property(t => t.metaDescription)
                .HasMaxLength(500);

            Property(t => t.metaKeywords)
                .HasMaxLength(500);

            Property(t => t.contentPhotoURL)
                .HasMaxLength(150);

            Property(t => t.contentPhotoThumbURL)
                .HasMaxLength(150);

            Property(t => t.contentVideoURL)
                .HasMaxLength(150);

            Property(t => t.currentStatus)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.language)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            Property(t => t.contentVideoURL2)
                .HasMaxLength(150);

            // Table & Column Mappings
            ToTable("Content");
            Property(t => t.contentID).HasColumnName("contentID");
            Property(t => t.siteDomainID).HasColumnName("siteDomainID");
            Property(t => t.contentKey).HasColumnName("contentKey");
            Property(t => t.title).HasColumnName("title");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.detail).HasColumnName("detail");
            Property(t => t.metaDescription).HasColumnName("metaDescription");
            Property(t => t.metaKeywords).HasColumnName("metaKeywords");
            Property(t => t.contentTypeID).HasColumnName("contentTypeID");
            Property(t => t.releaseDate).HasColumnName("releaseDate");
            Property(t => t.rating).HasColumnName("rating");
            Property(t => t.contentPhotoURL).HasColumnName("contentPhotoURL");
            Property(t => t.contentPhotoThumbURL).HasColumnName("contentPhotoThumbURL");
            Property(t => t.contentVideoURL).HasColumnName("contentVideoURL");
            Property(t => t.outboundURL).HasColumnName("outboundURL");
            Property(t => t.isEnabled).HasColumnName("isEnabled");
            Property(t => t.currentStatus).HasColumnName("currentStatus");
            Property(t => t.language).HasColumnName("language");
            Property(t => t.contentVideoURL2).HasColumnName("contentVideoURL2");

            // Relationships
            HasOptional(t => t.ContentType)
                .WithMany(t => t.Contents)
                .HasForeignKey(d => d.contentTypeID);
            HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.Contents)
                .HasForeignKey(d => d.createdByUserID);
        }
    }
}