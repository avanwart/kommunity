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
                    sb1.AppendFormat(@"<a rel=""nofollow"" class=""m_over"" href=""{0}"">
                                       <img style=""width:100px;height:100px"" src=""{1}"" alt=""{2}"" title=""{2}"" /></a>
                                                <br />
                                                <div style=""width:100px;margin-bottom:10px;"">
                                                <a rel=""nofollow"" class=""m_over"" href=""{0}"" target=""_blank"">
                                                <span class=""ad_text"">{2}</span></a>
                                                </div>", parts[1], parts[2], parts[0]);
                }
            }
            catch { }

            return sb1.ToString();
        }
    }
}
