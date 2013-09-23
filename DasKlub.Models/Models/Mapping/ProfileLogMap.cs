using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class ProfileLogMap : EntityTypeConfiguration<ProfileLog>
    {
        public ProfileLogMap()
        {
            // Primary Key
            this.HasKey(t => t.profileLogID);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProfileLog");
            this.Property(t => t.profileLogID).HasColumnName("profileLogID");
            this.Property(t => t.lookingUserAccountID).HasColumnName("lookingUserAccountID");
            this.Property(t => t.lookedAtUserAccountID).HasColumnName("lookedAtUserAccountID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");

            // Relationships
            this.HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.ProfileLogs)
                .HasForeignKey(d => d.lookingUserAccountID);
            this.HasOptional(t => t.UserAccount1)
                .WithMany(t => t.ProfileLogs1)
                .HasForeignKey(d => d.lookedAtUserAccountID);

        }
    }
}
