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

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Configs;
using BootBaronLib.Operational;
using BootBaronLib.Values;
using DasKlub.App_Start;
using DasKlub.Controllers;
using log4net;
using log4net.Config;
using   Microsoft.AspNet.SignalR ;

namespace DasKlub
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        //Here is the once-per-class call to initialize the log object
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapConnection<MyConnection>("echo", "echo/{*operation}");


            routes.IgnoreRoute("{*favicon}", new {favicon = "(.*/)?favicon.ico(/.*)"});

            #region store

            routes.MapRoute(
                "store_index",
                "store",
                new
                    {
                        controller = "Store",
                        action = "Index",
                    }
                );

            #endregion

            #region video

            routes.MapRoute(
                "live_video_detail",
                "video/cat/{cat}",
                new
                    {
                        controller = "Video",
                        action = "Category",
                        id = ""
                    }
                );


            routes.MapRoute(
                "video_items_post",
                "video/items",
                new
                    {
                        controller = "Video",
                        action = "Items",
                    }
                );

            routes.MapRoute(
                "video_items",
                "video/items/{pageNumber}",
                new
                    {
                        controller = "Video",
                        action = "Items",
                    }
                );

            routes.MapRoute(
                "video_contests",
                "video/contests",
                new
                    {
                        controller = "Video",
                        action = "Contests"
                    }
                );

            routes.MapRoute(
                "video_contest",
                "video/contest/{key}",
                new
                    {
                        controller = "Video",
                        action = "Contest"
                    }
                );

            #endregion

            #region find users

            routes.MapRoute(
                "find_users_items",
                "findusers/items/{pageNumber}",
                new
                    {
                        controller = "FindUsers",
                        action = "Items",
                    }
                );

            routes.MapRoute(
                "find_users_post",
                "findusers/items",
                new
                    {
                        controller = "FindUsers",
                        action = "Items",
                    }
                );

            routes.MapRoute(
                "browse_user_filter",
                "findusers",
                new
                    {
                        controller = "FindUsers",
                        action = "Index",
                        id = ""
                    }
                );

            #endregion

            #region account

            routes.MapRoute(
                "statusupdates_items",
                "account/statusupdates/{pageNumber}",
                new
                    {
                        controller = "Account",
                        action = "StatusUpdates",
                    }
                );


            routes.MapRoute(
                "status_update",
                "account/statusupdate/{statusUpdateID}",
                new
                    {
                        controller = "Account",
                        action = "StatusUpdate"
                    }
                );


            routes.MapRoute(
                "profile_user_visitors",
                "account/profilevisitors/{userName}",
                new
                    {
                        controller = "Account",
                        action = "ProfileVisitors"
                    }
                );


            routes.MapRoute(
                "user_visitors",
                "account/visitors",
                new
                    {
                        controller = "Account",
                        action = "Visitors"
                    }
                );


            routes.MapRoute(
                "user_cyber_contacts",
                "account/CyberAssociates/{userName}",
                new
                    {
                        controller = "Account",
                        action = "CyberAssociates"
                    }
                );


            routes.MapRoute(
                "user_irl_contacts",
                "account/irlcontacts/{userName}",
                new
                    {
                        controller = "Account",
                        action = "IRLContacts"
                    }
                );


            routes.MapRoute(
                "user_wish_list",
                "account/wishlist/{userName}",
                new
                    {
                        controller = "Account",
                        action = "WishList",
                        id = ""
                    }
                );

            //routes.MapRoute(
