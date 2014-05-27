using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.Resources;

namespace DasKlub.Web.Models
{

    #region Models

    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Messages), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Messages), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "PasswordsDoNotMatch",
            ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Messages), Name = "Password")]
        public string Password { get; set; }


        [Display(ResourceType = typeof (Messages), Name = "KeepMeLoggedIn")]
        public bool RememberMe { get; set; }
    }


    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [StringLength(15, ErrorMessageResourceName = "UserNameBetween4and15Characters", MinimumLength = 4
            , ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "UserName")]
        [RegularExpression(@"[A-Za-z][A-Za-z0-9_]{3,14}", ErrorMessageResourceName =
            "IncorrectFormat", ErrorMessageResourceType = typeof (Messages))]
        public virtual string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [RegularExpression(@".+\@.+\..+", ErrorMessageResourceName =
            "IncorrectFormat", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "EMail")]
        public string Email { get; set; }


        [Display(ResourceType = typeof (Messages), Name = "NewPassword")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [StringLength(15, ErrorMessageResourceName = "PasswordBetween4and15Characters",
            MinimumLength = 4, ErrorMessageResourceType = typeof (Messages))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Month")]
        public string Month { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Day")]
        public string Day { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Year")]
        public string Year { get; set; }


        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "YouAre")]
        public int? YouAreID { get; set; }


        public string RefUser { get; set; }
    }

    #endregion

    #region Services

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthenticationService
    {
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    #endregion

    #region Validation

    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return Messages.AlreadyInUse;

                case MembershipCreateStatus.DuplicateEmail:
                    return Messages.EMail;

                case MembershipCreateStatus.InvalidPassword:
                    return Messages.Password;

                case MembershipCreateStatus.InvalidEmail:
                    return Messages.EMail;

                case MembershipCreateStatus.InvalidAnswer:
                    return Messages.IncorrectPasswordAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return Messages.PasswordQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return Messages.UserName;

                case MembershipCreateStatus.ProviderError:
                    return Messages.ProviderName;

                case MembershipCreateStatus.UserRejected:
                    return Messages.Failure;

                default:
                    return Messages.Error;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
                                                                               ControllerContext context)
        {
            return new[]
                {
                    new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()),
                                                              _minCharacters, int.MaxValue)
                };
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                                 name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }

    #endregion
}