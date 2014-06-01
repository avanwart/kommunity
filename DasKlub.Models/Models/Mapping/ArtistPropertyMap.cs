using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ArtistPropertyMap : EntityTypeConfiguration<ArtistProperty>
    {
        public ArtistPropertyMap()
        {
            // Primary Key
            HasKey(t => t.artistPropertyID);

            // Properties
            Property(t => t.propertyType)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("ArtistProperty");
            Property(t => t.artistPropertyID).HasColumnName("artistPropertyID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.artistID).HasColumnName("artistID");
            Property(t => t.propertyContent).HasColumnName("propertyContent");
            Property(t => t.propertyType).HasColumnName("propertyType");

            // Relationships
            HasRequired(t => t.Artist)
                .WithMany(t => t.ArtistProperties)
                .HasForeignKey(d => d.artistID);
        }
    }
}