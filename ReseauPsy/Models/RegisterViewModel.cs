using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "Register_Error_EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "Register_Error_EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Register_Label_Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Register_Error_PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "Register_Error_PasswordLength", ErrorMessageResourceType = typeof(Resource) , MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Register_Label_PasswordConfirmation", ResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceName = "Registrer_Error_PasswordsNoMatch", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }
}