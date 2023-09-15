using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Global))]
        [StringLength(100, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(Global), MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])(?=.*\d).{6,}$", ErrorMessageResourceName = "ResetPassword_Error_Requirement", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Global))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirmation", ResourceType = typeof(Global))]
        [Required(ErrorMessageResourceName = "PasswordConfirmationRequired", ErrorMessageResourceType = typeof(Global))]
        [Compare("Password", ErrorMessageResourceName = "PasswordCompare", ErrorMessageResourceType = typeof(Global))]
        public string ConfirmPassword { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public string Code { get; set; }

        public bool IsCodeValid { get; set; }
        public bool IsLinkUsed { get; set; }

        public ResetPasswordViewModel()
        {
            SuccessMessage = "";
            ErrorMessage = "";
            IsCodeValid = true;
        }

        public ResetPasswordViewModel(string successMessage, string errorMessage)
        {
            SuccessMessage = successMessage;
            ErrorMessage = errorMessage;
            IsCodeValid = true;
        }

        public static string PasswordReseted = Resource.ResetPassword_SuccessMessage_PasswordReseted;
        public static string InvalidLink = Resource.ResetPassword_ErrorMessage_InvalidLink;
        public static string LinkAlreadyUsed = Resource.ResetPassword_ErrorMessage_LinkUsed;
        public static string LinkExpired = Resource.ResetPassword_ErrorMessage_LinkExpired;
        public static string FailedToReset = Resource.ResetPassword_ErrorMessage_FailedToReset;
    }

}