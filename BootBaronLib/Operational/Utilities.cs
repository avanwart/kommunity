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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DasKlub.Lib.AppSpec.DasKlub.BLL;
using DasKlub.Lib.AppSpec.DasKlub.BOL.Logging;
using DasKlub.Lib.Configs;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;
using HtmlAgilityPack;
using log4net;
using Content = Amazon.SimpleEmail.Model.Content;

namespace DasKlub.Lib.Operational
{
    /// <summary>
    ///     Performs operations
    ///     (calculations, error logging, e-mail sending, input validation, image file IO/resizing, JavaScript
    ///     help, encrytion and more)
    /// </summary>
    public class Utilities
    {
        //Here is the once-per-class call to initialize the log object
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region spam filter

        public static bool IsSpamIP(string ipAddress)
        {
            try
            {
                string rslt =
                    GETRequest(new Uri(string.Format("http://www.stopforumspam.com/api?ip={0}", ipAddress.Trim())));

                return rslt.ToLower().Contains(@"<appears>yes</appears>");
            }
            catch
            {
                return true;
            }
        }

        #endregion

        public static bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return extension != null && (extension.ToLower() == ".jpg" ||
                                         extension.ToLower() == ".png" ||
                                         extension.ToLower() == ".gif");
        }

        public static string UrlPathResolver(string filePath)
        {
            //   return string.Format("http://s3.amazonaws.com/{0}/{1}", DasKlub.Lib.Configs.AmazonCloudConfigs.AmazonBucketName, filePath);

            if (string.IsNullOrEmpty(filePath)) return string.Empty;


            string relativePath = VirtualPathUtility.ToAbsolute(filePath);
            return relativePath;
        }

        public static string CreateUniqueContentFilename(HttpPostedFileBase file)
        {
            return Guid.NewGuid() + Path.GetExtension(file.FileName);
        }


        public static string RomanNumeral(int number)
        {
            // Validate
            if (number < 0 || number > 3999)
                throw new ArgumentException("Value must be in the range 0 - 3,999.");

            if (number == 0) return "N";

            var values = new[] {1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1};
            var numerals = new[] {"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"};
            // Initialise the string builder
            var result = new StringBuilder(100);
            // Loop through each of the values to diminish the number
            for (var i = 0; i < 13; i++)
            {
                // If the number being converted is less than the test value, append
                // the corresponding numeral or numeral pair to the resultant string
                while (number >= values[i])
                {
                    number -= values[i];
                    result.Append(numerals[i]);
                }
            }

            // Done
            return result.ToString();
        }

        public static string URLAuthority()
        {
            return "http://" + HttpContext.Current.Request.Url.Authority;
        }

        public static string S3ContentPath(string filePath)
        {
            return string.Format(AmazonCloudConfigs.AmazonCloudDomain, AmazonCloudConfigs.AmazonBucketName, filePath);
        }


        public static string GetIPForDomain(string domain)
        {
            try
            {
                var iPAddress = Dns.GetHostAddresses(domain);
                return iPAddress[0].ToString();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return string.Empty;
            }
        }

        public static bool IsiPhoneOriPad()
        {
            if (HttpContext.Current.Request.UserAgent == null)
            {
                return false;
            }
            var userAgent = HttpContext.Current.Request.UserAgent.ToLower();

            return userAgent.Contains("iphone") || userAgent.Contains("ipad");
        }


        public static string TimeElapsedMessage(DateTime occurance)
        {
            DateTime now = GetDataBaseTime();

            return TimeElapsedMessage(occurance, now);
        }

