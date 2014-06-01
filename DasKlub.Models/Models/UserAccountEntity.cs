using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasKlubModel.Models;

namespace DasKlub.Models.Models
{
    [Table("UserAccount")]
    public class UserAccountEntity
    {
        public UserAccountEntity()
        {
            BlockedUsers = new List<BlockedUser>();
            Contents = new List<Content>();
            ContentComments = new List<ContentComment>();
            ContestVideoVotes = new List<ContestVideoVote>();
            DirectMessages = new List<DirectMessage>();
            DirectMessages1 = new List<DirectMessage>();
            PhotoItems = new List<PhotoItem>();
            Playlists = new List<Playlist>();
            ProfileLogs = new List<ProfileLog>();
            ProfileLogs1 = new List<ProfileLog>();
            StatusUpdates = new List<StatusUpdate>();
            UserAccountDetails = new List<UserAccountDetailEntity>();
            UserAccountMets = new List<UserAccountMet>();
            UserAccountMets1 = new List<UserAccountMet>();
            UserAccountVideos = new List<UserAccountVideo>();
            UserAddresses = new List<UserAddress>();
            UserConnections = new List<UserConnection>();
            UserConnections1 = new List<UserConnection>();
            UserPhotoes = new List<UserPhoto>();
            Languages = new List<Language>();
            Roles = new List<Role>();
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
        public bool? isApproved { get; set; }
        public DateTime? lastLoginDate { get; set; }
        public DateTime? lastPasswordChangeDate { get; set; }
        public DateTime? lastLockoutDate { get; set; }
        public short? failedPasswordAttemptCount { get; set; }
        public DateTime? failedPasswordAttemptWindowStart { get; set; }
        public short? failedPasswordAnswerAttemptCount { get; set; }
        public DateTime? failedPasswordAnswerAttemptWindowStart { get; set; }
        public string comment { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? updatedByUserAccountID { get; set; }
        public int? createdByUserAccountID { get; set; }
        public bool? isOnline { get; set; }
        public bool? isLockedOut { get; set; }
        public DateTime? lastActivityDate { get; set; }
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