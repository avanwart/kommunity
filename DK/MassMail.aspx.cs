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


            //SendMassMail();
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
                sb.AppendLine("|> This is the official Das Klub 2013 Quarter 1 Update. <| ");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Our Accomplishments:");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("-Completed Just Deux Industrial Dance Contest (36 entries) [ winners: http://dasklub.com/news/just-deux-top-6-winners ]");
                sb.AppendLine();
                sb.AppendLine("-Started and Recently Completed T3RR0R 3RR0R Industrial Dance Contest (28 entries) [ winners: http://dasklub.com/news/t3rr0r-3rr0r-das-klub-dance-contest-winners ]");
                sb.AppendLine();
                sb.AppendLine("-Achieved over 1,000,000 video views on all Das Klub related videos on YouTube/ have more than 50 countries on dasklub.com [ details: http://dasklub.com/news/admin-announcement ]");
                sb.AppendLine();
                sb.AppendLine("-Gained over 10,000 Facebook faces [ https://www.facebook.com/dasklub ]");
                sb.AppendLine();
                sb.AppendLine("-Started Featured Kommunity Member, this includes selected site member interviews (3 so far) [ http://dasklub.com/news/tag/featured-member ]");
                sb.AppendLine();
                sb.AppendLine("-Moved Das Klub open souce code for entire website from Codeplex to GitHub [ https://github.com/dasklub ]");
                sb.AppendLine();
                sb.AppendLine(@"-Updated Das Klub design layout to ""Cyborg"", added new chatroom, improved language support and find user filter saving");
                sb.AppendLine();
                sb.AppendLine("-Officially announced trip to Kinetik Festival May 23nd - May 26th [ http://festival-kinetik.net/ ]");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Your Statistics To Date:");
                sb.AppendLine();
                sb.AppendLine();

               var allUserStatusUpdates = new StatusUpdates();
                allUserStatusUpdates.GetAllUserStatusUpdates(ua1.UserAccountID);

                var totalApplauds = 0;
                var totalBeatDowns = 0;
                var totalStatusPosts = 0;
                foreach (StatusUpdate su1 in allUserStatusUpdates)
                {
                    totalStatusPosts++;

                    var acknowledgements = new Acknowledgements();
                    acknowledgements.GetAcknowledgementsForStatus(su1.StatusUpdateID);

                    foreach (var acknowledgement in acknowledgements)
                    {
                        if (acknowledgement.AcknowledgementType == 'A')
                        {
                            totalApplauds++;
                        }
                        else
                        {
                            totalBeatDowns++;
                        }
                    }
                }


                sb.AppendFormat(@"-Total stautus updates: {0}", totalStatusPosts);
                sb.AppendLine();
                sb.AppendFormat(@"-Total applauds received on status updates: {0}", totalApplauds);
                sb.AppendLine();
                sb.AppendFormat(@"-Total beatdowns received on status updates: {0}", totalBeatDowns);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Spring Greetings,");
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

                if (Utilities.SendMail(ua1.EMail, ua1.UserName + " [ Das Klub Q-1 Update ]", sb.ToString()))
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