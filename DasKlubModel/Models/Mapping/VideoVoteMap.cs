using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoVoteMap : EntityTypeConfiguration<VideoVote>
    {
        public VideoVoteMap()
        {
            // Primary Key
            this.HasKey(t => t.videoVoteID);

            // Properties
            this.Property(t => t.ipAddress)
                .HasMaxLength(50);

            this.Property(t => t.singlePick1)
                .HasMaxLength(50);

            this.Property(t => t.singlePick2)
                .HasMaxLength(50);

            this.Property(t => t.singlePick3)
                .HasMaxLength(50);

            this.Property(t => t.groupPick1)
                .HasMaxLength(50);

            this.Property(t => t.groupPick2)
                .HasMaxLength(50);

            this.Property(t => t.groupPick3)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VideoVote");
            this.Property(t => t.videoVoteID).HasColumnName("videoVoteID");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.singlePick1).HasColumnName("singlePick1");
            this.Property(t => t.singlePick2).HasColumnName("singlePick2");
            this.Property(t => t.singlePick3).HasColumnName("singlePick3");
            this.Property(t => t.groupPick1).HasColumnName("groupPick1");
            this.Property(t => t.groupPick2).HasColumnName("groupPick2");
            this.Property(t => t.groupPick3).HasColumnName("groupPick3");
        }
    }
}
