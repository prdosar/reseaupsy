using CustomUrl.AppCode;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LocalizationAttribute("fr-ca"));
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());
        }
    }
}
