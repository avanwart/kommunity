using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Xml;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Operational;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace DasKlub
{
    public partial class MassMail : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //  VideoCount();


         //   SendMassMail();
        }

        private static void SendMassMail()
        {
            var totalSent = 0;

            var uas = new UserAccounts();
            uas.GetAll();

            foreach (var ua1 in uas.OrderBy(x => x.CreateDate))//.Where(ua1 => ua1.CreateDate <= DateTime.UtcNow.AddDays(-5)))
            {
                // if( ContestVideo.IsUserContestVoted(ua1.UserAccountID, 9) )continue;


                var sb = new StringBuilder(100);

                sb.AppendFormat("Hello {0},", ua1.UserName);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Das Klub Presents: Vault-113 Dance Contest");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("-Rules/ Prizes: http://dasklub.com/news/vault-113-industrial-dance-contest");
                sb.AppendLine();
                sb.AppendLine("-Edited and raw videos will be judged as different contests (2 total winners)");
                sb.AppendLine();
                sb.AppendLine("-This is the last Das Klub Dance Contest of 2013 and probably the last one ever!");
                sb.AppendLine();
                sb.AppendLine("-Deadline: 2013-05-20");
                sb.AppendLine();
                sb.AppendLine("-Winners announced at Kinetik Festival");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Dance Now Or Never,");
                sb.AppendLine();
                sb.AppendLine("[ admin ] | RMW");
                sb.AppendLine();
                sb.AppendLine("Contact Email: info@dasklub.com");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("To unsubscribe from all future email communication, go to: http://dasklub.com/unsubscribe.aspx");

                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua1.UserAccountID);


                if (!uad.EmailMessages) continue;

                if (Utilities.SendMail(ua1.EMail, "The Last Das Klub Dance Contest EVER?", sb.ToString()))
                {
                    totalSent++;
                }
            }

            HttpContext.Current.Response.Write(totalSent);
        }

        private static void VideoCount()
        {
            var vids = new Videos();
            var conts = new Contests();
            conts.GetAll();

            int count = 0;
            int totalVids = 0;

            foreach (var c1 in conts)
            {
                var cv = new ContestVideo();
                var cvs = new ContestVideos();
                cvs.GetContestVideosForContest(c1.ContestID);
                totalVids += cvs.Count;
                count += (from cv2 in cvs
                          select new Video(cv2.VideoID)
                          into v1 let doc = new XmlDocument()
                          select
                              Utilities.GETRequest(
                                  new Uri("http://gdata.youtube.com/feeds/api/videos/" + v1.ProviderKey + @"?v=2&alt=json"))
                          into s where !string.IsNullOrWhiteSpace(s) select (JObject) JsonConvert.DeserializeObject(s)
                          into JObj select JObj["entry"]
                          into entry from thumbnail in entry["yt$statistics"] where !thumbnail.ToString().Contains("fav")
                          select
                              Convert.ToInt32(thumbnail.ToString()
                                                       .Replace(@"""viewCount"": """, string.Empty)
                                                       .Replace(@"""", string.Empty))).Sum();
            }
        }
    }
}