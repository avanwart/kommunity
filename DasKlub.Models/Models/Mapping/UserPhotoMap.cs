using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserPhotoMap : EntityTypeConfiguration<UserPhoto>
    {
        public UserPhotoMap()
        {
            // Primary Key
            HasKey(t => t.userPhotoID);

            // Properties
            Property(t => t.picURL)
                .HasMaxLength(75);

            Property(t => t.thumbPicURL)
                .HasMaxLength(75);

            Property(t => t.description)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("UserPhoto");
            Property(t => t.userPhotoID).HasColumnName("userPhotoID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.picURL).HasColumnName("picURL");
            Property(t => t.thumbPicURL).HasColumnName("thumbPicURL");
            Property(t => t.description).HasColumnName("description");
            Property(t => t.rankOrder).HasColumnName("rankOrder");

            // Relationships
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserPhotoes)
                .HasForeignKey(d => d.userAccountID);
        }
    }
}