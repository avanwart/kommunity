using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class SongPropertyMap : EntityTypeConfiguration<SongProperty>
    {
        public SongPropertyMap()
        {
            // Primary Key
            HasKey(t => t.songPropertyID);

            // Properties
            Property(t => t.propertyType)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("SongProperty");
            Property(t => t.songPropertyID).HasColumnName("songPropertyID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.songID).HasColumnName("songID");
            Property(t => t.propertyContent).HasColumnName("propertyContent");
            Property(t => t.propertyType).HasColumnName("propertyType");

            // Relationships
            HasRequired(t => t.Song)
                .WithMany(t => t.SongProperties)
                .HasForeignKey(d => d.songID);
        }
    }
}