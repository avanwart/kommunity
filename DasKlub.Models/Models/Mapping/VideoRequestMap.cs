using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoRequestMap : EntityTypeConfiguration<VideoRequest>
    {
        public VideoRequestMap()
        {
            // Primary Key
            HasKey(t => t.videoRequestID);

            // Properties
            Property(t => t.requestURL)
                .HasMaxLength(100);

            Property(t => t.statusType)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.videoKey)
                .HasMaxLength(20);

            // Table & Column Mappings
            ToTable("VideoRequest");
            Property(t => t.videoRequestID).HasColumnName("videoRequestID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.requestURL).HasColumnName("requestURL");
            Property(t => t.statusType).HasColumnName("statusType");
            Property(t => t.videoKey).HasColumnName("videoKey");
        }
    }
}