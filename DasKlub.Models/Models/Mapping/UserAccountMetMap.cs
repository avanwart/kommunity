using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountMetMap : EntityTypeConfiguration<UserAccountMet>
    {
        public UserAccountMetMap()
        {
            // Primary Key
            HasKey(t => new {t.userAccountRequester, t.userAccounted});

            // Properties
            Property(t => t.userAccountRequester)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.userAccounted)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("UserAccountMet");
            Property(t => t.userAccountRequester).HasColumnName("userAccountRequester");
            Property(t => t.userAccounted).HasColumnName("userAccounted");
            Property(t => t.haveMet).HasColumnName("haveMet");

            // Relationships
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserAccountMets)
                .HasForeignKey(d => d.userAccounted);
            HasRequired(t => t.UserAccount1)
                .WithMany(t => t.UserAccountMets1)
                .HasForeignKey(d => d.userAccountRequester);
        }
    }
}