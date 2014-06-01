using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    [Table("UserAccountDetail")]
    public class UserAccountDetailEntity
    {
        [Key]
        public int userAccountDetailID { get; set; }

        public int userAccountID { get; set; }
        public int? youAreID { get; set; }
        public int? relationshipStatusID { get; set; }
        public int? interestedInID { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? updatedByUserID { get; set; }
        public int? createdByUserID { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string profilePicURL { get; set; }
        public DateTime birthDate { get; set; }
        public string religion { get; set; }
        public string profileThumbPicURL { get; set; }
        public string ethnicity { get; set; }
        public double? heightCM { get; set; }
        public double? weightKG { get; set; }
        public string diet { get; set; }
        public int? accountViews { get; set; }
        public string externalURL { get; set; }
        public string smokes { get; set; }
        public string drinks { get; set; }
        public string handed { get; set; }
        public bool displayAge { get; set; }
        public bool enableProfileLogging { get; set; }
        public DateTime? lastPhotoUpdate { get; set; }
        public bool emailMessages { get; set; }
        public bool showOnMap { get; set; }
        public int? referringUserID { get; set; }
        public string browerType { get; set; }
        public bool membersOnlyProfile { get; set; }
        public string messangerType { get; set; }
        public string messangerName { get; set; }
        public string aboutDesc { get; set; }
        public string bandsSeen { get; set; }
        public string bandsToSee { get; set; }
        public string hardwareSoftware { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string defaultLanguage { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public string findUserFilter { get; set; }
        public virtual InterestedIn InterestedIn { get; set; }
        public virtual RelationshipStatu RelationshipStatu { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual YouAre YouAre { get; set; }
    }
}