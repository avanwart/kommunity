using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoMap : EntityTypeConfiguration<Video>
    {
        public VideoMap()
        {
            // Primary Key
            HasKey(t => t.videoID);

            // Properties
            Property(t => t.videoKey)
                .HasMaxLength(150);

            Property(t => t.providerKey)
                .HasMaxLength(50);

            Property(t => t.providerUserKey)
                .HasMaxLength(50);

            Property(t => t.providerCode)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("Video");
            Property(t => t.videoID).HasColumnName("videoID");
            Property(t => t.videoKey).HasColumnName("videoKey");
            Property(t => t.providerKey).HasColumnName("providerKey");
            Property(t => t.providerUserKey).HasColumnName("providerUserKey");
            Property(t => t.providerCode).HasColumnName("providerCode");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.isHidden).HasColumnName("isHidden");
            Property(t => t.isEnabled).HasColumnName("isEnabled");
            Property(t => t.statusID).HasColumnName("statusID");
            Property(t => t.duration).HasColumnName("duration");
            Property(t => t.intro).HasColumnName("intro");
            Property(t => t.lengthFromStart).HasColumnName("lengthFromStart");
            Property(t => t.volumeLevel).HasColumnName("volumeLevel");
            Property(t => t.enableTrim).HasColumnName("enableTrim");
            Property(t => t.publishDate).HasColumnName("publishDate");
        }
    }
}