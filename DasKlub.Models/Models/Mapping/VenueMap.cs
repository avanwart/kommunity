using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VenueMap : EntityTypeConfiguration<Venue>
    {
        public VenueMap()
        {
            // Primary Key
            HasKey(t => t.venueID);

            // Properties
            Property(t => t.venueName)
                .HasMaxLength(50);

            Property(t => t.addressLine1)
                .HasMaxLength(50);

            Property(t => t.addressLine2)
                .HasMaxLength(25);

            Property(t => t.city)
                .HasMaxLength(25);

            Property(t => t.region)
                .HasMaxLength(20);

            Property(t => t.postalCode)
                .HasMaxLength(15);

            Property(t => t.countryISO)
                .IsFixedLength()
                .HasMaxLength(2);

            Property(t => t.phoneNumber)
                .HasMaxLength(15);

            Property(t => t.venueType)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("Venue");
            Property(t => t.venueID).HasColumnName("venueID");
            Property(t => t.venueName).HasColumnName("venueName");
            Property(t => t.addressLine1).HasColumnName("addressLine1");
            Property(t => t.addressLine2).HasColumnName("addressLine2");
            Property(t => t.city).HasColumnName("city");
            Property(t => t.region).HasColumnName("region");
            Property(t => t.postalCode).HasColumnName("postalCode");
            Property(t => t.countryISO).HasColumnName("countryISO");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.venueURL).HasColumnName("venueURL");
            Property(t => t.isEnabled).HasColumnName("isEnabled");
            Property(t => t.latitude).HasColumnName("latitude");
            Property(t => t.longitude).HasColumnName("longitude");
            Property(t => t.phoneNumber).HasColumnName("phoneNumber");
            Property(t => t.venueType).HasColumnName("venueType");
            Property(t => t.description).HasColumnName("description");
        }
    }
}