using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountMetMap : EntityTypeConfiguration<UserAccountMet>
    {
        public UserAccountMetMap()
        {
            // Primary Key
            this.HasKey(t => new { t.userAccountRequester, t.userAccounted });

            // Properties
            this.Property(t => t.userAccountRequester)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.userAccounted)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("UserAccountMet");
            this.Property(t => t.userAccountRequester).HasColumnName("userAccountRequester");
            this.Property(t => t.userAccounted).HasColumnName("userAccounted");
            this.Property(t => t.haveMet).HasColumnName("haveMet");

            // Relationships
            this.HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserAccountMets)
                .HasForeignKey(d => d.userAccounted);
            this.HasRequired(t => t.UserAccount1)
                .WithMany(t => t.UserAccountMets1)
                .HasForeignKey(d => d.userAccountRequester);

        }
    }
}
