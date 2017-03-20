using System.Web.Optimization;

namespace DentalSoft.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            RegisterScriptBundles(bundles);
            RegisterStyleBundles(bundles);

            BundleTable.EnableOptimizations = false;
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.cosmo.min.css"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                  "~/Content/kendo/kendo.common.min.css",
                        "~/Content/kendo/kendo.common-bootstrap.min.css",
                        "~/Content/Kendo/kendo.default.min.css"));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Patients").Include(
                        "~/Content/Views/teethStatus.css",
                        "~/Content/Widgets/imageSlider.css",
                        "~/Content/Plugin/zoom.css",
                        "~/Content/Plugin/glisse.css"));
        }

        private static void RegisterScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                        "~/Scripts/kendo/kendo.all.min.js",
                          "~/Scripts/kendo/kendo.aspnetmvc.min.js",
                          "~/Scripts/Kendo/kendo.timezones.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                        .Include("~/Scripts/kendo/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));        

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/Widgets").Include(
                     "~/Scripts/Widgets/AjaxSubmit.js"));

            bundles.Add(new ScriptBundle("~/bundles/Patients").Include(
                 "~/Scripts/Views/Patients/PatientView.js",
                "~/Scripts/Widgets/teethChartView.js",
                "~/Scripts/Widgets/imageSlider.js",
                "~/Scripts/velocity.min.js",
                "~/Scripts/Plugin/Zooming/enhance.js",
                "~/Scripts/Plugin/Gallery/glisse.js",
                "~/Scripts/Views/Patients/PatientModelView.js",
                "~/Scripts/Views/Patients/TeethStatusView.js",
                "~/Scripts/Views/Patients/StatusDetailView.js",
                "~/Scripts/Views/Patients/ImageGalleryDetailView.js",
                "~/Scripts/Views/Patients/PersonalDataView.js",
                 "~/Scripts/Views/Patients/FinancialFlanView.js"));
        }
    }
}
