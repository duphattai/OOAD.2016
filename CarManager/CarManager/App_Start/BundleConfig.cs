﻿using System.Web;
using System.Web.Optimization;

namespace CarManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                          "~/Scripts/jquery-{version}.js",
                          "~/Scripts/bootstrap.js",
                          "~/Scripts/bootstrap-confirmation.js",
                          "~/Scripts/jquery.unobtrusive-ajax.js",
                          "~/Scripts/skrollr.min.js"
                          ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                        "~/Scripts/select2/select2.js",
                        "~/Scripts/select2/vi.js",
                        "~/Scripts/admin.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap-datetimepicker.js"
                        ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/select2.css",
                      "~/Content/admin.css",
                      "~/Content/report.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-datetimepicker.css"
                      ));

        }
    }
}
