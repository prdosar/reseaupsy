using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy.Controllers
{
    [Authorize(Roles = RoleName.AdminFullAccess + "," + RoleName.Therapist)]
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var result = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().ChangePassword(
                User.Identity.GetUserId(),
                oldPassword,
                newPassword);

            if (result.Succeeded)
            {
                var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(User.Identity.GetUserId());
                return Json(new { success = true });
            }
            else if (result.Errors.First() == "Incorrect password.")
            {
                return Json(new { success = "wrong password" });
            }

            throw new Exception();
        }

    }
}