using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAddressMap : EntityTypeConfiguration<UserAddress>
    {
        public UserAddressMap()
        {
            // Primary Key
            HasKey(t => t.userAddressID);

            // Properties
            Property(t => t.firstName)
                .HasMaxLength(50);

            Property(t => t.middleName)
                .HasMaxLength(50);

            Property(t => t.lastName)
                .HasMaxLength(50);

            Property(t => t.addressLine1)
                .HasMaxLength(75);

            Property(t => t.addressLine2)
                .HasMaxLength(50);

            Property(t => t.addressLine3)
                .HasMaxLength(50);

            Property(t => t.city)
                .HasMaxLength(50);

            Property(t => t.region)
                .HasMaxLength(50);

            Property(t => t.postalCode)
                .HasMaxLength(50);

            Property(t => t.countryISO)
                .IsFixedLength()
                .HasMaxLength(2);

            Property(t => t.addressStatus)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("UserAddress");
            Property(t => t.userAddressID).HasColumnName("userAddressID");
            Property(t => t.firstName).HasColumnName("firstName");
            Property(t => t.middleName).HasColumnName("middleName");
            Property(t => t.lastName).HasColumnName("lastName");
            Property(t => t.addressLine1).HasColumnName("addressLine1");
            Property(t => t.addressLine2).HasColumnName("addressLine2");
            Property(t => t.addressLine3).HasColumnName("addressLine3");
            Property(t => t.city).HasColumnName("city");
            Property(t => t.region).HasColumnName("region");
            Property(t => t.postalCode).HasColumnName("postalCode");
            Property(t => t.countryISO).HasColumnName("countryISO");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.addressStatus).HasColumnName("addressStatus");
            Property(t => t.choice1).HasColumnName("choice1");
            Property(t => t.choice2).HasColumnName("choice2");

            // Relationships
            HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.UserAddresses)
                .HasForeignKey(d => d.userAccountID);
        }
    }
}