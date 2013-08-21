using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserConnectionMap : EntityTypeConfiguration<UserConnection>
    {
        public UserConnectionMap()
        {
            // Primary Key
            this.HasKey(t => t.userConnectionID);

            // Properties
            this.Property(t => t.statusType)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UserConnection");
            this.Property(t => t.userConnectionID).HasColumnName("userConnectionID");
            this.Property(t => t.fromUserAccountID).HasColumnName("fromUserAccountID");
            this.Property(t => t.toUserAccountID).HasColumnName("toUserAccountID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.statusType).HasColumnName("statusType");
            this.Property(t => t.isConfirmed).HasColumnName("isConfirmed");

            // Relationships
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.UserConnections)
                .HasForeignKey(d => d.fromUserAccountID);
            this.HasRequired(t => t.UserAccount1)
                .WithMany(t => t.UserConnections1)
                .HasForeignKey(d => d.toUserAccountID);

        }
    }
}
