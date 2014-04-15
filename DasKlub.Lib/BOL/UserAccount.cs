using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Lib.BOL.VideoContest;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.Configs;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using LitS3;

namespace DasKlub.Lib.BOL
{
    public class UserAccount : BaseIUserLogCRUD, ICacheName, IUnorderdListItem, IURLTo
    {
        #region BOLAction Members

        /// <summary>
        ///     INSERT the enduser in the database
        /// </summary>
        public override int Create()
        {
            if (UserName == string.Empty || EMail == string.Empty) return 0;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserAccount";
            // create a new parameter

            comm.AddParameter("username", UserName);
            comm.AddParameter("eMail", EMail);
            comm.AddParameter("password", Password);
            comm.AddParameter("passwordFormat", PasswordFormat);
            comm.AddParameter("passwordSalt", PasswordSalt);
            comm.AddParameter("passwordQuestion", PasswordQuestion);
            comm.AddParameter("passwordAnswer", PasswordAnswer);
            comm.AddParameter("isApproved", IsApproved);
            comm.AddParameter("isOnLine", IsOnLine);
            comm.AddParameter("comment", Comment);
            comm.AddParameter("isLockedOut", IsLockedOut);
            comm.AddParameter("LastPasswordChangedDate", DateTime.UtcNow);
            comm.AddParameter("ipAddress", HttpContext.Current.Request.UserHostAddress);

            // the result is their ID
            // execute the stored procedure
            string result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            UserAccountID = Convert.ToInt32(result);

            return UserAccountID;
        }

