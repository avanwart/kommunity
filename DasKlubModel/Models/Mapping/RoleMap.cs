using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.roleID);

            // Properties
            this.Property(t => t.roleName)
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.roleID).HasColumnName("roleID");
            this.Property(t => t.roleName).HasColumnName("roleName");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updatedDate).HasColumnName("updatedDate");
            this.Property(t => t.createdByEndUserID).HasColumnName("createdByEndUserID");
            this.Property(t => t.updatedByEndUserID).HasColumnName("updatedByEndUserID");
            this.Property(t => t.description).HasColumnName("description");

            // Relationships
            this.HasMany(t => t.UserAccounts)
                .WithMany(t => t.Roles)
                .Map(m =>
                    {
                        m.ToTable("UserAccountRole");
                        m.MapLeftKey("roleID");
                        m.MapRightKey("userAccountID");
                    });


        }
    }
}
