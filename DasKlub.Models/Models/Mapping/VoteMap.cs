using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VoteMap : EntityTypeConfiguration<Vote>
    {
        public VoteMap()
        {
            // Primary Key
            HasKey(t => t.voteID);

            // Properties
            // Table & Column Mappings
            ToTable("Vote");
            Property(t => t.voteID).HasColumnName("voteID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.videoID).HasColumnName("videoID");
            Property(t => t.score).HasColumnName("score");
        }
    }
}