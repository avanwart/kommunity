using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.VideoContest;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DasKlub.Web
{
    public partial class MassMail : Page
    {
        private IMailService _mail;

        public MassMail(IMailService mail)
        {
            _mail = mail;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder(100);

            sb.AppendFormat("Hello,");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("The Das Klub Forum has been revitalized!");
            sb.AppendLine();
            sb.AppendLine("Go to: http://dasklub.com and see the newest and most popular forum threads.");
            sb.AppendLine();
            sb.AppendLine("More detail: http://dasklub.com/forum");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("How it works:");
            sb.AppendLine();
            sb.AppendLine(
                "When you leave a post, other users subscribed to that post get a notification email. You can see a link in green when on http://dasklub.com for any forum post you're subscribed to that you you haven't read.");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Guten Tanzen,");
            sb.AppendLine();
            sb.AppendLine("| The Admin |");
            sb.AppendLine();
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
            sb.AppendLine(
                "To unsubscribe from all future email communication, go to: http://dasklub.com/unsubscribe.aspx");

           // SendMassMail(sb.ToString());
        }

        private static void SendMassMail(string message)
        {
            var totalSent = 0;

            var uas = new UserAccounts();
            uas.GetAll();

            foreach (var ua1 in uas.OrderBy(x => x.CreateDate))
                //.Where(ua1 => ua1.CreateDate <= DateTime.UtcNow.AddDays(-5)))
            {
                
                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua1.UserAccountID);

                if (!uad.EmailMessages || ua1.UserName != "bootlegbaron") continue;
                 
                //if (_mail.SendMail(ua1.EMail, "Das Klub Forum Revitalized!", message))
                //{
                //    totalSent++;
                //}
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

            foreach (Contest c1 in conts)
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
                                  new Uri("http://gdata.youtube.com/feeds/api/videos/" + v1.ProviderKey +
                                          @"?v=2&alt=json"))
                          into s where !string.IsNullOrWhiteSpace(s) select (JObject) JsonConvert.DeserializeObject(s)
                          into JObj select JObj["entry"]
                          into entry from thumbnail in entry["yt$statistics"]
                          where !thumbnail.ToString().Contains("fav")
                          select
                              Convert.ToInt32(thumbnail.ToString()
                                                       .Replace(@"""viewCount"": """, string.Empty)
                                                       .Replace(@"""", string.Empty))).Sum();
            }
        }
    }
}
