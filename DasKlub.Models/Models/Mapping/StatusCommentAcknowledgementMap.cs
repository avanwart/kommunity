using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusCommentAcknowledgementMap : EntityTypeConfiguration<StatusCommentAcknowledgement>
    {
        public StatusCommentAcknowledgementMap()
        {
            // Primary Key
            HasKey(t => t.statusCommentAcknowledgementID);

            // Properties
            Property(t => t.acknowledgementType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("StatusCommentAcknowledgement");
            Property(t => t.statusCommentAcknowledgementID).HasColumnName("statusCommentAcknowledgementID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.statusCommentID).HasColumnName("statusCommentID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.acknowledgementType).HasColumnName("acknowledgementType");

            // Relationships
            HasRequired(t => t.StatusComment)
                .WithMany(t => t.StatusCommentAcknowledgements)
                .HasForeignKey(d => d.statusCommentID);
        }
    }
}