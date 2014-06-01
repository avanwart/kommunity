using System.Data.Entity.ModelConfiguration;
using DasKlub.Models.Models;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountMap : EntityTypeConfiguration<UserAccountEntity>
    {
        public UserAccountMap()
        {
            // Primary Key
            HasKey(t => t.userAccountID);

            // Properties
            Property(t => t.userName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.password)
                .HasMaxLength(75);

            Property(t => t.passwordFormat)
                .HasMaxLength(50);

            Property(t => t.passwordSalt)
                .HasMaxLength(75);

            Property(t => t.eMail)
                .IsRequired()
                .HasMaxLength(75);

            Property(t => t.passwordQuestion)
                .HasMaxLength(100);

            Property(t => t.passwordAnswer)
                .HasMaxLength(100);

            Property(t => t.comment)
                .HasMaxLength(50);

            Property(t => t.ipAddress)
                .HasMaxLength(25);

            // Table & Column Mappings
            ToTable("UserAccountEntity");
            Property(t => t.userAccountID).HasColumnName("userAccountID");
            Property(t => t.userName).HasColumnName("userName");
            Property(t => t.password).HasColumnName("password");
            Property(t => t.passwordFormat).HasColumnName("passwordFormat");
            Property(t => t.passwordSalt).HasColumnName("passwordSalt");
            Property(t => t.eMail).HasColumnName("eMail");
            Property(t => t.passwordQuestion).HasColumnName("passwordQuestion");
            Property(t => t.passwordAnswer).HasColumnName("passwordAnswer");
            Property(t => t.isApproved).HasColumnName("isApproved");
            Property(t => t.lastLoginDate).HasColumnName("lastLoginDate");
            Property(t => t.lastPasswordChangeDate).HasColumnName("lastPasswordChangeDate");
            Property(t => t.lastLockoutDate).HasColumnName("lastLockoutDate");
            Property(t => t.failedPasswordAttemptCount).HasColumnName("failedPasswordAttemptCount");
            Property(t => t.failedPasswordAttemptWindowStart).HasColumnName("failedPasswordAttemptWindowStart");
            Property(t => t.failedPasswordAnswerAttemptCount).HasColumnName("failedPasswordAnswerAttemptCount");
            Property(t => t.failedPasswordAnswerAttemptWindowStart)
                .HasColumnName("failedPasswordAnswerAttemptWindowStart");
            Property(t => t.comment).HasColumnName("comment");
            Property(t => t.createDate).HasColumnName("createDate");
            Property(t => t.updateDate).HasColumnName("updateDate");
            Property(t => t.updatedByUserAccountID).HasColumnName("updatedByUserAccountID");
            Property(t => t.createdByUserAccountID).HasColumnName("createdByUserAccountID");
            Property(t => t.isOnline).HasColumnName("isOnline");
            Property(t => t.isLockedOut).HasColumnName("isLockedOut");
            Property(t => t.lastActivityDate).HasColumnName("lastActivityDate");
            Property(t => t.ipAddress).HasColumnName("ipAddress");
        }
    }
}