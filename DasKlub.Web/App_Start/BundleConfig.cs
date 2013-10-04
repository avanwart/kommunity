//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System.Web.Optimization;

namespace DasKlub.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jsfooter_desktop").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/scrollpagination.js",
                //"~/Scripts/textarea_expander.js",
                "~/content/script/site_wide_13.js",
                // "~/scripts/jquery.signalR-{version}.js",
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

            bundles.Add(new StyleBundle("~/Content/style/css_head2").Include(
                "~/content/style/cyborg_theme.css",
                "~/content/bootstrap-responsive.css",
                "~/content/style/jquery-ui-{version}.custom.css",
                "~/content/style/flag_sprites.css",
                "~/content/style/site_spec_01.css"
                            ));

            bundles.Add(new StyleBundle("~/Content/mediaelement/css_mediaelement").Include(
                "~/content/mediaelement/mediaelementplayer.css"
                            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}