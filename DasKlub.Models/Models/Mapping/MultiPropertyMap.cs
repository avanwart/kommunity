using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class MultiPropertyMap : EntityTypeConfiguration<MultiProperty>
    {
        public MultiPropertyMap()
        {
            // Primary Key
            HasKey(t => t.multiPropertyID);

            // Properties
            Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("MultiProperty");
            Property(t => t.multiPropertyID).HasColumnName("multiPropertyID");
            Property(t => t.propertyTypeID).HasColumnName("propertyTypeID");
            Property(t => t.name).HasColumnName("name");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.propertyContent).HasColumnName("propertyContent");

            // Relationships
            HasMany(t => t.Videos)
                .WithMany(t => t.MultiProperties)
                .Map(m =>
                {
                    m.ToTable("MultiPropertyVideo");
                    m.MapLeftKey("multiPropertyID");
                    m.MapRightKey("videoID");
                });

            HasRequired(t => t.PropertyType)
                .WithMany(t => t.MultiProperties)
                .HasForeignKey(d => d.propertyTypeID);
        }
    }
}