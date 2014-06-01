using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContestVideoMap : EntityTypeConfiguration<ContestVideo>
    {
        public ContestVideoMap()
        {
            // Primary Key
            HasKey(t => t.contestVideoID);

            // Properties
            Property(t => t.subContest)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("ContestVideo");
            Property(t => t.contestVideoID).HasColumnName("contestVideoID");
            Property(t => t.videoID).HasColumnName("videoID");
            Property(t => t.contestID).HasColumnName("contestID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.contestRank).HasColumnName("contestRank");
            Property(t => t.subContest).HasColumnName("subContest");

            // Relationships
            HasRequired(t => t.Contest)
                .WithMany(t => t.ContestVideos)
                .HasForeignKey(d => d.contestID);
            HasRequired(t => t.Video)
                .WithMany(t => t.ContestVideos)
                .HasForeignKey(d => d.videoID);
        }
    }
}