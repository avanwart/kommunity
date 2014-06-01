using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class HostedVideoLogMap : EntityTypeConfiguration<HostedVideoLog>
    {
        public HostedVideoLogMap()
        {
            // Primary Key
            HasKey(t => t.videoLogID);

            // Properties
            Property(t => t.viewURL)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.ipAddress)
                .HasMaxLength(25);

            Property(t => t.videoType)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("HostedVideoLog");
            Property(t => t.videoLogID).HasColumnName("videoLogID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.viewURL).HasColumnName("viewURL");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
            Property(t => t.secondsElapsed).HasColumnName("secondsElapsed");
            Property(t => t.videoType).HasColumnName("videoType");
        }
    }
}