using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Helper
{
    /// <summary>
    /// Class Used to modify the identity profile of a user
    /// </summary>
    public class ModifyIdentityUser
    {
        /// <summary>
        /// Change the Email of the user
        /// </summary>
        /// <param name="aspNetUserId">The AspnetUser Id</param>
        /// <param name="newEmail">The new enail</param>
        public static void ModifyEmail(string aspNetUserId, string newEmail)
        {
            ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var identityUser = userManager.FindById(aspNetUserId);
            userManager.SetEmail(identityUser.Id, newEmail);
            identityUser.UserName = newEmail;
            userManager.Update(identityUser);
        }

    }
}