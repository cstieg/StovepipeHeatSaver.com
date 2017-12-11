﻿using System.Web;
using System.Web.Optimization;

namespace StovepipeHeatSaver
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/Site/site.js",
                      "~/Scripts/Site/ShoppingCart.js",
                      "~/Scripts/Site/RandomId.js",
                      "~/Scripts/Site/ImageUpload.js",
                      "~/Scripts/Site/LightboxMessage.js",
                      "~/Scripts/Site/EditIndex.js"));

            bundles.Add(new ScriptBundle("~/bundles/nicedit").Include(
                      "~/Scripts/NicEdit/nicEdit.js"));

            bundles.Add(new ScriptBundle("~/bundles/sortable").Include(
                      "~/Scripts/Sortable/sortable.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/EditIndex.css",
                      "~/Content/site.css",
                      "~/Content/LightboxMessage.css"));
        }
    }
}
