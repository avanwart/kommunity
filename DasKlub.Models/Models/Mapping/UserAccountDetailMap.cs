using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountDetailMap : EntityTypeConfiguration<UserAccountDetailEntity>
    {
        public UserAccountDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.userAccountDetailID);

            // Properties
            this.Property(t => t.country)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.region)
                .HasMaxLength(25);

            this.Property(t => t.city)
                .HasMaxLength(25);

            this.Property(t => t.postalCode)
                .HasMaxLength(15);

            this.Property(t => t.profilePicURL)
                .HasMaxLength(75);

            this.Property(t => t.religion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.profileThumbPicURL)
                .HasMaxLength(75);

            this.Property(t => t.ethnicity)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.diet)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.externalURL)
                .HasMaxLength(100);

            this.Property(t => t.smokes)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.drinks)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.handed)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.browerType)
                .HasMaxLength(15);

            this.Property(t => t.messangerType)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.messangerName)
                .HasMaxLength(25);

            this.Property(t => t.firstName)
                .HasMaxLength(20);

            this.Property(t => t.lastName)
                .HasMaxLength(20);

            this.Property(t => t.defaultLanguage)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("UserAccountDetail");
            this.Property(t => t.userAccountDetailID).HasColumnName("userAccountDetailID");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.youAreID).HasColumnName("youAreID");
            this.Property(t => t.relationshipStatusID).HasColumnName("relationshipStatusID");
            this.Property(t => t.interestedInID).HasColumnName("interestedInID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            this.Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            this.Property(t => t.country).HasColumnName("country");
            this.Property(t => t.region).HasColumnName("region");
            this.Property(t => t.city).HasColumnName("city");
            this.Property(t => t.postalCode).HasColumnName("postalCode");
            this.Property(t => t.profilePicURL).HasColumnName("profilePicURL");
            this.Property(t => t.birthDate).HasColumnName("birthDate");
            this.Property(t => t.religion).HasColumnName("religion");
            this.Property(t => t.profileThumbPicURL).HasColumnName("profileThumbPicURL");
            this.Property(t => t.ethnicity).HasColumnName("ethnicity");
            this.Property(t => t.heightCM).HasColumnName("heightCM");
            this.Property(t => t.weightKG).HasColumnName("weightKG");
            this.Property(t => t.diet).HasColumnName("diet");
            this.Property(t => t.accountViews).HasColumnName("accountViews");
            this.Property(t => t.externalURL).HasColumnName("externalURL");
            this.Property(t => t.smokes).HasColumnName("smokes");
            this.Property(t => t.drinks).HasColumnName("drinks");
            this.Property(t => t.handed).HasColumnName("handed");
            this.Property(t => t.displayAge).HasColumnName("displayAge");
            this.Property(t => t.enableProfileLogging).HasColumnName("enableProfileLogging");
            this.Property(t => t.lastPhotoUpdate).HasColumnName("lastPhotoUpdate");
            this.Property(t => t.emailMessages).HasColumnName("emailMessages");
            this.Property(t => t.showOnMap).HasColumnName("showOnMap");
            this.Property(t => t.referringUserID).HasColumnName("referringUserID");
            this.Property(t => t.browerType).HasColumnName("browerType");
            this.Property(t => t.membersOnlyProfile).HasColumnName("membersOnlyProfile");
            this.Property(t => t.messangerType).HasColumnName("messangerType");
            this.Property(t => t.messangerName).HasColumnName("messangerName");
            this.Property(t => t.aboutDesc).HasColumnName("aboutDesc");
            this.Property(t => t.bandsSeen).HasColumnName("bandsSeen");
            this.Property(t => t.bandsToSee).HasColumnName("bandsToSee");
            this.Property(t => t.hardwareSoftware).HasColumnName("hardwareSoftware");
            this.Property(t => t.firstName).HasColumnName("firstName");
            this.Property(t => t.lastName).HasColumnName("lastName");
            this.Property(t => t.defaultLanguage).HasColumnName("defaultLanguage");
            this.Property(t => t.latitude).HasColumnName("latitude");
            this.Property(t => t.longitude).HasColumnName("longitude");
            this.Property(t => t.findUserFilter).HasColumnName("findUserFilter");

            // Relationships
            this.HasOptional(t => t.InterestedIn)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.interestedInID);
            this.HasOptional(t => t.RelationshipStatu)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.relationshipStatusID);
            this.HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.userAccountID);
            this.HasOptional(t => t.YouAre)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.youAreID);

        }
    }
}
