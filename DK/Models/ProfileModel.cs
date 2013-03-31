//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Collections;

namespace DasKlub.Models
{
    public class ProfileModel
    {
        public string UserName { get; set; }

        public int UserAccountID { get; set; }

        public int UserConnectionID { get; set; }

        public int PhotoCount { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string MessageToTheWorld { get; set; }

        public string Website { get; set; }

        public string BandsSeen { get; set; }

        public string BandsToSee { get; set; }

        public DateTime LastStatusUpdate { get; set; }

        public string NewsArticles { get; set; }

        public int NewsCount { get; set; }

        public string HardwareAndSoftwareSkills { get; set; }

        public string MostRecentStatusUpdate { get; set; }

        public string RoleIcon { get; set; }

        public char RelationshipStatus { get; set; }

        public string RelationshipStatusFull { get; set; }

        public char InterestedIn { get; set; }

        public char YouAre { get; set; }

        public string YouAreFull { get; set; }

        public string InterestedInFull { get; set; }

        public bool DisplayOnMap { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime LastActivityDate { get; set; }

        public bool DisplayAge { get; set; }

        public int Age { get; set; }

        #region users 

        public int CyberFriendCount { get; set; }

        public int IRLFriendCount { get; set; }

        public int ViewingUsersCount { get; set; }

        #endregion


        public ArrayList VideoPlaylist   { get; set; }

        public string UploadedVideos { get; set; }

        public string FavoriteVideos { get; set; }

        public string PhotoItems { get; set; }

        public string Handed { get; set; }

        public bool IsBirthday { get; set; }

        public string ProfilePhotoMain { get; set; }

        public string DefaultLanguage { get; set; }

        public string ProfilePhotoMainThumb { get; set; }

 

        public bool EnableProfileLogging { get; set; }

        public bool IsViewingSelf { get; set; }

        public bool HasMoreThanMaxPhotos { get; set; }

        public int ProfileVisitorCount { get; set; }
 

        #region friendships

        public bool IsNotCyberFriend { get; set; }
        public bool IsWatingToBeCyberFriend { get; set; }
        public bool IsCyberFriend { get; set; }
        public bool IsDeniedCyberFriend { get; set; }

        public bool IsNotRealFriend { get; set; }
        public bool IsWatingToBeRealFriend { get; set; }
        public bool IsRealFriend { get; set; }
        public bool IsDeniedRealFriend { get; set; }
        #endregion



        public string MetaDescription { get; set; }

        public string SongRecords { get; set; }
    }
}