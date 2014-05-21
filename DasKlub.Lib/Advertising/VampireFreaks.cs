using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DasKlub.Lib.BLL;

namespace DasKlub.Lib.Advertising
{
    public class VampireFreaks
    {
        public static string RandomAdvertisement()
        {
            return string.Empty;// malware
            var sb1 = new StringBuilder(100);

            try
            {
                var doc = new HtmlAgilityPack.HtmlDocument();

                if (HttpContext.Current.Cache["top_week_picks"] == null)
                {
                    using (var wc = new WebClient())
                    {
                        var topPicks = wc.DownloadString("http://store.vampirefreaks.com/?cat=monthly+top+sellers&aff=dasklub&cols=1&numitems=1000");

                        HttpContext.Current.Cache.AddObjToCache(topPicks, "top_week_picks");
                    }
                }

                doc.LoadHtml((string)HttpContext.Current.Cache["top_week_picks"]);

                var adChoices = new Dictionary<int, string>();

                var itemCnt = 0;

                foreach (var table in doc.DocumentNode.SelectNodes("//table"))
                {
                    foreach (var row in table.SelectNodes("tr"))
                    {
                        var link1And2 = string.Empty;

                        var linkText = string.Empty;

                        foreach (var cell in row.SelectNodes("th|td"))
                        {
                            var regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);

                            var mactches = regx.Matches(cell.InnerHtml);
                            linkText = cell.InnerText;

                            foreach (Match match in mactches)
                            {
                                link1And2 += match.Value + "|";
                            }
                        }

                        itemCnt++;

                        adChoices.Add(itemCnt, linkText + "|" + link1And2);
                    }
                }


                var randObj = new Random();

                var parts = adChoices[randObj.Next(0, adChoices.Count - 1)].Split('|');

                if (parts.Length > 3 &&
                     !string.IsNullOrWhiteSpace(parts[0]) &&
                     !string.IsNullOrWhiteSpace(parts[1]) &&
                     !string.IsNullOrWhiteSpace(parts[2]))
                {

                    var linkText = parts[0];
                    const string regexPattern = @"[0-9]+\.[0-9][0-9](?:[^0-9]|$)";
                    var countOfDollarSigns = Regex.Matches(linkText, regexPattern).Count;

                    if (countOfDollarSigns == 2)
                    {
                        var matchFinder = new Regex(regexPattern, RegexOptions.Singleline);
                        var allMatches = matchFinder.Matches(linkText);

                        foreach (var allMatch in allMatches)
                        {
                            // remove first instance of price, it's a pre-discount price
                            var firstMatch = allMatch;
                            linkText = linkText.Replace("$" + firstMatch, string.Empty);
                            break;
                        }
                    }
                    else
                    {
                        // add leading space, which is missing
                        linkText = linkText.Replace("$", " $");
                    }

                    var outboundLink = parts[1].Replace("&", "&amp;");

                    sb1.AppendFormat(@"<a rel=""nofollow"" class=""m_over"" href=""{0}"">
                                       <img style=""height:100px"" src=""{1}"" alt=""{2}"" title=""{2}"" /></a>
                                                <br />
                                                <div style=""width:100px"">
                                                <a rel=""nofollow"" class=""m_over"" href=""{0}"" target=""_blank"">
                                                <span class=""ad_text"">{2}</span></a>
                                                </div>", outboundLink, parts[2], linkText);
                }
            }
            catch { }

            return sb1.ToString();
        }
    }
}
