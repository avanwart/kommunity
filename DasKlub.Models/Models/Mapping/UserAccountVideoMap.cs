using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountVideoMap : EntityTypeConfiguration<UserAccountVideo>
    {
        public UserAccountVideoMap()
        {
            // Primary Key
            HasKey(t => new {t.videoID, t.userAccountID, t.videoType});

            // Properties
            Property(t => t.videoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.userAccountID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.videoType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            ToTable("UserAccountVideo");
            Property(t => t.videoID).HasColumnName("videoID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.videoType).HasColumnName("videoType");

            // Relationships
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserAccountVideos)
                .HasForeignKey(d => d.userAccountID);
            HasRequired(t => t.Video)
                .WithMany(t => t.UserAccountVideos)
                .HasForeignKey(d => d.videoID);
        }
    }
}