//"delete_account_photo",
//"account/photodelete/{number}",
//new
//{
//    controller = "Account",
//    action = "PhotoDelete",
//    id = ""
//}
//);

            #endregion

            routes.MapRoute(
                "market_brand",
                "market/brand/{brandKey}",
                new
                    {
                        controller = "Market",
                        action = "Brands",
                        id = ""
                    }
                );


            routes.MapRoute(
                "shopping_cart",
                "market/shoppingcart",
                new
                    {
                        controller = "Market",
                        action = "ShoppingCart",
                        id = ""
                    }
                );


            routes.MapRoute(
                "shopping_cart_removeitem",
                "market/removeitem/{productVariationID}",
                new
                    {
                        controller = "Market",
                        action = "RemoveItem",
                        id = ""
                    }
                );


            routes.MapRoute(
                "content_branding",
                "SiteContent",
                new
                    {
                        controller = "Home",
                        action = "SiteContent"
                    }
                );


            routes.MapRoute(
                "user_list",
                "users",
                new
                    {
                        controller = "Users",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "lesson_index",
                "lessons",
                new
                    {
                        controller = "Lessons",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "contact_index",
                "contact",
                new
                    {
                        controller = "Home",
                        action = "Contact"
                    }
                );

            #region news

            routes.MapRoute(
                "news_index",
                "news",
                new
                    {
                        controller = "News",
                        action = "Index"
                    }
                );

            routes.MapRoute(
                "news_index_lang",
                "news/lang/{lang}",
                new
                    {
                        controller = "News",
                        action = "Lang"
                    }
                );


            routes.MapRoute(
                "news_items_lang",
                "news/langitems/{lang}/{pageNumber}",
                new
                    {
                        controller = "News",
                        action = "LangItems",
                    }
                );


            routes.MapRoute(
                "video_log",
                "news/videolog",
                new
                    {
                        controller = "News",
                        action = "VideoLog"
                    }
                );


            routes.MapRoute(
                "news_items",
                "news/items/{pageNumber}",
                new
                    {
                        controller = "News",
                        action = "Items",
                    }
                );


            routes.MapRoute(
                "news_delete",
                "news/deletecomment",
                new
                    {
                        controller = "News",
                        action = "DeleteComment",
                    }
                );

            routes.MapRoute(
                "news_items_post",
                "news/items",
                new
                    {
                        controller = "News",
                        action = "Items",
                    }
                );


            routes.MapRoute(
                "news_tag",
                "news/tag/{key}",
                new
                    {
                        controller = "News",
                        action = "Tag",
                    }
                );


            routes.MapRoute(
                "news_tag_items_post",
                "news/tagitems/{key}",
                new
                    {
                        controller = "News",
                        action = "TagItems",
                    }
                );

            routes.MapRoute(
                "news_tag_items",
                "news/tagitems/{key}/{pageNumber}",
                new
                    {
                        controller = "News",
                        action = "TagItems",
                    }
                );

            routes.MapRoute(
                "news_detail",
                "news/{key}",
                new
                    {
                        controller = "News",
                        action = "Detail",
                    }
                );

            #endregion

            routes.MapRoute(
                "market_brand_index",
                "market/BrandList",
                new
                    {
                        controller = "Market",
                        action = "BrandList"
                    }
                );

            #region mail

            routes.MapRoute(
                "mail_items",
                "account/mailitems/{pageNumber}",
                new
                    {
                        controller = "Account",
                        action = "MailItems",
                    }
                );

            routes.MapRoute(
                "reply_mail_items",
                "account/replymailitems/{pageNumber}",
                new
                    {
                        controller = "Account",
                        action = "ReplyMailItems",
                    }
                );
            routes.MapRoute(
                "oubtox_mail_items",
                "account/outboxmailitems/{pageNumber}",
                new
                    {
                        controller = "Account",
                        action = "OutboxMailItems",
                    }
                );

            #endregion

            routes.MapRoute(
                "search_results",
                "search",
                new
                    {
                        controller = "Search",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "tour_feeds_index",
                "feeds/tour",
                new
                    {
                        controller = "Feeds",
                        action = "Tour"
                    }
                );


            routes.MapRoute(
                "video_index",
                "video",
                new
                    {
                        controller = "Video",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "buy_index",
                "buy",
                new
                    {
                        controller = "Buy",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "band_index",
                "bands",
                new
                    {
                        controller = "Bands",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "band_filter",
                "video/bands/{firstLetter}",
                new
                    {
                        controller = "Video",
                        action = "Filter"
                    }
                );


            routes.MapRoute(
                "message_reply",
                "account/reply/{userName}",
                new
                    {
                        controller = "Account",
                        action = "Reply"
                    }
                );


            routes.MapRoute(
                "user_filter",
                "video/users/{firstLetter}",
                new
                    {
                        controller = "Video",
                        action = "UserFilter"
                    }
                );


            routes.MapRoute(
                "market_detail",
                "market/{productKey}",
                new {controller = "Market", action = "Detail", id = ""}
                );


            routes.MapRoute(
                "market_index",
                "market",
                new
                    {
                        controller = "Market",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "map_index",
                "map",
                new
                    {
                        controller = "Map",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "radio_index",
                "radio",
                new
                    {
                        controller = "Radio",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "kalendar_index",
                "kalendar",
                new
                    {
                        controller = "Kalendar",
                        action = "Index"
                    }
                );


            routes.MapRoute(
                "live_video",
                "video/live",
                new {controller = "Video", action = "Live", id = ""}
                );


            routes.MapRoute(
                "about_us",
                "about",
                new {controller = "Home", action = "About", id = ""}
                );


            routes.MapRoute(
                "video_detail",
                "video/{videoContext}",
                new {controller = "Video", action = "Detail", id = ""}
                );


            routes.MapRoute(
                "users_handler",
                "Handler",
                new
                    {
                        controller = "Account",
                        action = "Handler"
                    }
                );


            routes.MapRoute(
                "wall_user_detail",
                "{userName}/WallMessages/{pageNumber}",
                new {controller = "Profile", action = "WallMessages"}
                );


            routes.MapRoute(
                "wall_user_delete",
                "{userName}/DeleteWallItem/{wallItemID}",
                new {controller = "Profile", action = "DeleteWallItem"}
                );

            #region photos

            routes.MapRoute(
                "photo_list",
                "photos",
                new
                    {
                        controller = "Photos",
                        action = "Index"
                    }
                );

            routes.MapRoute(
                "photo_list_page",
                "photos/PhotoItems",
                new
                    {
                        controller = "Photos",
                        action = "PhotoItems",
                        id = ""
                    }
                );


            routes.MapRoute(
                "photo_list_page2",
                "photos/PhotoItems/{pageNumber}",
                new
                    {
                        controller = "Photos",
                        action = "PhotoItems",
                    }
                );


            routes.MapRoute(
                "photo_item",
                "photos/{photoItemID}",
                new
                    {
                        controller = "Photos",
                        action = "Detail",
                        id = ""
                    }
                );


            routes.MapRoute(
                "photo_delete",
                "photos/delete/{photoItemID}",
                new
                    {
                        controller = "Photos",
                        action = "Delete",
                        id = ""
                    }
                );

            #endregion

            routes.MapRoute(
                "user_news",
                "{userName}/news",
                new
                    {
                        controller = "Profile",
                        action = "UserNews"
                    }
                );


            routes.MapRoute(
                "user_photos",
                "{userName}/userphotos",
                new
                    {
                        controller = "Profile",
                        action = "UserPhotos"
                    }
                );


            routes.MapRoute(
                "user_photo_item",
                "{userName}/userphoto/{photoItemID}",
                new
                    {
                        controller = "Profile",
                        action = "UserPhoto"
                    }
                );


            // dispays the user profile
            routes.MapRoute(
                "user_detail",
                "{userName}",
                new {controller = "Profile", action = "ProfileDetail"}
                );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }


        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            Log.Info("Application Started");

            if (GeneralConfigs.EnableVideoCheck)
            {
                ClearBadVideos();
            }

            Application[SiteEnums.ApplicationVariableNames.LogError.ToString()] = true;

            TimerStarter.StartTimer();

            AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DefaultModelBinder.ResourceClassKey = "Messages";

            //System.Threading.Timer ChatRoomsCleanerTimer =
            //    new System.Threading.Timer(
            //        new TimerCallback(ChatEngine.CleanChatRooms), null, 1200000, 1200000);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString[SiteEnums.QueryStringNames.language.ToString()]) &&
                User != null)
            {
                MembershipUser mu = Membership.GetUser();

                if (mu != null)
                {
                    var uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

                    string language = Request.QueryString[SiteEnums.QueryStringNames.language.ToString()];

                    uad.DefaultLanguage = language;
                    uad.Update();
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetNoServerCaching(); //.SetMaxAge(TimeSpan.FromDays(366));

            var browserLanguage = GeneralConfigs.DefaultLanguage;

            if (Request.UserLanguages != null && Request.UserLanguages.Length > 0)
            {
                browserLanguage = Request.UserLanguages[0];
            }

            var language = GeneralConfigs.DefaultLanguage;

            if (HttpContext.Current == null) return;
            if (!string.IsNullOrWhiteSpace(Request.QueryString[SiteEnums.QueryStringNames.language.ToString()]))
            {
                language = Request.QueryString[SiteEnums.QueryStringNames.language.ToString()];

                var nvc = new NameValueCollection {{SiteEnums.CookieValue.language.ToString(), language}};

                Utilities.CookieMaker(SiteEnums.CookieName.Usersetting, nvc);
            }
            else if (Request.Cookies[SiteEnums.CookieName.Usersetting.ToString()] != null)
            {
                var hc = Request.Cookies[SiteEnums.CookieName.Usersetting.ToString()];

                if (hc != null)
                {
                    language = hc[SiteEnums.CookieValue.language.ToString()];
                }
            }
            else if (!string.IsNullOrWhiteSpace(browserLanguage))
            {
                language = browserLanguage.Substring(0, 2);

                var isImplmented = Enum.GetValues(typeof (SiteEnums.SiteLanguages)).Cast<SiteEnums.SiteLanguages>().Any(possibleLang => possibleLang.ToString() == language.ToUpper());

                if (!isImplmented)
                {
                    language = GeneralConfigs.DefaultLanguage;
                }

                var nvc = new NameValueCollection {{SiteEnums.CookieValue.language.ToString(), language}};

                Utilities.CookieMaker(SiteEnums.CookieName.Usersetting, nvc);
            }
            else
            {
                language = GeneralConfigs.DefaultLanguage;
            }


            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;

            Utilities.LogError(httpException);

            Log.Error("Application Error", httpException);

            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;

            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }


            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            IController errorsController = new ErrorsController();
            var wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);
            errorsController.Execute(rc);
        }


        private static void ClearBadVideos()
        {
            var vids = new Videos();

            vids.GetAll();

            foreach (var vv1 in from vv1 in vids where vv1.IsEnabled let sss = Utilities.GETRequest(new Uri(
string.Format("http://i3.ytimg.com/vi/{0}/1.jpg",
vv1.ProviderKey)), true) where sss != null where !Convert.ToBoolean(sss) select vv1)
            {
                vv1.IsEnabled = false;
                vv1.Update();
            }
        }

        private abstract class MyConnection : PersistentConnection
        {
            protected Task OnReceivedAsync(string clientId, string data)
            {
                // Broadcast data to all clients
                return Connection.Broadcast(data);
            }
        }
    }

    /// <summary>
    ///     http://weblogs.asp.net/muhanadyounis/archive/2009/01/12/global-timer-background-timer.aspx
    /// </summary>
    public static class TimerStarter
    {
        private static Timer _threadingTimer;

        public static void StartTimer()
        {
            if (null == _threadingTimer)
            {
                _threadingTimer = new Timer(CheckData,
                                           HttpContext.Current, 0, GeneralConfigs.PostInterval);
            }
        }

        private static void CheckData(object sender)
        {
            //db
            UserAccounts.UpdateWhoIsOnline();
        }
    }
}