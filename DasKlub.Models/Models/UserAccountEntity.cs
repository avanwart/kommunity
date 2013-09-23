using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasKlubModel.Models;

namespace DasKlub.Models.Models
{
    [Table("UserAccount")]
    public partial class UserAccountEntity
    {
        public UserAccountEntity()
        {

            this.BlockedUsers = new List<BlockedUser>();
            this.Contents = new List<Content>();
            this.ContentComments = new List<ContentComment>();
            this.ContestVideoVotes = new List<ContestVideoVote>();
            this.DirectMessages = new List<DirectMessage>();
            this.DirectMessages1 = new List<DirectMessage>();
            this.PhotoItems = new List<PhotoItem>();
            this.Playlists = new List<Playlist>();
            this.ProfileLogs = new List<ProfileLog>();
            this.ProfileLogs1 = new List<ProfileLog>();
            this.StatusUpdates = new List<StatusUpdate>();
            this.UserAccountDetails = new List<UserAccountDetailEntity>();
            this.UserAccountMets = new List<UserAccountMet>();
            this.UserAccountMets1 = new List<UserAccountMet>();
            this.UserAccountVideos = new List<UserAccountVideo>();
            this.UserAddresses = new List<UserAddress>();
            this.UserConnections = new List<UserConnection>();
            this.UserConnections1 = new List<UserConnection>();
            this.UserPhotoes = new List<UserPhoto>();
            this.Languages = new List<Language>();
            this.Roles = new List<Role>();
        }

        [Key]
        public int userAccountID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string passwordFormat { get; set; }
        public string passwordSalt { get; set; }
        public string eMail { get; set; }
        public string passwordQuestion { get; set; }
        public string passwordAnswer { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public Nullable<System.DateTime> lastLoginDate { get; set; }
        public Nullable<System.DateTime> lastPasswordChangeDate { get; set; }
        public Nullable<System.DateTime> lastLockoutDate { get; set; }
        public Nullable<short> failedPasswordAttemptCount { get; set; }
        public Nullable<System.DateTime> failedPasswordAttemptWindowStart { get; set; }
        public Nullable<short> failedPasswordAnswerAttemptCount { get; set; }
        public Nullable<System.DateTime> failedPasswordAnswerAttemptWindowStart { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> updatedByUserAccountID { get; set; }
        public Nullable<int> createdByUserAccountID { get; set; }
        public Nullable<bool> isOnline { get; set; }
        public Nullable<bool> isLockedOut { get; set; }
        public Nullable<System.DateTime> lastActivityDate { get; set; }
        public string ipAddress { get; set; }
        public virtual ICollection<BlockedUser> BlockedUsers { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<ContentComment> ContentComments { get; set; }
        public virtual ICollection<ContestVideoVote> ContestVideoVotes { get; set; }
        public virtual ICollection<DirectMessage> DirectMessages { get; set; }
        public virtual ICollection<DirectMessage> DirectMessages1 { get; set; }
        public virtual ICollection<PhotoItem> PhotoItems { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<ProfileLog> ProfileLogs { get; set; }
        public virtual ICollection<ProfileLog> ProfileLogs1 { get; set; }
        public virtual ICollection<StatusUpdate> StatusUpdates { get; set; }
        public virtual List<UserAccountDetailEntity> UserAccountDetails { get; set; }
        public virtual ICollection<UserAccountMet> UserAccountMets { get; set; }
        public virtual ICollection<UserAccountMet> UserAccountMets1 { get; set; }
        public virtual ICollection<UserAccountVideo> UserAccountVideos { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<UserConnection> UserConnections { get; set; }
        public virtual ICollection<UserConnection> UserConnections1 { get; set; }
        public virtual ICollection<UserPhoto> UserPhotoes { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
