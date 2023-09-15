using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace ReseauPsy.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Global))]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Global))]
        [Display(Name = "Email", ResourceType = typeof(Global))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Global))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Global))]
        public string Password { get; set; }

        [Display(Name = "Login_RememberMe", ResourceType = typeof(Resource))]
        public bool RememberMe { get; set; }

        public string ErrorMessage = "";

        public LoginViewModel()
        {

        }

        public LoginViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public LoginViewModel(LoginViewModel loginViewModel, string errorMessage)
        {
            Email = loginViewModel.Email;
            Password = loginViewModel.Password;
            RememberMe = loginViewModel.RememberMe;
            ErrorMessage = errorMessage;
        }

        public static string accountLocked = Resource.Login_Error_AccountLocked;
        public static string invalidConnexion = Resource.Login_Error_ConnectionFailed; 
    }

}