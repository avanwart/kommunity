using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class VoteMap : EntityTypeConfiguration<Vote>
    {
        public VoteMap()
        {
            // Primary Key
            this.HasKey(t => t.voteID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Vote");
            this.Property(t => t.voteID).HasColumnName("voteID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.videoID).HasColumnName("videoID");
            this.Property(t => t.score).HasColumnName("score");
        }
    }
}
