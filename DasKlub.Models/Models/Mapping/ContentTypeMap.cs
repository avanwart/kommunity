using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContentTypeMap : EntityTypeConfiguration<ContentType>
    {
        public ContentTypeMap()
        {
            // Primary Key
            HasKey(t => t.contentTypeID);

            // Properties
            Property(t => t.contentName)
                .HasMaxLength(50);

            Property(t => t.contentCode)
                .IsFixedLength()
                .HasMaxLength(5);

            // Table & Column Mappings
            ToTable("ContentType");
            Property(t => t.contentTypeID).HasColumnName("contentTypeID");
            Property(t => t.contentName).HasColumnName("contentName");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.contentCode).HasColumnName("contentCode");
        }
    }
}