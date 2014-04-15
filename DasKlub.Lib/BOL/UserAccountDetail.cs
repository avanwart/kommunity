using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL
{
    public class UserAccountDetail : BaseIUserLogCRUD, ICacheName
    {
        #region contructors 

        public UserAccountDetail()
        {
        }

        public UserAccountDetail(int userAccountDetailID)
        {
            Get(userAccountDetailID);
        }

        #endregion

        #region properties

        private string _aboutDesc = string.Empty;
        private string _bandsSeen = string.Empty;
        private string _bandsToSee = string.Empty;
        private DateTime _birthDate = DateTime.MinValue;
        private string _browerType = string.Empty;
        private string _city = string.Empty;
        private string _country = string.Empty;
        private string _defaultLanguage = string.Empty;
        private char _diet = char.MinValue;
        private bool _displayAge = true;
        private char _drinks = char.MinValue;
        private bool _emailMessages = true;
        private bool _enableProfileLogging = true;
        private char _ethnicity = char.MinValue;
        private string _externalURL = string.Empty;
        private string _findUserFilter = string.Empty;
        private string _firstName = string.Empty;
        private char _handed = char.MinValue;
        private string _hardwareSoftware = string.Empty;
        private string _lastName = string.Empty;
        private decimal? _latitude = 0;

        private decimal? _longitude = 0;
        private string _messangerName = string.Empty;


        private string _messangerType = string.Empty;
        private string _postalCode = string.Empty;
        private string _profilePicURL = string.Empty;
        private string _profileThumbPicURL = string.Empty;
        private string _region = string.Empty;
        private char _religion = char.MinValue;
        private bool _showOnMap = true;
        private char _smokes = char.MinValue;
        public int UserAccountDetailID { get; set; }

        public decimal? Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        public decimal? Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public string MessangerType
        {
            get { return _messangerType; }
            set { _messangerType = value; }
        }

        public string MessangerName
        {
            get { return _messangerName; }
            set { _messangerName = value; }
        }


        public bool MembersOnlyProfile { get; set; }

        public string BrowerType
        {
            get
            {
                if (string.IsNullOrEmpty(_browerType))
                {
                    _browerType = HttpContext.Current.Request.Browser.Type;
                }

                return _browerType;
            }
            set { _browerType = value; }
        }

        public bool EmailMessages
        {
            get { return _emailMessages; }
            set { _emailMessages = value; }
        }


        public string DefaultLanguage
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_defaultLanguage))
                {
                    return _defaultLanguage.ToUpper();
                }
                return _defaultLanguage;
            }
            set { _defaultLanguage = value; }
        }

        public bool EnableProfileLogging
        {
            get { return _enableProfileLogging; }
            set { _enableProfileLogging = value; }
        }

        public DateTime? LastPhotoUpdate { get; set; }

        public int UserAccountID { get; set; }

        public string Country
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_country)) return SiteEnums.CountryCodeISO.U0.ToString();

                return _country;
            }
            set { _country = value; }
        }

        public string Region
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_region)) return string.Empty;
                return _region;
            }
            set { _region = value; }
        }

        public string City
        {
            get
            {
                if (string.IsNullOrEmpty(_city)) return string.Empty;
                return _city;
            }
            set { _city = value; }
        }

        public string PostalCode
        {
            get
            {
                if (string.IsNullOrEmpty(_postalCode)) return string.Empty;
                return _postalCode;
            }
            set { _postalCode = value; }
        }


        public string ProfilePicURL
        {
            get { return _profilePicURL; }
            set { _profilePicURL = value; }
        }

        public string HardwareSoftware
        {
            get
            {
                if (string.IsNullOrEmpty(_hardwareSoftware)) return string.Empty;
                return _hardwareSoftware;
            }
            set { _hardwareSoftware = value; }
        }


        public string FirstName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_firstName)) return string.Empty;
                return _firstName.Trim();
            }
            set { _firstName = value; }
        }

        public string LastName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_lastName)) return string.Empty;
                return _lastName.Trim();
            }
            set { _lastName = value; }
        }


        public string AboutDesc
        {
            get
            {
                if (string.IsNullOrEmpty(_aboutDesc)) return string.Empty;

                return _aboutDesc.Trim();
            }
            set { _aboutDesc = value; }
        }


        public int? YouAreID { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Messages), Name = "RelationshipStatus")]
        public int? RelationshipStatusID { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Messages), Name = "InterestedIn")]
        public int? InterestedInID { get; set; }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }

        public char Religion
        {
            get { return _religion; }
            set { _religion = value; }
        }

        public char Ethnicity
        {
            get { return _ethnicity; }
            set { _ethnicity = value; }
        }

        public float HeightCM { get; set; }

        public float WeightKG { get; set; }

        public char Diet
        {
            get { return _diet; }
            set { _diet = value; }
        }

        public int AccountViews { get; set; }

        public string ExternalURL
        {
            get
            {
                if (string.IsNullOrEmpty(_externalURL))
                {
                    return string.Empty;
                }
                return _externalURL;
            }
            set { _externalURL = value; }
        }

        public char Smokes
        {
            get { return _smokes; }
            set { _smokes = value; }
        }

        public char Drinks
        {
            get { return _drinks; }
            set { _drinks = value; }
        }

        [Display(ResourceType = typeof (Messages), Name = "Handed")]
        public char Handed
        {
            get { return _handed; }
            set { _handed = value; }
        }

        public bool DisplayAge
        {
            get { return _displayAge; }
            set { _displayAge = value; }
        }

        public string ProfileThumbPicURL
        {
            get { return _profileThumbPicURL; }
            set { _profileThumbPicURL = value; }
        }


        public string BandsSeen
        {
            get
            {
                if (_bandsSeen == null) return string.Empty;
                return _bandsSeen.Trim();
            }
            set { _bandsSeen = value; }
        }

        public string BandsToSee
        {
            get
            {
                if (_bandsToSee == null) return string.Empty;
                return _bandsToSee.Trim();
            }
            set { _bandsToSee = value; }
        }


        public bool ShowOnMap
        {
            get { return _showOnMap; }
            set { _showOnMap = value; }
        }

        public int ReferringUserID { get; set; }


        public string FindUserFilter
        {
            get { return _findUserFilter; }
            set { _findUserFilter = value; }
        }

        #endregion

        #region non-db properties

        private bool _getJustOne;

        public bool ShowOnMapLegal
        {
            get
            {
                if (!string.IsNullOrEmpty(Country.Trim()) && Over18 && ShowOnMap) return true;
                else return false;
            }
        }

        public bool IsBirthdayToday
        {
            get
            {
                if (DisplayAge &&
                    DateTime.UtcNow.Month == BirthDate.Month &&
                    (DateTime.UtcNow.Day == BirthDate.Day))
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
                var theCO = SiteEnums.CountryCodeISO.U0;

                if (Enum.TryParse(Country, out theCO))
                {
                    return Utilities.ResourceValue(Utilities.GetEnumDescription(theCO));
                }

                return Messages.Unknown;
            }
        }

        public string GenderIconSmall
        {
            get { return GenderIcon.Replace(".png", "_small.png"); }
        }

        public string GenderIconLink
        {
            get
            {
                var sb = new StringBuilder(100);

                var youAre = new YouAre(Convert.ToInt32(YouAreID));

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
                        sb.Append(VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/sex/{0}.png", youAre.TypeLetter.ToString())));
                        break;
                    default:
                        sb.Append(VirtualPathUtility.ToAbsolute(
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
                var sb = new StringBuilder(100);

                var youAre = new YouAre(Convert.ToInt32(YouAreID));

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
                        sb.Append(VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/sex/{0}_D.png", youAre.TypeLetter.ToString())));
                        break;
                    default:
                        sb.Append(VirtualPathUtility.ToAbsolute(
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
                var sb = new StringBuilder(100);

                sb.Append(@"<img src=""");

                var ua = new UserAccount(UserAccountID);

                if (YouAreID == null) return string.Empty;


                var youAre = new YouAre(Convert.ToInt32(YouAreID));

                switch (youAre.TypeLetter)
                {
                    case 'A':
                    case 'M':
                    case 'C':
                    case 'F':
                    case 'B':
                    case 'R':
                    case 'G':
                        sb.Append(VirtualPathUtility.ToAbsolute(
                            string.Format("~/content/images/sex/{0}.png", youAre.TypeLetter.ToString())));
                        break;
                    default:
                        sb.Append(VirtualPathUtility.ToAbsolute(
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
                var ua = new UserAccount(UserAccountID);

                string[] rls = Roles.GetRolesForUser(ua.UserName);

                var sb = new StringBuilder(100);

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
                                sb.Append(VirtualPathUtility.ToAbsolute(
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
                            break; //just the 1st role's icon
                        }
                        else _getJustOne = false;
                    }
                }

                return sb.ToString();
            }
        }


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
                if (string.IsNullOrEmpty(Country.Trim()))
                {
                    return VirtualPathUtility.ToAbsolute("~/content/images/countries/defaultcountry_small.png");
                }
                else
                {
                    return VirtualPathUtility.ToAbsolute("~/content/images/countries/flag_sprite.png");
                    //return System.Web.VirtualPathUtility.ToAbsolute( string.Format( "~/content/images/countries/{0}_small.png",  this.Country ));
                    //images/countries/
                }
            }
        }

        public string CountryFlag
        {
            get
            {
                if (string.IsNullOrEmpty(Country.Trim()))
                {
                    return VirtualPathUtility.ToAbsolute("~/content/images/countries/defaultcountry.png");
                }
                else
                {
                    return VirtualPathUtility.ToAbsolute(string.Format("~/content/images/countries/{0}.png", Country));
                }
            }
        }

        public string FullProfilePicURL
        {
            get
            {
                if (string.IsNullOrEmpty(ProfileThumbPicURL))
                {
                    return VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuser.png");
                }
                else
                {
                    return Utilities.S3ContentPath(ProfilePicURL);
                }
            }
        }


        public string FullProfilePicThumbURL
        {
            get
            {
                if (string.IsNullOrEmpty(ProfileThumbPicURL))
                {
                    return VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuserthumb.png");
                }
                else
                {
                    return Utilities.S3ContentPath(ProfileThumbPicURL);
                }
            }
        }


        public char SexLetter
        {
            get
            {
                if (YouAreID == null) return 'U';

                var youAre = new YouAre(Convert.ToInt32(YouAreID));

                return youAre.TypeLetter;
            }
        }

        public string Sex
        {
            get
            {
                if (YouAreID == null) return string.Empty;

                var youAre = new YouAre(Convert.ToInt32(YouAreID));

                return Utilities.ResourceValue(youAre.Name);
            }
        }

        public string AboutDescription
        {
            get { return Utilities.MakeLink(AboutDesc).Replace("\r\n", "<br />"); }
        }

        public int YearsOld
        {
            get { return Utilities.CalculateAge(BirthDate); }
        }


        public bool Over16
        {
            get
            {
                int age = Utilities.CalculateAge(BirthDate);
                return (age >= 16);
            }
        }


        public bool Over18
        {
            get
            {
                int age = Utilities.CalculateAge(BirthDate);
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

                YouAre youAre = null;
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
            var countries = new List<SiteEnums.CountryCodeISO>();

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctUserCountries";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                SiteEnums.CountryCodeISO country;

                string uniqueCountry = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    uniqueCountry = FromObj.StringFromObj(dr["country"]);

                    if (!string.IsNullOrWhiteSpace(uniqueCountry))
                    {
                        country =
                            (SiteEnums.CountryCodeISO) Enum.Parse(typeof (SiteEnums.CountryCodeISO), uniqueCountry);
                        countries.Add(country);
                    }
                }
            }

            return countries;
        }


        public static List<SiteEnums.SiteLanguages> GetDistinctUserLanguages()
        {
            var languages = new List<SiteEnums.SiteLanguages>();

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

                    language = (SiteEnums.SiteLanguages) Enum.Parse(typeof (SiteEnums.SiteLanguages), defaultLang);
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

            comm.AddParameter("userAccountDetailID", UserAccountDetailID);

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

            return DbAct.ExecuteSelectCommand(comm);
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserAccountDetail";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Country), Country);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Region), Region);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => City), City);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => YouAreID), YouAreID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ProfilePicURL), ProfilePicURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => AboutDesc), AboutDesc);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => RelationshipStatusID), RelationshipStatusID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => InterestedInID), InterestedInID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BirthDate), BirthDate);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Religion), Religion);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Ethnicity), Ethnicity);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => HeightCM), HeightCM);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => WeightKG), WeightKG);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Diet), Diet);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => AccountViews), AccountViews);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ExternalURL), ExternalURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Smokes), Smokes);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Drinks), Drinks);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Handed), Handed);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => DisplayAge), DisplayAge);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ProfileThumbPicURL), ProfileThumbPicURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BandsToSee), BandsToSee);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BandsSeen), BandsSeen);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => EnableProfileLogging), EnableProfileLogging);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => LastPhotoUpdate), LastPhotoUpdate);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => EmailMessages), EmailMessages);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ShowOnMap), ShowOnMap);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PostalCode), PostalCode);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ReferringUserID), ReferringUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MembersOnlyProfile), MembersOnlyProfile);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MessangerType), MessangerType);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MessangerName), MessangerName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => HardwareSoftware), HardwareSoftware);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BrowerType), BrowerType);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FirstName), FirstName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => LastName), LastName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => DefaultLanguage), DefaultLanguage);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Latitude), Latitude);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Longitude), Longitude);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FindUserFilter), FindUserFilter);

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
                UserAccountDetailID = Convert.ToInt32(result);

                return UserAccountDetailID;
            }
        }

        public void GetUserAccountDeailForUser(int userAccountID)
        {
            UserAccountID = userAccountID;

            if (HttpContext.Current == null || HttpRuntime.Cache[CacheNameAlt] == null)
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
                    if (HttpContext.Current != null) HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheNameAlt);

                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheNameAlt]);
            }
        }

        public override void Get(int userAccountDetailID)
        {
            UserAccountDetailID = userAccountDetailID;

            if (HttpContext.Current == null || HttpRuntime.Cache[CacheName] == null)
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
                        HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            UserAccountDetailID =
                FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => UserAccountDetailID)]);
            UserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => UserAccountID)]);
            Country = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Country)]);
            Region = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Region)]);
            City = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => City)]);
            PostalCode = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => PostalCode)]);
            YouAreID = FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => YouAreID)]);
            ProfilePicURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ProfilePicURL)]);
            AboutDesc = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => AboutDesc)]);
            RelationshipStatusID =
                FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => RelationshipStatusID)]);
            InterestedInID = FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => InterestedInID)]);
            BirthDate = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => BirthDate)]);
            Religion = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => Religion)]);
            Ethnicity = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => Ethnicity)]);
            HeightCM = FromObj.FloatFromObj(dr[StaticReflection.GetMemberName<string>(x => HeightCM)]);
            WeightKG = FromObj.FloatFromObj(dr[StaticReflection.GetMemberName<string>(x => WeightKG)]);
            Diet = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => Diet)]);
            AccountViews = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => AccountViews)]);
            ExternalURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ExternalURL)]);
            Smokes = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => Smokes)]);
            Drinks = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => Drinks)]);
            Handed = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => Handed)]);
            DisplayAge = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => DisplayAge)]);
            ProfileThumbPicURL =
                FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ProfileThumbPicURL)]);
            BandsSeen = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => BandsSeen)]);
            BandsToSee = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => BandsToSee)]);
            EnableProfileLogging =
                FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => EnableProfileLogging)]);
            LastPhotoUpdate =
                FromObj.DateNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => LastPhotoUpdate)]);
            EmailMessages = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => EmailMessages)]);
            ShowOnMap = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => ShowOnMap)]);
            ReferringUserID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ReferringUserID)]);
            BrowerType = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => BrowerType)]);
            MembersOnlyProfile = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => MembersOnlyProfile)]);
            MessangerName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => MessangerName)]);
            HardwareSoftware = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => HardwareSoftware)]);
            FirstName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => FirstName)]);
            LastName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => LastName)]);
            DefaultLanguage = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => DefaultLanguage)]);

            Latitude = FromObj.DecimalNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => Latitude)]);
            Longitude = FromObj.DecimalNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => Longitude)]);
            FindUserFilter = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => FindUserFilter)]);
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserAccountDetail";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UpdatedByUserID), UpdatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountDetailID), UserAccountDetailID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Country), Country);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Region), Region);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => City), City);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => YouAreID), YouAreID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ProfilePicURL), ProfilePicURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => AboutDesc), AboutDesc);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => RelationshipStatusID), RelationshipStatusID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => InterestedInID), InterestedInID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BirthDate), BirthDate);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Religion), Religion);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Ethnicity), Ethnicity);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => HeightCM), HeightCM);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => WeightKG), WeightKG);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Diet), Diet);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => AccountViews), AccountViews);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ExternalURL), ExternalURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Smokes), Smokes);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Drinks), Drinks);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Handed), Handed);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => DisplayAge), DisplayAge);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ProfileThumbPicURL), ProfileThumbPicURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BandsToSee), BandsToSee);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BandsSeen), BandsSeen);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => EnableProfileLogging), EnableProfileLogging);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => LastPhotoUpdate), LastPhotoUpdate);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => EmailMessages), EmailMessages);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ShowOnMap), ShowOnMap);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ReferringUserID), ReferringUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MembersOnlyProfile), MembersOnlyProfile);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MessangerType), MessangerType);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MessangerName), MessangerName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => HardwareSoftware), HardwareSoftware);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PostalCode), PostalCode);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => BrowerType), BrowerType);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FirstName), FirstName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => LastName), LastName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => DefaultLanguage), DefaultLanguage);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Latitude), Latitude);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Longitude), Longitude);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FindUserFilter), FindUserFilter);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            return (result != -1);
        }

        #endregion

        #region ICacheName Members

        public string CacheNameAlt
        {
            get { return string.Format("{0}-user-{1}", GetType().FullName, UserAccountID); }
        }

        public string CacheName
        {
            get { return string.Format("{0}-{1}", GetType().FullName, UserAccountDetailID.ToString()); }
        }

        public void RemoveCache()
        {
            if (HttpContext.Current != null)
            {
                HttpRuntime.Cache.DeleteCacheObj(CacheName);
                HttpRuntime.Cache.DeleteCacheObj(CacheNameAlt);
            }
        }

        #endregion

        public string InterestedInIconSmall
        {
            get { return InterestedInIcon.Replace(".png", "_small.png"); }
        }


        public string HandedFull
        {
            get
            {
                switch (Handed)
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
                if (InterestedInID == null) return string.Empty;

                var youAre = new InterestedIn(Convert.ToInt32(InterestedInID));

                return Utilities.ResourceValue(youAre.Name);
            }
        }


        public string InterestedInIcon
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(@"<img src=""");

                switch (InterestedInID)
                {
                    case 'A':
                    case 'M':
                    case 'F':
                    case 'B':
                    case 'P':
                    case 'R':
                    case 'C':
                        sb.Append(VirtualPathUtility.ToAbsolute(
                            "~/content/images/interestedin/" + InterestedInID + ".png"));
                        break;
                    default:
                        sb.Append(VirtualPathUtility.ToAbsolute(
                            "~/content/images/interestedin/U.png"));
                        break;
                }

                sb.Append(@""" alt=""");
                if (char.MinValue != InterestedInID)
                {
                    sb.Append(InterestedInID);
                }
                else sb.Append(Messages.Unknown);
                sb.Append(@""" title=""");

                sb.AppendFormat("{0}: ", Messages.InterestedIn);

                switch (InterestedInID)
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
                if (RelationshipStatusID == null) return string.Empty;

                var youAre = new RelationshipStatus(Convert.ToInt32(RelationshipStatusID));

                return Utilities.ResourceValue(youAre.Name);
            }
        }

        public string RelationshipStatusIconSmall
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(@"<img src=""");


                switch (RelationshipStatusID)
                {
                    case 'G':
                        sb.Append(VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_green.png"));
                        break;
                    case 'Y':
                        sb.Append(VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_yellow.png"));
                        break;
                    case 'R':
                        sb.Append(VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_red.png"));
                        break;
                    default:
                        sb.Append(VirtualPathUtility.ToAbsolute("~/content/images/relationshipstatus/status_unknown.png"));
                        break;
                }

                sb.Append(@""" alt=""");

                if (RelationshipStatusID == char.MinValue)
                {
                    sb.Append(Messages.Unknown);
                }
                else
                {
                    sb.Append(RelationshipStatusID);
                }


                sb.Append(@""" title=""");

                sb.AppendFormat("{0}: ", Messages.RelationshipStatus);
                switch (RelationshipStatusID)
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
                var ua = new UserAccount(UserAccountID);
                var sb = new StringBuilder(100);
                sb.AppendFormat(@"<a title=""{0}"" class=""m_over"" href=""{1}"">", ua.UserName,
                                VirtualPathUtility.ToAbsolute("~/" + ua.UserName.ToLower()));
                sb.AppendFormat(@"<img title=""{0}"" alt=""{0}"" src=""", ua.UserName);
                sb.Append(FullProfilePicThumbURL);
                sb.Append(@""" />");
                sb.Append(@"</a>");
                return sb.ToString();
            }
        }

        public int ForumPosts { get; set; }

        public string SmallUserIcon
        {
            get
            {
                var sb = new StringBuilder(100);
                var ua = new UserAccount(UserAccountID);

                sb.AppendFormat(@"<span class=""profile_username""><a title=""{0}"" class=""m_over"" href=""{1}"">", 
                                ua.UserName, 
                                VirtualPathUtility.ToAbsolute(string.Format("~/{0}", ua.UserName.ToLower())));
                sb.Append(ua.UserName);
                sb.Append(@"</a></span>");
                sb.Append("<br />");

                sb.Append(UserFace);

                sb.Append("<br />");

 
                sb.AppendFormat(@"<img title=""{0}"" alt=""{0}"" src=""{1}"" />", 
                                Sex,
                                VirtualPathUtility.ToAbsolute(string.Format("~/content/images/sex/{0}.png", SexLetter.ToString())));

                sb.AppendFormat(@"<div title=""{0}"" class=""sprites sprite-{1}_small""></div>", CountryName, Country);

                // hack: latin isn't supported
                sb.AppendFormat(@"<span title=""{1}"" class=""default_lang"">{0}</span>",
                                (DefaultLanguage == "FO") ? "LA" : DefaultLanguage,
                                Utilities.GetLanguageNameForCode(DefaultLanguage));
                _getJustOne = true;
                sb.Append(SiteBages);

                if (ua.IsOnLine)
                {
                    sb.AppendFormat(@"<img style=""height: 8px; width:8px"" title=""{0}"" alt=""{0}"" src=""{1}"" />",
                                    Messages.IsOnline,
                                    VirtualPathUtility.ToAbsolute("~/content/images/status/abutton2_e0.gif"));
                }

                if (ForumPosts > 0)
                {
                    sb.Append("<br />");
                    sb.AppendFormat(@"<span class=""forum_post_count"">Posts: {0}</span>", ForumPosts);
                }

                return sb.ToString();
            }
        }


        public char RelationshipStatus
        {
            get
            {
                if (RelationshipStatusID == null) return char.MinValue;

                var rstaus = new RelationshipStatus(Convert.ToInt32(RelationshipStatusID));

                return rstaus.TypeLetter;
            }
        }

        public char InterestedIn
        {
            get
            {
                if (InterestedInID == null) return char.MinValue;

                var rstaus = new InterestedIn(Convert.ToInt32(InterestedInID));

                return rstaus.TypeLetter;
            }
        }

        public char YouAre
        {
            get
            {
                if (YouAreID == null) return 'U';

                var rstaus = new YouAre(Convert.ToInt32(YouAreID));

                return rstaus.TypeLetter;
            }
        }

        public int Set()
        {
            if (UserAccountDetailID == 0) return Create();
            else
            {
                if (Update())
                {
                    return 1;
                }
                else return 0;
            }
        }
    }
}