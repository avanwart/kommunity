using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountDetailMap : EntityTypeConfiguration<UserAccountDetailEntity>
    {
        public UserAccountDetailMap()
        {
            // Primary Key
            HasKey(t => t.userAccountDetailID);

            // Properties
            Property(t => t.country)
                .IsFixedLength()
                .HasMaxLength(2);

            Property(t => t.region)
                .HasMaxLength(25);

            Property(t => t.city)
                .HasMaxLength(25);

            Property(t => t.postalCode)
                .HasMaxLength(15);

            Property(t => t.profilePicURL)
                .HasMaxLength(75);

            Property(t => t.religion)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.profileThumbPicURL)
                .HasMaxLength(75);

            Property(t => t.ethnicity)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.diet)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.externalURL)
                .HasMaxLength(100);

            Property(t => t.smokes)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.drinks)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.handed)
                .IsFixedLength()
                .HasMaxLength(1);

            Property(t => t.browerType)
                .HasMaxLength(15);

            Property(t => t.messangerType)
                .IsFixedLength()
                .HasMaxLength(2);

            Property(t => t.messangerName)
                .HasMaxLength(25);

            Property(t => t.firstName)
                .HasMaxLength(20);

            Property(t => t.lastName)
                .HasMaxLength(20);

            Property(t => t.defaultLanguage)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("UserAccountDetail");
            Property(t => t.userAccountDetailID).HasColumnName("userAccountDetailID");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.youAreID).HasColumnName("youAreID");
            Property(t => t.relationshipStatusID).HasColumnName("relationshipStatusID");
            Property(t => t.interestedInID).HasColumnName("interestedInID");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.updatedByUserID).HasColumnName("updatedByUserID");
            Property(t => t.createdByUserID).HasColumnName("createdByUserID");
            Property(t => t.country).HasColumnName("country");
            Property(t => t.region).HasColumnName("region");
            Property(t => t.city).HasColumnName("city");
            Property(t => t.postalCode).HasColumnName("postalCode");
            Property(t => t.profilePicURL).HasColumnName("profilePicURL");
            Property(t => t.birthDate).HasColumnName("birthDate");
            Property(t => t.religion).HasColumnName("religion");
            Property(t => t.profileThumbPicURL).HasColumnName("profileThumbPicURL");
            Property(t => t.ethnicity).HasColumnName("ethnicity");
            Property(t => t.heightCM).HasColumnName("heightCM");
            Property(t => t.weightKG).HasColumnName("weightKG");
            Property(t => t.diet).HasColumnName("diet");
            Property(t => t.accountViews).HasColumnName("accountViews");
            Property(t => t.externalURL).HasColumnName("externalURL");
            Property(t => t.smokes).HasColumnName("smokes");
            Property(t => t.drinks).HasColumnName("drinks");
            Property(t => t.handed).HasColumnName("handed");
            Property(t => t.displayAge).HasColumnName("displayAge");
            Property(t => t.enableProfileLogging).HasColumnName("enableProfileLogging");
            Property(t => t.lastPhotoUpdate).HasColumnName("lastPhotoUpdate");
            Property(t => t.emailMessages).HasColumnName("emailMessages");
            Property(t => t.showOnMap).HasColumnName("showOnMap");
            Property(t => t.referringUserID).HasColumnName("referringUserID");
            Property(t => t.browerType).HasColumnName("browerType");
            Property(t => t.membersOnlyProfile).HasColumnName("membersOnlyProfile");
            Property(t => t.messangerType).HasColumnName("messangerType");
            Property(t => t.messangerName).HasColumnName("messangerName");
            Property(t => t.aboutDesc).HasColumnName("aboutDesc");
            Property(t => t.bandsSeen).HasColumnName("bandsSeen");
            Property(t => t.bandsToSee).HasColumnName("bandsToSee");
            Property(t => t.hardwareSoftware).HasColumnName("hardwareSoftware");
            Property(t => t.firstName).HasColumnName("firstName");
            Property(t => t.lastName).HasColumnName("lastName");
            Property(t => t.defaultLanguage).HasColumnName("defaultLanguage");
            Property(t => t.latitude).HasColumnName("latitude");
            Property(t => t.longitude).HasColumnName("longitude");
            Property(t => t.findUserFilter).HasColumnName("findUserFilter");

            // Relationships
            HasOptional(t => t.InterestedIn)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.interestedInID);
            HasOptional(t => t.RelationshipStatu)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.relationshipStatusID);
            HasRequired(t => t.UserAccountEntity)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.userAccountID);
            HasOptional(t => t.YouAre)
                .WithMany(t => t.UserAccountDetails)
                .HasForeignKey(d => d.youAreID);
        }
    }
}