using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountVideoMap : EntityTypeConfiguration<UserAccountVideo>
    {
        public UserAccountVideoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.videoID, t.userAccountID, t.videoType });

            // Properties
            this.Property(t => t.videoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.userAccountID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.videoType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UserAccountVideo");
            this.Property(t => t.videoID).HasColumnName("videoID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.videoType).HasColumnName("videoType");

            // Relationships
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.UserAccountVideos)
                .HasForeignKey(d => d.userAccountID);
            this.HasRequired(t => t.Video)
                .WithMany(t => t.UserAccountVideos)
                .HasForeignKey(d => d.videoID);

        }
    }
}
