using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VideoVoteMap : EntityTypeConfiguration<VideoVote>
    {
        public VideoVoteMap()
        {
            // Primary Key
            HasKey(t => t.videoVoteID);

            // Properties
            Property(t => t.ipAddress)
                .HasMaxLength(50);

            Property(t => t.singlePick1)
                .HasMaxLength(50);

            Property(t => t.singlePick2)
                .HasMaxLength(50);

            Property(t => t.singlePick3)
                .HasMaxLength(50);

            Property(t => t.groupPick1)
                .HasMaxLength(50);

            Property(t => t.groupPick2)
                .HasMaxLength(50);

            Property(t => t.groupPick3)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("VideoVote");
            Property(t => t.videoVoteID).HasColumnName("videoVoteID");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.singlePick1).HasColumnName("singlePick1");
            Property(t => t.singlePick2).HasColumnName("singlePick2");
            Property(t => t.singlePick3).HasColumnName("singlePick3");
            Property(t => t.groupPick1).HasColumnName("groupPick1");
            Property(t => t.groupPick2).HasColumnName("groupPick2");
            Property(t => t.groupPick3).HasColumnName("groupPick3");
        }
    }
}