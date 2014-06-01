using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ProfileLogMap : EntityTypeConfiguration<ProfileLog>
    {
        public ProfileLogMap()
        {
            // Primary Key
            HasKey(t => t.profileLogID);

            // Properties
            // Table & Column Mappings
            ToTable("ProfileLog");
            Property(t => t.profileLogID).HasColumnName("profileLogID");
            Property(t => t.lookingUserAccountID).HasColumnName("lookingUserAccountID");
            Property(t => t.lookedAtUserAccountID).HasColumnName("lookedAtUserAccountID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");

            // Relationships
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.ProfileLogs)
                .HasForeignKey(d => d.lookingUserAccountID);
            HasOptional(t => t.UserAccount1)
                .WithMany(t => t.ProfileLogs1)
                .HasForeignKey(d => d.lookedAtUserAccountID);
        }
    }
}