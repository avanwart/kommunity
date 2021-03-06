﻿// needed
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using log4net;
using log4net.Config;

namespace DasKlub.Web
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


        public void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            XmlConfigurator.Configure();

            Log.Info("Application Started");

            Application[SiteEnums.ApplicationVariableNames.LogError.ToString()] = true;

            TimerStarter.StartTimer();
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterGlobalFilters(GlobalFilters.Filters);

            DefaultModelBinder.ResourceClassKey = "Messages";
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString[SiteEnums.QueryStringNames.language.ToString()]) ||
                User == null) return;
            MembershipUser mu = Membership.GetUser();

            if (mu == null) return;
            var uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

            string language = Request.QueryString[SiteEnums.QueryStringNames.language.ToString()];

            uad.DefaultLanguage = language;
            uad.Update();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetNoServerCaching(); //.SetMaxAge(TimeSpan.FromDays(366));

            string browserLanguage = GeneralConfigs.DefaultLanguage;

            if (Request.UserLanguages != null && Request.UserLanguages.Length > 0)
            {
                browserLanguage = Request.UserLanguages[0];
            }

            string language = GeneralConfigs.DefaultLanguage;

            if (HttpContext.Current == null) return;
            if (!string.IsNullOrWhiteSpace(Request.QueryString[SiteEnums.QueryStringNames.language.ToString()]))
            {
                language = Request.QueryString[SiteEnums.QueryStringNames.language.ToString()];

                var nvc = new NameValueCollection {{SiteEnums.CookieValue.language.ToString(), language}};

                Utilities.CookieMaker(SiteEnums.CookieName.Usersetting, nvc);
            }
            else if (Request.Cookies[SiteEnums.CookieName.Usersetting.ToString()] != null)
            {
                HttpCookie hc = Request.Cookies[SiteEnums.CookieName.Usersetting.ToString()];

                if (hc != null)
                {
                    language = hc[SiteEnums.CookieValue.language.ToString()];
                }
            }
            else if (!string.IsNullOrWhiteSpace(browserLanguage))
            {
                language = browserLanguage.Substring(0, 2);

                bool isImplmented =
                    Enum.GetValues(typeof (SiteEnums.SiteLanguages))
                        .Cast<SiteEnums.SiteLanguages>()
                        .Any(possibleLang => possibleLang.ToString() == language.ToUpper());

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
            Exception exception = Server.GetLastError();
            var httpException = exception as HttpException;

            //Utilities.LogError(httpException);

            //Log.Error("Application Error", httpException);

            //Response.Clear();
            //Server.ClearError();
            //var routeData = new RouteData();
            //routeData.Values["controller"] = "Errors";
            //routeData.Values["action"] = "General";
            //routeData.Values["exception"] = exception;
            //Response.StatusCode = 500;

            //if (httpException != null)
            //{
            //    Response.StatusCode = httpException.GetHttpCode();
            //    switch (Response.StatusCode)
            //    {
            //        case 403:
            //            routeData.Values["action"] = "Http403";
            //            break;
            //        case 404:
            //            routeData.Values["action"] = "Http404";
            //            break;
            //    }
            //}


            //// Avoid IIS7 getting in the middle
            //Response.TrySkipIisCustomErrors = true;
            //IController errorsController = new ErrorsController();
            //var wrapper = new HttpContextWrapper(Context);
            //var rc = new RequestContext(wrapper, routeData);
            //errorsController.Execute(rc);
        }


        private static void ClearBadVideos()
        {
            var vids = new Videos();

            vids.GetAll();

            foreach (Video vv1 in from vv1 in vids
                where vv1.IsEnabled
                let sss = Utilities.GETRequest(new Uri(
                    string.Format("http://i3.ytimg.com/vi/{0}/1.jpg",
                        vv1.ProviderKey)), true)
                where sss != null
                where !Convert.ToBoolean(sss)
                select vv1)
            {
                vv1.IsEnabled = false;
                vv1.Update();
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
                _threadingTimer = new Timer(CheckData, HttpContext.Current, 0, GeneralConfigs.PostInterval);
            }
        }

        private static void CheckData(object sender)
        {
            //db
            UserAccounts.UpdateWhoIsOnline();

            var chatters = new ChatRoomUsers();
            chatters.GetChattingUsers();

            foreach (ChatRoomUser chatUser in from chatUser in chatters
                let user = new UserAccount(chatUser.CreatedByUserID)
                where !user.IsOnLine
                select chatUser)
            {
                chatUser.DeleteChatRoomUser();
            }
        }
    }
}