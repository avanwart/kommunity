using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusCommentAcknowledgementMap : EntityTypeConfiguration<StatusCommentAcknowledgement>
    {
        public StatusCommentAcknowledgementMap()
        {
            // Primary Key
            this.HasKey(t => t.statusCommentAcknowledgementID);

            // Properties
            this.Property(t => t.acknowledgementType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("StatusCommentAcknowledgement");
            this.Property(t => t.statusCommentAcknowledgementID).HasColumnName("statusCommentAcknowledgementID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.statusCommentID).HasColumnName("statusCommentID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.acknowledgementType).HasColumnName("acknowledgementType");

            // Relationships
            this.HasRequired(t => t.StatusComment)
                .WithMany(t => t.StatusCommentAcknowledgements)
                .HasForeignKey(d => d.statusCommentID);

        }
    }
}
