using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserPhotoMap : EntityTypeConfiguration<UserPhoto>
    {
        public UserPhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.userPhotoID);

            // Properties
            this.Property(t => t.picURL)
                .HasMaxLength(75);

            this.Property(t => t.thumbPicURL)
                .HasMaxLength(75);

            this.Property(t => t.description)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("UserPhoto");
            this.Property(t => t.userPhotoID).HasColumnName("userPhotoID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.picURL).HasColumnName("picURL");
            this.Property(t => t.thumbPicURL).HasColumnName("thumbPicURL");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            this.HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserPhotoes)
                .HasForeignKey(d => d.userAccountID);

        }
    }
}
