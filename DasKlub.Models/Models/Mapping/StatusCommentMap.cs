using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusCommentMap : EntityTypeConfiguration<StatusComment>
    {
        public StatusCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.statusCommentID);

            // Properties
            this.Property(t => t.statusType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.message)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("StatusComment");
            this.Property(t => t.statusCommentID).HasColumnName("statusCommentID");
            this.Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.statusType).HasColumnName("statusType");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.message).HasColumnName("message");

            // Relationships
            this.HasRequired(t => t.StatusUpdate)
                .WithMany(t => t.StatusComments)
                .HasForeignKey(d => d.statusUpdateID);

        }
    }
}
