using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class StatusCommentMap : EntityTypeConfiguration<StatusComment>
    {
        public StatusCommentMap()
        {
            // Primary Key
            HasKey(t => t.statusCommentID);

            // Properties
            Property(t => t.statusType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.message)
                .IsRequired();

            // Table & Column Mappings
            ToTable("StatusComment");
            Property(t => t.statusCommentID).HasColumnName("statusCommentID");
            Property(t => t.statusUpdateID).HasColumnName("statusUpdateID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.statusType).HasColumnName("statusType");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.message).HasColumnName("message");

            // Relationships
            HasRequired(t => t.StatusUpdate)
                .WithMany(t => t.StatusComments)
                .HasForeignKey(d => d.statusUpdateID);
        }
    }
}