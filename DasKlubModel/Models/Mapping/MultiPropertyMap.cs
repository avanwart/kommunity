using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class MultiPropertyMap : EntityTypeConfiguration<MultiProperty>
    {
        public MultiPropertyMap()
        {
            // Primary Key
            this.HasKey(t => t.multiPropertyID);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MultiProperty");
            this.Property(t => t.multiPropertyID).HasColumnName("multiPropertyID");
            this.Property(t => t.propertyTypeID).HasColumnName("propertyTypeID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.propertyContent).HasColumnName("propertyContent");

            // Relationships
            this.HasMany(t => t.Videos)
                .WithMany(t => t.MultiProperties)
                .Map(m =>
                    {
                        m.ToTable("MultiPropertyVideo");
                        m.MapLeftKey("multiPropertyID");
                        m.MapRightKey("videoID");
                    });

            this.HasRequired(t => t.PropertyType)
                .WithMany(t => t.MultiProperties)
                .HasForeignKey(d => d.propertyTypeID);

        }
    }
}
