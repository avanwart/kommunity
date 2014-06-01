using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserConnectionMap : EntityTypeConfiguration<UserConnection>
    {
        public UserConnectionMap()
        {
            // Primary Key
            HasKey(t => t.userConnectionID);

            // Properties
            Property(t => t.statusType)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("UserConnection");
            Property(t => t.userConnectionID).HasColumnName("userConnectionID");
            Property(t => t.fromUserAccountID).HasColumnName("fromUserAccountID");
            Property(t => t.toUserAccountID).HasColumnName("toUserAccountID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.statusType).HasColumnName("statusType");
            Property(t => t.isConfirmed).HasColumnName("isConfirmed");

            // Relationships
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserConnections)
                .HasForeignKey(d => d.fromUserAccountID);
            HasRequired(t => t.UserAccount1)
                .WithMany(t => t.UserConnections1)
                .HasForeignKey(d => d.toUserAccountID);
        }
    }
}