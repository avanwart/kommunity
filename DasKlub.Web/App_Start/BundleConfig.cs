using System.Web.Optimization;

namespace DasKlub.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR-{version}.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/jsfooter_desktop").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/scrollpagination.js",
                "~/content/script/site_wide_13.js",
                "~/content/mediaelement/mediaelement-and-player.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/jsfooter_mobile").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/scrollpagination.js",
                "~/content/script/site_wide_13.js",
                "~/content/mediaelement/mediaelement-and-player.js"
                            ));

            bundles.Add(new StyleBundle("~/content/style/css_head2").Include(
                "~/content/style/cyborg_theme.css",
                "~/content/bootstrap-responsive.css",
                "~/content/style/jquery-ui-{version}.custom.css",
                "~/content/style/flag_sprites.css",
                "~/content/style/site_spec_01.css"
                            ));

            bundles.Add(new StyleBundle("~/content/mediaelement/css_mediaelement").Include(
                "~/content/mediaelement/mediaelementplayer.css"
                            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}