using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy.Controllers
{
    public class GeneralController : Controller
    {
        // GET: General
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermsOfUse()
        {
            return View();
        }


    }
}