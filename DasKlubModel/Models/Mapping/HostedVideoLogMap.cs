using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class HostedVideoLogMap : EntityTypeConfiguration<HostedVideoLog>
    {
        public HostedVideoLogMap()
        {
            // Primary Key
            this.HasKey(t => t.videoLogID);

            // Properties
            this.Property(t => t.viewURL)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.ipAddress)
                .HasMaxLength(25);

            this.Property(t => t.videoType)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("HostedVideoLog");
            this.Property(t => t.videoLogID).HasColumnName("videoLogID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.viewURL).HasColumnName("viewURL");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
            this.Property(t => t.secondsElapsed).HasColumnName("secondsElapsed");
            this.Property(t => t.videoType).HasColumnName("videoType");
        }
    }
}
