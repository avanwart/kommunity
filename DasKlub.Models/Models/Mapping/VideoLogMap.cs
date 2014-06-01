using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoLogMap : EntityTypeConfiguration<VideoLog>
    {
        public VideoLogMap()
        {
            // Primary Key
            HasKey(t => t.videoLogID);

            // Properties
            Property(t => t.ipAddress)
                .HasMaxLength(25);

            // Table & Column Mappings
            ToTable("VideoLog");
            Property(t => t.videoLogID).HasColumnName("videoLogID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.videoID).HasColumnName("videoID");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
        }
    }
}