        private static string TimeElapsedMessage(DateTime occurance, DateTime now)
        {
            string timeElapsed;

            var elapsed = now.Subtract(occurance);


            if (elapsed.TotalSeconds <= 1)
            {
                // now
                timeElapsed = string.Format("{0}", Messages.JustNow);
            }
            else if (elapsed.TotalMinutes < 1)
            {
                // seconds old
                timeElapsed = string.Format(Messages.SecondsAgo, (int) Math.Round(elapsed.TotalSeconds));
            }
            else if (elapsed.TotalMinutes < 2)
            {
                // less than 2 minutes ago
                timeElapsed = string.Format("{0}", Messages.AboutOneMinuteAgo);
            }
            else if (elapsed.TotalMinutes < 120)
            {
                // minutes old

                timeElapsed = string.Format(Messages.MinutesAgo, (int) Math.Round(elapsed.TotalMinutes));
            }
            else if (elapsed.TotalHours < 24)
            {
                // hours old

                timeElapsed = string.Format(Messages.HoursAgo, (int) Math.Round(elapsed.TotalHours));
            }
            else if (elapsed.TotalDays < 2)
            {
                // 1 day ago
                timeElapsed = string.Format(Messages.Yesterday);
            }
            else if (elapsed.TotalDays < 14)
            {
                // less than 1 week

                timeElapsed = string.Format(Messages.DaysAgo, (int) Math.Round(elapsed.TotalDays));
            }
            else if (elapsed.TotalDays < 60)
            {
                // 1 to 4 weeks ago
                timeElapsed =
                    string.Format(Messages.WeeksAgo,
                                  (int) Math.Round(elapsed.TotalDays/7));
            }
            else if (elapsed.TotalDays < 365)
            {
                // months old but less than a year old
                timeElapsed
                    = string.Format(Messages.MonthsAgo, (int) Math.Round(elapsed.TotalDays/30));
            }
            else if (elapsed.TotalDays < (365*2))
            {
                // 1 year 
                timeElapsed = string.Format(Messages.YearAgo, (int) Math.Round(elapsed.TotalDays/365.2425));
            }
            else
            {
                // over a year old
                timeElapsed = string.Format(Messages.YearsAgo, (int) Math.Round(elapsed.TotalDays/365.2425));
            }

            return timeElapsed;
        }

