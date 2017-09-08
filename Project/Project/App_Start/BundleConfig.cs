using System.Web;
using System.Web.Optimization;

namespace Project
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include(
                    "~/Scripts/bootstrap-datetimepicker.js",
                    "~/Scripts/bootstrap.min.js",
                    "~/Scripts/jquery-1.12.4.min.js",
                    "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/site")
                .Include("~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                    "~/Content/bootstrap.min.css",
                    "~/Content/bootstrap-datetimepicker.min.css",
                    "~/Content/site.css"
                ));
        }
    }
}
