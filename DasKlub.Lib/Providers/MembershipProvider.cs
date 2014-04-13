using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.Providers
{
    /// <summary>
    ///     The membership provider for the site
    /// </summary>
    /// <see>http://scottfindlater.blogspot.com/2008/07/asp-net-security-membership-forms.html</see>
    /// <see>http://www.asp.net/learn/videos/video-189.aspx</see>
    internal class DKMembershipProvider : MembershipProvider
    {
        #region members

        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private MachineKeySection _machineKey; // Used when determining encryption key values.
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;
        private MembershipPasswordFormat _passwordFormat;
        private string _passwordStrengthRegularExpression;
        private bool _requiresQuestionAndAnswer;
        private bool _requiresUniqueEmail;

        #endregion

        #region properties

        /// <summary>
        ///     Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>
        ///     true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.
        /// </returns>
        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        /// <summary>
        ///     Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>
        ///     true if the membership provider supports password reset; otherwise, false. The default is true.
        /// </returns>
        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        /// <summary>
        ///     Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>
        ///     true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.
        /// </returns>
        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        /// <summary>
        ///     The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>
        ///     The name of the application using the custom membership provider.
        /// </returns>
        public override string ApplicationName { get; set; }

        /// <summary>
        ///     Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        ///     The number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </returns>
        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        /// <summary>
        ///     Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        ///     The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </returns>
        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        /// <summary>
        ///     Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <returns>
        ///     true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.
        /// </returns>
        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        /// <summary>
        ///     Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="T:System.Web.Security.MembershipPasswordFormat" /> values indicating the format for storing passwords in the data store.
        /// </returns>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }

        /// <summary>
        ///     Gets the minimum length required for a password.
        /// </summary>
        /// <returns>
        ///     The minimum length required for a password.
        /// </returns>
        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        /// <summary>
        ///     Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>
        ///     The minimum number of special characters that must be present in a valid password.
        /// </returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        /// <summary>
        ///     Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>
        ///     A regular expression used to evaluate a password.
        /// </returns>
        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        private string ConnectionString { get; set; }

        #endregion

        #region public methods

        /// <summary>
        ///     Set the properties from the start
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null) throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name)) name = "DKMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "the DK membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "10"));
            _passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "15"));
            _minRequiredNonAlphanumericCharacters =
                Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "0"));
            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _passwordStrengthRegularExpression =
                Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            _requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            _requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

            var temp_format = config["passwordFormat"] ?? "Hashed";

            switch (temp_format)
            {
                case "Hashed":
                    _passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    _passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    _passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Initialize SqlConnection.
            ConnectionStringSettings connectionStringSettings =
                ConfigurationManager.ConnectionStrings[config["connectionStringName"]];
            if (connectionStringSettings == null || connectionStringSettings.ConnectionString.Trim() == string.Empty)
            {
                throw new ProviderException("Connection string cannot be blank.");
            }
            ConnectionString = connectionStringSettings.ConnectionString;

            // Get encryption and decryption key information from the configuration.
            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
            _machineKey = (MachineKeySection) cfg.GetSection("system.web/machineKey");

            if (_machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException(
                        "Hashed or Encrypted passwords are not supported with auto-generated keys.");
        }

        /// <summary>
        ///     Change the user's password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override bool ChangePassword(
            string username,
            string oldPassword,
            string newPassword)
        {
            var args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null) throw args.FailureInformation;
                throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            }

            var eu = new UserAccount(username);

            if (eu.UserAccountID < 1) throw new ProviderException("Change password failed. No unique user found.");

            if (EncodePassword(oldPassword) != eu.Password)
            {
                return false;
            }

            eu.Password = EncodePassword(newPassword);

            try
            {
                return eu.Update();
            }
            catch
            {
                return false;
            }
        }


        public override bool ChangePasswordQuestionAndAnswer
            (
            string username,
            string password,
            string newPasswordQuestion,
            string newPasswordAnswer
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Make a new end user in the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser
            (
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status
            )
        {
            username = username.Trim();
            password = password.Trim();
            email = email.Trim();
            if (passwordQuestion != null)
            {
                passwordQuestion = passwordQuestion.Trim();
            }

            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }

            // Validate username/password
            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            // Check whether user with passed username already exists
            var mu = GetUser(username, false);

            if (mu == null)
            {
                var eu = new UserAccount
                    {
                        UserName = username,
                        EMail = email,
                        Password = EncodePassword(password),
                        PasswordFormat = MembershipPasswordFormat.Hashed.ToString(),
                        PasswordSalt = string.Empty,
                        PasswordQuestion = passwordQuestion,
                        PasswordAnswer = EncodePassword(passwordAnswer),
                        FailedPasswordAttemptCount = 0,
                        IsOnLine = false,
                        IsApproved = isApproved,
                        Comment = string.Empty,
                        IsLockedOut = false,
                    };
                try
                {
                    eu.Create();

                    status = eu.UserAccountID > 0 ? MembershipCreateStatus.Success : MembershipCreateStatus.UserRejected;
                }
                catch
                {
                    status = MembershipCreateStatus.UserRejected;
                }

                return GetUser(username, false);
            }
            status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        /// <summary>
        ///     Delete the given user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var eu = new UserAccount(username);
            return eu.Delete(deleteAllRelatedData);
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the password for the specified user name from the data source.
        /// </summary>
        /// <returns>The password for the specified user name.</returns>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval) throw new ProviderException("Password Retrieval Not Enabled.");

            var password = string.Empty;

            var eu = new UserAccount(username);

            if (eu.UserAccountID < 1)
                throw new ProviderException("Get password failed. No unique user found.");

            if (eu.UserAccountID > 0)
            {
                if (eu.IsLockedOut)
                    throw new MembershipPasswordException("The supplied user is locked out.");
            }
            else throw new MembershipPasswordException("The supplied user name is not found.");

            if (RequiresQuestionAndAnswer && !IsValidPasswordAnswerComparison(answer, eu.PasswordAnswer))
            {
                UpdateFailureCount(username, SiteEnums.MembershipFailureTypes.passwordAnswer);
                throw new MembershipPasswordException("Incorrect password answer.");
            }
            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                //  reset the password
                // password = Utilities.RandomPassword.Generate();
                eu.Password = EncodePassword(password);
                eu.Update();
            }

            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(eu.Password);
            }

            return password;
        }

        /// <summary>
        ///     Get the user by name and online status
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (username == string.Empty)
            {
                return null;
            }

            MembershipUser mu = null;
            var eu = new UserAccount(username);
            if (eu.UserAccountID != 0) // && eu.IsOnLine == userIsOnline)
            {
                eu.Update(); // updates the last activity date
                // get the values for this end user
                mu = GetMembershipUserFromUserAccount(eu);
            }
            return mu;
        }

        /// <summary>
        ///     get the user from their ID
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser mu = null;
            var eu = new UserAccount(Convert.ToInt32(providerUserKey));
            if (eu.UserAccountID != 0 && eu.IsOnLine == userIsOnline)
            {
                // get the values for this end user
                mu = GetMembershipUserFromUserAccount(eu);
            }
            else
            {
                // not sure why it matters if they are online, this isn't properly checked anyway

                // get the values for this end user
                mu = GetMembershipUserFromUserAccount(eu);
            }
            return mu;
        }

        /// <summary>
        ///     Get ther username from the e-mail address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            return UserAccount.GetUserAccountNameFromEMail(email);
        }

        /// <summary>
        ///     Same thing as: GetPassword method here
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval) throw new ProviderException(Messages.PasswordRetrievalNotEnabled);

            string password = RandomPassword.Generate(8, 10); // string.Empty;

            var eu = new UserAccount(username);

            if (eu.UserAccountID < 1)
                throw new ProviderException(Messages.GetPasswordFailedNoUnique);

            if (eu.UserAccountID > 0)
            {
                // this is going to be reset anyway
                //if (eu.IsLockedOut)
                //    throw new MembershipPasswordException("The supplied user is locked out.");
            }
            else throw new MembershipPasswordException(Messages.TheSuppliedUserNameIsNotFound);

            if (RequiresQuestionAndAnswer && !IsValidPasswordAnswerComparison(answer, eu.PasswordAnswer))
            {
                UpdateFailureCount(username, SiteEnums.MembershipFailureTypes.passwordAnswer);
                throw new MembershipPasswordException(Messages.IncorrectPasswordAnswer);
            }
            else
            {
                if (PasswordFormat == MembershipPasswordFormat.Hashed)
                {
                    //  reset the password
                    eu.IsLockedOut = false;
                    eu.FailedPasswordAttemptCount = 0;
                    eu.Password = EncodePassword(password);
                    eu.Update();
                }
            }

            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(eu.Password);
            }

            return password;
        }

        /// <summary>
        ///     Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <returns>true if the membership user was successfully unlocked; otherwise, false.</returns>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        public override bool UnlockUser(string userName)
        {
            try
            {
                var eu = new UserAccount(userName);

                if (eu.UserAccountID < 1 || eu.UserAccountID == 0) return false;

                eu.IsLockedOut = false;
                eu.FailedPasswordAnswerAttemptCount = 0;
                eu.FailedPasswordAttemptCount = 0;
                eu.Update();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Update the underlying end user for this membership user
        /// </summary>
        /// <param name="user"></param>
        public override void UpdateUser(MembershipUser user)
        {
            var eu = new UserAccount(Convert.ToInt32(user.ProviderUserKey));

            if (eu.UserAccountID < 1)
                throw new ProviderException(Messages.UpdateUserFailedNoUniqueUserFound);
            if (eu.UserAccountID == 0)
                return;


            eu.Comment = user.Comment;
            eu.CreateDate = user.CreationDate;
            eu.EMail = user.Email;
            eu.IsApproved = user.IsApproved;
            eu.IsLockedOut = user.IsLockedOut;
            eu.IsOnLine = user.IsOnline;
            eu.LastActivityDate = user.LastActivityDate;
            eu.LastLockoutDate = user.LastLockoutDate;
            eu.LastLoginDate = user.LastLoginDate;
            eu.LastPasswordChangedDate = user.LastPasswordChangedDate;
            eu.PasswordQuestion = user.PasswordQuestion;
            eu.PasswordQuestion = user.ProviderName;
            eu.UserName = user.UserName;

            eu.Update();
        }

        /// <summary>
        ///     Confirms that this person can log in,
        ///     they are approved and not locked out
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            username = username.Trim();
            password = password.Trim();

            if (username == string.Empty || password == string.Empty)
            {
                return false;
            }

            var eu = new UserAccount(username);

            if (eu.UserAccountID == 0) return false;

            if (eu.IsLockedOut) return false;

            if (IsValidPasswordComparison(password, eu.Password))
            {
                //if (eu.IsApproved && eu.IsLockedOut == false)
                if (eu.IsLockedOut == false)
                {
                    isValid = true;
                    eu.IsOnLine = true;
                    eu.Update();
                }
            }
            else
            {
                UpdateFailureCount(username, SiteEnums.MembershipFailureTypes.password);
            }

            return isValid;
        }

        #endregion

        #region private methods

        /// <summary>
        ///     A helper method that performs the checks and updates associated with password failure tracking.
        /// </summary>
        private void UpdateFailureCount(string username, SiteEnums.MembershipFailureTypes failureType)
        {
            var eu = new UserAccount(username);
            if (eu.UserAccountID == 0) throw new ProviderException(Messages.UnableToUpdateFailureCount);

            int failureCount = 0;

            if (failureType == SiteEnums.MembershipFailureTypes.password)
            {
                failureCount = Convert.ToInt32(eu.FailedPasswordAttemptCount);
            }

            if (failureType == SiteEnums.MembershipFailureTypes.passwordAnswer)
            {
                failureCount = Convert.ToInt32(eu.FailedPasswordAnswerAttemptCount);
            }

            if (failureCount == 0) 
            {
                // First password failure or outside of PasswordAttemptWindow. 
                // Start a new password failure count from 1 and a new window starting now.
                if (failureType == SiteEnums.MembershipFailureTypes.password)
                {
                    eu.FailedPasswordAttemptCount = 1;
                    //  eu.FailedPasswordAttemptWindowStart = FromDB.GetUTCDate();
                }
                if (failureType == SiteEnums.MembershipFailureTypes.passwordAnswer)
                {
                    eu.FailedPasswordAnswerAttemptCount = 1;
                    //  eu.FailedPasswordAnswerAttemptWindowStart = FromDB.GetUTCDate();
                }

                try
                {
                    eu.Update();
                }
                catch
                {
                    throw new ProviderException(Messages.UnableToUpdateFailureCountAndWindowStart);
                }
            }
            else
            {
                if (failureCount++ >= MaxInvalidPasswordAttempts)
                {
                    // Max password attempts have exceeded the failure threshold. Lock out the user.
                    eu.IsLockedOut = true;
                    //  eu.LastLockoutDate = FromDB.GetUTCDate();

                    try
                    {
                        eu.Update();
                    }
                    catch
                    {
                        throw new ProviderException(Messages.UnableToLockOutUser);
                    }
                }
                else
                {
                    // Max password attempts have not exceeded the failure threshold. Update
                    // the failure counts. Leave the window the same.
                    if (failureType == SiteEnums.MembershipFailureTypes.password)
                    {
                        eu.FailedPasswordAttemptCount = failureCount;
                    }
                    if (failureType == SiteEnums.MembershipFailureTypes.passwordAnswer)
                    {
                        eu.FailedPasswordAnswerAttemptCount = failureCount;
                    }

                    try
                    {
                        eu.Update();
                    }
                    catch
                    {
                        throw new ProviderException(Messages.UnableToUpdateFailureCount);
                    }
                }
            }
        }

        /// <summary>
        ///     Decrypts or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException(Messages.CannotUnencodeAHashedPassword);
                default:
                    throw new ProviderException(Messages.UnsupportedPasswordFormat);
            }
            return password;
        }

        /// <summary>
        ///     Compares password values based on the MembershipPasswordFormat.
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="dbpassword">database password</param>
        /// <returns>whether the passwords are identical</returns>
        private bool IsValidPasswordComparison(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            return pass1 == pass2;
        }


        /// <summary>
        ///     Check the password anwser against the database
        /// </summary>
        /// <param name="passwordAnswer"></param>
        /// <param name="dbpasswordAnswer"></param>
        /// <returns></returns>
        private bool IsValidPasswordAnswerComparison(string passwordAnswer, string dbpasswordAnswer)
        {
            string pass1 = passwordAnswer;
            string pass2 = dbpasswordAnswer;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpasswordAnswer);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(passwordAnswer);
                    break;
                default:
                    break;
            }

            return pass1 == pass2;
        }


        /// <summary>
        ///     Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncodePassword(string password)
        {
            if (password == null) return string.Empty;

            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    var hash = new HMACSHA1 {Key = HexToByte(_machineKey.ValidationKey)};
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));

                    break;
                default:
                    throw new ProviderException(Messages.UnsupportedPasswordFormat);
            }

            return encodedPassword;
        }

        /// <summary>
        ///     Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length/2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i*2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>
        ///     A helper function that takes the current persistent user and creates a MembershiUser from the values.
        /// </summary>
        /// <param name="user">user object containing the user data retrieved from database</param>
        /// <returns>membership user object</returns>
        private MembershipUser GetMembershipUserFromUserAccount(UserAccount eu)
        {
            return new MembershipUser
                (
                Name,
                eu.UserName,
                eu.UserAccountID,
                eu.EMail,
                eu.PasswordQuestion,
                eu.Comment,
                eu.IsApproved,
                eu.IsLockedOut,
                eu.CreateDate,
                eu.LastLoginDate,
                eu.LastActivityDate,
                eu.LastPasswordChangedDate,
                eu.LastLockoutDate
                );
        }

        /// <summary>
        ///     A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        #endregion
    }
}