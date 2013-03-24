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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.BaseTypes;
using BootBaronLib.Configs;
using BootBaronLib.DAL;
using BootBaronLib.Enums;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Values;
using LitS3;


namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserAccount : BaseIUserLogCRUD, BootBaronLib.Interfaces.ICacheName, IUnorderdListItem, IURLTo
    {
        #region BOLAction Members

        /// <summary>
        /// INSERT the enduser in the database
        /// </summary>
        public override int Create()
        {
            if (this.UserName == string.Empty || EMail == string.Empty) return 0;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserAccount";
            // create a new parameter
            
            ADOExtenstion.AddParameter(comm, "username",  this.UserName);
            ADOExtenstion.AddParameter(comm, "eMail",  this.EMail);
            ADOExtenstion.AddParameter(comm, "password", this.Password);
            ADOExtenstion.AddParameter(comm, "passwordFormat", this.PasswordFormat);
            ADOExtenstion.AddParameter(comm, "passwordSalt", this.PasswordSalt);
            ADOExtenstion.AddParameter(comm, "passwordQuestion", this.PasswordQuestion);
            ADOExtenstion.AddParameter(comm, "passwordAnswer", this.PasswordAnswer);
            ADOExtenstion.AddParameter(comm, "isApproved", IsApproved);
            ADOExtenstion.AddParameter(comm, "isOnLine", this.IsOnLine);
            ADOExtenstion.AddParameter(comm, "comment", this.Comment);
            ADOExtenstion.AddParameter(comm, "isLockedOut",   IsLockedOut);
            ADOExtenstion.AddParameter(comm, "LastPasswordChangedDate",  DateTime.UtcNow);
            ADOExtenstion.AddParameter(comm, "ipAddress",   HttpContext.Current.Request.UserHostAddress);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.UserAccountID = Convert.ToInt32(result);

            return this.UserAccountID;
        }

        /// <summary>
        /// UPDATE the enduser in the database
        /// </summary>
        public override bool Update()
        {
            if (UserAccountID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserAccount";
            // create a new parameter

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IpAddress),  HttpContext.Current.Request.UserHostAddress);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), this.UserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserName), this.UserName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.EMail), this.EMail);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Password), this.Password);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PasswordFormat) , this.PasswordFormat);
            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.PasswordSalt) ,  this.PasswordSalt);
            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.PasswordQuestion) , this.PasswordQuestion);
            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.PasswordAnswer) , this.PasswordAnswer);
            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.FailedPasswordAttemptCount) ,  this.FailedPasswordAttemptCount);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FailedPasswordAttemptWindowStart)  , this.FailedPasswordAttemptWindowStart);
            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.IsApproved)    ,  IsApproved);
            ADOExtenstion.AddParameter(comm,   StaticReflection.GetMemberName<string>(x => this.Comment)  , this.Comment);
            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.IsLockedOut) ,  IsLockedOut);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FailedPasswordAnswerAttemptCount)  , this.FailedPasswordAnswerAttemptCount);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FailedPasswordAnswerAttemptWindowStart), this.FailedPasswordAnswerAttemptWindowStart);

            if (LastLoginDate == DateTime.MinValue)
            {
                LastLoginDate = new DateTime(1900, 1, 1);
            }

            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.LastLoginDate) ,  this.LastLoginDate);
     
            DbParameter param = comm.CreateParameter();
            // isOnLine
            param = comm.CreateParameter();
            param.ParameterName = "@isOnLine";

            // something isn't working
            if (this.SigningOut)
            {
                param.Value = false;
            }
            //else if (this.LastActivityDate > DateTime.UtcNow.AddMinutes(-10))
            //{
            //    param.Value = true;
            //}
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
        /// Delete the user and their data
        /// </summary>
        /// <returns>if they were deleted</returns>
        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserAccount";
            // create a new parameter

            ADOExtenstion.AddParameter(comm,  StaticReflection.GetMemberName<string>(x => this.UserAccountID) ,  UserAccountID);

            RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override void Get(int userAccountID)
        {
            this.UserAccountID = userAccountID;

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetUserAccountFromID";
                // create a new parameter
                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), UserAccountID);
                
                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    }
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
            try
            {
                if (dr == null) return;

                this.Comment = FromObj.StringFromObj(dr["Comment"]);
                this.CreateDate = FromObj.DateFromObj(dr["CreateDate"]);
                this.EMail = FromObj.StringFromObj(dr["eMail"]);
                this.PasswordSalt = FromObj.StringFromObj(dr["PasswordSalt"]);
                this.UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                this.FailedPasswordAttemptCount = FromObj.IntFromObj(dr["FailedPasswordAttemptCount"]);
                this.FailedPasswordAttemptWindowStart = FromObj.DateFromObj(dr["FailedPasswordAttemptWindowStart"]);
                this.FailedPasswordAnswerAttemptCount = FromObj.IntFromObj(dr["FailedPasswordAnswerAttemptCount"]);
                this.FailedPasswordAnswerAttemptWindowStart = FromObj.DateFromObj(dr["FailedPasswordAnswerAttemptWindowStart"]);
                this.IsApproved = FromObj.BoolFromObj(dr["IsApproved"]);
                this.IsLockedOut = FromObj.BoolFromObj(dr["IsLockedOut"]);
                this.IsOnLine = FromObj.BoolFromObj(dr["IsOnLine"]);
                this.LastActivityDate = FromObj.DateFromObj(dr["LastActivityDate"]);
                this.LastLockoutDate = FromObj.DateFromObj(dr["LastLockoutDate"]);
                this.LastLoginDate = FromObj.DateFromObj((dr["LastLoginDate"]));
                this.LastPasswordChangedDate = FromObj.DateFromObj(dr["LastPasswordChangeDate"]);
                this.Password = FromObj.StringFromObj(dr["password"]);
                this.PasswordAnswer = FromObj.StringFromObj(dr["passwordAnswer"]);
                this.PasswordFormat = FromObj.StringFromObj(dr["passwordFormat"]);
                this.PasswordQuestion = FromObj.StringFromObj(dr["passwordQuestion"]);
                this.UserName = FromObj.StringFromObj(dr["userName"]);
                this.IpAddress = FromObj.StringFromObj(dr["ipAddress"]);
            }
            catch { }
        }

        #endregion

        #region Constructors

        public UserAccount() {/*do nothing constructor*/ }

        /// <summary>
        /// Fill a new UserAccount object from the username
        /// </summary>
        /// <param name="username">the user to logging in or logged in</param>
        /// <returns>the UserAccount</returns>
        public UserAccount(string username)
        {
            this.UserName = username;

            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetUserAccountFromUsername";

                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserName), this.UserName);
                
                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }
        }
    


        /// <summary>
        /// Get the user from their id or membership user provider key
        /// </summary>
        /// <param name="userAccountID"></param>
        public UserAccount(int userAccountID)
        {
            Get(userAccountID);
        }

        /// <summary>
        /// Get the user from the membership user
        /// </summary>
        /// <param name="mu"></param>
        public UserAccount(MembershipUser mu)
        {
            if (mu != null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();

                ADOExtenstion.AddParameter(comm,  "UserAccountID",  Convert.ToInt32(mu.ProviderUserKey));

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    Get(dt.Rows[0]);
                }
            }
        }

        public UserAccount(DataRow dr)
        {
            Get(dr);
        }
        #endregion

        #region private variables

        private int _UserAccountID = 0;
        private string _username = string.Empty;
        private string _eMail = string.Empty;
        private string _password = string.Empty;
        private string _passwordFormat = string.Empty;
        private string _passwordSalt = string.Empty;
        private string _passwordQuestion = string.Empty;
        private string _passwordAnswer = string.Empty;
        private DateTime _createDate = DateTime.MinValue;
        private DateTime _lastLoginDate = DateTime.MinValue;
        private DateTime _lastLockoutDate = DateTime.MinValue;
        private int _failedPasswordAttemptCount = 0;
        private DateTime _failedPasswordAttemptWindowStart = DateTime.MinValue;
        private int _failedPasswordAnswerAttemptCount = 0;
        private DateTime _failedPasswordAnswerAttemptWindowStart = DateTime.MinValue;
        private bool _isOnLine = false;
        private bool _isApproved = false;
        private string _comment = string.Empty;
        private bool _isLockedOut = false;
        private DateTime _lastActivityDateTime = DateTime.MinValue;
        private DateTime _lastPasswordChangedDateTime = DateTime.MinValue;
        private string _ipAddress = string.Empty;

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
            get {
                if (_failedPasswordAttemptWindowStart == DateTime.MinValue)
                {
                    _failedPasswordAttemptWindowStart = new DateTime(1900, 1, 1);
                }
                
                return _failedPasswordAttemptWindowStart; }
            set { _failedPasswordAttemptWindowStart = value; }
        }

        public int FailedPasswordAnswerAttemptCount
        {
            get { return _failedPasswordAnswerAttemptCount; }
            set { _failedPasswordAnswerAttemptCount = value; }
        }

        public DateTime FailedPasswordAnswerAttemptWindowStart
        {
            get {

                if (_failedPasswordAnswerAttemptWindowStart == DateTime.MinValue)
                {
                    _failedPasswordAnswerAttemptWindowStart = new DateTime(1900, 1, 1);
                }
                
                
                
                return _failedPasswordAnswerAttemptWindowStart; }
            set { _failedPasswordAnswerAttemptWindowStart = value; }
        }

        public DateTime LastPasswordChangedDate
        {
            get { return _lastPasswordChangedDateTime; }
            set { _lastPasswordChangedDateTime = value; }
        }

        public DateTime LastActivityDate
        {
            get { 
                return _lastActivityDateTime; }
            set { _lastActivityDateTime = value; }
        }

        public bool IsLockedOut
        {
            get { return _isLockedOut; }
            set { _isLockedOut = value; }
        }

        public bool IsApproved
        {
            get { return _isApproved; }
            set { _isApproved = value; }
        }

        /// <summary>
        /// A user is considered online if the current 
        /// date and time minus the UserIsOnlineTimeWindow 
        /// property value is earlier than the LastActivityDate for the user. 
        /// 
        /// The LastActivityDate for a user is updated to the current date and 
        /// time by the CreateUser, UpdateUser and ValidateUser methods, 
        /// and can be updated by some of the overloads of the GetUser method. 
        /// </summary>
        /// <see>http://msdn.microsoft.com/en-us/library/system.web.security.membershipuser.isonline.aspx</see>
        public bool IsOnLine
        {
            get
            {


                return _isOnLine;
            }
            set { _isOnLine = value; }
        }

        public int FailedPasswordAttemptCount
        {
            get { return _failedPasswordAttemptCount; }
            set { _failedPasswordAttemptCount = value; }
        }

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
            get {
                if (_passwordQuestion == null)
                    _passwordQuestion = string.Empty;
                return _passwordQuestion; }
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
            get
            { return _eMail; }
            set { _eMail = value; }
        }

        public string UserName
        {
            get {
                if (_username != null)
                {
                    _username = _username.Trim();
                }
                return _username; }
            set { _username = value; }
        }

        public int UserAccountID
        {
            get { return _UserAccountID; }
            set { _UserAccountID = value; }
        }

        public string Comment
        {
            get {
                if (_comment == null) _comment = string.Empty;

                return _comment; }
            set { _comment = value; }
        }

        #endregion

        #region public static methods

        /// <summary>
        /// Get the user from their e-mail
        /// </summary>
        /// <param name="eMail"></param>
        /// <param name="iseMail"></param>
        public static string GetUserAccountNameFromEMail(string eMail)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAccountnameFromEMail";
            
            ADOExtenstion.AddParameter(comm, "eMail", eMail.Trim());

            string username = DbAct.ExecuteScalar(comm);
            return username;
        }

        public static bool IsAccountIPTaken(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress)) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsAccountIPTaken";

            ADOExtenstion.AddParameter(comm, "ipAddress", ipAddress.Trim());

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }

        /// <summary>
        /// Add a user to a role
        /// </summary>
        /// <param name="UserAccountID"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>   
        public static bool AddUserToRole(int UserAccountID, string roleName)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            ADOExtenstion.AddParameter(comm,  "UserAccountID", UserAccountID);
            ADOExtenstion.AddParameter(comm,  "roleName",  roleName);

            int result = Convert.ToInt32(DbAct.ExecuteScalar(comm));
            if (result == 0)
            { return false; }
            else { return true; }
        }

        /// <summary>
        /// Remove a user from a role
        /// </summary>
        /// <param name="UserAccountID"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool DeleteUserFromRole(int UserAccountID, string roleName)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            ADOExtenstion.AddParameter(comm,  "UserAccountID",  UserAccountID);
            ADOExtenstion.AddParameter(comm,  "roleName",  roleName);

            int result = Convert.ToInt32(DbAct.ExecuteScalar(comm));
            if (result == 0)
            { return false; }
            else { return true; }
        }

        /// <summary>
        /// Get all of the roles for this user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>string array</returns>
        public static string[] GetRolesForUser(string username)
        {
            UserAccount eu = new UserAccount(username);

            ArrayList allRoles = new ArrayList();
            DataTable dt;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRolesForUser";
            // create a new parameter
            
            ADOExtenstion.AddParameter(comm,  "UserAccountID", eu.UserAccountID);
            
            // exec
            dt = DbAct.ExecuteSelectCommand(comm);
            foreach (DataRow r in dt.Rows)
            {
                allRoles.Add(FromObj.StringFromObj(r["roleName"]));
            }
            string[] stringArray = (string[])allRoles.ToArray(typeof(string));
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
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNewestPhotoUploader";

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
                Get(FromObj.IntFromObj(dt.Rows[0]["userAccountID"]));
        }


        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}-{1}-{2}", this.GetType().FullName ,
                 this.UserName, this.UserAccountID.ToString()); }
        }
 
        public void RemoveCache()
        {
            if (HttpContext.Current == null) return;

            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);

            // remove username cache
            int useraccountID = this.UserAccountID;
            this.UserAccountID = 0;

            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);

            // remove user account id cache
            string username = this.UserName;
            this.UserName = string.Empty;
            this.UserAccountID = useraccountID;

            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);

            this.UserName = username;
            this.UserAccountID = useraccountID;
        }

        #endregion


        public string TinyUserIcon
        {
            get
            {
                UserAccountDetail uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(this.UserAccountID);

                StringBuilder sb = new StringBuilder(100);

                sb.AppendFormat(@"<img style=""width:20px;height:20px;"" title=""{0}"" alt=""{0}"" src=""", this.UserName);
                sb.Append(uad.FullProfilePicThumbURL);
                sb.Append(@""" />");

                return sb.ToString();
            }

        }

        public bool IsAdmin
        {
            get
            {
                string[] roles = UserAccount.GetRolesForUser(this.UserName);

                foreach (string role in roles)
                {
                    if (role.ToLower() == SiteEnums.RoleTypes.admin.ToString()) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// DELETES THE USER COMPLETELY!
        /// </summary>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public bool Delete(bool deleteAllRelatedData)
        {

            if (!deleteAllRelatedData) return this.Delete();

            S3Service s3 = new S3Service();

            s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
            s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;


            UserAccountDetail uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(this.UserAccountID);
            uad.Delete();

            if (!string.IsNullOrWhiteSpace( s3.AccessKeyID) &&
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
             
            UserConnection usc = new UserConnection();
            usc.UserAccountID = this.UserAccountID;
            usc.Delete(true);

            ContentComments conComs = new ContentComments();

            conComs.GetUserContentComments(this.UserAccountID);

            foreach (ContentComment concom1 in conComs)
            {
                concom1.Delete();
            }

            // user wall
            WallMessages.DeleteUserWall(this.UserAccountID);

            UserAddress uaddress = new UserAddress();
            uaddress.UserAccountID = this.UserAccountID;
            uaddress.Delete();

            UserPhotos uphos = new UserPhotos();
            uphos.GetUserPhotos(this.UserAccountID);

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

                if (!string.IsNullOrWhiteSpace(s3.AccessKeyID) &&
                !string.IsNullOrWhiteSpace(s3.SecretAccessKey) && 
                !string.IsNullOrWhiteSpace(up1.FullProfilePicURL))
                {
                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, up1.FullProfilePicURL))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, up1.FullProfilePicURL);
                    }
                }

            }

            // delete any acknowledgements they made
            Acknowledgements.DeleteAllAcknowledgements(this.UserAccountID);


            // get all status updates and delete all the acknowledgements for each
            StatusUpdates allUserStatusUpdates = new StatusUpdates();
            allUserStatusUpdates.GetAllUserStatusUpdates(this.UserAccountID);

            foreach (StatusUpdate su1 in allUserStatusUpdates)
            {
                Acknowledgements.DeleteStatusAcknowledgements(su1.StatusUpdateID);
                StatusComments statcoms = new StatusComments();
                statcoms.GetAllStatusCommentsForUpdate(su1.StatusUpdateID);

                StatusUpdateNotifications.DeleteNotificationsForStatusUpdate(su1.StatusUpdateID);

                foreach (StatusComment sc1 in statcoms)
                {
                    StatusCommentAcknowledgements.DeleteStatusCommentAcknowledgements(sc1.StatusCommentID);
                    sc1.Delete();
                }
            }


            // delete the comment Acknowledgements
            StatusCommentAcknowledgements.DeleteAllCommentAcknowledgements(this.UserAccountID);

            // all the user comments
            StatusComments.DeleteStatusCommentsForUser(this.UserAccountID);


            // delete the statuses
            StatusUpdates.DeleteAllStatusUpdates(this.UserAccountID);

            PhotoItems pitms = new PhotoItems();
            pitms.GetUserPhotos(this.UserAccountID);

            foreach (PhotoItem pitm1 in pitms)
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

            DirectMessages.DeleteAllDirectMessages(this.UserAccountID);

            ProfileLogs.DeleteProfileLog(this.UserAccountID);
            UserAccountVideos.DeleteUserAccountVideo(this.UserAccountID);
            Votes.DeleteUserAccountVideo(this.UserAccountID);
            UserAccountRole.DeleteUserRoles(this.UserAccountID);

            BlockedUsers uabs = new BlockedUsers();
            uabs.GetBlockedUsers(this.UserAccountID);

            foreach (BlockedUser uab1 in uabs)
            {
                uab1.Delete();
            }
  
            ContestVideoVotes.DeleteAllUserContestVotes(this.UserAccountID);

            Playlist plyslt = new Playlist();
            plyslt.GetUserPlaylist(this.UserAccountID);
            PlaylistVideos uavs = new PlaylistVideos();

            uavs.GetPlaylistVideosForPlaylistAll(plyslt.PlaylistID);

            foreach (PlaylistVideo plv1 in uavs)
            {
                plv1.Delete();
            }

            plyslt.Delete();


            //delete their files folder
            string mainPath = "~/content/users/" + this.UserAccountID.ToString();

            if (Directory.Exists(HttpContext.Current.Server.MapPath(mainPath)))
            {
                try
                {
                    Directory.Delete(HttpContext.Current.Server.MapPath(mainPath));
                }
                catch { }
            }

            string[] roles = Roles.GetRolesForUser(this.UserName);

            foreach (string role1 in roles)
            {
                Roles.RemoveUserFromRole(this.UserName, role1);
            }

            UserContent.Contents contents = new UserContent.Contents();

            contents.GetContentForUser(this.UserAccountID);

            foreach (Content c1 in contents)
            {
                c1.Delete();
            }

            return this.Delete();
        }


        public void GetUserAccountByEmail(string email)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserAccountByEmail";
            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@email";
            param.Value = email;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        #region non-db properties

      

        private bool _isProfileLinkNewWindow = false;

        public bool IsProfileLinkNewWindow
        {
            get { return _isProfileLinkNewWindow; }
            set { _isProfileLinkNewWindow = value; }
        }

        #endregion

        private bool _signingOut = false;

        public bool SigningOut
        {
            get { return _signingOut; }
            set { _signingOut = value; }
        }


        public static bool IsUserOnline(int userAccountID)
        {
            if (userAccountID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsUserOnline";
 
            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

            // execute the stored procedure
            return Convert.ToBoolean( DbAct.ExecuteScalar(comm) );
        }


        public string FlagIcon
        {
            get
            {
                UserAccountDetail uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(this.UserAccountID);

                return uad.CountryFlagThumb;

            }

        }



        public string CountryName
        {
            get
            {
                UserAccountDetail uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(this.UserAccountID);

                return uad.CountryName;

            }

        }



        public string Country 
        {
            get
            {
                UserAccountDetail uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(this.UserAccountID);

                return uad.Country;

            }

        }


        

        public string ToUnorderdListItem
        {
            get {        
            
                UserAccountDetail uad = new UserAccountDetail();

                uad.GetUserAccountDeailForUser(this.UserAccountID);

                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<li>");


                if (this.IsProfileLinkNewWindow)
                {
                    string fullIcon = uad.SmallUserIcon.Replace("<a ", @"<a  target=""_blank"" ");
                    sb.Append(fullIcon);
                }
                else
                {
                    sb.Append(uad.SmallUserIcon);
                }
                //sb.Append(@"<div class=""user_item_thumb"">");
 

                //if (this.IsOnLine)
                //{
                //    sb.AppendFormat(
                //    @"<img style=""height: 12px; width: 12px;"" alt=""{0}"" title=""{0}""", BootBaronLib.Resources.Messages.IsOnline);
                //    sb.Append(@" src=""");
                //    sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/content/images/status/abutton2_e0.gif"));
                //    sb.Append(@""" />&nbsp;");
                //}

                //sb.Append(@"<a ");

                //if (this.IsProfileLinkNewWindow)
                //{
                //    sb.Append(@" target=""_blank""");
                //}

                //sb.Append(@" href=""/");
                //sb.Append(this.UserName);
                //sb.Append(@""">");
                //sb.Append(this.UserName);
                //sb.Append(@"</a>");
 

                //sb.Append(@"<br />");
 

                //sb.Append(@"<div class=""user_photo_thumb"">");

                //sb.Append(@"<a class=""m_over""");

                //if (this.IsProfileLinkNewWindow)
                //{
                //    sb.Append(@" target=""_blank""");
                //}
                //sb.Append(@" href=""/");
                //sb.Append(this.UserName);
                //sb.Append(@""">");
                //sb.Append(@"<img src=""");
                //sb.Append(uad.FullProfilePicThumbURL);
                //sb.Append(@""" title=""");
                //sb.Append(this.UserName);
                //sb.Append(@"""");
                //sb.Append(@" alt=""");
                //sb.Append(uad.Sex);
                //sb.Append(@""" />");
                //sb.Append(@"</a>");
                //sb.Append(@" ");
                //sb.AppendLine(uad.SiteBagesSmall);

            

                //sb.Append(@"</div>");
 
               
                //sb.Append(@"</div>");
                sb.Append(@"</li>");


                return sb.ToString();
            }
        }

        public static UserAccount GetNewestUserUserAccount()
        {
            UserAccounts uas = new UserAccounts();

            uas.GetNewestUsers();

            if (uas.Count > 0)
            {
                //TODO: this is a waste here, the results are greater than one
                return uas[0];
            }
            else return null;
        }

        public Uri UrlTo
        {
            get
            {
                return new Uri(Utilities.URLAuthority() + System.Web.VirtualPathUtility.ToAbsolute(
                    string.Format("~/{0}", this.UserName)));
            }
        }
    }

    public class UserAccounts : List<UserAccount>, IGetAll, ICacheName, IUnorderdList
    {


        public int GetListUsers(int pageNumber, int resultSize,
            int? ageFrom, int? ageTo, int? interestedInID,
            int? relationshipStatusID, int? youAreID, string country,
            string postalcode, string lang, out bool sortByDistance)
        {
            string currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
 
          
            SiteStructs.LatLong longLat = new SiteStructs.LatLong();

            if (!string.IsNullOrWhiteSpace(postalcode) &&
                !string.IsNullOrWhiteSpace(country))
            {
                longLat = GeoData.GetLatLongForCountryPostal(country, postalcode);
            }

            sortByDistance = false;

            if (longLat.longitude != 0 && longLat.latitude != 0)
            {
                sortByDistance = true;// sorting by distance or activity
            } 

            StringBuilder sb = new StringBuilder(100);


            StringBuilder whereCondition = new StringBuilder(100);

            if (ageFrom != null || ageTo != null || interestedInID != null || relationshipStatusID != null ||
                youAreID != null || !string.IsNullOrEmpty(country) || !string.IsNullOrEmpty(postalcode) ||
                !string.IsNullOrEmpty(lang))
            {


                ArrayList alFilters = new ArrayList();

                if (ageFrom != null && ageTo != null)
                {
                    DateTime yearFrom = DateTime.UtcNow.AddYears(-Convert.ToInt32(ageFrom));
                    DateTime yearTo = DateTime.UtcNow.AddYears(-Convert.ToInt32(ageTo));
                    alFilters.Add(string.Format(@" birthDate BETWEEN '{1}' AND '{0}' ", yearFrom.ToString("yyyy-MM-dd HH':'mm':'ss"), yearTo.ToString("yyyy-MM-dd HH':'mm':'ss")));
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

                if (!sortByDistance &&  !string.IsNullOrEmpty(country))
                {
                    alFilters.Add(string.Format(@" country = '{0}' ", country.Substring(0, 2)));
                }

                if (  !string.IsNullOrEmpty(lang))
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

                    if (i == alFilters.Count)
                    {
                        whereCondition.AppendFormat(" {0} ", filter);
                    }
                    else
                    {
                        whereCondition.AppendFormat(" {0} AND ", filter);
                    }
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

 ", pageNumber, resultSize, longLat.latitude.ToString(), longLat.longitude.ToString(), whereCondition.ToString());

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

 ", pageNumber, resultSize , whereCondition.ToString());
            }
           
       
            int totalResults = 0;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand(true);

            // set the stored procedure name
            comm.CommandText = sb.ToString();
            //comm.CommandType = CommandType.Text;

            // execute the stored procedure
            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                totalResults = FromObj.IntFromObj(ds.Tables[0].Rows[0]["totalResults"]);

                UserAccount ua = null;

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    ua = new UserAccount(FromObj.IntFromObj(dr["userAccountID"]));
                    this.Add(ua);
                }
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);


            return totalResults;
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
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount ua = null;
                foreach (DataRow dr in dt.Rows)
                {
                    ua = new UserAccount(dr);

                    this.Add(ua);
                }
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

                    if (fillList) this.Add(ua);
                    else ua.RemoveCache();
                }
            }
        }








        public static void GetWhoIsOffline()
        {
            UserAccounts uas = new UserAccounts();

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

            string rslt = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrWhiteSpace(rslt)) return 0;
            else
                return Convert.ToInt32(rslt);
        }




        public UserAccounts() { }


        private int _lookedAtCount = 0;

        public int LookedAtCount
        {
            get { return _lookedAtCount; }
            set { _lookedAtCount = value; }
        }

        public void GetMostLookedAtUsersDays(int daysBack, int total)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostLookedAtUsersDays";

            ADOExtenstion.AddParameter(comm, "daysBack", daysBack);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);


            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount user = null;
                foreach (DataRow dr in dt.Rows)
                {
                    user = new UserAccount(FromObj.IntFromObj(dr["lookedAtUserAccountID"]));
                    this.LookedAtCount = FromObj.IntFromObj(dr["count"]);
                    if (this.Count == total) break;
                    this.Add(user);
                }
            }
        }







        public void GetMostLookedAtUsersDays(int daysBack)
        {
            GetMostLookedAtUsersDays(daysBack, 4);
        }



        public static void UpdateWhoIsOnline()
        {
            UserAccounts.GetWhoIsOffline();// update cache

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

            ADOExtenstion.AddParameter(comm, "gender", filter);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);


            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccount art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new UserAccount(dr);
                    this.Add(art);
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
                    this.Add(art);
                }
            }
        }



        public void  GetOnlineUsers()
        {

            //if (HttpContext.Current.Cache[this.CacheName] == null)
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
                    this.Add(art);
                }

                // HttpContext.Current.Cache.AddObjToCache(this, this.CacheName);
            }
        }

        #region IGetAll Members

        public void GetAll()
        {

            //if (HttpContext.Current.Cache[this.CacheName] == null)
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
                    this.Add(art);
                }

                // HttpContext.Current.Cache.AddObjToCache(this, this.CacheName);
            }
            //}
            //else
            //{
            //    UserAccounts uads = (UserAccounts)HttpContext.Current.Cache[this.CacheName];

            //    foreach (UserAccount uad in uads) this.Add(uad);
            //}
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}map", this.GetType().FullName); }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }


        #endregion

        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }


        public string ToUnorderdList
        {
            get
            {
                if (this.Count == 0) return string.Empty;

                StringBuilder sb = new StringBuilder(100);

                if (IncludeStartAndEndTags) sb.Append(@"<ul class=""user_list"">");

                foreach (UserAccount ua in this)
                {
                    sb.Append(ua.ToUnorderdListItem);
                }

                if (IncludeStartAndEndTags) sb.Append(@"</ul>");

                return sb.ToString();
            }
        }

        public void GetMappableUsers()
        {

            if (HttpContext.Current.Cache[this.CacheName] == null)
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
                    UserAccount art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new UserAccount(dr);
                        this.Add(art);
                    }

                    HttpContext.Current.Cache.AddObjToCache(this, this.CacheName);
                }


            }
            else
            {
                UserAccounts uads = (UserAccounts)HttpContext.Current.Cache[this.CacheName];

                foreach (UserAccount uad in uads) this.Add(uad);
            }


        }
    }
}
