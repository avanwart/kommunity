using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContestVideoVoteMap : EntityTypeConfiguration<ContestVideoVote>
    {
        public ContestVideoVoteMap()
        {
            // Primary Key
            this.HasKey(t => t.contestVideoVoteID);

            // Properties
            // Table & Column Mappings
            this.ToTable("ContestVideoVote");
            this.Property(t => t.contestVideoVoteID).HasColumnName("contestVideoVoteID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.contestVideoID).HasColumnName("contestVideoID");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");

            // Relationships
            this.HasOptional(t => t.ContestVideo)
                .WithMany(t => t.ContestVideoVotes)
                .HasForeignKey(d => d.contestVideoID);
            this.HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.ContestVideoVotes)
                .HasForeignKey(d => d.userAccountID);

        }
    }
}
