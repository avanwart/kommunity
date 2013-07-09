using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Operational;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DasKlub.Web
{
    public partial class MassMail : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //  VideoCount();


           //  SendMassMail();
        }

        private static void SendMassMail()
        {
            int totalSent = 0;

            var uas = new UserAccounts();
            uas.GetAll();

            foreach (UserAccount ua1 in uas.OrderBy(x => x.CreateDate))
                //.Where(ua1 => ua1.CreateDate <= DateTime.UtcNow.AddDays(-5)))
            {
                // if( ContestVideo.IsUserContestVoted(ua1.UserAccountID, 9) )continue;


                var sb = new StringBuilder(100);

                sb.AppendFormat("Hello {0},", ua1.UserName);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Support Das Klub and be ELITE!");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Go to: http://dasklub.com/store and BUY the NEW Gear and Skulls T-Shirt. It's an amazing design that will start conversations and turn heads... and make them fall off.");
                sb.AppendLine();
                //sb.AppendLine("-Edited and raw videos will be judged as different contests (2 total winners)");
                //sb.AppendLine();
                //sb.AppendLine("-This is the last Das Klub Dance Contest of 2013 and probably the last one ever!");
                //sb.AppendLine();
                //sb.AppendLine("-Deadline: 2013-05-20");
                //sb.AppendLine();
                //sb.AppendLine("-Winners announced at Kinetik Festival");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Support the cause or get out of the way,");
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
                sb.AppendLine(
                    "To unsubscribe from all future email communication, go to: http://dasklub.com/unsubscribe.aspx");

                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua1.UserAccountID);


                if (!uad.EmailMessages || ua1.UserName.Trim().ToLower().Contains("necromatrix") || uad.Sex.Trim() != "Male"
                    || ua1.UserName.ToLower().Contains("edinblack")) continue;

                if (Utilities.SendMail(ua1.EMail, "New Das Klub Male T-Shirt", sb.ToString()))
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