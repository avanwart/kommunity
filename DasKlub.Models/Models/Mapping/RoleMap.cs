using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            HasKey(t => t.roleID);

            // Properties
            Property(t => t.roleName)
                .HasMaxLength(50);

            Property(t => t.description)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("Role");
            Property(t => t.roleID).HasColumnName("roleID");
            Property(t => t.roleName).HasColumnName("roleName");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updatedDate).HasColumnName("updatedDate");
            Property(t => t.createdByEndUserID).HasColumnName("createdByEndUserID");
            Property(t => t.updatedByEndUserID).HasColumnName("updatedByEndUserID");
            Property(t => t.description).HasColumnName("description");

            // Relationships
            HasMany(t => t.UserAccounts)
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