using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VenueMap : EntityTypeConfiguration<Venue>
    {
        public VenueMap()
        {
            // Primary Key
            this.HasKey(t => t.venueID);

            // Properties
            this.Property(t => t.venueName)
                .HasMaxLength(50);

            this.Property(t => t.addressLine1)
                .HasMaxLength(50);

            this.Property(t => t.addressLine2)
                .HasMaxLength(25);

            this.Property(t => t.city)
                .HasMaxLength(25);

            this.Property(t => t.region)
                .HasMaxLength(20);

            this.Property(t => t.postalCode)
                .HasMaxLength(15);

            this.Property(t => t.countryISO)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.phoneNumber)
                .HasMaxLength(15);

            this.Property(t => t.venueType)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Venue");
            this.Property(t => t.venueID).HasColumnName("venueID");
            this.Property(t => t.venueName).HasColumnName("venueName");
            this.Property(t => t.addressLine1).HasColumnName("addressLine1");
            this.Property(t => t.addressLine2).HasColumnName("addressLine2");
            this.Property(t => t.city).HasColumnName("city");
            this.Property(t => t.region).HasColumnName("region");
            this.Property(t => t.postalCode).HasColumnName("postalCode");
            this.Property(t => t.countryISO).HasColumnName("countryISO");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.venueURL).HasColumnName("venueURL");
            this.Property(t => t.isEnabled).HasColumnName("isEnabled");
            this.Property(t => t.latitude).HasColumnName("latitude");
            this.Property(t => t.longitude).HasColumnName("longitude");
            this.Property(t => t.phoneNumber).HasColumnName("phoneNumber");
            this.Property(t => t.venueType).HasColumnName("venueType");
            this.Property(t => t.description).HasColumnName("description");
        }
    }
}
