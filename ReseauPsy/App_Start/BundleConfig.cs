using System.Web;
using System.Web.Optimization;

namespace ReseauPsy
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/_assets/vendors/jquery-3.4.1/jquery-3.4.1.js",
                        "~/_assets/vendors/bootstrap-5.1.3-dist/js/bootstrap.bundle.js",
                        "~/_assets/vendors/toastr-2.1.3/toastr.min.js",
                        "~/_assets/vendors/jquery-ui-1.12.1/jquery-ui.js",
                        "~/_assets/vendors/jquery-ui-1.12.1/datepicker-fr-CA.js",
                        "~/_assets/vendors/fullcalendar-5.9.0/main.js",
                        "~/_assets/vendors/fullcalendar-5.9.0/locales-all.js",
                        "~/_assets/vendors/moment/moment-with-locales.min.js",
                        "~/_assets/vendors/jquery-mask-1.14.15/jquery.mask.js",
                        "~/_assets/vendors/Typeahead/typeahead.bundle.js",
                        "~/_assets/vendors/ApexCharts-v3.31.0/apexcharts.js",
                        "~/_assets/vendors/bootstrap-select-1.14.0-beta2/bootstrap-select.min.js",
                        "~/_assets/js/Site.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/_assets/vendors/jquery-validate-1.17.0/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/_assets/vendors/modernizr-2.8.3/modernizr-2.8.3.js"));

            //bundles.Add(new Bundle("~/bundles/bootstrap").Include(
            //          ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/_assets/vendors/bootstrap-5.1.3-dist/css/bootstrap.min.css",
                      "~/_assets/vendors/toastr-2.1.3/toastr.min.css",
                      "~/_assets/vendors/jquery-ui-1.12.1/jquery-ui.css",
                      "~/_assets/vendors/fontawesome-free-6.0.0-beta2-web/css/all.css",
                      "~/_assets/vendors/fullcalendar-5.9.0/main.css",
                      "~/_assets/vendors/Typeahead/typeahead.css",
                      "~/_assets/vendors/bootstrap-select-1.14.0-beta2/bootstrap-select.min.css",
                      "~/_assets/css/style.css"
                      ));
            BundleTable.EnableOptimizations = false;

        }
    }
}
