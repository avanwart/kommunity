using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SongPropertyMap : EntityTypeConfiguration<SongProperty>
    {
        public SongPropertyMap()
        {
            // Primary Key
            this.HasKey(t => t.songPropertyID);

            // Properties
            this.Property(t => t.propertyType)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("SongProperty");
            this.Property(t => t.songPropertyID).HasColumnName("songPropertyID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.songID).HasColumnName("songID");
            this.Property(t => t.propertyContent).HasColumnName("propertyContent");
            this.Property(t => t.propertyType).HasColumnName("propertyType");

            // Relationships
            this.HasRequired(t => t.Song)
                .WithMany(t => t.SongProperties)
                .HasForeignKey(d => d.songID);

        }
    }
}
