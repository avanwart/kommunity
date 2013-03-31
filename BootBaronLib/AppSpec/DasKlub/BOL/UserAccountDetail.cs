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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Resources;
using BootBaronLib.Values;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserAccountDetail : BaseIUserLogCRUD, ICacheName
    {
        #region contructors 

        public UserAccountDetail() { }

        public UserAccountDetail(int userAccountDetailID) 
        {
            Get(userAccountDetailID);
        }

        #endregion

        #region properties

        private int _userAccountDetailID = 0;

        public int UserAccountDetailID
        {
            get { return _userAccountDetailID; }
            set { _userAccountDetailID = value; }
        }


        private decimal? _latitude = 0;

        public decimal? Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        private decimal? _longitude = 0;

        public decimal? Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }


        private string _messangerType = string.Empty;

        public string MessangerType
        {
            get { return _messangerType; }
            set { _messangerType = value; }
        }

        private string _messangerName = string.Empty;

        public string MessangerName
        {
            get { return _messangerName; }
            set { _messangerName = value; }
        }


        private bool _membersOnlyProfile = false;

        public bool MembersOnlyProfile
        {
            get { return _membersOnlyProfile; }
            set { _membersOnlyProfile = value; }
        }

        private string _browerType = string.Empty;

        public string BrowerType
        {
            get {
                if (string.IsNullOrEmpty(_browerType))
                {
                    _browerType = HttpContext.Current.Request.Browser.Type;
                }
                
                return _browerType; }
            set { _browerType = value; }
        }

        private bool _emailMessages = true;

        public bool EmailMessages
        {
            get { return _emailMessages; }
            set { _emailMessages = value; }
        }


        private string _defaultLanguage = string.Empty;

        public string DefaultLanguage
        {
            get {
                if (!string.IsNullOrWhiteSpace(_defaultLanguage))
                {
                    return _defaultLanguage.ToUpper();
                }
                return _defaultLanguage; }
            set { _defaultLanguage = value; }
        }

        private bool _enableProfileLogging = true;

        public bool EnableProfileLogging
        {
            get { return _enableProfileLogging; }
            set { _enableProfileLogging = value; }
        }

        private DateTime? _lastPhotoUpdate = null;

        public DateTime? LastPhotoUpdate
        {
            get
            {
                return _lastPhotoUpdate;
            }
            set { _lastPhotoUpdate = value; }
        }

        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }

        private string _country = string.Empty;

        public string Country
        {
            get {
                if (string.IsNullOrWhiteSpace(_country)) return SiteEnums.CountryCodeISO.U0.ToString();
                
                return _country; }
            set { _country = value; }
        }

        private string _region = string.Empty;

        public string Region
        {
            get {
                if (string.IsNullOrWhiteSpace(_region)) return string.Empty;
                return _region; }
            set { _region = value; }
        }

        private string _city = string.Empty;

        public string City
        {
            get {
                if (string.IsNullOrEmpty(_city)) return string.Empty;
                return _city; }
            set { _city = value; }
        }

        private string _postalCode = string.Empty;

        public string PostalCode
        {
            get {
                if (string.IsNullOrEmpty(_postalCode)) return string.Empty;
                return _postalCode; }
            set { _postalCode = value; }
        }


        private string _profilePicURL = string.Empty;

        public string ProfilePicURL
        {
            get { return _profilePicURL; }
            set { _profilePicURL = value; }
        }

        private string _hardwareSoftware = string.Empty;

        public string HardwareSoftware
        {
            get {
                if (string.IsNullOrEmpty(_hardwareSoftware)) return string.Empty;
                return _hardwareSoftware; }
            set { _hardwareSoftware = value; }
        }


        private string _firstName = string.Empty;

        public string FirstName
        {
            get {

                if (string.IsNullOrWhiteSpace(_firstName)) return string.Empty;
                return _firstName.Trim() ;
            }
            set { _firstName = value; }
        }

        private string _lastName = string.Empty;

        public string LastName
        {
            get {
                if (string.IsNullOrWhiteSpace(_lastName)) return string.Empty;
                return _lastName.Trim(); }
            set { _lastName = value; }
        }


        private string _aboutDesc = string.Empty;

        public string AboutDesc
        {
            get {
                if (string.IsNullOrEmpty(_aboutDesc)) return string.Empty;

                return _aboutDesc.Trim(); }
            set { _aboutDesc = value; }
        }





        private int? _youAreID = null;

        public int? YouAreID
        {
            get { return _youAreID; }
            set { _youAreID = value; }
        }

        private int? _relationshipStatusID = null;

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Messages))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(BootBaronLib.Resources.Messages), Name = "RelationshipStatus")]
        public int? RelationshipStatusID
        {
            get { return _relationshipStatusID; }
            set { _relationshipStatusID = value; }
        }

        private int? _interestedInID = null;

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Messages))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(BootBaronLib.Resources.Messages), Name = "InterestedIn")]
        public int? InterestedInID
        {
            get { return _interestedInID; }
            set { _interestedInID = value; }
        }

        private DateTime _birthDate = DateTime.MinValue;

        public DateTime BirthDate
        {
            get {
                return _birthDate; }
            set { _birthDate = value; }
        }

        private char _religion = char.MinValue;

        public char Religion
        {
            get { return _religion; }
            set { _religion = value; }
        }

        private char _ethnicity = char.MinValue;

        public char Ethnicity
        {
            get { return _ethnicity; }
            set { _ethnicity = value; }
        }

        private float _heightCM = 0;

        public float HeightCM
        {
            get { return _heightCM; }
            set { _heightCM = value; }
        }

        private float _weightKG = 0;

        public float WeightKG
        {
            get { return _weightKG; }
            set { _weightKG = value; }
        }

        private char _diet = char.MinValue;

        public char Diet
        {
            get { return _diet; }
            set { _diet = value; }
        }

        private int _accountViews = 0;

        public int AccountViews
        {
            get { return _accountViews; }
            set { _accountViews = value; }
        }

        private string _externalURL = string.Empty;

        public string ExternalURL
        {
            get {
                if (string.IsNullOrEmpty(_externalURL))
                {
                    return string.Empty;
                }
                return _externalURL; }
            set { _externalURL = value; }
        }

        private char _smokes = char.MinValue;

        public char Smokes
        {
            get { return _smokes; }
            set { _smokes = value; }
        }

        private char _drinks = char.MinValue;

        public char Drinks
        {
            get { return _drinks; }
            set { _drinks = value; }
        }

        private char _handed = char.MinValue;

        [Display(ResourceType = typeof(BootBaronLib.Resources.Messages), Name = "Handed")]
        public char Handed
        {
            get { return _handed; }
            set { _handed = value; }
        }

        private bool _displayAge = true;

        public bool DisplayAge
        {
            get { return _displayAge; }
            set { _displayAge = value; }
        }

        private string _profileThumbPicURL = string.Empty;

        public string ProfileThumbPicURL
        {
            get { return _profileThumbPicURL; }
            set { _profileThumbPicURL = value; }
        }


        private string _bandsSeen = string.Empty;

        public string BandsSeen
        {
            get {
                if (_bandsSeen == null) return string.Empty;
                return _bandsSeen.Trim(); }
            set { _bandsSeen = value; }
        }

        private string _bandsToSee = string.Empty;

        public string BandsToSee
        {
            get {
                if (_bandsToSee == null) return string.Empty;
                return _bandsToSee.Trim(); }
            set { _bandsToSee = value; }
        }




        private bool _showOnMap = true;

        public bool ShowOnMap
        {
            get { return _showOnMap; }
            set { _showOnMap = value; }
        }

        private int _referringUserID = 0;

        public int ReferringUserID
        {
            get { return _referringUserID; }
            set { _referringUserID = value; }
        }


        private string _findUserFilter = string.Empty;

        public string FindUserFilter
        {
            get { return _findUserFilter; }
            set { _findUserFilter = value; }
        }

        #endregion

        #region non-db properties

        public bool ShowOnMapLegal
        {
            get 
            {

                if (!string.IsNullOrEmpty(this.Country.Trim()) && this.Over18 && this.ShowOnMap) return true;
                else return false;

            }
        }

        public bool IsBirthdayToday
        {
            get
            {
                if (this.DisplayAge &&
                    DateTime.UtcNow.Month == BirthDate.Month &&
                    (DateTime.UtcNow.Day == BirthDate.Day ))
                {
                    return true;
                }
                else return false;
            }

        }

        public string CountryName
        {
            get
            {
                SiteEnums.CountryCodeISO theCO = SiteEnums.CountryCodeISO.U0;

                if (Enum.TryParse(this.Country, out theCO))
                {
                    return Utilities.ResourceValue( Utilities.GetEnumDescription(theCO));
                }

                return Resources.Messages.Unknown;
            }
        }

        public string GenderIconSmall
        {
            get
            {
                return this.GenderIcon.Replace(".png", "_small.png");
            }
        }

        public string GenderIconLink
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                YouAre youAre = new YouAre(Convert.ToInt32(this.YouAreID));

                switch (youAre.TypeLetter)
                {
                    case 'A':
                    case 'M':
                    case 'C':
                    case 'F':
                    case 'B':
                    case 'R':
                    case 'G':
                    case 'D':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/sex/{0}.png", youAre.TypeLetter.ToString())));
                        break;
                    default:
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            "~/content/images/sex/U.png"));
                        break;
                }
                return sb.ToString();
            }
        }


        public string GenderIconLinkDark
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                YouAre youAre = new YouAre(Convert.ToInt32(this.YouAreID));

                switch (youAre.TypeLetter)
                {
                    case 'A':
                    case 'M':
                    case 'C':
                    case 'F':
                    case 'B':
                    case 'R':
                    case 'G':
                    case 'D':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/sex/{0}_D.png", youAre.TypeLetter.ToString())));
                        break;
                    default:
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            "~/content/images/sex/U.png"));
                        break;
                }
                return sb.ToString();
            }
        }

        public string GenderIcon
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<img src=""");

                UserAccount ua = new UserAccount(this.UserAccountID);

                if (this.YouAreID == null) return string.Empty;


                YouAre youAre = new YouAre(Convert.ToInt32(this.YouAreID));

                switch (youAre.TypeLetter)
                {
                    case 'A':
                    case 'M':
                    case 'C':
                    case 'F':
                    case 'B':
                    case 'R':
                    case 'G':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/sex/{0}.png", youAre.TypeLetter.ToString())));
                        break;
                    default:
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            "~/content/images/sex/U.png"));
                        break;
                }
                sb.Append(@""" title=""");
                sb.AppendFormat("{0}: ", ua.UserName);
                switch (youAre.TypeLetter)
                {
                    case 'A':
                        sb.Append(Messages.Alien);
                        break;
                    case 'M':
                        sb.Append(Messages.Male);
                        break;
                    case 'F':
                        sb.Append(Messages.Female);
                        break;
                    case 'B':
                        sb.Append(Messages.Band);
                        break;
                    case 'C':
                        sb.Append(Messages.Cyborg);
                        break;
                    case 'R':
                        sb.Append(Messages.Robot);
                        break;
                    case 'G':
                        sb.Append(Messages.DanceGroup);
                        break;
                    default:
                        sb.Append(Messages.Unknown);
                        break;
                }

                sb.Append(@""" alt=""");
                sb.Append(youAre.LocalizedName);
                sb.Append(@""" />");

                return sb.ToString();
            }
        }


        public string SiteBages
        {
            get
            {
                UserAccount ua = new UserAccount(this.UserAccountID);

                string[] rls = Roles.GetRolesForUser(ua.UserName);

                StringBuilder sb = new StringBuilder(100);

                Role role = null;

                if (rls.Length > 0)
                {
                    foreach (string rle in rls)
                    {
                        sb.Append(@"<img class=""small_site_badge"" src=""");

                        switch (rle)
                        {
                            // override here
                            default:
                                sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                                    "~/content/images/roles/" + rle + ".png"));
                                break;
                        }

                        role = new Role(rle);

                        sb.Append(@""" alt=""");
                        sb.Append(role.Description);
                        sb.Append(@""" title=""");
                        sb.Append(role.Description);
                        sb.Append(@""" />");

                        if (_getJustOne)
                        {
                            break;//just the 1st role's icon
                        }
                        else _getJustOne = false;
                      
                    }
                }

                return sb.ToString();

            }
        }

        private bool _getJustOne = false;
      

        public string SiteBagesSmall
        {
            get
            {
                _getJustOne = true;
                return SiteBages.Replace(".png", "_small.png");
            }
        }


        public string CountryFlagThumb
        {
            get
            {
                if (string.IsNullOrEmpty(this.Country.Trim()))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute("~/content/images/countries/defaultcountry_small.png");
                }
                else
                {
                    return System.Web.VirtualPathUtility.ToAbsolute( "~/content/images/countries/flag_sprite.png" );
                    //return System.Web.VirtualPathUtility.ToAbsolute( string.Format( "~/content/images/countries/{0}_small.png",  this.Country ));
                    //images/countries/
                }
            }
        }

        public string CountryFlag
        {
            get
            {
                if (string.IsNullOrEmpty(this.Country.Trim()))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute("~/content/images/countries/defaultcountry.png");
                }
                else
                {
                    return System.Web.VirtualPathUtility.ToAbsolute(string.Format("~/content/images/countries/{0}.png", this.Country));
                }
            }
        }

        public string FullProfilePicURL
        {
            get
            {
               
                if (string.IsNullOrEmpty(this.ProfileThumbPicURL))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuser.png");
                }
                else
                {
                    return Utilities.S3ContentPath(this.ProfilePicURL); 
                }
            }
        }


        public string FullProfilePicThumbURL
        {
            get
            {
                if (string.IsNullOrEmpty(this.ProfileThumbPicURL))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuserthumb.png");
                }
                else
                {
                    return Utilities.S3ContentPath(this.ProfileThumbPicURL); 
                }
            }
        }




        public char SexLetter
        {
            get
            {
                if (this.YouAreID == null) return 'U';

                YouAre youAre = new YouAre(Convert.ToInt32(this.YouAreID));

                return youAre.TypeLetter;
            }
        }

        public string Sex
        {
            get
            {
                if (this.YouAreID == null) return string.Empty;

                YouAre youAre = new YouAre(Convert.ToInt32(this.YouAreID));

                return Utilities.ResourceValue(youAre.Name);
            }
        }

        public string AboutDescription
        {
            get
            {
                return Utilities.MakeLink(this.AboutDesc).Replace("\r\n", "<br />");
            }
        }

        public int YearsOld
        {
            get
            {
                return Utilities.CalculateAge(this.BirthDate);
            }
        }


        public bool Over16
        {
            get
            {
                int age = Utilities.CalculateAge(this.BirthDate);
                return (age >= 16);
            }
        }


        public bool Over18
        {
            get
            {
                int age = Utilities.CalculateAge(this.BirthDate);
                return (age >= 18);
            }
        }

        #endregion

        #region methods


        public static RelationshipStatuses GetDistinctRelationshipStatus()
        {
            RelationshipStatuses relations = null;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctRelationshipStatus";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                relations = new RelationshipStatuses();

                RelationshipStatus relation = null;

                foreach (DataRow dr in dt.Rows)
                {
                    relation = new RelationshipStatus(FromObj.IntFromObj(dr["relationshipStatusID"]));

                    if (relation.RelationshipStatusID == 0) continue;

                    relations.Add(relation);
                }
            }

            return relations;
        }

        public static InterestedIns GetDistinctInterests()
        {

            InterestedIns iterests = null;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctInterests";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                iterests = new InterestedIns();

                InterestedIn iterest = null;

                foreach (DataRow dr in dt.Rows)
                {
                    iterest = new InterestedIn(FromObj.IntFromObj(dr["interestedInID"]));

                    if (iterest.InterestedInID == 0) continue;

                    iterests.Add(iterest);
                }
            }

            return iterests;
        }


        public static YouAres GetDistinctYouAres()
        {
            YouAres youAres = null;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctYouAres";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                youAres = new YouAres();

                YouAre youAre=null;
                foreach (DataRow dr in dt.Rows)
                {
                    youAre = new YouAre(FromObj.IntFromObj(dr["youAreID"]));

                    if (youAre.YouAreID == 0) continue;

                    youAres.Add(youAre);
                }
            }

            return youAres;
        }

        public static List<SiteEnums.CountryCodeISO> GetDistinctUserCountries()
        {

            List<SiteEnums.CountryCodeISO> countries = new List<SiteEnums.CountryCodeISO>();

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctUserCountries";
 
            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                SiteEnums.CountryCodeISO country ;

                string uniqueCountry = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    uniqueCountry = FromObj.StringFromObj(dr["country"]);

                    if(!string.IsNullOrWhiteSpace(uniqueCountry))
                    {
                    country = (SiteEnums.CountryCodeISO)Enum.Parse(typeof(SiteEnums.CountryCodeISO), uniqueCountry);
                    countries.Add(country);
                    }
                }
            }

            return countries;
        }



        public static List<SiteEnums.SiteLanguages>  GetDistinctUserLanguages()
        {
            List<SiteEnums.SiteLanguages> languages = new List<SiteEnums.SiteLanguages>();

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctUserLanguages";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                SiteEnums.SiteLanguages language;
                string defaultLang = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    defaultLang = FromObj.StringFromObj(dr["defaultLanguage"]);

                    if (string.IsNullOrWhiteSpace(defaultLang)) continue;

                    language = (SiteEnums.SiteLanguages)Enum.Parse(typeof(SiteEnums.SiteLanguages), defaultLang);
                    languages.Add(language);
                }
            }

            return languages;
 
        }

        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserAccountDetail";

            ADOExtenstion.AddParameter(comm, "userAccountDetailID",UserAccountDetailID);

            RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public static DataTable GetUserAffReport(int referringUserID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAffReport";
            // create a new parameter
            DbParameter param = comm.CreateParameter();

            param.ParameterName = "@referringUserID";
            param.Value = referringUserID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);

            return  DbAct.ExecuteSelectCommand(comm);
        }

        public override int Create()
        {
               // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserAccountDetail";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), UserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Country), Country);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Region), Region);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.City), City);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.YouAreID), YouAreID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ProfilePicURL), ProfilePicURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.AboutDesc), AboutDesc);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.RelationshipStatusID), RelationshipStatusID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.InterestedInID), InterestedInID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BirthDate), BirthDate);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Religion), Religion);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Ethnicity), Ethnicity);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.HeightCM), HeightCM);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.WeightKG), WeightKG);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Diet), Diet);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.AccountViews), AccountViews);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ExternalURL), ExternalURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Smokes), Smokes);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Drinks), Drinks);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Handed), Handed);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.DisplayAge), DisplayAge);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ProfileThumbPicURL), ProfileThumbPicURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BandsToSee), BandsToSee);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BandsSeen), BandsSeen);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.EnableProfileLogging), EnableProfileLogging);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.LastPhotoUpdate), LastPhotoUpdate);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.EmailMessages), EmailMessages);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ShowOnMap), ShowOnMap);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PostalCode), PostalCode);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ReferringUserID), ReferringUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MembersOnlyProfile), MembersOnlyProfile);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MessangerType), MessangerType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MessangerName), MessangerName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.HardwareSoftware), HardwareSoftware);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BrowerType), BrowerType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FirstName), FirstName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.LastName), LastName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.DefaultLanguage), DefaultLanguage);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Latitude), Latitude);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Longitude), Longitude);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FindUserFilter), FindUserFilter);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            else
            {
                this.UserAccountDetailID = Convert.ToInt32(result);

                return this.UserAccountDetailID;
            }
        }

        public void GetUserAccountDeailForUser(int userAccountID)
        {
            this.UserAccountID = userAccountID;

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheNameAlt] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetUserAccountDetailForUser";
                // create a new parameter
                DbParameter param = comm.CreateParameter();
                param.ParameterName = "@userAccountID";
                param.Value = userAccountID;
                param.DbType = DbType.Int32;
                comm.Parameters.Add(param);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if ( HttpContext.Current != null) HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheNameAlt);

                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheNameAlt]);
            }
        }

        public override void Get(int userAccountDetailID)
        {
            this.UserAccountDetailID = userAccountDetailID;

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetUserAccountDetail";
                // create a new parameter
                DbParameter param = comm.CreateParameter();
                param.ParameterName = "@userAccountDetailID";
                param.Value = userAccountDetailID;
                param.DbType = DbType.Int32;
                comm.Parameters.Add(param);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current != null)
                        HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }
        }

        public override void Get(DataRow dr)
        {

            base.Get(dr);

            this.UserAccountDetailID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.UserAccountDetailID)]);
            this.UserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.UserAccountID)]);
            this.Country = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Country)]);
            this.Region = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Region)]);
            this.City = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.City)]);
            this.PostalCode = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.PostalCode)]);
            this.YouAreID = FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => this.YouAreID)]);
            this.ProfilePicURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ProfilePicURL)]);
            this.AboutDesc = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.AboutDesc)]);
            this.RelationshipStatusID = FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => this.RelationshipStatusID)]);
            this.InterestedInID = FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => this.InterestedInID)]);
            this.BirthDate = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => this.BirthDate)]);
            this.Religion = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Religion)]);
            this.Ethnicity = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Ethnicity)]);
            this.HeightCM = FromObj.FloatFromObj(dr[StaticReflection.GetMemberName<string>(x => this.HeightCM)]);
            this.WeightKG = FromObj.FloatFromObj(dr[StaticReflection.GetMemberName<string>(x => this.WeightKG)]);
            this.Diet = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Diet)]);
            this.AccountViews = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.AccountViews)]);
            this.ExternalURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ExternalURL)]);
            this.Smokes = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Smokes)]);
            this.Drinks = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Drinks)]);
            this.Handed = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Handed)]);
            this.DisplayAge = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.DisplayAge)]);
            this.ProfileThumbPicURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ProfileThumbPicURL)]);
            this.BandsSeen = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.BandsSeen)]);
            this.BandsToSee = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.BandsToSee)]);
            this.EnableProfileLogging = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.EnableProfileLogging)]);
            this.LastPhotoUpdate = FromObj.DateNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => this.LastPhotoUpdate)]);
            this.EmailMessages = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.EmailMessages)]);
            this.ShowOnMap = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ShowOnMap)]);
            this.ReferringUserID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ReferringUserID)]);
            this.BrowerType = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.BrowerType)]);
            this.MembersOnlyProfile = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.MembersOnlyProfile)]);
            this.MessangerName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.MessangerName)]);
            this.HardwareSoftware = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.HardwareSoftware)]);
            this.FirstName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FirstName)]);
            this.LastName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.LastName)]);
            this.DefaultLanguage = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.DefaultLanguage)]);

            this.Latitude = FromObj.DecimalNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Latitude)]);
            this.Longitude = FromObj.DecimalNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Longitude)]);
            this.FindUserFilter = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FindUserFilter)]);
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserAccountDetail";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountDetailID), UserAccountDetailID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), UserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Country), Country);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Region), Region);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.City), City);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.YouAreID), YouAreID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ProfilePicURL), ProfilePicURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.AboutDesc), AboutDesc);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.RelationshipStatusID), RelationshipStatusID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.InterestedInID), InterestedInID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BirthDate), BirthDate);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Religion), Religion);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Ethnicity), Ethnicity);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.HeightCM), HeightCM);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.WeightKG), WeightKG);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Diet), Diet);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.AccountViews), AccountViews);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ExternalURL), ExternalURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Smokes), Smokes);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Drinks), Drinks);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Handed), Handed);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.DisplayAge), DisplayAge);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ProfileThumbPicURL), ProfileThumbPicURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BandsToSee), BandsToSee);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BandsSeen), BandsSeen);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.EnableProfileLogging), EnableProfileLogging);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.LastPhotoUpdate), LastPhotoUpdate);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.EmailMessages), EmailMessages);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ShowOnMap), ShowOnMap);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ReferringUserID), ReferringUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MembersOnlyProfile), MembersOnlyProfile);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MessangerType), MessangerType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MessangerName), MessangerName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.HardwareSoftware), HardwareSoftware);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PostalCode), PostalCode);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.BrowerType), BrowerType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FirstName), FirstName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.LastName), LastName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.DefaultLanguage), DefaultLanguage);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Latitude), Latitude);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Longitude), Longitude);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FindUserFilter), FindUserFilter);
            
            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            return (result != -1);
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}-{1}", this.GetType().FullName , this.UserAccountDetailID.ToString()); }
        }

        public string CacheNameAlt { get { return string.Format("{0}-user-{1}", this.GetType().FullName , this.UserAccountID); } }

        public void RemoveCache()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
                HttpContext.Current.Cache.DeleteCacheObj(this.CacheNameAlt);
            }
        }

        #endregion

        public int Set()
        {
            if (this.UserAccountDetailID == 0) return this.Create();
            else
            {
                if (this.Update())
                {
                    return 1;
                }
                else return 0;
            }
        }


        public string InterestedInIconSmall
        {
            get
            {
                return this.InterestedInIcon.Replace(".png", "_small.png");
            }
        }



        public string HandedFull
        {
            get
            {
                switch (this.Handed)
                {
                    case 'R':
                        return Messages.Right;
                    case 'L':
                        return Messages.Left;
                    case 'A':
                        return Messages.Ambidextrous;
                    default:
                        return Messages.Unknown;
                }
            }
        }

        public string InterestedFull
        {


            get
            {
                if (this.InterestedInID == null) return string.Empty;

                InterestedIn youAre = new InterestedIn(Convert.ToInt32(this.InterestedInID));

                return Utilities.ResourceValue(youAre.Name);
            }

        }

  
        public string InterestedInIcon
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"<img src=""");

                switch (this.InterestedInID)
                {
                    case 'A':
                    case 'M':
                    case 'F':
                    case 'B':
                    case 'P':
                    case 'R':
                    case 'C':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            "~/content/images/interestedin/" + this.InterestedInID + ".png"));
                        break;
                    default:
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(
                            "~/content/images/interestedin/U.png"));
                        break;
                }

                sb.Append(@""" alt=""");
                if (char.MinValue != this.InterestedInID)
                {
                    sb.Append(this.InterestedInID);
                }
                else sb.Append(Messages.Unknown);
                sb.Append(@""" title=""");

                sb.AppendFormat("{0}: ", Messages.InterestedIn);

                switch (this.InterestedInID)
                {
                    case 'A':
                        sb.Append(Messages.Alien);
                        break;
                    case 'M':
                        sb.Append(Messages.Male);
                        break;
                    case 'F':
                        sb.Append(Messages.Female);
                        break;
                    case 'B':
                        sb.Append(Messages.MaleAndFemale);
                        break;
                    case 'P':
                        sb.Append(Messages.Audience);
                        break;
                    case 'R':
                        sb.Append(Messages.Robot);
                        break;
                    case 'C':
                        sb.Append(Messages.Cyborg);
                        break;
                    default:
                        sb.Append(Messages.Unknown);
                        break;
                }

                sb.Append(@"""   />");

                return sb.ToString();
            }
        }


        public string RelationshipStatusFull
        {

            get
            {
                if (this.RelationshipStatusID == null) return string.Empty;

                RelationshipStatus youAre = new RelationshipStatus(Convert.ToInt32(this.RelationshipStatusID));

                return Utilities.ResourceValue(youAre.Name);
            }
        }
         
        public string RelationshipStatusIconSmall
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"<img src=""");


                switch (this.RelationshipStatusID)
                {

                    case 'G':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_green.png"));
                        break;
                    case 'Y':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_yellow.png"));
                        break;
                    case 'R':
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_red.png"));
                        break;
                    default:
                        sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_unknown.png"));
                        break;
                }

                sb.Append(@""" alt=""");

                if (this.RelationshipStatusID == char.MinValue)
                {
                    sb.Append(Messages.Unknown);
                }
                else
                {
                    sb.Append(this.RelationshipStatusID);
                }

                
                
                sb.Append(@""" title=""");

                sb.AppendFormat("{0}: ", Messages.RelationshipStatus);
                switch (this.RelationshipStatusID)
                {
                    case 'G':
                        sb.AppendFormat("{0}: ", Messages.SingleAndLooking);
                        break;
                    case 'Y':
                        sb.AppendFormat("{0}: ", Messages.LookingForFriends);
                        break;
                    case 'R':
                        sb.AppendFormat("{0}: ", Messages.TakenOrNotInterested);
                        break;
                    default:
                        sb.Append(Messages.Unknown);
                        break;
                }

                sb.Append(@""" />");

                return sb.ToString();
            }
        }

        public string UserFace
        {
            get
            {
                UserAccount ua = new UserAccount(this.UserAccountID);
                StringBuilder sb = new StringBuilder(100);
                sb.AppendFormat(@"<a title=""{0}"" class=""m_over"" href=""{1}"">", ua.UserName, VirtualPathUtility.ToAbsolute("~/" + ua.UserName));
                sb.AppendFormat(@"<img title=""{0}"" alt=""{0}"" src=""", ua.UserName);
                sb.Append(this.FullProfilePicThumbURL);
                sb.Append(@""" />");
                sb.Append(@"</a>");
                return sb.ToString();
            }
        }

        public string  SmallUserIcon
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);
                UserAccount ua = new UserAccount(this.UserAccountID);
                
                sb.Append(UserFace);

                sb.Append("<br />");

                sb.AppendFormat(@"<img title=""{0}"" alt=""{0}"" src=""{1}"" />", this.Sex,
                        VirtualPathUtility.ToAbsolute("~/content/images/sex/" + this.SexLetter.ToString() + ".png"));

               // sb.AppendFormat(@"<img title=""{0}"" alt=""{0}"" src=""{1}"" class=""flag_bg sprite-{2}_small"" />", this.CountryName, this.CountryFlagThumb, this.Country);

                sb.AppendFormat(@"<div title=""{0}"" class=""sprites sprite-{1}_small""></div>", this.CountryName,   this.Country);
 


                // hack: latin isn't supported
                sb.AppendFormat(@"<span title=""{1}"" class=""default_lang"">{0}</span>", ( this.DefaultLanguage == "FO") ? "LA" :this.DefaultLanguage, 
                    Utilities.GetLanguageNameForCode(this.DefaultLanguage));
                //sb.Append("&nbsp;");
                this._getJustOne = true;
                sb.Append(SiteBages);

                if (ua.IsOnLine)
                {
                    sb.AppendFormat(@"<img style=""height: 8px; width:8px"" title=""{0}"" alt=""{0}"" src=""{1}"" />", Messages.IsOnline,
                    System.Web.VirtualPathUtility.ToAbsolute("~/content/images/status/abutton2_e0.gif"));
                }

                return sb.ToString();
            }
        
        }


        public char RelationshipStatus
        {
            get
            {
                if (this.RelationshipStatusID == null) return char.MinValue;

                RelationshipStatus rstaus = new RelationshipStatus(Convert.ToInt32(this.RelationshipStatusID));

                return rstaus.TypeLetter;

            }
        }

        public char InterestedIn
        {
            get
            {
                if (this.InterestedInID == null) return char.MinValue;

                InterestedIn rstaus = new InterestedIn(Convert.ToInt32(this.InterestedInID));

                return rstaus.TypeLetter;

            }
        }

        public char YouAre
        {
            get
            {
                if (this.YouAreID == null) return 'U';

                YouAre rstaus = new YouAre(Convert.ToInt32(this.YouAreID));

                return rstaus.TypeLetter;

            }
        }
    }
}
