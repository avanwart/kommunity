using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContentCommentMap : EntityTypeConfiguration<ContentComment>
    {
        public ContentCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.contentCommentID);

            // Properties
            this.Property(t => t.statusType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.fromName)
                .HasMaxLength(50);

            this.Property(t => t.fromEmail)
                .HasMaxLength(50);

            this.Property(t => t.ipAddress)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ContentComment");
            this.Property(t => t.contentCommentID).HasColumnName("contentCommentID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.statusType).HasColumnName("statusType");
            this.Property(t => t.detail).HasColumnName("detail");
            this.Property(t => t.contentID).HasColumnName("contentID");
            this.Property(t => t.fromName).HasColumnName("fromName");
            this.Property(t => t.fromEmail).HasColumnName("fromEmail");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");

            // Relationships
            this.HasOptional(t => t.Content)
                .WithMany(t => t.ContentComments)
                .HasForeignKey(d => d.contentID);
            this.HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.ContentComments)
                .HasForeignKey(d => d.createdByUserID);

        }
    }
}
