using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ArtistPropertyMap : EntityTypeConfiguration<ArtistProperty>
    {
        public ArtistPropertyMap()
        {
            // Primary Key
            this.HasKey(t => t.artistPropertyID);

            // Properties
            this.Property(t => t.propertyType)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("ArtistProperty");
            this.Property(t => t.artistPropertyID).HasColumnName("artistPropertyID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.artistID).HasColumnName("artistID");
            this.Property(t => t.propertyContent).HasColumnName("propertyContent");
            this.Property(t => t.propertyType).HasColumnName("propertyType");

            // Relationships
            this.HasRequired(t => t.Artist)
                .WithMany(t => t.ArtistProperties)
                .HasForeignKey(d => d.artistID);

        }
    }
}
