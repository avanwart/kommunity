using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class DirectMessageMap : EntityTypeConfiguration<DirectMessage>
    {
        public DirectMessageMap()
        {
            // Primary Key
            HasKey(t => t.directMessageID);

            // Properties
            Property(t => t.message)
                .IsRequired();

            // Table & Column Mappings
            ToTable("DirectMessage");
            Property(t => t.directMessageID).HasColumnName("directMessageID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.fromUserAccountID).HasColumnName("fromUserAccountID");
            Property(t => t.toUserAccountID).HasColumnName("toUserAccountID");
            Property(t => t.isRead).HasColumnName("isRead");
            Property(t => t.message).HasColumnName("message");
            Property(t => t.isEnabled).HasColumnName("isEnabled");

            // Relationships
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.DirectMessages)
                .HasForeignKey(d => d.fromUserAccountID);
            HasRequired(t => t.UserAccount1)
                .WithMany(t => t.DirectMessages1)
                .HasForeignKey(d => d.toUserAccountID);
        }
    }
}