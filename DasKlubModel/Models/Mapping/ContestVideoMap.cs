using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContestVideoMap : EntityTypeConfiguration<ContestVideo>
    {
        public ContestVideoMap()
        {
            // Primary Key
            this.HasKey(t => t.contestVideoID);

            // Properties
            this.Property(t => t.subContest)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("ContestVideo");
            this.Property(t => t.contestVideoID).HasColumnName("contestVideoID");
            this.Property(t => t.videoID).HasColumnName("videoID");
            this.Property(t => t.contestID).HasColumnName("contestID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.contestRank).HasColumnName("contestRank");
            this.Property(t => t.subContest).HasColumnName("subContest");

            // Relationships
            this.HasRequired(t => t.Contest)
                .WithMany(t => t.ContestVideos)
                .HasForeignKey(d => d.contestID);
            this.HasRequired(t => t.Video)
                .WithMany(t => t.ContestVideos)
                .HasForeignKey(d => d.videoID);

        }
    }
}
