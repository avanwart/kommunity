using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAddressMap : EntityTypeConfiguration<UserAddress>
    {
        public UserAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.userAddressID);

            // Properties
            this.Property(t => t.firstName)
                .HasMaxLength(50);

            this.Property(t => t.middleName)
                .HasMaxLength(50);

            this.Property(t => t.lastName)
                .HasMaxLength(50);

            this.Property(t => t.addressLine1)
                .HasMaxLength(75);

            this.Property(t => t.addressLine2)
                .HasMaxLength(50);

            this.Property(t => t.addressLine3)
                .HasMaxLength(50);

            this.Property(t => t.city)
                .HasMaxLength(50);

            this.Property(t => t.region)
                .HasMaxLength(50);

            this.Property(t => t.postalCode)
                .HasMaxLength(50);

            this.Property(t => t.countryISO)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.addressStatus)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UserAddress");
            this.Property(t => t.userAddressID).HasColumnName("userAddressID");
            this.Property(t => t.firstName).HasColumnName("firstName");
            this.Property(t => t.middleName).HasColumnName("middleName");
            this.Property(t => t.lastName).HasColumnName("lastName");
            this.Property(t => t.addressLine1).HasColumnName("addressLine1");
            this.Property(t => t.addressLine2).HasColumnName("addressLine2");
            this.Property(t => t.addressLine3).HasColumnName("addressLine3");
            this.Property(t => t.city).HasColumnName("city");
            this.Property(t => t.region).HasColumnName("region");
            this.Property(t => t.postalCode).HasColumnName("postalCode");
            this.Property(t => t.countryISO).HasColumnName("countryISO");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.addressStatus).HasColumnName("addressStatus");
            this.Property(t => t.choice1).HasColumnName("choice1");
            this.Property(t => t.choice2).HasColumnName("choice2");

            // Relationships
            this.HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.UserAddresses)
                .HasForeignKey(d => d.userAccountID);

        }
    }
}
