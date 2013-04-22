using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DasKlubModel.Models.Mapping
{
    public class UserAccountMap : EntityTypeConfiguration<UserAccount>
    {
        public UserAccountMap()
        {
            // Primary Key
            this.HasKey(t => t.userAccountID);

            // Properties
            this.Property(t => t.userName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.password)
                .HasMaxLength(75);

            this.Property(t => t.passwordFormat)
                .HasMaxLength(50);

            this.Property(t => t.passwordSalt)
                .HasMaxLength(75);

            this.Property(t => t.eMail)
                .IsRequired()
                .HasMaxLength(75);

            this.Property(t => t.passwordQuestion)
                .HasMaxLength(100);

            this.Property(t => t.passwordAnswer)
                .HasMaxLength(100);

            this.Property(t => t.comment)
                .HasMaxLength(50);

            this.Property(t => t.ipAddress)
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("UserAccount");
            this.Property(t => t.userAccountID).HasColumnName("userAccountID");
            this.Property(t => t.userName).HasColumnName("userName");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.passwordFormat).HasColumnName("passwordFormat");
            this.Property(t => t.passwordSalt).HasColumnName("passwordSalt");
            this.Property(t => t.eMail).HasColumnName("eMail");
            this.Property(t => t.passwordQuestion).HasColumnName("passwordQuestion");
            this.Property(t => t.passwordAnswer).HasColumnName("passwordAnswer");
            this.Property(t => t.isApproved).HasColumnName("isApproved");
            this.Property(t => t.lastLoginDate).HasColumnName("lastLoginDate");
            this.Property(t => t.lastPasswordChangeDate).HasColumnName("lastPasswordChangeDate");
            this.Property(t => t.lastLockoutDate).HasColumnName("lastLockoutDate");
            this.Property(t => t.failedPasswordAttemptCount).HasColumnName("failedPasswordAttemptCount");
            this.Property(t => t.failedPasswordAttemptWindowStart).HasColumnName("failedPasswordAttemptWindowStart");
            this.Property(t => t.failedPasswordAnswerAttemptCount).HasColumnName("failedPasswordAnswerAttemptCount");
            this.Property(t => t.failedPasswordAnswerAttemptWindowStart).HasColumnName("failedPasswordAnswerAttemptWindowStart");
            this.Property(t => t.comment).HasColumnName("comment");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.updatedByUserAccountID).HasColumnName("updatedByUserAccountID");
            this.Property(t => t.createdByUserAccountID).HasColumnName("createdByUserAccountID");
            this.Property(t => t.isOnline).HasColumnName("isOnline");
            this.Property(t => t.isLockedOut).HasColumnName("isLockedOut");
            this.Property(t => t.lastActivityDate).HasColumnName("lastActivityDate");
            this.Property(t => t.ipAddress).HasColumnName("ipAddress");
        }
    }
}
