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

        public MassMail( )
        {
           
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            return;
            _mail = new MailService();

            var sb = new StringBuilder(100);

            sb.AppendFormat("Get your FREE Das Klub Sticker!");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Don't miss your chance to get a FREE Das Klub Sticker (while supplies last).");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Just sign in and go to this page: http://dasklub.com/account/useraddress after you fill this form out, a sticker will be sent to you." );
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("-Das Klub's Admin");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Donate here with PayPal: https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=HDQER4PRXHVR8 ");
            sb.AppendLine();
            sb.AppendLine(
                "To unsubscribe from all future email communication, go to: http://dasklub.com/unsubscribe.aspx");

            SendMassMail(sb.ToString());
        }

        private void SendMassMail(string message)
        {
            var totalSent = 0;

            var uas = new UserAccounts();
            uas.GetAll();

            foreach (var ua1 in uas.OrderBy(x => x.CreateDate))
            {
                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua1.UserAccountID);

                if (!uad.EmailMessages) continue;

                var userAddress = new UserAddress();
                userAddress.GetUserAddress(ua1.UserAccountID);

                if (userAddress.UserAddressID != 0) continue;

                 
               //if (_mail.SendMail("dasklubber@gmail.com", ua1.EMail, "FREE Das Klub Sticker!", message))
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
