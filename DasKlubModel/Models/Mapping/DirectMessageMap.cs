using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class DirectMessageMap : EntityTypeConfiguration<DirectMessage>
    {
        public DirectMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.directMessageID);

            // Properties
            this.Property(t => t.message)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("DirectMessage");
            this.Property(t => t.directMessageID).HasColumnName("directMessageID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.fromUserAccountID).HasColumnName("fromUserAccountID");
            this.Property(t => t.toUserAccountID).HasColumnName("toUserAccountID");
            this.Property(t => t.isRead).HasColumnName("isRead");
            this.Property(t => t.message).HasColumnName("message");
            this.Property(t => t.isEnabled).HasColumnName("isEnabled");

            // Relationships
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.DirectMessages)
                .HasForeignKey(d => d.fromUserAccountID);
            this.HasRequired(t => t.UserAccount1)
                .WithMany(t => t.DirectMessages1)
                .HasForeignKey(d => d.toUserAccountID);

        }
    }
}