        public static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }


        /// <summary>
        ///     http://weblogs.asp.net/farazshahkhan/archive/2008/08/09/regex-to-find-url-within-text-and-make-them-as-link.aspx
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string MakeLink(string txt)
        {
            return MakeLink(txt, string.Empty);
        }

        public static string MakeLink(string txt, bool replaceLines)
        {
            return !replaceLines ? MakeLink(txt) : MakeLink(txt).Replace("\r\n", "<br />");
        }


        /// <summary>
        ///     http://weblogs.asp.net/farazshahkhan/archive/2008/08/09/regex-to-find-url-within-text-and-make-them-as-link.aspx
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="linkText"></param>
        /// <returns></returns>
        private static string MakeLink(string txt, string linkText)
        {
            // force all https to http
            if (txt.Contains("https://"))
                txt = txt.Replace("https://", "http://");

            var regx =
                new Regex(
                    "http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                    RegexOptions.IgnoreCase);

            var mactches = regx.Matches(txt);

            foreach (Match match in mactches)
            {
                if (match.Value.Contains(@"//www.youtube.com/embed/")) continue; // because it might be embeded


                txt = txt.Replace(match.Value,
                                  string.Format(@"<a target=""_blank"" href=""{0}"">{1}</a>", match.Value, Messages.Link));
            }

            return txt;
        }

        private static CultureInfo GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        ///     2 letter
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanguageCode()
        {
            return GetCurrentCulture().TwoLetterISOLanguageName.ToUpper();
        }


        public static string GetLanguageNameForCode(string defaultLanguage)
        {
            if (string.IsNullOrWhiteSpace(defaultLanguage))
            {
                return GeneralConfigs.DefaultLanguage;
            }

            var lang =
                (SiteEnums.SiteLanguages) Enum.Parse(typeof (SiteEnums.SiteLanguages), defaultLanguage.ToUpper());

            var langKey = GetEnumDescription(lang);

            return ResourceValue(langKey);
        }

        #region SQL injection

        //Defines the set of characters that will be checked.
        //You can add to this list, or remove items from this list, as appropriate for your site
        private static readonly string[] BlackList =
            {
                "--", ";--", ";", "/*", "*/", "@@", "@",
                "delete", "drop", "end", "exec", "execute", "select",
                "table", "update"
            };

        #endregion

        #region validation

        /// <summary>
        ///     Is this e-mail address in the correct form?
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        /// <TODO>add the logic that will check it with the host</TODO>
        public static bool IsEmail(string inputEmail)
        {
            if (string.IsNullOrWhiteSpace(inputEmail))
            {
                return false;
            }
            const string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);
            return re.IsMatch(inputEmail);
        }

        #endregion

        #region time

        public static DateTime GetDataBaseTime()
        {
            double totalSecondsDif = 0;
            const string cacheName = "db_time";

            if (HttpContext.Current != null && HttpRuntime.Cache[cacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetCurrentTime";

                // execute the stored procedure
                DateTime dateT = FromObj.DateFromObj(DbAct.ExecuteScalar(comm));

                TimeSpan span = DateTime.UtcNow.Subtract(dateT);

                totalSecondsDif = span.TotalSeconds;

                HttpRuntime.Cache.AddObjToCache(totalSecondsDif, cacheName);
            }
            else
            {
                if (HttpContext.Current != null) totalSecondsDif = (double) HttpRuntime.Cache[cacheName];
            }


            return DateTime.UtcNow.AddSeconds(totalSecondsDif);
        }

        #endregion

        #region listmod

        public class General
        {
            public static void SortDropDownList(DropDownList ddlList)
            {
                ArrayList arl = null;
                if (ddlList.Items.Count > 0)
                {
                    arl = new ArrayList(ddlList.Items.Count);
                    foreach (ListItem li in ddlList.Items)
                    {
                        arl.Add(li);
                    }
                }
                arl.Sort(new ListItemComparer());
                ddlList.Items.Clear();
                for (int i = 0; i < arl.Count; i++)
                {
                    ddlList.Items.Add(arl[i].ToString());
                }
            }
        }

        public class ListItemComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                var lix = (ListItem) x;
                var liy = (ListItem) y;
                var c = new CaseInsensitiveComparer();
                return c.Compare(lix.Text, liy.Text);
            }

            #endregion
        }

        #endregion

        #region resources

        public static string ResourceValue(string resourceKey)
        {
            try
            {
                var rm = new ResourceManager("DasKlub.Lib.Resources.Messages", Assembly.GetExecutingAssembly());

                return rm.GetString(resourceKey);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region cookies

        /// <summary>
        ///     Given the cookie name, check if it exists, if it does, check if the name
        ///     in the name value collection exists, if so remove it and add the new one
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="nvc"></param>
        public static void CookieMaker(SiteEnums.CookieName cn, NameValueCollection nvc)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cn.ToString()];

            // if the cookie exists, expire it
            if (cookie != null)
            {
                foreach (string s in nvc)
                {
                    if (cookie.Values[s] != null)
                    {
                        cookie.Values.Remove(s);
                        cookie.Values.Add(s, nvc[s]); // changed 2010-12-02
                    }
                    else
                    {
                        //cookie.Values.Add(nvc);
                        cookie.Values.Add(s, nvc[s]);
                    }
                }
            }
            else
            {
                // make a new cookie
                cookie = new HttpCookie(cn.ToString());
                cookie.Values.Add(nvc);
            }

            cookie.Expires = DateTime.UtcNow.AddDays(7);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        #endregion

        #region math

        /// <summary>
        ///     How many years have passed since birthdate
        /// </summary>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        private static int CalculateAge(DateTime birthDay, DateTime currentDate)
        {
            if (birthDay == DateTime.MinValue) return 0;

            DateTime now = currentDate;
            int age = now.Year - birthDay.Year;
            if (birthDay > now.AddYears(-age)) age--;

            return age;
        }

        /// <summary>
        ///     How many years have passed since this date
        /// </summary>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        public static int CalculateAge(DateTime birthDay)
        {
            return CalculateAge(birthDay, DateTime.UtcNow);
        }

        #endregion

        #region error logging
 

        /// <summary>
        ///     The base method that writes to the file system and sends mail
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        private static void LogError(string msg, Exception ex)
        {
            Log.Error(msg, ex);
            var context = HttpContext.Current;

            var exMessage = new StringBuilder();
            if (context == null) return;

            if (context.Request.UrlReferrer != null)
            {
                exMessage.Append("\n\n Previous Page: " + context.Request.UrlReferrer);
            }
            if (context.Request.UserHostAddress != null &&
                !string.IsNullOrEmpty(context.Request.UserHostAddress))
            {
                exMessage.Append("\n\n User Host Address: " + context.Request.UserHostAddress);
            }
            if (!string.IsNullOrEmpty(context.Request.UserAgent))
            {
                exMessage.Append("\n\n User Agent: " + context.Request.UserAgent);
            }
            if (context.Request.Browser != null)
            {
                exMessage.Append("\n\n Browser Version: " + context.Request.Browser.Version);
            }
            exMessage.Append("\n\n Headers: " + context.Request.Headers);
            if (!string.IsNullOrEmpty(context.Request.ApplicationPath))
            {
                exMessage.Append("\n\n Application Path: " + context.Request.ApplicationPath);
            }

            if (!string.IsNullOrEmpty(context.Request.RawUrl))
            {
                exMessage.Append("\n\n Page Location: " + context.Request.RawUrl);
            }
            if (!string.IsNullOrEmpty(context.Request.UserHostAddress))
            {
                exMessage.Append("\n\n Full Page URL: " + context.Request.Url);
            }

            Log.Fatal(exMessage.ToString());
        }
    

        /// <summary>
        ///     Log an exception to a file and mail it
        /// </summary>
        /// <param name="ex"></param>
        public static void LogError(Exception ex)
        {
            LogError(string.Empty, ex);
        }

        /// <summary>
        ///     Write a message to the error log, useful for debugging
        /// </summary>
        /// <param name="msg"></param>
        public static void LogError(string msg)
        {
            LogError(msg, true);
        }


        /// <summary>
        ///     Write a message to the error log, useful for debugging (send mail version specification)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sendEmail">if errors are being sent</param>
        private static void LogError(string msg, bool sendEmail)
        {
            LogError(msg, null);
        }

        #endregion

        #region outbound requests

        /// <summary>
        ///     Given the URL, do a GET request and return the response
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GETRequest(Uri input)
        {
            return GETRequest(input, input.Host);
        }

        public static bool? GETRequest(Uri input, bool getResponse)
        {
            if (!getResponse)
            {
                return null;
            }

            WebRequest request = WebRequest.Create(input) as HttpWebRequest;
            if (request != null)
            {
                request.Method = SiteEnums.HTTPTypes.GET.ToString();

                try
                {
                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK) return true;
                        using (var dataStream = response.GetResponseStream())
                        {
                            if (dataStream != null)
                                using (var reader = new StreamReader(dataStream))
                                {
                                    reader.ReadToEnd();
                                }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError &&
                        ex.Response != null)
                    {
                        var resp = (HttpWebResponse) ex.Response;
                        if (resp.StatusCode == HttpStatusCode.NotFound)
                        {
                            return false;
                            // Do something
                        }
                    }
                }
                catch
                {
                    // Utilities.LogError(debugMsg, ex);
                }
            }

            return false;
        }

        /// <summary>
        ///     Given the URL, do a GET request and return the response
        /// </summary>
        /// <param name="input"></param>
        /// <param name="debugMsg"></param>
        /// <returns></returns>
        private static string GETRequest(Uri input, string debugMsg)
        {
            var responseData = string.Empty;

            WebRequest request = WebRequest.Create(input) as HttpWebRequest;
            if (request != null)
            {
                request.Method = SiteEnums.HTTPTypes.GET.ToString();

                try
                {
                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        using (var dataStream = response.GetResponseStream())
                        {
                            if (dataStream != null)
                                using (var reader = new StreamReader(dataStream))
                                {
                                    responseData = reader.ReadToEnd();
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(debugMsg, ex);
                }
            }
            return responseData;
        }

        #endregion

   
        #region enum methods

        /// <summary>
        ///     Turns an enum into a string, if it has a description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        #endregion
    }
}