        /// <summary>
        ///     UPDATE the enduser in the database
        /// </summary>
        public override bool Update()
        {
            if (UserAccountID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserAccount";
            // create a new parameter

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IpAddress),
                              HttpContext.Current.Request.UserHostAddress);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserName), UserName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => EMail), EMail);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Password), Password);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PasswordFormat), PasswordFormat);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PasswordSalt), PasswordSalt);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PasswordQuestion), PasswordQuestion);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PasswordAnswer), PasswordAnswer);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FailedPasswordAttemptCount),
                              FailedPasswordAttemptCount);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FailedPasswordAttemptWindowStart),
                              FailedPasswordAttemptWindowStart);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsApproved), IsApproved);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Comment), Comment);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsLockedOut), IsLockedOut);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FailedPasswordAnswerAttemptCount),
                              FailedPasswordAnswerAttemptCount);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FailedPasswordAnswerAttemptWindowStart),
                              FailedPasswordAnswerAttemptWindowStart);

            if (LastLoginDate == DateTime.MinValue)
            {
                LastLoginDate = new DateTime(1900, 1, 1);
            }

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => LastLoginDate), LastLoginDate);

            DbParameter param = comm.CreateParameter();
            // isOnLine
            param = comm.CreateParameter();
            param.ParameterName = "@isOnLine";

            // something isn't working
            if (SigningOut)
            {
                param.Value = false;
            }
            else
            {
                // param.Value = this.IsOnLine;
                param.Value = true;
            }

            param.DbType = DbType.Boolean;
            comm.Parameters.Add(param);


            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            RemoveCache();

            return result;
        }


        /// <summary>
        ///     Delete the user and their data
        /// </summary>
        /// <returns>if they were deleted</returns>
        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserAccount";
            // create a new parameter

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);

            RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override sealed void Get(int userAccountID)
        {
            UserAccountID = userAccountID;

            if (HttpContext.Current == null || HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetUserAccountFromID";
                // create a new parameter
                comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current != null)
                    {
                        HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    }
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public override sealed void Get(DataRow dr)
        {
            try
            {
                if (dr == null) return;

                Comment = FromObj.StringFromObj(dr["Comment"]);
                CreateDate = FromObj.DateFromObj(dr["CreateDate"]);
                EMail = FromObj.StringFromObj(dr["eMail"]);
                PasswordSalt = FromObj.StringFromObj(dr["PasswordSalt"]);
                UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                FailedPasswordAttemptCount = FromObj.IntFromObj(dr["FailedPasswordAttemptCount"]);
                FailedPasswordAttemptWindowStart = FromObj.DateFromObj(dr["FailedPasswordAttemptWindowStart"]);
                FailedPasswordAnswerAttemptCount = FromObj.IntFromObj(dr["FailedPasswordAnswerAttemptCount"]);
                FailedPasswordAnswerAttemptWindowStart =
                    FromObj.DateFromObj(dr["FailedPasswordAnswerAttemptWindowStart"]);
                IsApproved = FromObj.BoolFromObj(dr["IsApproved"]);
                IsLockedOut = FromObj.BoolFromObj(dr["IsLockedOut"]);
                IsOnLine = FromObj.BoolFromObj(dr["IsOnLine"]);
                LastActivityDate = FromObj.DateFromObj(dr["LastActivityDate"]);
                LastLockoutDate = FromObj.DateFromObj(dr["LastLockoutDate"]);
                LastLoginDate = FromObj.DateFromObj((dr["LastLoginDate"]));
                LastPasswordChangedDate = FromObj.DateFromObj(dr["LastPasswordChangeDate"]);
                Password = FromObj.StringFromObj(dr["password"]);
                PasswordAnswer = FromObj.StringFromObj(dr["passwordAnswer"]);
                PasswordFormat = FromObj.StringFromObj(dr["passwordFormat"]);
                PasswordQuestion = FromObj.StringFromObj(dr["passwordQuestion"]);
                UserName = FromObj.StringFromObj(dr["userName"]);
                IpAddress = FromObj.StringFromObj(dr["ipAddress"]);
            }
            catch
            {
            }
        }

        #endregion

        #region Constructors

        public UserAccount()
        {
        }

        /// <summary>
        ///     Fill a new UserAccount object from the username
        /// </summary>
        /// <param name="username">the user to logging in or logged in</param>
        /// <returns>the UserAccount</returns>
        public UserAccount(string username)
        {
            UserName = username;

            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                var comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetUserAccountFromUsername";

                comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserName), UserName);

                // execute the stored procedure
                var dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt == null || dt.Rows.Count <= 0) return;
                HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                Get(dt.Rows[0]);
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }


        /// <summary>
        ///     Get the user from their id or membership user provider key
        /// </summary>
        /// <param name="userAccountID"></param>
        public UserAccount(int userAccountID)
        {
            Get(userAccountID);
        }

        /// <summary>
        ///     Get the user from the membership user
        /// </summary>
        /// <param name="mu"></param>
        public UserAccount(MembershipUser mu)
        {
            if (mu == null) return;

            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();

            comm.AddParameter("UserAccountID", Convert.ToInt32(mu.ProviderUserKey));

            var dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public UserAccount(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        #region private variables

        private string _comment = string.Empty;
        private DateTime _createDate = DateTime.MinValue;
        private string _eMail = string.Empty;
        private DateTime _failedPasswordAnswerAttemptWindowStart = DateTime.MinValue;
        private DateTime _failedPasswordAttemptWindowStart = DateTime.MinValue;
        private string _ipAddress = string.Empty;
        private DateTime _lastActivityDateTime = DateTime.MinValue;
        private DateTime _lastLockoutDate = DateTime.MinValue;
        private DateTime _lastLoginDate = DateTime.MinValue;
        private DateTime _lastPasswordChangedDateTime = DateTime.MinValue;
        private string _password = string.Empty;
        private string _passwordAnswer = string.Empty;
        private string _passwordFormat = string.Empty;
        private string _passwordQuestion = string.Empty;
        private string _passwordSalt = string.Empty;
        private string _username = string.Empty;

        #endregion

        #region public properties

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }


        public string PasswordSalt
        {
            get { return _passwordSalt; }
            set { _passwordSalt = value; }
        }

        public DateTime FailedPasswordAttemptWindowStart
        {
            get
            {
                if (_failedPasswordAttemptWindowStart == DateTime.MinValue)
                {
                    _failedPasswordAttemptWindowStart = new DateTime(1900, 1, 1);
                }

                return _failedPasswordAttemptWindowStart;
            }
            set { _failedPasswordAttemptWindowStart = value; }
        }

        public int FailedPasswordAnswerAttemptCount { get; set; }

        public DateTime FailedPasswordAnswerAttemptWindowStart
        {
            get
            {
                if (_failedPasswordAnswerAttemptWindowStart == DateTime.MinValue)
                {
                    _failedPasswordAnswerAttemptWindowStart = new DateTime(1900, 1, 1);
                }


                return _failedPasswordAnswerAttemptWindowStart;
            }
            set { _failedPasswordAnswerAttemptWindowStart = value; }
        }

        public DateTime LastPasswordChangedDate
        {
            get { return _lastPasswordChangedDateTime; }
            set { _lastPasswordChangedDateTime = value; }
        }

        public DateTime LastActivityDate
        {
            get { return _lastActivityDateTime; }
            set { _lastActivityDateTime = value; }
        }

        public bool IsLockedOut { get; set; }

        public bool IsApproved { get; set; }

        /// <summary>
        ///     A user is considered online if the current
        ///     date and time minus the UserIsOnlineTimeWindow
        ///     property value is earlier than the LastActivityDate for the user.
        ///     The LastActivityDate for a user is updated to the current date and
        ///     time by the CreateUser, UpdateUser and ValidateUser methods,
        ///     and can be updated by some of the overloads of the GetUser method.
        /// </summary>
        /// <see>http://msdn.microsoft.com/en-us/library/system.web.security.membershipuser.isonline.aspx</see>
        public bool IsOnLine { get; set; }

        public int FailedPasswordAttemptCount { get; set; }

        public DateTime LastLockoutDate
        {
            get { return _lastLockoutDate; }
            set { _lastLockoutDate = value; }
        }

        public DateTime LastLoginDate
        {
            get { return _lastLoginDate; }
            set { _lastLoginDate = value; }
        }


        public string PasswordAnswer
        {
            get { return _passwordAnswer; }
            set { _passwordAnswer = value; }
        }

        public string PasswordQuestion
        {
            get { return _passwordQuestion ?? (_passwordQuestion = string.Empty); }
            set { _passwordQuestion = value; }
        }


        public string PasswordFormat
        {
            get { return _passwordFormat; }
            set { _passwordFormat = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string EMail
        {
            get { return _eMail; }
            set { _eMail = value; }
        }

        public string UserName
        {
            get
            {
                if (_username != null)
                {
                    _username = _username.Trim();
                }
                return _username;
            }
            set { _username = value; }
        }

        public int UserAccountID { get; private set; }

        public string Comment
        {
            get { return _comment ?? (_comment = string.Empty); }
            set { _comment = value; }
        }

        #endregion

        #region public static methods

        /// <summary>
        ///     Get the user from their e-mail
        /// </summary>
        /// <param name="eMail"></param>
        public static string GetUserAccountNameFromEMail(string eMail)
        {
            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetUserAccountnameFromEMail";
            comm.AddParameter("eMail", eMail.Trim());

            string username = DbAct.ExecuteScalar(comm);
            return username;
        }

        public static bool IsAccountIPTaken(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress)) return false;

            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_IsAccountIPTaken";
            comm.AddParameter("ipAddress", ipAddress.Trim());
            
            return DbAct.ExecuteScalar(comm) == "1";
        }

        /// <summary>
        ///     Add a user to a role
        /// </summary>
        /// <param name="UserAccountID"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool AddUserToRole(int UserAccountID, string roleName)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.AddParameter("UserAccountID", UserAccountID);
            comm.AddParameter("roleName", roleName);

            int result = Convert.ToInt32(DbAct.ExecuteScalar(comm));
            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///     Remove a user from a role
        /// </summary>
        /// <param name="UserAccountID"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool DeleteUserFromRole(int UserAccountID, string roleName)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.AddParameter("UserAccountID", UserAccountID);
            comm.AddParameter("roleName", roleName);

            int result = Convert.ToInt32(DbAct.ExecuteScalar(comm));
            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///     Get all of the roles for this user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>string array</returns>
        public static string[] GetRolesForUser(string username)
        {
            var eu = new UserAccount(username);

            var allRoles = new ArrayList();
            DataTable dt;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRolesForUser";
            // create a new parameter

            comm.AddParameter("UserAccountID", eu.UserAccountID);

            // exec
            dt = DbAct.ExecuteSelectCommand(comm);
            foreach (DataRow r in dt.Rows)
            {
                allRoles.Add(FromObj.StringFromObj(r["roleName"]));
            }
            var stringArray = (string[]) allRoles.ToArray(typeof (string));
            return stringArray;
        }


        public void GetRandomUserAccount()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRandomUserAccountID";

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
                Get(FromObj.IntFromObj(dt.Rows[0]["userAccountID"]));
        }

        public void GetNewestPhotoUploader()
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNewestPhotoUploader";

            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
                Get(FromObj.IntFromObj(dt.Rows[0]["userAccountID"]));
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}-{2}", GetType().FullName, UserName.ToLower(CultureInfo.InvariantCulture), UserAccountID);
            }
        }

        public void RemoveCache()
        {
            if (HttpContext.Current == null) return;

            HttpRuntime.Cache.DeleteCacheObj(CacheName);

            // remove username cache
            int useraccountID = UserAccountID;
            UserAccountID = 0;

            HttpRuntime.Cache.DeleteCacheObj(CacheName);

            // remove user account id cache
            string username = UserName;
            UserName = string.Empty;
            UserAccountID = useraccountID;

            HttpRuntime.Cache.DeleteCacheObj(CacheName);

            UserName = username;
            UserAccountID = useraccountID;
        }

        #endregion

        public string TinyUserIcon
        {
            get
            {
                var uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(UserAccountID);

                var sb = new StringBuilder(100);

                sb.AppendFormat(@"<img style=""width:20px;height:20px;"" title=""{0}"" alt=""{0}"" src=""", UserName);
                sb.Append(uad.FullProfilePicThumbURL);
                sb.Append(@""" />");

                return sb.ToString();
            }
        }

        public bool IsAdmin
        {
            get
            {
                string[] roles = GetRolesForUser(UserName);

                return roles.Any(role => role.ToLower() == SiteEnums.RoleTypes.admin.ToString());
            }
        }

        public bool SigningOut { get; set; }


        public string FlagIcon
        {
            get
            {
                var uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(UserAccountID);

                return uad.CountryFlagThumb;
            }
        }


        public string CountryName
        {
            get
            {
                var uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(UserAccountID);

                return uad.CountryName;
            }
        }


        public string Country
        {
            get
            {
                var uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(UserAccountID);

                return uad.Country;
            }
        }

        public int ForumPosts { get; set; }

        public string ToUnorderdListItem
        {
            get
            {
                var uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(UserAccountID);

                uad.ForumPosts = this.ForumPosts;

                var sb = new StringBuilder(100);

                sb.Append(@"<li>");

                if (IsProfileLinkNewWindow)
                {
                    string fullIcon = uad.SmallUserIcon.Replace("<a ", @"<a  target=""_blank"" ");
                    sb.Append(fullIcon);
                }
                else
                {
                    sb.Append(uad.SmallUserIcon);
                }

                sb.Append(@"</li>");

                return sb.ToString();
            }
        }

        public Uri UrlTo
        {
            get
            {
                return new Uri(Utilities.URLAuthority() + 
                               VirtualPathUtility.ToAbsolute(string.Format("~/{0}", UserName.ToLower())));
            }
        }

        /// <summary>
        ///     DELETES THE USER COMPLETELY!
        /// </summary>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public bool Delete(bool deleteAllRelatedData)
        {
            if (!deleteAllRelatedData) return Delete();

            var s3 = new S3Service
                {
                    AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                    SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                };


            var uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(UserAccountID);
            uad.Delete();

            if (!string.IsNullOrWhiteSpace(s3.AccessKeyID) &&
                !string.IsNullOrWhiteSpace(s3.SecretAccessKey) &&
                !string.IsNullOrWhiteSpace(uad.ProfilePicURL))
            {
                if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, uad.ProfilePicURL))
                {
                    s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, uad.ProfilePicURL);
                }
            }

            if (!string.IsNullOrWhiteSpace(s3.AccessKeyID) &&
                !string.IsNullOrWhiteSpace(s3.SecretAccessKey) &&
                !string.IsNullOrWhiteSpace(uad.ProfileThumbPicURL))
            {
                if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, uad.ProfileThumbPicURL))
                {
                    s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, uad.ProfileThumbPicURL);
                }
            }

            var usc = new UserConnection {UserAccountID = UserAccountID};
            usc.Delete(true);

            var conComs = new ContentComments();

            conComs.GetUserContentComments(UserAccountID);

            foreach (ContentComment concom1 in conComs)
            {
                concom1.Delete();
            }

            // user wall
            WallMessages.DeleteUserWall(UserAccountID);

            var uaddress = new UserAddress {UserAccountID = UserAccountID};
            uaddress.Delete();

            var uphos = new UserPhotos();
            uphos.GetUserPhotos(UserAccountID);

            foreach (UserPhoto up1 in uphos)
            {
                up1.Delete();


                if (!string.IsNullOrWhiteSpace(s3.AccessKeyID) &&
                    !string.IsNullOrWhiteSpace(s3.SecretAccessKey) &&
                    !string.IsNullOrWhiteSpace(up1.FullProfilePicThumbURL))
                {
                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, up1.FullProfilePicThumbURL))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, up1.FullProfilePicThumbURL);
                    }
                }

                if (string.IsNullOrWhiteSpace(s3.AccessKeyID) || string.IsNullOrWhiteSpace(s3.SecretAccessKey) ||
                    string.IsNullOrWhiteSpace(up1.FullProfilePicURL)) continue;
                if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, up1.FullProfilePicURL))
                {
                    s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, up1.FullProfilePicURL);
                }
            }

            // delete any acknowledgements they made
            Acknowledgements.DeleteAllAcknowledgements(UserAccountID);


            // get all status updates and delete all the acknowledgements for each
            var allUserStatusUpdates = new StatusUpdates();
            allUserStatusUpdates.GetAllUserStatusUpdates(UserAccountID);

            foreach (StatusUpdate su1 in allUserStatusUpdates)
            {
                Acknowledgements.DeleteStatusAcknowledgements(su1.StatusUpdateID);
                var statcoms = new StatusComments();
                statcoms.GetAllStatusCommentsForUpdate(su1.StatusUpdateID);

                StatusUpdateNotifications.DeleteNotificationsForStatusUpdate(su1.StatusUpdateID);

                foreach (StatusComment sc1 in statcoms)
                {
                    StatusCommentAcknowledgements.DeleteStatusCommentAcknowledgements(sc1.StatusCommentID);
                    sc1.Delete();
                }
            }


            // delete the comment Acknowledgements
            StatusCommentAcknowledgements.DeleteAllCommentAcknowledgements(UserAccountID);

            // all the user comments
            StatusComments.DeleteStatusCommentsForUser(UserAccountID);


            // delete the statuses
            StatusUpdates.DeleteAllStatusUpdates(UserAccountID);

            var pitms = new PhotoItems();
            pitms.GetUserPhotos(UserAccountID);

            foreach (var pitm1 in pitms)
            {
                if (!string.IsNullOrWhiteSpace(s3.AccessKeyID) &&
                    !string.IsNullOrWhiteSpace(s3.SecretAccessKey) &&
                    !string.IsNullOrWhiteSpace(pitm1.FilePathRaw))
                {
                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm1.FilePathRaw))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm1.FilePathRaw);
                    }
                }

                if (!string.IsNullOrWhiteSpace(s3.AccessKeyID) &&
                    !string.IsNullOrWhiteSpace(s3.SecretAccessKey) &&
                    !string.IsNullOrWhiteSpace(pitm1.FilePathStandard))
                {
                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm1.FilePathStandard))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm1.FilePathStandard);
                    }
                }

                if (!string.IsNullOrWhiteSpace(s3.AccessKeyID) &&
                    !string.IsNullOrWhiteSpace(s3.SecretAccessKey) &&
                    !string.IsNullOrWhiteSpace(pitm1.FilePathThumb))
                {
                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm1.FilePathThumb))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm1.FilePathThumb);
                    }
                }
                pitm1.Delete();
            }

            DirectMessages.DeleteAllDirectMessages(UserAccountID);

            ProfileLogs.DeleteProfileLog(UserAccountID);
            UserAccountVideos.DeleteUserAccountVideo(UserAccountID);
            Votes.DeleteUserAccountVideo(UserAccountID);
            UserAccountRole.DeleteUserRoles(UserAccountID);

            var uabs = new BlockedUsers();
            uabs.GetBlockedUsers(UserAccountID);

            foreach (var uab1 in uabs)
            {
                uab1.Delete();
            }

            ContestVideoVotes.DeleteAllUserContestVotes(UserAccountID);

            var plyslt = new Playlist();
            plyslt.GetUserPlaylist(UserAccountID);
            var uavs = new PlaylistVideos();

            uavs.GetPlaylistVideosForPlaylistAll(plyslt.PlaylistID);

            foreach (var plv1 in uavs)
            {
                plv1.Delete();
            }

            plyslt.Delete();


            //delete their files folder
            string mainPath = "~/content/users/" + UserAccountID;

            if (Directory.Exists(HttpContext.Current.Server.MapPath(mainPath)))
            {
                try
                {
                    Directory.Delete(HttpContext.Current.Server.MapPath(mainPath));
                }
                catch
                {
                }
            }

            var roles = Roles.GetRolesForUser(UserName);

            foreach (var role1 in roles)
            {
                Roles.RemoveUserFromRole(UserName, role1);
            }

            // TODO: REMOVE CONSTRAINT ON NEWS CONTENT, IT NEEDS TO STAY OWNED BY SITE

            return Delete();
        }


        public void GetUserAccountByEmail(string email)
        {
            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetUserAccountByEmail";
            var param = comm.CreateParameter();
            param.ParameterName = "@email";
            param.Value = email.Trim();
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            
            var dt = DbAct.ExecuteSelectCommand(comm);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public static bool IsUserOnline(int userAccountID)
        {
            if (userAccountID == 0) return false;

            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_IsUserOnline";
            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return Convert.ToBoolean(DbAct.ExecuteScalar(comm));
        }

        public static UserAccount GetNewestUserUserAccount()
        {
            var uas = new UserAccounts();

            uas.GetNewestUsers();

            return uas.Count > 0 ? uas[0] : null;
        }

        #region non-db properties

        public bool IsProfileLinkNewWindow { get; set; }

        #endregion
    }

    public class UserAccounts : List<UserAccount>, IGetAll, ICacheName, IUnorderdList
    {
        private bool _includeStartAndEndTags = true;
        private int LookedAtCount { get; set; }

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }


        public string ToUnorderdList
        {
            get
            {
                if (Count == 0) return string.Empty;

                var sb = new StringBuilder(100);

                if (IncludeStartAndEndTags) sb.Append(@"<ul class=""user_list"">");

                foreach (var ua in this)
                {
                    sb.Append(ua.ToUnorderdListItem);
                }

                if (IncludeStartAndEndTags) sb.Append(@"</ul>");

                return sb.ToString();
            }
        }

        public int GetListUsers(int pageNumber, int resultSize,
                                int? ageFrom, int? ageTo, int? interestedInID,
                                int? relationshipStatusID, int? youAreID, string country,
                                string postalcode, string lang, out bool sortByDistance)
        {
            string currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());


            var longLat = new SiteStructs.LatLong();

            if (!string.IsNullOrWhiteSpace(postalcode) &&
                !string.IsNullOrWhiteSpace(country))
            {
                longLat = GeoData.GetLatLongForCountryPostal(country, postalcode);
            }

            sortByDistance = false;

            if (longLat.longitude != 0 && longLat.latitude != 0)
            {
                sortByDistance = true; // sorting by distance or activity
            }

            var sb = new StringBuilder(100);


            var whereCondition = new StringBuilder(100);

            if (ageFrom != null || ageTo != null || interestedInID != null || relationshipStatusID != null ||
                youAreID != null || !string.IsNullOrEmpty(country) || !string.IsNullOrEmpty(postalcode) ||
                !string.IsNullOrEmpty(lang))
            {
                var alFilters = new ArrayList();

                if (ageFrom != null && ageTo != null)
                {
                    DateTime yearFrom = DateTime.UtcNow.AddYears(-Convert.ToInt32(ageFrom));
                    DateTime yearTo = DateTime.UtcNow.AddYears(-Convert.ToInt32(ageTo));
                    alFilters.Add(string.Format(@" birthDate BETWEEN '{1}' AND '{0}' ",
                                                yearFrom.ToString("yyyy-MM-dd HH':'mm':'ss"),
                                                yearTo.ToString("yyyy-MM-dd HH':'mm':'ss")));
                }

                if (interestedInID != null)
                {
                    alFilters.Add(string.Format(@" interestedInID = {0} ", Convert.ToInt32(interestedInID)));
                }

                if (relationshipStatusID != null)
                {
                    alFilters.Add(string.Format(@" relationshipStatusID = {0} ", Convert.ToInt32(relationshipStatusID)));
                }

                if (youAreID != null)
                {
                    alFilters.Add(string.Format(@" youAreID = {0} ", Convert.ToInt32(youAreID)));
                }

                if (!sortByDistance && !string.IsNullOrEmpty(country))
                {
                    alFilters.Add(string.Format(@" country = '{0}' ", country.Substring(0, 2)));
                }

                if (!string.IsNullOrEmpty(lang))
                {
                    alFilters.Add(string.Format(@" [defaultLanguage] = '{0}' ", lang.Substring(0, 2)));
                }

                if (sortByDistance)
                {
                    alFilters.Add(string.Format(@" showOnMap = {0} ", "1"));
                    alFilters.Add(@" latitude IS NOT NULL AND longitude IS NOT NULL ");
                }

                whereCondition.Append(" WHERE ");

                int i = 0;

                foreach (string filter in alFilters)
                {
                    i++;

                    whereCondition.AppendFormat(i == alFilters.Count ? " {0} " : " {0} AND ", filter);
                }
            }

            if (sortByDistance)
            {
                sb.AppendFormat(@" 

DECLARE  @PageIndex INT = {0}
DECLARE  @PageSize INT = {1}

SET NOCOUNT ON; 
SELECT ROW_NUMBER() OVER 
( ORDER BY dbo.[DistanceBetween]({2}, {3}, latitude, longitude)   ASC  ) AS RowNumber 
                 ,[userAccountID]
                ,[youAreID]
                ,[relationshipStatusID]
                ,[interestedInID]
                ,[longitude]
                ,[latitude]
                ,[birthDate]
                ,[defaultLanguage]
                ,[isOnline]
                ,[lastActivityDate]
                ,[country]
                ,dbo.[DistanceBetween]({2}, {3}, latitude, longitude)  AS Distance
INTO #Results
FROM [vwUserSearchFilter] 
{4}
 

SELECT  COUNT(*) as 'totalResults'
FROM #Results

SELECT * FROM #Results
WHERE RowNumber BETWEEN(@PageIndex -1)
* @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1

DROP TABLE #Results

 ", pageNumber, resultSize, longLat.latitude, longLat.longitude, whereCondition);
            }
            else
            {
                sb.AppendFormat(@" 

DECLARE  @PageIndex INT = {0}
DECLARE  @PageSize INT = {1}

SET NOCOUNT ON; 
SELECT ROW_NUMBER() OVER 
( ORDER BY lastActivityDate DESC  ) AS RowNumber 
                 ,[userAccountID]
                ,[youAreID]
                ,[relationshipStatusID]
                ,[interestedInID]
                ,[longitude]
                ,[latitude]
                ,[birthDate]
                ,[defaultLanguage]
                ,[isOnline]
                ,[lastActivityDate]
                ,[country]
               
INTO #Results
FROM [vwUserSearchFilter] 
{2}
 

SELECT  COUNT(*) as 'totalResults'
FROM #Results

SELECT * FROM #Results
WHERE RowNumber BETWEEN(@PageIndex -1)
* @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1

DROP TABLE #Results

 ", pageNumber, resultSize, whereCondition);
            }

            var totalResults = 0;
            var comm = DbAct.CreateCommand(true);
            comm.CommandText = sb.ToString();
            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                totalResults = FromObj.IntFromObj(ds.Tables[0].Rows[0]["totalResults"]);

                foreach (var ua in from DataRow dr in ds.Tables[1].Rows select new UserAccount(FromObj.IntFromObj(dr["userAccountID"])))
                {
                    Add(ua);
                }
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

            return totalResults;
        }

        /// <summary>
        /// Get the top people from acknowledgements on statuses and comments
        /// </summary>
        public void GetMostApplaudedLastDays(int daysBack = 7)
        {

            var comm = DbAct.CreateCommand(true);

            comm.CommandText = string.Format( @"   
   select top 7  createdbyuserid, sum(total) total from (
SELECT  sc.createdbyuserid
     ,count(*) total
  FROM  [StatusCommentAcknowledgement] sca inner join StatusComment sc on sca.statuscommentid = sc.statuscommentid
  where acknowledgementtype = 'A'
  and sc.createdate between dateadd(day,  -{0}, GETUTCDATE()) and getutcdate()
    group by sc.createdbyuserid union all
    SELECT  su.createdbyuserID
     ,count(*) total
  FROM  Acknowledgement ack INNER join statusupdate su on ack.statusupdateid = su.statusupdateid
    where acknowledgementtype = 'A'
  and ack.createdate between dateadd(day,  -{0}, GETUTCDATE()) and getutcdate()

    group by su.createdbyuserid

)  x group by  createdbyuserid
order by total desc
", daysBack);

            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var ua in from DataRow dr in dt.Rows select new UserAccount(FromObj.IntFromObj(dr["createdbyuserid"])))
            {
                Add(ua);
            }
        }

        public void GetNewestUsers()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNewestUsers";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (UserAccount ua in from DataRow dr in dt.Rows select new UserAccount(dr))
            {
                Add(ua);
            }
        }


        public void GetWhoIsOffline(bool fillList)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetWhoIsOffline";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount ua = null;
                foreach (DataRow dr in dt.Rows)
                {
                    ua = new UserAccount(dr);

                    if (fillList) Add(ua);
                    else ua.RemoveCache();
                }
            }
        }


        private static void GetWhoIsOffline()
        {
            var uas = new UserAccounts();

            uas.GetWhoIsOffline(false);
        }


        public static int GetUserCount()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserCount";

            // execute the stored procedure
            return Convert.ToInt32(DbAct.ExecuteScalar(comm));
        }


        public static int GetOnlineUserCount()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetOnlineUserCount";

            // execute the stored procedure

            var rslt = DbAct.ExecuteScalar(comm);

            return string.IsNullOrWhiteSpace(rslt) ? 0 : Convert.ToInt32(rslt);
        }


        private void GetMostLookedAtUsersDays(int daysBack, int total)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostLookedAtUsersDays";

            comm.AddParameter("daysBack", daysBack);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);


            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;
            foreach (DataRow dr in dt.Rows)
            {
                var user = new UserAccount(FromObj.IntFromObj(dr["lookedAtUserAccountID"]));
                LookedAtCount = FromObj.IntFromObj(dr["count"]);
                if (Count == total) break;
                Add(user);
            }
        }


        public void GetMostLookedAtUsersDays(int daysBack)
        {
            GetMostLookedAtUsersDays(daysBack, 4);
        }


        public static void UpdateWhoIsOnline()
        {
            GetWhoIsOffline(); // update cache

            // set the DB

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateWhoIsOnline";

            // execute the stored procedure
            DbAct.ExecuteNonQuery(comm);
        }


        public void GetActiveUsers(string filter)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetActiveUsersFilter";

            comm.AddParameter("gender", filter);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);


            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new UserAccount(dr);
                    Add(art);
                }
            }
        }

        public void GetActiveUsers()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetActiveUsers";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new UserAccount(dr);
                    Add(art);
                }
            }
        }


        public void GetOnlineUsers()
        {
            //if (HttpRuntime.Cache[this.CacheName] == null)
            //{
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetOnlineUsers";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new UserAccount(dr);
                    Add(art);
                }

                // HttpRuntime.Cache.AddObjToCache(this, this.CacheName);
            }
        }

        public void GetMappableUsers()
        {
            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetMappableUsers";

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (var art in from DataRow dr in dt.Rows select new UserAccount(dr))
                    {
                        Add(art);
                    }

                    HttpRuntime.Cache.AddObjToCache(this, CacheName);
                }
            }
            else
            {
                var uads = (UserAccounts) HttpRuntime.Cache[CacheName];

                foreach (UserAccount uad in uads) Add(uad);
            }
        }

        #region IGetAll Members

        public void GetAll()
        {
            //if (HttpRuntime.Cache[this.CacheName] == null)
            //{
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllUserAccounts";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new UserAccount(dr);
                    Add(art);
                }
            }
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}map", GetType().FullName); }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion
    }
}