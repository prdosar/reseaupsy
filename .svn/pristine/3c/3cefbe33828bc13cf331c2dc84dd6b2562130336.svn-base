using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Global))]
        [Display(Name = "Email", ResourceType = typeof(Global))]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Global))]
        public string Email { get; set; }
        public string SuccessMessage { get; set; }

        public ForgotPasswordViewModel()
        {
            SuccessMessage = "";
        }

        public ForgotPasswordViewModel(string message)
        {
            SuccessMessage = message;
        }

        public static string EmailSent = Resource.ForgotPassword_Success_Message; 
    }
}