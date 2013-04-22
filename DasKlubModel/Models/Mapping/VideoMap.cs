using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoMap : EntityTypeConfiguration<Video>
    {
        public VideoMap()
        {
            // Primary Key
            this.HasKey(t => t.videoID);

            // Properties
            this.Property(t => t.videoKey)
                .HasMaxLength(150);

            this.Property(t => t.providerKey)
                .HasMaxLength(50);

            this.Property(t => t.providerUserKey)
                .HasMaxLength(50);

            this.Property(t => t.providerCode)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("Video");
            this.Property(t => t.videoID).HasColumnName("videoID");
            this.Property(t => t.videoKey).HasColumnName("videoKey");
            this.Property(t => t.providerKey).HasColumnName("providerKey");
            this.Property(t => t.providerUserKey).HasColumnName("providerUserKey");
            this.Property(t => t.providerCode).HasColumnName("providerCode");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.isHidden).HasColumnName("isHidden");
            this.Property(t => t.isEnabled).HasColumnName("isEnabled");
            this.Property(t => t.statusID).HasColumnName("statusID");
            this.Property(t => t.duration).HasColumnName("duration");
            this.Property(t => t.intro).HasColumnName("intro");
            this.Property(t => t.lengthFromStart).HasColumnName("lengthFromStart");
            this.Property(t => t.volumeLevel).HasColumnName("volumeLevel");
            this.Property(t => t.enableTrim).HasColumnName("enableTrim");
            this.Property(t => t.publishDate).HasColumnName("publishDate");
        }
    }
}
