using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ContestVideoVoteMap : EntityTypeConfiguration<ContestVideoVote>
    {
        public ContestVideoVoteMap()
        {
            // Primary Key
            HasKey(t => t.contestVideoVoteID);

            // Properties
            // Table & Column Mappings
            ToTable("ContestVideoVote");
            Property(t => t.contestVideoVoteID).HasColumnName("contestVideoVoteID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.contestVideoID).HasColumnName("contestVideoID");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");

            // Relationships
            HasOptional(t => t.ContestVideo)
                .WithMany(t => t.ContestVideoVotes)
                .HasForeignKey(d => d.contestVideoID);
            HasOptional(t => t.UserAccountEntity)
                .WithMany(t => t.ContestVideoVotes)
                .HasForeignKey(d => d.userAccountID);
        }
    }
}