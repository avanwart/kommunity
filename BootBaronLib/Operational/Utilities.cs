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
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL.Logging;
using BootBaronLib.Configs;
using BootBaronLib.DAL;
using BootBaronLib.Resources;
using BootBaronLib.Values;
using HtmlAgilityPack;
using Microsoft.Win32;
using log4net;
using Content = Amazon.SimpleEmail.Model.Content;
using Image = System.Drawing.Image;

namespace BootBaronLib.Operational
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
            //   return string.Format("http://s3.amazonaws.com/{0}/{1}", BootBaronLib.Configs.AmazonCloudConfigs.AmazonBucketName, filePath);

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
            for (int i = 0; i < 13; i++)
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


        public static string RandomAdvertisement()
        {
            var sb1 = new StringBuilder(100);

            try
            {
                var doc = new HtmlDocument();

                string topPicks;

                if (HttpContext.Current.Cache["top_week_picks"] == null)
                {
                    using (var wc = new WebClient())
                    {
                        topPicks =
                            wc.DownloadString(
                                "http://store.vampirefreaks.com/?cat=monthly+top+sellers&aff=dasklub&cols=1&numitems=1000");

                        HttpContext.Current.Cache.AddObjToCache(topPicks, "top_week_picks");
                    }
                }

                doc.LoadHtml((string) HttpContext.Current.Cache["top_week_picks"]);

                var adChoices = new Dictionary<int, string>();

                int itemCnt = 0;

                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        string link1and2 = string.Empty;

                        string linkText = string.Empty;
                        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                        {
                            var regx =
                                new Regex(
                                    "http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                                    RegexOptions.IgnoreCase);

                            MatchCollection mactches = regx.Matches(cell.InnerHtml);
                            linkText = cell.InnerText;

                            foreach (Match match in mactches)
                            {
                                link1and2 += match.Value + "|";
                            }
                        }

                        itemCnt++;

                        adChoices.Add(itemCnt, linkText + "|" + link1and2);
                    }
                }


                var randObj = new Random();

                string[] parts = adChoices[randObj.Next(0, adChoices.Count - 1)].Split('|');

                if (parts != null && parts.Length > 3 &&
                    !string.IsNullOrWhiteSpace(parts[0]) &&
                    !string.IsNullOrWhiteSpace(parts[1]) &&
                    !string.IsNullOrWhiteSpace(parts[2]))
                {
                    sb1.AppendFormat(
                        @"<a rel=""nofollow"" class=""m_over"" href=""{0}"" target=""_blank""><img style=""width:100px;height:100px"" src=""{1}"" alt=""{2}"" title=""{2}"" /></a>",
                        parts[1], parts[2], parts[0]);
                }
            }
            catch
            {
            }

            return sb1.ToString();
        }

        public static string S3ContentPath(string filePath)
        {
            return string.Format(AmazonCloudConfigs.AmazonCloudDomain, AmazonCloudConfigs.AmazonBucketName, filePath);
        }

        public static string YouTubeKey(string url)
        {
            var regx =
                new Regex(
                    "(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                    RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(url);
            string vidKey = string.Empty;
            string theLink = string.Empty;

            foreach (Match match in mactches)
            {
                if (match.Value.Contains("http://www.youtube.com/watch?"))
                {
                    NameValueCollection nvcKey =
                        HttpUtility.ParseQueryString(match.Value.Replace("http://www.youtube.com/watch?", string.Empty));

                    return nvcKey["v"];
                }
                else if (match.Value.Contains("http://youtu.be/"))
                {
                    return match.Value.Replace("http://youtu.be/", string.Empty);
                }
            }
            return string.Empty;
        }


        public static T NumToEnum<T>(int number)
        {
            return (T) Enum.ToObject(typeof (T), number);
        }

        public static string GetIPForDomain(string domain)
        {
            try
            {
                IPAddress[] iPAddress = Dns.GetHostAddresses(domain);
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
            string userAgent = HttpContext.Current.Request.UserAgent.ToLower();

            return userAgent.Contains("iphone") || userAgent.Contains("ipad");
        }


        public static string TimeElapsedMessage(DateTime occurance)
        {
            DateTime now = GetDataBaseTime();

            return TimeElapsedMessage(occurance, now);
        }

        public static string TimeElapsedMessage(DateTime occurance, DateTime now)
        {
            string timeElapsed;

            TimeSpan elapsed = now.Subtract(occurance);


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
            else if (elapsed.TotalMinutes < 60)
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
            else if (elapsed.TotalDays > 365 && elapsed.TotalDays < 730)
            {
                // 1 year 
                timeElapsed = string.Format(Messages.YearAgo, (int) Math.Round(elapsed.TotalDays/365.24));
            }
            else
            {
                // over a year old
                timeElapsed = string.Format(Messages.YearsAgo, (int) Math.Round(elapsed.TotalDays/365.24));
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
            if (!replaceLines) return MakeLink(txt);
            else
                return MakeLink(txt).Replace("\r\n", "<br />");
        }


        /// <summary>
        ///     http://weblogs.asp.net/farazshahkhan/archive/2008/08/09/regex-to-find-url-within-text-and-make-them-as-link.aspx
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string MakeLink(string txt, string linkText)
        {
            // BUG: NOT GETTING HTTPS URLS
            var regx =
                //new Regex("(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",  RegexOptions.IgnoreCase);
                new Regex(
                    "http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                    RegexOptions.IgnoreCase);

            //  Regex regx = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);


            MatchCollection mactches = regx.Matches(txt);

            foreach (Match match in mactches)
            {
                if (match.Value.Contains(@"//www.youtube.com/embed/")) continue; // because it might be embeded

                if (string.IsNullOrEmpty(linkText))
                {
                    txt = txt.Replace(match.Value,
                                      @"<a target=""_blank"" href='" + match.Value + "'>" + match.Value + "</a>");
                }
                else
                {
                    txt = txt.Replace(match.Value,
                                      @"<a target=""_blank"" href='" + match.Value + "'>" + linkText + "</a>");
                }
            }


            return txt;
        }

        public static CultureInfo GetCurrentCulture()
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


        public static string GetCurrentLanguage()
        {
            string[] languages = HttpContext.Current.Request.UserLanguages;

            return languages != null ? languages[0].ToLowerInvariant().Trim() : string.Empty;
        }

        public static string GetLanguageNameForCode(string defaultLanguage)
        {
            if (string.IsNullOrWhiteSpace(defaultLanguage))
            {
                return GeneralConfigs.DefaultLanguage;
            }

            var lang =
                (SiteEnums.SiteLanguages) Enum.Parse(typeof (SiteEnums.SiteLanguages), defaultLanguage.ToUpper());

            string langKey = GetEnumDescription(lang);

            return ResourceValue(langKey);
        }

        #region mail

        /// <summary>
        ///     Sends a mail message with the default sender with appended message at
        ///     the bottom
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool SendMail(string toEmail, string subject, string body)
        {
            // append to the bottom of all messages a message that says not to reply
            body =
                Messages.DoNotRespondToThisEmail + Environment.NewLine +
                "-----------------------------------------------" +
                Environment.NewLine + Environment.NewLine +
                body +
                Environment.NewLine + Environment.NewLine + Environment.NewLine +
                "-----------------------------------------------"
                + Environment.NewLine
                + Messages.EditYourEmailSettings
                + Environment.NewLine
                + Environment.NewLine
                + GeneralConfigs.EmailSettingsURL
                + Environment.NewLine
                + Environment.NewLine + Messages.DoNotRespondToThisEmail;

            return SendMail(toEmail, AmazonCloudConfigs.SendFromEmail, subject, body);
        }


        /// <summary>
        ///     Sends a mail message
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="fromEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        /// <see
        ///     cref=">http://www.neuraplex.com/Blog/tabid/125/EntryId/34/Amazon-SES-Simple-Email-Service-C-code-examples-ASP-NET-CODES.aspx" />
        public static bool SendMail(string toEmail, string fromEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(toEmail) ||
                string.IsNullOrEmpty(fromEmail) ||
                string.IsNullOrEmpty(subject) ||
                string.IsNullOrEmpty(body)) return false; // don't send if anything is missing

            try
            {
                toEmail = toEmail.Trim();
                fromEmail = fromEmail.Trim();

                // check amazon's settings for your email mail limits
                var amConfig = new AmazonSimpleEmailServiceConfig {UseSecureStringForAwsSecretKey = false};

                var amzClient =
                    new AmazonSimpleEmailServiceClient(AmazonCloudConfigs.AmazonAccessKey,
                                                       AmazonCloudConfigs.AmazonSecretKey, amConfig);
                var to = new ArrayList {toEmail};

                var dest = new Destination();
                //dest.WithBccAddresses((string[])to.ToArray(typeof(string)));
                dest.WithToAddresses((string[]) to.ToArray(typeof (string)));

                var bdy = new Body();
                bdy.Text = new Content(body); //use plain text, not html
                var title = new Content(subject);
                var message = new Message(title, bdy);
                var ser = new SendEmailRequest(AmazonCloudConfigs.SendFromEmail, dest, message);

                SendEmailResponse seResponse = amzClient.SendEmail(ser);
                SendEmailResult seResult = seResponse.SendEmailResult;

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static ArrayList GetVerifiedSenders()
        {
            //INITIALIZE AWS CLIENT/////////////////////////////////////////////////////////
            var amConfig = new AmazonSimpleEmailServiceConfig {UseSecureStringForAwsSecretKey = false};
            var amzClient = new AmazonSimpleEmailServiceClient(AmazonCloudConfigs.AmazonSecretKey,
                                                               AmazonCloudConfigs.AmazonAccessKey, amConfig);
            //LIST VERIFIED EMAILS/////////////////////////////////////////////////////////
            var lveReq = new ListVerifiedEmailAddressesRequest();
            ListVerifiedEmailAddressesResponse lveResp = amzClient.ListVerifiedEmailAddresses(lveReq);
            ListVerifiedEmailAddressesResult lveResult = lveResp.ListVerifiedEmailAddressesResult;

            var allUsers = new ArrayList();

            foreach (string email in lveResult.VerifiedEmailAddresses)
            {
                allUsers.Add(email);
            }

            return allUsers;
        }

        #endregion

        #region SQL injection

        //Defines the set of characters that will be checked.
        //You can add to this list, or remove items from this list, as appropriate for your site
        private static readonly string[] BlackList =
            {
                "--", ";--", ";", "/*", "*/", "@@", "@",
                "delete", "drop", "end", "exec", "execute", "select",
                "table", "update"
            };

        /// <summary>
        ///     Replaces SQL characters
        /// </summary>
        /// <param name="inputSQL"></param>
        /// <returns>doubled quotes version of string</returns>
        public static string MakeSQLSafe(string input)
        {
            return input.Replace("'", "''");
        }

        /// <summary>
        ///     Check for valid SQL safe string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidInput(string input)
        {
            return !BlackList.Any(t => (input.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)) || IsEmail(input);
            // it's valid
        }

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

        /// <summary>
        ///     Check if a string is a guid
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        /// <see>http://msdn.microsoft.com/en-us/library/7c5ka91b(VS.80).aspx</see>
        public static bool IsGuid(string candidate)
        {
            var isGuid =
                new Regex(
                    @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$",
                    RegexOptions.Compiled);

            bool isValid = false;

            //  output = Guid.Empty;

            if (!string.IsNullOrEmpty(candidate))
            {
                if (isGuid.IsMatch(candidate))
                {
                    //  output = new Guid(candidate);
                    isValid = true;
                }
            }

            return isValid;
        }

        #endregion

        #region user interface

        /// <summary>
        ///     Configures what button to be clicked when the uses presses Enter in a textbox.
        ///     The text box doesn't have to be a TextBox control, but it must
        ///     be derived from either HtmlControl or WebControl, and the HTML control it
        ///     generates should accept an 'onkeydown' attribute. The HTML generated by
        ///     the button must support the 'Click' event
        /// </summary>
        /// <param name="page"></param>
        /// <param name="TextBoxToTie"></param>
        /// <param name="ButtonToTie"></param>
        public static void TieButton(Page page, Control TextBoxToTie, Control ButtonToTie)
        {
            if (TextBoxToTie == null) return;

            // Init jscript
            string jsString = string.Empty;

            // Check button type and get required jscript
            if (ButtonToTie is LinkButton)
            {
                jsString = "if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {"
                           + page.ClientScript.GetPostBackEventReference(ButtonToTie, "").Replace(":", "$") +
                           ";return false;} else return true;";
            }
            else if (ButtonToTie is ImageButton)
            {
                jsString = "if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {"
                           + page.ClientScript.GetPostBackEventReference(ButtonToTie, "").Replace(":", "$") +
                           ";return false;} else return true;";
            }
            else
            {
                jsString = "if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {document."
                           + "forms[0].elements['" + ButtonToTie.UniqueID.Replace(":", "_") +
                           "'].click();return false;} else return true; ";
            }

            // Attach jscript to the onkeydown attribute - we have to cater for HtmlControl or WebControl
            if (TextBoxToTie is HtmlControl)
            {
                ((HtmlControl) TextBoxToTie).Attributes.Add("onkeydown", jsString);
            }
            else if (TextBoxToTie is WebControl)
            {
                ((WebControl) TextBoxToTie).Attributes.Add("onkeydown", jsString);
            }
        }

        #endregion

        #region collections

        /// <summary>
        ///     Takes and arraylist which may contain duplicate items and makes a unique arraylist
        /// </summary>
        /// <param name="list"></param>
        /// <returns>unique arraylist</returns>
        public static ArrayList RemoveDuplicates(ArrayList list)
        {
            var ret = new ArrayList();
            foreach (object obj in list)
            {
                if (!ret.Contains(obj)) ret.Add(obj);
            }
            return ret;
        }

        #endregion

        #region page processing

        /// <summary>
        ///     Write out the file to the browser
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="contentType"></param>
        /// <param name="extension"></param>
        /// <param name="fileName"></param>
        /// <see>http://en.kioskea.net/contents/courrier-electronique/mime.php3</see>
        public static void WriteOutFile(string fileLocation, string contentType, string fileName)
        {
            try
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = contentType;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.WriteFile(fileLocation);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        /// <summary>
        ///     Get the control that caused a postback
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <see>http://www.aspdotnetfaq.com/Faq/How-to-determine-which-Control-caused-PostBack-on-ASP-NET-page.aspx</see>
        public static Control GetPostBackControl(Page page)
        {
            Control postbackControlInstance = null;
            string postbackControlName = page.Request.Params.Get("__EVENTTARGET");
            if (postbackControlName != null && postbackControlName != string.Empty)
            {
                postbackControlInstance = page.FindControl(postbackControlName);
            }
            else
            {
                // handle the Button control postbacks
                for (int i = 0; i < page.Request.Form.Keys.Count; i++)
                {
                    if (postbackControlInstance == null) return null;

                    postbackControlInstance = page.FindControl(page.Request.Form.Keys[i]);
                    if (postbackControlInstance is Button)
                    {
                        return postbackControlInstance;
                    }
                }
            }

            // handle the ImageButton postbacks
            if (postbackControlInstance == null)
            {
                for (int i = 0; i < page.Request.Form.Count; i++)
                {
                    if ((page.Request.Form.Keys[i].EndsWith(".x")) || (page.Request.Form.Keys[i].EndsWith(".y")))
                    {
                        postbackControlInstance =
                            page.FindControl(page.Request.Form.Keys[i].Substring(0, page.Request.Form.Keys[i].Length - 2));
                        return postbackControlInstance;
                    }
                }
            }

            return postbackControlInstance;
        }

        #endregion

        #region imagery

        public static Color GetRandomColor()
        {
            var rand = new Random();

            return Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
        }


        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (Width/(float) sourceWidth);
            nPercentH = (Height/(float) sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((Width -
                                         (sourceWidth*nPercent))/2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((Height -
                                         (sourceHeight*nPercent))/2);
            }

            var destWidth = (int) (sourceWidth*nPercent);
            var destHeight = (int) (sourceHeight*nPercent);

            var bmPhoto = new Bitmap(Width, Height,
                                     PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                  imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                              new Rectangle(destX, destY, destWidth, destHeight),
                              new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                              GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }


        /// <summary>
        ///     Take an image, resizes it and save it to the file system
        /// </summary>
        /// <param name="s">file stream</param>
        /// <param name="filePath">path to write it too</param>
        /// <param name="maxWidth">image will not be wider than this</param>
        /// <param name="maxHeight">image will not be higher than this</param>
        /// <param name="bilinear">reduce distortion</param>
        /// <returns></returns>
        /// <see> http://www.codeproject.com/KB/GDI-plus/imageprocessing4.aspx </see>
        public static bool ResizeImage(Stream s,
                                       string filePath,
                                       int maxWidth,
                                       int maxHeight,
                                       bool isBilinear)
        {
            var b = new Bitmap(s);

            // current height/ width
            decimal bitmapWidth = b.Width;
            decimal bitmapHeigth = b.Height;

            // new height/ width
            //int targetWidth = 0;
            //int targetHeigth = 0;

            int targetWidth = maxWidth;
            int targetHeigth = maxHeight;


            decimal imageRatio = bitmapHeigth/bitmapWidth;

            if (bitmapWidth >= bitmapHeigth)
            {
                // wide pic

                if (maxWidth <= bitmapWidth)
                {
                    targetWidth = maxWidth;
                    targetHeigth = Convert.ToInt32(targetWidth*imageRatio);
                }

                if (maxHeight <= targetHeigth)
                {
                    targetHeigth = maxHeight;
                    targetWidth = Convert.ToInt32(targetHeigth*imageRatio);
                }
            }
            else
            {
                // high pic

                if (maxWidth <= bitmapWidth)
                {
                    targetWidth = maxWidth;
                    targetHeigth = Convert.ToInt32(targetWidth*imageRatio);
                }

                if (maxHeight <= targetHeigth)
                {
                    targetHeigth = maxHeight;
                    targetWidth = Convert.ToInt32(targetHeigth/imageRatio);
                }
            }

            var bTemp = (Bitmap) b.Clone();
            try
            {
                b = new Bitmap(targetWidth, targetHeigth, bTemp.PixelFormat);
            }
            catch (ArgumentException)
            {
                // incorrect type
                return false;
            }
            catch
            {
                // other kind
                return false;
            }
            double nXFactor = bTemp.Width/(double) targetWidth;
            double nYFactor = bTemp.Height/(double) targetHeigth;

            if (isBilinear)
            {
                // optimization
                double fraction_x, fraction_y, one_minus_x, one_minus_y;
                int ceil_x, ceil_y, floor_x, floor_y;
                var c1 = new Color();
                var c2 = new Color();
                var c3 = new Color();
                var c4 = new Color();
                byte red, green, blue;

                byte b1, b2;

                for (int x = 0; x < b.Width; ++x)
                    for (int y = 0; y < b.Height; ++y)
                    {
                        // Setup
                        floor_x = (int) Math.Floor(x*nXFactor);
                        floor_y = (int) Math.Floor(y*nYFactor);
                        ceil_x = floor_x + 1;
                        if (ceil_x >= bTemp.Width) ceil_x = floor_x;
                        ceil_y = floor_y + 1;
                        if (ceil_y >= bTemp.Height) ceil_y = floor_y;
                        fraction_x = x*nXFactor - floor_x;
                        fraction_y = y*nYFactor - floor_y;
                        one_minus_x = 1.0 - fraction_x;
                        one_minus_y = 1.0 - fraction_y;

                        c1 = bTemp.GetPixel(floor_x, floor_y);
                        c2 = bTemp.GetPixel(ceil_x, floor_y);
                        c3 = bTemp.GetPixel(floor_x, ceil_y);
                        c4 = bTemp.GetPixel(ceil_x, ceil_y);

                        // Blue
                        b1 = (byte) (one_minus_x*c1.B + fraction_x*c2.B);
                        b2 = (byte) (one_minus_x*c3.B + fraction_x*c4.B);
                        blue = (byte) (one_minus_y*(b1) + fraction_y*(b2));

                        // Green
                        b1 = (byte) (one_minus_x*c1.G + fraction_x*c2.G);
                        b2 = (byte) (one_minus_x*c3.G + fraction_x*c4.G);
                        green = (byte) (one_minus_y*(b1) + fraction_y*(b2));

                        // Red
                        b1 = (byte) (one_minus_x*c1.R + fraction_x*c2.R);
                        b2 = (byte) (one_minus_x*c3.R + fraction_x*c4.R);
                        red = (byte) (one_minus_y*(b1) + fraction_y*(b2));


                        if (b.PixelFormat == PixelFormat.Format8bppIndexed)
                        {
                            b = CreateNonIndexedImage(b);
                        }

                        b.SetPixel(x, y, Color.FromArgb(255, red, green, blue));
                    }
            }
            else
            {
                for (int x = 0; x < b.Width; ++x)
                    for (int y = 0; y < b.Height; ++y)
                        b.SetPixel(x, y, bTemp.GetPixel((int) (Math.Floor(x*nXFactor)),
                                                        (int) (Math.Floor(y*nYFactor))));
            }

            // save it
            try
            {
                b.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                b.Dispose();
                s.Dispose();
            }
        }


        public static Bitmap CreateNonIndexedImage(Image src)
        {
            var newBmp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);

            using (Graphics gfx = Graphics.FromImage(newBmp))
            {
                gfx.DrawImage(src, 0, 0);
            }

            return newBmp;
        }

        #endregion

        #region ipaddress look up

        /// <summary>
        ///     Given the IP Address, get back a dataset of the location
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        /// <see
        ///     cref=">http://www.aspsnippets.com/post/2009/04/12/Find-Visitors-Geographic-Location-using-IP-Address-in-ASPNet.aspx" />
        public static DataTable GetLocation(string ipaddress)
        {
            //Create a WebRequest
            WebRequest rssReq = WebRequest.Create("http://freegeoip.appspot.com/xml/" + ipaddress);

            //Create a Proxy
            var px = new WebProxy("http://freegeoip.appspot.com/xml/" + ipaddress, true);

            //Assign the proxy to the WebRequest
            rssReq.Proxy = px;

            //Set the timeout in Seconds for the WebRequest
            rssReq.Timeout = 2000;

            try
            {
                //Get the WebResponse 
                WebResponse rep = rssReq.GetResponse();

                //Read the Response in a XMLTextReader
                var xtr = new XmlTextReader(rep.GetResponseStream());

                //Create a new DataSet
                var ds = new DataSet();

                //Read the Response into the DataSet
                ds.ReadXml(xtr);
                return ds.Tables[0];
            }
            catch
            {
                return null;
            }
            ////////////////////////////////////////////////////////////////
            /// EXAMPLE USE:
            /// 
            //Get IP Address
            //string ipaddress;

            //ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //if (string.IsNullOrEmpty(ipaddress))
            //{
            //    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            //}

            //DataTable dt = GetLocation(ipaddress);
            //if (dt != null)
            //{
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        litLocationCity.Text = dt.Rows[0]["RegionName"].ToString();//state
            //        //lblCity.Text = dt.Rows[0]["City"].ToString();
            //        //lblRegion.Text = dt.Rows[0]["RegionName"].ToString();
            //        //lblCountry.Text = dt.Rows[0]["CountryName"].ToString();
            //        //lblCountryCode.Text = dt.Rows[0]["CountryCode"].ToString();
            //    }
            //    else
            //    {

            //    }
            //}

            //        // more
            //            /// <summary>
            ///// Set the drop down list to the value that is the 
            ///// user's state
            ///// </summary>
            //private void SetUserState()
            //{
            //    // TODO: improve this functionality later
            //    return;

            //    //Get IP Address
            //    string ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //    if (string.IsNullOrEmpty(ipaddress))
            //    {
            //        ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            //    }

            //    if (string.IsNullOrEmpty(ipaddress)) return;

            //    DataTable dt = FT.Operational.Utilities.GetLocation(ipaddress);

            //    if (dt != null)
            //    {
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            ddlState.SelectedItem.Text = dt.Rows[0]["RegionName"].ToString();
            //        }
            //    }
            // }
        }

        #endregion

        #region time

        public static DateTime GetDataBaseTime()
        {
            double totalSecondsDif = 0;
            string cacheName = "db_time";

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetCurrentTime";

                // execute the stored procedure
                DateTime dateT = FromObj.DateFromObj(DbAct.ExecuteScalar(comm));

                TimeSpan span = DateTime.UtcNow.Subtract(dateT);

                totalSecondsDif = span.TotalSeconds;

                HttpContext.Current.Cache.AddObjToCache(totalSecondsDif, cacheName);
            }
            else
            {
                totalSecondsDif = (double) HttpContext.Current.Cache[cacheName];
            }


            return DateTime.UtcNow.AddSeconds(totalSecondsDif);
        }

        public static string GetUTCNowYYYYMMDDHHMM()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmm");
        }

        #endregion

        #region web controls

        /// <summary>
        ///     Add this many years from now to the drop down list
        /// </summary>
        /// <param name="ddlYears"></param>
        /// <param name="j"></param>
        public static void AddYearsToDropDownList(ref DropDownList ddlYears, int j)
        {
            if (ddlYears == null) return;

            ddlYears.Items.Clear();

            for (int i = 0; i < j; i++)
                ddlYears.Items.Add(new ListItem(DateTime.UtcNow.AddYears(i).Year.ToString()));
        }

        #endregion

        #region file system

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                // Console.WriteLine(@”Copying {0}\{1}”, target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        public static void CopyAll(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyAll(folder, dest);
            }
        }

        #endregion

        #region file system types

        public static string MimeType(string Filename)
        {
            try
            {
                string mime = "application/octetstream";
                string ext = Path.GetExtension(Filename).ToLower();
                RegistryKey rk = Registry.ClassesRoot.OpenSubKey(ext);
                if (rk != null && rk.GetValue("Content Type") != null)
                    mime = rk.GetValue("Content Type").ToString();
                return mime;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return string.Empty;
            }
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
                var rm = new ResourceManager("BootBaronLib.Resources.Messages", Assembly.GetExecutingAssembly());

                return rm.GetString(resourceKey);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region cookies

        public static void CookieMaker(
            HttpCookie processorCookie,
            string coookieValue,
            SiteEnums.CookieName cn)
        {
            processorCookie = HttpContext.Current.Request.Cookies[cn.ToString()];

            // if the cookie exists, see if the value for the contact exists
            if (processorCookie != null)
            {
                // check if the cookievalue exists, remove it
                if (processorCookie.Values[cn.ToString()] != null)
                {
                    processorCookie.Values.Remove(cn.ToString());
                }
            }
            else
            {
                // make a new cookie
                processorCookie = new HttpCookie(cn.ToString());
            }

            // add the new value to the cookie, the new contact id
            processorCookie[cn.ToString()] = coookieValue;
            processorCookie.Expires = DateTime.Now.AddDays(7);

            HttpContext.Current.Response.Cookies.Add(processorCookie);
        }


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

        #region domain settings

        /// <summary>
        ///     When domains are passed to this, it will only allow the page
        ///     to be acceessible from this domain, other domains to which
        ///     point at this file will be rejected to a 404 page
        /// </summary>
        /// <param name="allowableDomains">the allowable domains as in whatever.com</param>
        public static void AllowOnlyDomains(ArrayList allowableDomains)
        {
            string currentDomain = GetCurrentDomain().ToLower().Trim();

            foreach (string s in allowableDomains)
            {
                if (s.ToLower().Trim().Contains(currentDomain))
                {
                    return;
                }
            }
            HttpContext.Current.Response.Redirect("~/Errors/404.htm");
        }

        /// <summary>
        ///     Get the current domain name, excluding the www. as in: whatever.com
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomain()
        {
#if !(DEBUG)
            try
            {
                Uri url = new Uri(HttpContext.Current.Request.Url.ToString());
                return url.Host.Replace("www.", string.Empty).ToLower(); 
            }
            catch { return string.Empty; }

             #endif
#if (DEBUG)
            return string.Empty;
#endif
        }

        /// <summary>
        ///     Take the current incoming URL, check if it's secure
        ///     if it is not secure, redirect to the current page securely,
        ///     changing the url from http:// to https:// lowercase
        /// </summary>
        public static void MakeSecurePage()
        {
#if !(DEBUG)
    // if (HttpContext.Current.IsDebuggingEnabled) return;

    // THIS DOESN'T WORK IN AN ASSMBLY FOR THE WEBSITE
            
            #endif

            // for some reason this is not working correctly, it's saying it's secure and not localhost when it's only http
            if (!HttpContext.Current.Request.IsSecureConnection &&
                !HttpContext.Current.Request.Url.ToString().ToLower().Contains("localhost"))
            {
                HttpContext.Current.Response.Redirect
                    (
                        HttpContext.Current.Request.Url.ToString().ToLower().Replace("http://", "https://")
                    );
            }
        }

        /// <summary>
        ///     Used to redirect to SSL page with the requirement of having
        ///     and SSL
        /// </summary>
        /// <param name="requiresWWW"></param>
        public static void MakeSecurePage(bool requiresWWW)
        {
            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("localhost"))
            {
                return;
            }

#if !(DEBUG)
    // if (HttpContext.Current.IsDebuggingEnabled) return;

    // THIS DOESN'T WORK IN AN ASSMBLY FOR THE WEBSITE
            
            #endif

            if (requiresWWW &&
                !HttpContext.Current.Request.Url.ToString().ToLower().Contains("https://www.") &&
                HttpContext.Current.Request.Url.ToString().ToLower().Contains("https://"))
            {
                // needs WWW and does not contains it
                HttpContext.Current.Response.Redirect(
                    HttpContext.Current.Request.Url.ToString().ToLower().Replace("https://", "https://www."));
            }
            //

            if (!HttpContext.Current.Request.IsSecureConnection)
            {
                if (requiresWWW && !HttpContext.Current.Request.Url.ToString().ToLower().Contains("http://www."))
                {
                    // needs WWW and does not contain it
                    HttpContext.Current.Response.Redirect(
                        HttpContext.Current.Request.Url.ToString().ToLower().Replace("http://", "https://www."));
                }
                else if (!requiresWWW && HttpContext.Current.Request.Url.ToString().ToLower().Contains("http://www."))
                {
                    // does not require a WWW but does contain it
                    HttpContext.Current.Response.Redirect(
                        HttpContext.Current.Request.Url.ToString().ToLower().Replace("http://www.", "https://"));
                }
                else if (!requiresWWW && HttpContext.Current.Request.Url.ToString().ToLower().Contains("https://www."))
                {
                    // this may be impossible to hit if it's a non-WWW site that has a WWW already
                    HttpContext.Current.Response.Redirect(
                        HttpContext.Current.Request.Url.ToString().ToLower().Replace("https://www.", "https://"));
                }
                else if (requiresWWW && HttpContext.Current.Request.Url.ToString().ToLower().Contains("http://www."))
                {
                    // needs WWW and contains it
                    HttpContext.Current.Response.Redirect(
                        HttpContext.Current.Request.Url.ToString().ToLower().Replace("http://", "https://"));
                }

                else
                {
                    // replace to non WWW 
                    HttpContext.Current.Response.Redirect(
                        HttpContext.Current.Request.Url.ToString().ToLower().Replace("http://", "https://"));
                }
            }
        }

        #endregion

        #region bytes

        /// <summary>
        ///     To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        public static String UTF8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        ///     Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        public static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            var encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        #endregion

        #region pixel loading

        #endregion

        #region string formatting

        /// <summary>
        ///     Returns a red string, centered
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ErrorMessage(string msg)
        {
            return @"<center><span style=""color: red;background-color: white;"">" + msg + @"</span></center>";
        }

        /// <summary>
        ///     Returns the a string of numbers
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string ExtractNumbers(string expr)
        {
            if (expr == null) return string.Empty;

            return string.Join(null, Regex.Split(expr, "[^\\d]"));
        }

        #endregion

        #region math

        /// <summary>
        ///     Random percent of times true out of 100
        /// </summary>
        /// <param name="percentTrue"></param>
        /// <returns></returns>
        public static bool RandomTrueOrFalse(int percentTrue)
        {
            int i = new Random().Next(100);

            if (i <= percentTrue) return true;
            else return false;
        }

        /// <summary>
        ///     How many years have passed since birthdate
        /// </summary>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        public static int CalculateAge(DateTime birthDay, DateTime currentDate)
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
        ///     A message to debug and the actual exception
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void LogError(string msg, Exception ex)
        {
            LogError(msg, ex, true);
        }

        /// <summary>
        ///     The base method that writes to the file system and sends mail
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <param name="sendMail"></param>
        private static void LogError(string msg, Exception ex, bool sendMail)
        {
            Log.Error(msg, ex);

            var exMessage = new StringBuilder();

            HttpContext context = HttpContext.Current;

            string serverName = string.Empty;

            if (context != null && context.Server != null && context.Server.MachineName != null)
            {
                serverName = HttpContext.Current.Server.MachineName;
            }
            else
            {
                serverName = Environment.MachineName;
            }

            if (context != null)
            {
                if (ex != null)
                {
                    if (
                        ex.Message.Contains(
                            "A potentially dangerous Request.Path value was detected from the client (&)"))
                        return; // probably a bot
                }

                if (ex != null &&
                    ex.Message != null &&
                    (ex.Message.Contains(
                        "A connection was successfully established with the server, but then an error occurred during the login process. (provider: TCP Provider, error: 0 - The specified network name is no longer available.)") ||
                     ex.Message.Contains(
                         "Message: A transport-level error has occurred when receiving results from the server") ||
                     ex.Message.Contains(
                         "A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)") ||
                     ex.Message.Contains(
                         "Timeout expired.")
                    )
                    )
                {
                    //System.Threading.Thread.Sleep(1000); // last time this was too long and crashed the site, hopefully this time that is not the case

                    if (
                        Convert.ToInt32(context.Application[SiteEnums.ApplicationVariableNames.ErrorCount.ToString()]) ==
                        0)
                    {
                        //TODO: FIGURE THIS OUT, KEEPS EMAILING HUNDREDS OF TIMES
                        //SendMail(
                        //        Configs.GeneralConfigs.SendToErrorEmail,
                        //        Configs.AmazonCloudConfigs.SendFromEmail, "DB CON DOWN: " + serverName, ex.ToString());

                        context.Application[SiteEnums.ApplicationVariableNames.LogError.ToString()] = false;
                    }


                    Log.Fatal("Error logging everything");

                    return; // do not log more
                }
                else
                {
                    Log.Debug("EXCEPTION", ex);
                }
            }

            try
            {
                exMessage.Append("UTC: " + String.Format("{0:u}", DateTime.UtcNow));

                if (!string.IsNullOrEmpty(msg))
                {
                    exMessage.Append("\n\n Debug Message: " + msg);
                }


                if (ex != null)
                {
                    // build the error message
                    if (!string.IsNullOrEmpty(ex.Message))
                    {
                        exMessage.Append("\n\n Message: " + ex.Message);
                    }
                    if (!string.IsNullOrEmpty(ex.Source))
                    {
                        exMessage.Append("\n\n Source: " + ex.Source);
                    }
                    if (ex.TargetSite != null)
                    {
                        exMessage.Append("\n\n Method: " + ex.TargetSite);
                    }
                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        exMessage.Append("\n\n Stack Trace: \n\n" + ex.StackTrace);
                    }
                    if (ex.InnerException != null)
                    {
                        exMessage.Append("\n\n Inner Exception: \n\n" + ex.InnerException);
                    }
                }


                if (context != null)
                {
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
                    if (context.Request.Headers != null)
                    {
                        exMessage.Append("\n\n Headers: " + context.Request.Headers);
                    }
                    if (!string.IsNullOrEmpty(context.Request.ApplicationPath))
                    {
                        exMessage.Append("\n\n Application Path: " + context.Request.ApplicationPath);
                    }
                    if (!string.IsNullOrEmpty(serverName))
                    {
                        exMessage.Append("\n\n Server Name: " + serverName);
                    }
                    else
                    {
                        serverName = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(context.Request.RawUrl))
                    {
                        exMessage.Append("\n\n Page Location: " + context.Request.RawUrl);
                    }
                    if (!string.IsNullOrEmpty(context.Request.UserHostAddress))
                    {
                        exMessage.Append("\n\n Full Page URL: " + context.Request.Url);
                    }
                }
            }
            catch
            {
                //SendMail(
                //      Configs.GeneralConfigs.SendToErrorEmail,
                //      Configs.AmazonCloudConfigs.SendFromEmail,
                //    "Error On: " +
                //    serverName,
                //    "EXCEPTION MAIN: " + exMessage.ToString() +
                //    " EXCEPTION LOGGING: " + ex2.ToString());
            }


            if (GeneralConfigs.EnableErrorLogEmail && sendMail)
            {
                string messageError = exMessage.ToString();

                if (messageError.ToLower().Contains("network") ||
                    messageError.ToLower().Contains("timeout"))
                {
                    // annoying messages
                    return;
                }
                else
                {
                    SendMail(
                        GeneralConfigs.SendToErrorEmail,
                        AmazonCloudConfigs.SendFromEmail,
                        "Error On: " + serverName, exMessage.ToString());
                }
            }

            if (context == null) return;

           
            var el = new ErrorLog {Message = exMessage.ToString()};

            if (el.Message.Contains("Timeout expired.")) return;

            var mu = Membership.GetUser();

            if (mu != null)
            {
                el.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
            }

            el.Url = HttpContext.Current.Request.Url.ToString();
            if (el.Message.ToLower().Contains("@transport-level") || el.Message.ToLower().Contains("@tcp-provider") ||
                el.Message.ToLower().Contains("@network-related")) return;
            el.ResponseCode = -1; // not following 
            // hopeless database call else
            el.Create();
        }

        private static void SendMail(bool p, string p_2, string p_3, string p_4)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Log an exception to a file and mail it
        /// </summary>
        /// <param name="ex"></param>
        public static void LogError(Exception ex)
        {
            LogError(string.Empty, ex, true);
        }

        /// <summary>
        ///     Log the exception with mail sending option
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sendMail"></param>
        public static void LogError(Exception ex, bool sendMail)
        {
            LogError(string.Empty, ex, sendMail);
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
        public static void LogError(string msg, bool sendEmail)
        {
            LogError(msg, null, sendEmail);
        }

        #endregion

        #region FTP

        /// <summary>
        ///     FTP upload a file
        /// </summary>
        /// <param name="ftpAddress">the IP address or host name</param>
        /// <param name="directoryOnFTPPath">
        ///     specifies a path within the FTP
        ///     example: MyFolderName/ or MyFolderName/MySubFolderName/
        /// </param>
        /// <param name="fileToUploadPath"></param>
        /// <param name="username">username for FTP authentication</param>
        /// <param name="password">password for FTP authentication</param>
        /// <see cref="http://www.vcskicks.com/csharp_ftp_upload.php" />
        public static bool FTPFileUpload(string ftpAddress, string directoryOnFTPPath, string fileToUploadPath,
                                         string username, string password)
        {
            if (!ftpAddress.StartsWith("ftp://")) ftpAddress = "ftp://" + ftpAddress;

            FileStream stream = null;
            Stream reqStream = null;

            try
            {
                //Create FTP request
                var request =
                    (FtpWebRequest)
                    WebRequest.Create(ftpAddress + "/" + directoryOnFTPPath + Path.GetFileName(fileToUploadPath));

                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(username, password);


                request.UsePassive = true; // true for 21, false for 22
                request.UseBinary = true;
                request.KeepAlive = false;


                //Load the file
                stream = File.OpenRead(fileToUploadPath);
                var buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                //Upload file
                reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();
            }
            catch (Exception ex)
            {
                LogError("FTP UPLOAD EXCEPTION", ex);
                return false;
            }
            finally
            {
                stream.Dispose();
                reqStream.Dispose();
            }

            return true;
        }

        /// <summary>
        ///     FTP Upload of file to default directory
        /// </summary>
        /// <param name="ftpAddress">the IP address or host name</param>
        /// <param name="fileToUploadPath"></param>
        /// <param name="username">username for FTP authentication</param>
        /// <param name="password">password for FTP authentication</param>
        /// <see cref="http://www.vcskicks.com/csharp_ftp_upload.php" />
        public static bool FTPFileUpload(string ftpAddress, string fileToUploadPath, string username, string password)
        {
            return FTPFileUpload(ftpAddress, string.Empty, fileToUploadPath, username, password);
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
            string responseData = string.Empty;

            WebRequest request = WebRequest.Create(input) as HttpWebRequest;
            request.Method = SiteEnums.HTTPTypes.GET.ToString();

            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK) return true;
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(dataStream))
                        {
                            responseData = reader.ReadToEnd();
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
                    else
                    {
                        // Do something else
                    }
                }
                else
                {
                    // Do something else
                }
            }
            catch
            {
                // Utilities.LogError(debugMsg, ex);
            }

            return false;
        }

        /// <summary>
        ///     Given the URL, do a GET request and return the response
        /// </summary>
        /// <param name="input"></param>
        /// <param name="debugMsg"></param>
        /// <returns></returns>
        public static string GETRequest(Uri input, string debugMsg)
        {
            string responseData = string.Empty;

            WebRequest request = WebRequest.Create(input) as HttpWebRequest;
            request.Method = SiteEnums.HTTPTypes.GET.ToString();

            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
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
            return responseData;
        }

        /// <summary>
        ///     POST a string to a URI and return the result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sendTo"></param>
        /// <returns></returns>
        public static string POSTRequest(string input, Uri sendTo, string debugMsg)
        {
            var whc = new WebHeaderCollection();

            whc.Add(HttpRequestHeader.ContentType, GetEnumDescription(SiteEnums.FormContentTypes.URLENCODE));

            return POSTRequest(input, sendTo, whc, debugMsg);
        }

        /// <summary>
        ///     POST a string to a URI and return the result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sendTo"></param>
        /// <returns></returns>
        public static string POSTRequest(string input, Uri sendTo)
        {
            var whc = new WebHeaderCollection();

            whc.Add(HttpRequestHeader.ContentType, GetEnumDescription(SiteEnums.FormContentTypes.URLENCODE));

            return POSTRequest(input, sendTo, whc, sendTo.Host);
        }

        /// <summary>
        ///     POST a string to a URI with headers and return the result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sendTo"></param>
        /// <param name="whc"></param>
        /// <returns></returns>
        public static string POSTRequest(string input, Uri sendTo, WebHeaderCollection whc, string debugMsg)
        {
            return POSTRequest(input, sendTo, whc, debugMsg, false);
        }


        public static string POSTRequest(string input, Uri sendTo, WebHeaderCollection whc, string debugMsg,
                                         bool byPassAllCertificates)
        {
            string toReturn = string.Empty;

            using (var wc = new WebClient())
            {
                wc.Headers = whc;

                if (byPassAllCertificates)
                {
                    ServicePointManager.ServerCertificateValidationCallback += ByPassAllCertificates;
                }

                try
                {
                    toReturn = wc.UploadString(sendTo.AbsoluteUri, SiteEnums.HTTPTypes.POST.ToString(), input);
                    return toReturn;
                }
                catch
                {
                    //// log the post
                    //ErrorLog ell = new ErrorLog();

                    //ell.CurrentPage = sendTo.ToString();
                    //ell.LogMessage = input;
                    //ell.DebugMessage = debugMsg;
                    //ell.IsException = true;
                    //if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    //    ell.IpAddress = HttpContext.Current.Request.UserHostAddress;
                    //ell.ServerStatusCode = "-1";

                    //HttpCookie offerCookie = HttpContext.Current.Request.Cookies[SiteEnums.CookieName.chocolatechip.ToString()];

                    //int contactUserID = 0;

                    //if (offerCookie != null &&
                    //    offerCookie.Values[SiteEnums.CookieValue.contactuserid.ToString()] != null)
                    //    contactUserID = Convert.ToInt32(offerCookie[SiteEnums.CookieValue.contactuserid.ToString()]);

                    //ell.ContactUserID = contactUserID;

                    //ell.Create();

                    //// log the exception
                    //Utilities.LogError(debugMsg, ex);
                }
            }

            return string.Empty;
        }


        /// <summary>
        ///     Delegate to ignore the SSL credentials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        /// <see cref=">http://stackoverflow.com/questions/536352/webclient-https-issues" />
        private static bool ByPassAllCertificates(object sender, X509Certificate cert, X509Chain chain,
                                                  SslPolicyErrors error)
        {
            //always accepts
            return true;
        }

        #endregion

        #region auto form submit

        public static void SubmitHTMLForm(string formName, string postToURL, NameValueCollection nvc)
        {
            //NameValueCollection nvc1 = new NameValueCollection();

            //nvc1.Add("sForename", "fname");
            //nvc1.Add("sSurname", "lname");
            //nvc1.Add("sEmail", "test@aol.com");
            //nvc1.Add("sTelephone", "1234567890");
            //nvc1.Add("sMobile", "");
            //nvc1.Add("sAddr1", "123 fake street");
            //nvc1.Add("sAddr2", "");
            //nvc1.Add("sAddr3", "");
            //nvc1.Add("sTown", "cityname");
            //nvc1.Add("sCounty", "FL");
            //nvc1.Add("sPostcode", "12344");
            //nvc1.Add("sCountry", "US");
            //nvc1.Add("sPassword", "password");
            //nvc1.Add("cPassword", "password");
            //nvc1.Add("date", "2010-06-16");
            ////  nvc1.Add("submit", "Sign Me Up!");
            //nvc1.Add("sCouponCode", "");
            ////nvc1.Add("MM_insert", "frmSignUp");

            //Utilities.SubmitHTMLForm("frmSignUp", "http://www.twittertacticsrevealed.com/members/signup.php", nvc1);


            HttpContext.Current.Response.Clear();

            var sb = new StringBuilder();

            sb.AppendFormat("<html><body onload='document.{0}.submit()'>", formName);

            sb.AppendFormat("<form method=\"POST\" action=\"{0}\" name=\"{1}\" id=\"{1}\">", postToURL, formName);

            string[] values = null;

            foreach (string key in nvc.Keys)
            {
                values = nvc.GetValues(key);

                foreach (string value in values)
                    sb.AppendFormat("<input type=hidden name=\"{0}\" value=\"{1}\">", key, value);
            }

            sb.Append("</form></body></html>");

            HttpContext.Current.Response.Write(sb.ToString());
        }

        #endregion

        #region datetime

        /// <summary>
        ///     Given the start and end date, return a list of days inbetween the range as
        /// </summary>
        /// <param name="startingDate"></param>
        /// <param name="endingDate"></param>
        /// <returns></returns>
        /// <see
        ///     cref="http://geekswithblogs.net/thibbard/archive/2007/03/01/CSharpCodeToGetGenericListOfDatesBetweenStartingAndEndingDate.aspx" />
        public static List<DateTime> GetDateRange(DateTime startingDate, DateTime endingDate)
        {
            if (startingDate > endingDate) return null;

            var rv = new List<DateTime>();
            DateTime tmpDate = startingDate;

            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= endingDate);

            return rv;
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        public static double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        #endregion

        #region query strings

        /// <summary>
        ///     Replace a key with a value in the current URL
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueToReplace"></param>
        /// <returns></returns>
        public static string ReplaceQueryStringValue(string key, string valueToReplace)
        {
            var filtered = new NameValueCollection(HttpContext.Current.Request.QueryString);
            filtered.Remove(key);
            filtered.Add(key, valueToReplace);

            string currentURL = HttpContext.Current.Request.Url.ToString().Replace(
                HttpContext.Current.Request.Url.Query, string.Empty);

            var newQS = new StringBuilder();

            foreach (string  kvp in filtered.Keys)
            {
                newQS.Append("&");
                newQS.Append(kvp);
                newQS.Append("=");
                newQS.Append(filtered.Get(kvp));
            }

            return currentURL += "?" + newQS;
        }

        /// <summary>
        ///     Replace in a Full URL
        /// </summary>
        /// <param name="fullurlToModify"></param>
        /// <param name="key"></param>
        /// <param name="valueToReplace"></param>
        /// <returns></returns>
        public static string ReplaceQueryStringValue(
            Uri fullurlToModify,
            string key,
            string valueToReplace)
        {
            NameValueCollection filtered = HttpUtility.ParseQueryString(fullurlToModify.Query);

            filtered.Remove(key);
            filtered.Add(key, valueToReplace);

            string currentURL = fullurlToModify.ToString().Replace(
                fullurlToModify.Query, string.Empty);

            var newQS = new StringBuilder();

            foreach (string kvp in filtered.Keys)
            {
                newQS.Append("&");
                newQS.Append(kvp);
                newQS.Append("=");
                newQS.Append(filtered.Get(kvp));
            }

            return currentURL += "?" + newQS;
        }

        /// <summary>
        ///     Take a namevaluecollection and turn it into querystring parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="delimiter"></param>
        /// <param name="omitEmpty"></param>
        /// <returns></returns>
        public static String ConstructQueryString(NameValueCollection parameters, String delimiter, Boolean omitEmpty)
        {
            if (String.IsNullOrEmpty(delimiter))
                delimiter = "&";

            Char equals = '=';
            var items = new List<String>();

            for (int i = 0; i < parameters.Count; i++)
            {
                foreach (String value in parameters.GetValues(i))
                {
                    Boolean addValue = (omitEmpty) ? !String.IsNullOrEmpty(value) : true;
                    if (addValue)
                        items.Add(String.Concat(parameters.GetKey(i), equals, HttpUtility.UrlEncode(value)));
                }
            }

            return String.Join(delimiter, items.ToArray());
        }

        public static String ConstructQueryString(NameValueCollection parameters)
        {
            return ConstructQueryString(parameters, "&", false);
        }

        /// <summary>
        ///     Get a value from the string
        /// </summary>
        /// <param name="query"></param>
        /// <param name="keyOfValue"></param>
        /// <returns></returns>
        public static string GetQueryStringValue(string query, string keyOfValue)
        {
            var nvc = new NameValueCollection(
                HttpUtility.ParseQueryString(HttpContext.Current.Server.UrlDecode(query)));

            return nvc[keyOfValue];
        }

        #endregion

        #region enum methods

        /// <summary>
        ///     Get the enum name by using it's description
        /// </summary>
        /// <param name="value"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string GetEnumName(Type value, string description)
        {
            FieldInfo[] fis = value.GetFields();
            foreach (FieldInfo fi in fis)
            {
                var attributes =
                    (DescriptionAttribute[]) fi.GetCustomAttributes
                                                 (typeof (DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    if (attributes[0].Description == description)
                    {
                        return fi.Name;
                    }
                }
            }
            return description;
        }

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

        public class CGWebClient : WebClient
        {
            private CookieContainer cookieContainer;
            private int timeout;
            private string userAgent;

            public CGWebClient()
            {
                timeout = -1;
                userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
                //cookieContainer = new CookieContainer();
                //cookieContainer.Add(new Cookie("example", "example_value"));
            }

            public CookieContainer CookieContainer
            {
                get { return cookieContainer; }
                set { cookieContainer = value; }
            }

            public string UserAgent
            {
                get { return userAgent; }
                set { userAgent = value; }
            }

            public int Timeout
            {
                get { return timeout; }
                set { timeout = value; }
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                RefreshUserAgent();

                if (request.GetType() == typeof (HttpWebRequest))
                {
                    ((HttpWebRequest) request).CookieContainer = cookieContainer;
                    ((HttpWebRequest) request).UserAgent = userAgent;
                    (request).Timeout = timeout;
                }

                return request;
            }

            private void RefreshUserAgent()
            {
                var UserAgents = new List<string>();
                UserAgents.Add("Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)");
                UserAgents.Add(
                    "Mozilla/4.0 (compatible; MSIE 8.0; AOL 9.5; AOLBuild 4337.43; Windows NT 6.0; Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618)");
                UserAgents.Add(
                    "Mozilla/4.0 (compatible; MSIE 7.0; AOL 9.5; AOLBuild 4337.34; Windows NT 6.0; WOW64; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30618)");
                UserAgents.Add(
                    "Mozilla/5.0 (X11; U; Linux i686; pl-PL; rv:1.9.0.2) Gecko/20121223 Ubuntu/9.25 (jaunty) Firefox/3.8");
                UserAgents.Add(
                    "Mozilla/5.0 (Windows; U; Windows NT 5.1; ja; rv:1.9.2a1pre) Gecko/20090402 Firefox/3.6a1pre (.NET CLR 3.5.30729)");
                UserAgents.Add(
                    "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.1b4) Gecko/20090423 Firefox/3.5b4 GTB5 (.NET CLR 3.5.30729)");
                UserAgents.Add(
                    "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Avant Browser; .NET CLR 2.0.50727; MAXTHON 2.0)");
                UserAgents.Add(
                    "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; Media Center PC 6.0; InfoPath.2; MS-RTC LM 8)");
                UserAgents.Add(
                    "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; WOW64; Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; InfoPath.2; .NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618)");
                UserAgents.Add("Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 6.0)");
                UserAgents.Add(
                    "Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.1; Media Center PC 3.0; .NET CLR 1.0.3705; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.1)");
                UserAgents.Add("Opera/9.70 (Linux i686 ; U; zh-cn) Presto/2.2.0");
                UserAgents.Add("Opera 9.7 (Windows NT 5.2; U; en)");
                UserAgents.Add(
                    "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.8pre) Gecko/20070928 Firefox/2.0.0.7 Navigator/9.0RC1");
                UserAgents.Add(
                    "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.1.7pre) Gecko/20070815 Firefox/2.0.0.6 Navigator/9.0b3");
                UserAgents.Add(
                    "Mozilla/5.0 (Windows; U; Windows NT 5.1; en) AppleWebKit/526.9 (KHTML, like Gecko) Version/4.0dp1 Safari/526.8");
                UserAgents.Add(
                    "Mozilla/5.0 (Windows; U; Windows NT 6.0; ru-RU) AppleWebKit/528.16 (KHTML, like Gecko) Version/4.0 Safari/528.16");
                UserAgents.Add("Opera/9.64 (X11; Linux x86_64; U; en) Presto/2.1.1");

                var r = new Random();
                UserAgent = UserAgents[r.Next(0, UserAgents.Count)];

                UserAgents = null;
            }
        }
    }
}