using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CustomUrl.AppCode
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        private string _DefaultLanguage = "fr-ca";

        public LocalizationAttribute(string defaultLanguage)
        {
            _DefaultLanguage = defaultLanguage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string lang = (string)filterContext.RouteData.Values["lang"] ?? _DefaultLanguage;

            //Set the culture to canada by default
            switch (lang)
            {
                case "fr": lang = "fr-ca";
                        break;

                case "en": lang = "en-ca";
                    break;

                default:
                    lang = "fr-ca";
                    break;
            }

            try
            {
                Thread.CurrentThread.CurrentCulture =
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            catch (Exception e)
            {
                throw new NotSupportedException(String.Format("ERROR: Invalid language code '{0}'.", lang));
            }
        }
    }
}