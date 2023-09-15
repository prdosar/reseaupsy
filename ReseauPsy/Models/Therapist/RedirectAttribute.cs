using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;


namespace ReseauPsy.Models.Therapist
{
    public class RedirectAttribute : ActionFilterAttribute
    {

        private ReseauPsyEntities _context;

        public RedirectAttribute()
        {
            this._context = new ReseauPsyEntities();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            var aspnetUserId = HttpContext.Current.User.Identity.GetUserId();

            var therapist = _context.Therapists
                .Where(x => x.AspNetUserId == aspnetUserId)
                .Single();

            string actionName = filterContext.ActionDescriptor.ActionName;

            var isInscriptionCompleted = _context.VerifiyIfTherapistInscriptionCompleted(therapist.Id)
                .First().IsInscriptionCompleted;


            //If the profile is incomplete, we redirect to my profile and dont allow other action other than profile and modify profile
            if (!isInscriptionCompleted && actionName.ToLower() != "myprofile" && actionName.ToLower() != "modifyprofile")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Therapist",
                    action = "MyProfile"
                }));
            }
        }
    }
}