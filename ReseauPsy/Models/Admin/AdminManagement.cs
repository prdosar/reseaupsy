using Business;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class AdminManagement
    {
        public int AdminId { get; set; }

        public string Title
        {
            get
            {
                return AdminId != 0 ?
                    ReseauPsy.Resources.Resource.DetailsAdmin_Title_EditAdmin :
                    ReseauPsy.Resources.Resource.DetailsAdmin_Title_CreateAdmin;
            }
        }

        [Required(ErrorMessageResourceName = "DetailsAdmin_Error_NameRequired", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "DetailsAdmin_Label_AdminName", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "DetailsAdmin_Error_EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "DetailsAdmin_Error_EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "DetailsAdmin_Label_Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceName = "DetailsAdmin_Error_PasswordNoMatch", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "DetailsAdmin_Label_ConfirmPassword", ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }

        public List<AdminRole> AdminRoles { get; set; }

        [Required(ErrorMessageResourceName = "DetailsAdmin_Error_RoleRequired", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "DetailsAdmin_Label_Role", ResourceType = typeof(Resource))]
        public string SelectedRole { get; set; }

        public AdminManagement()
        {
            AdminRoles = new List<AdminRole>();
        }

    }

    public class AdminRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}