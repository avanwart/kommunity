using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
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
using System.Web.UI.WebControls;
using DasKlub.Lib.BLL;
using DasKlub.Lib.Configs;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;
using log4net;
using System.Collections.Generic;

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

            var values = new[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            var numerals = new[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
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
            return string.Concat("http://", HttpContext.Current.Request.Url.Authority);
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
            var now = DateTime.UtcNow;

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
                timeElapsed = string.Format(Messages.SecondsAgo, (int)Math.Floor(elapsed.TotalSeconds));
            }
            else if (elapsed.TotalMinutes < 2)
            {
                // less than 2 minutes ago
                timeElapsed = string.Format("{0}", Messages.AboutOneMinuteAgo);
            }
            else if (elapsed.TotalMinutes < 120)
            {
                // minutes old
                timeElapsed = string.Format(Messages.MinutesAgo, (int)Math.Floor(elapsed.TotalMinutes));
            }
            else if (elapsed.TotalHours < 24)
            {
                // hours old
                timeElapsed = string.Format(Messages.HoursAgo, (int)Math.Floor(elapsed.TotalHours));
            }
            else if (elapsed.TotalDays < 2)
            {
                // 1 day ago
                timeElapsed = string.Format(Messages.Yesterday);
            }
            else if (elapsed.TotalDays < 14)
            {
                // less than 1 week
                timeElapsed = string.Format(Messages.DaysAgo, (int)Math.Floor(elapsed.TotalDays));
            }
            else if (elapsed.TotalDays < 60)
            {
                // 1 to 4 weeks ago
                timeElapsed =
                    string.Format(Messages.WeeksAgo,
                                  (int)Math.Floor(elapsed.TotalDays / 7));
            }
            else if (elapsed.TotalDays < 365)
            {
                // months old but less than a year old
                timeElapsed
                    = string.Format(Messages.MonthsAgo, (int)Math.Floor(elapsed.TotalDays / 30));
            }
            else if (elapsed.TotalDays < (365 * 2))
            {
                // 1 year 
                timeElapsed = string.Format(Messages.YearAgo, 1);
            }
            else
            {
                // over a year old
                var years = (int)Math.Floor(elapsed.TotalDays / 365.2425);

                timeElapsed = string.Format(Messages.YearsAgo, years);
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

            foreach (var match in mactches.Cast<Match>().Where(match => !match.Value.Contains(@"//www.youtube.com/embed/")))
            {
                const int maxChars = 30;
                var displayLink = match.Value;
                if (displayLink.Length > maxChars)
                {
                    displayLink = string.Format("{0}...", displayLink.Substring(0, maxChars));
                }
                txt = txt.Replace(match.Value,
                    string.Format(@"<a target=""_blank"" href='{0}'>{1}</a>", match.Value, displayLink));
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
                (SiteEnums.SiteLanguages)Enum.Parse(typeof(SiteEnums.SiteLanguages), defaultLanguage.ToUpper());

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
                if (HttpContext.Current != null) totalSecondsDif = (double)HttpRuntime.Cache[cacheName];
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

                foreach (var t in arl)
                {
                    ddlList.Items.Add(t.ToString());
                }
            }
        }

        public class ListItemComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                var lix = (ListItem)x;
                var liy = (ListItem)y;
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

        public static void LogError(Exception ex)
        {
            var exception = ex as SqlException;

            if (exception != null)
            {
                var sqlEx = exception;
                if (sqlEx.ErrorCode == -2146232060)
                {
                    // connection is bad, forget it
                }
                else
                {
                    Log.Fatal(ex);
                }
            }
            else
            {
                Log.Fatal(ex);
            }

        }


        public static void LogError(string msg)
        {
            Log.Fatal(msg);
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
                    using (var response = (HttpWebResponse)request.GetResponse())
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
                        var resp = (HttpWebResponse)ex.Response;
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
                    using (var response = (HttpWebResponse)request.GetResponse())
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
                    LogError(ex);
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
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        #endregion

        public static string ConvertTextToHtml(string inputText)
        {
            var linkTextMaxLength   = 30;
            var regx                = new Regex(
                    @"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?",
                    RegexOptions.IgnoreCase);
            var mactches            = regx.Matches(inputText);
            var newText             = string.Concat(inputText, " ");// hack: space at end for regex match
            var allLinks            = mactches.Cast<Match>()
                                              .GroupBy(x => x.Value)
                                              .Select(x => x.First());

            foreach (var link in allLinks)
            {
                var     matchedUrl          = link.Value;
                string  replacementText;

                if ((matchedUrl.Contains("youtube.com") && matchedUrl.Contains("v=")) || // excludes channel links
                     matchedUrl.Contains("youtu.be"))
                {
                    replacementText = FormatYouTubeVideo(matchedUrl);
                }
                else
                {
                    replacementText = FormatLink(linkTextMaxLength, link, matchedUrl);
                }

                newText = ReplaceNewLineSpaceWithLink(newText, link, replacementText);
            }

            var listBreaksHTML = newText.Replace(Environment.NewLine, string.Concat("<br />", Environment.NewLine));

            return listBreaksHTML.Trim();
        }

        private static string ReplaceNewLineSpaceWithLink(string newText, Match link, string replacementText)
        {
            // replace links with new lines and spaces after them then put those characters back in
            var regexReplace    = new Regex(string.Concat(Regex.Escape(link.Value), "(", Environment.NewLine, ")"));
            newText             = regexReplace.Replace(newText, string.Concat(replacementText, Environment.NewLine));
            regexReplace        = new Regex(string.Concat(Regex.Escape(link.Value), @"(\s)"));
            newText             = regexReplace.Replace(newText, string.Concat(replacementText, " "));
            return newText;
        }

        private
        static
        string
        FormatLink(int linkTextMaxLength, Match link, string matchedUrl)
        {
            if (HttpContext.Current == null)
            {
                HttpContext.Current = new HttpContext(
                                            new HttpRequest(    
                                                string.Empty, 
                                                GeneralConfigs.SiteDomain,
                                                string.Empty), 
                                                new HttpResponse(
                                                    new StringWriter()));
            }

            string replacementText;
            var linkText            = link.Value;
            var internalHost        = HttpContext.Current.Request.Url.Host;

            if (linkText.Length > linkTextMaxLength)
            {
                var ellipsis    = "...";
                linkText        = string.Concat(
                                        linkText.Substring(0,
                                                           linkTextMaxLength - ellipsis.Length
                                                           ),
                                        ellipsis);
            }

            if (matchedUrl.Contains(internalHost))
            {
                // internal link
                replacementText = string.Format(@"<a href=""{0}"">{1}</a>", 
                                                  matchedUrl, 
                                                  linkText);
            }
            else
            {
                replacementText = string.Format(@"<a target=""_blank"" href=""{0}"">{1}</a>", 
                                                matchedUrl, 
                                                linkText);
            }
            return replacementText;
        }

        private static string FormatYouTubeVideo(string matchedUrl, int height = 200, int width = 300)
        {
            string videoKey;
            string replacementText;

            if (matchedUrl.Contains("youtu.be"))
            {
                videoKey = matchedUrl.Replace("http://youtu.be/", string.Empty);
            }
            else
            {
                var nvcKey  = HttpUtility.ParseQueryString(new Uri(matchedUrl).Query);
                videoKey    = nvcKey["v"];
            }

            // YouTube video
            replacementText = string.Format(
@"<div class=""you_tube_iframe""><iframe width=""{2}"" height=""{1}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>",
        videoKey, height, ((width == 0) ? (object)"100%" : width));

            return replacementText;
        }
    }
}