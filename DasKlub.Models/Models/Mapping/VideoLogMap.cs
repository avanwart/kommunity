using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoLogMap : EntityTypeConfiguration<VideoLog>
    {
        public VideoLogMap()
        {
            // Primary Key
            this.HasKey(t => t.videoLogID);

            // Properties
            this.Property(t => t.ipAddress)
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("VideoLog");
            this.Property(t => t.videoLogID).HasColumnName("videoLogID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.videoID).HasColumnName("videoID");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
        }
    }
}
