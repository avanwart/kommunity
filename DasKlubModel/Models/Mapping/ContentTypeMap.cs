using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContentTypeMap : EntityTypeConfiguration<ContentType>
    {
        public ContentTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.contentTypeID);

            // Properties
            this.Property(t => t.contentName)
                .HasMaxLength(50);

            this.Property(t => t.contentCode)
                .IsFixedLength()
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("ContentType");
            this.Property(t => t.contentTypeID).HasColumnName("contentTypeID");
            this.Property(t => t.contentName).HasColumnName("contentName");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.contentCode).HasColumnName("contentCode");
        }
    }
}
