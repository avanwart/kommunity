using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContentCommentMap : EntityTypeConfiguration<ContentComment>
    {
        public ContentCommentMap()
        {
            // Primary Key
            HasKey(t => t.contentCommentID);

            // Properties
            Property(t => t.statusType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.fromName)
                .HasMaxLength(50);

            Property(t => t.fromEmail)
                .HasMaxLength(50);

            Property(t => t.ipAddress)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("ContentComment");
            Property(t => t.contentCommentID).HasColumnName("contentCommentID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.statusType).HasColumnName("statusType");
            Property(t => t.detail).HasColumnName("detail");
            Property(t => t.contentID).HasColumnName("contentID");
            Property(t => t.fromName).HasColumnName("fromName");
            Property(t => t.fromEmail).HasColumnName("fromEmail");
            Property(t => t.ipAddress).HasColumnName("ipAddress");

            // Relationships
            HasOptional(t => t.Content)
                .WithMany(t => t.ContentComments)
                .HasForeignKey(d => d.contentID);
            HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.ContentComments)
                .HasForeignKey(d => d.createdByUserID);
        }
    }
}