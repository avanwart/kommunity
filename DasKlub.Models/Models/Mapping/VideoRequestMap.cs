using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoRequestMap : EntityTypeConfiguration<VideoRequest>
    {
        public VideoRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.videoRequestID);

            // Properties
            this.Property(t => t.requestURL)
                .HasMaxLength(100);

            this.Property(t => t.statusType)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.videoKey)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("VideoRequest");
            this.Property(t => t.videoRequestID).HasColumnName("videoRequestID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.requestURL).HasColumnName("requestURL");
            this.Property(t => t.statusType).HasColumnName("statusType");
            this.Property(t => t.videoKey).HasColumnName("videoKey");
        }
    }
}
