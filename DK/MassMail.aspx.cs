using System;
using System.Text;
using System.Web.UI;
using System.Xml;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Operational;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DasKlub
{
    public partial class MassMail : Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                foreach (ContestVideo cv2 in cvs)
                {
                    var v1 = new Video(cv2.VideoID);
                    var doc = new XmlDocument();
                    string s =
                        Utilities.GETRequest(
                            new Uri("http://gdata.youtube.com/feeds/api/videos/" + v1.ProviderKey + @"?v=2&alt=json"));
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    var JObj = (JObject) JsonConvert.DeserializeObject(s);

                    JToken entry = JObj["entry"];
                    //string sss = entry["viewCount"]["$t"].ToString();

                    //var playCount = entry["yt$statistics.viewCount.valueOf()" + " views"];

                    //Console.WriteLine("DESC : " + entry["media$group"]["media$description"]["$t"]);

                    foreach (JToken thumbnail in entry["yt$statistics"])
                    {
                        if (thumbnail.ToString().Contains("fav")) continue;
                        count += Convert.ToInt32(
                            thumbnail.ToString()
                                     .Replace(@"""viewCount"": """, string.Empty)
                                     .Replace(@"""", string.Empty));
                    }
                }
            }

            return;

            //var sb = new StringBuilder(100);

            //sb.Append("Dear Valued User,");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine(
            //    "You are with an account that still has the ability to vote in the Just Deux Industrial Dance Contest, there are only 16 hours left.");
            //sb.AppendLine();
            //sb.AppendLine(
            //    "The competition is extremely close for the top 10, 1 single vote can make the difference for the right video. Please watch all the videos and vote on your favorite.");
            //sb.AppendLine();
            //sb.AppendLine("-Go to: http://dasklub.com/account/videovote to cast your vote NOW!");
            //sb.AppendLine("-Only one vote per user, the top 10 with the most votes go on to the next round");
            //sb.AppendLine("-From the top 10, Just Deux will pick their top 6 and they will all get prizes!");
            //sb.AppendLine("-After voting, you can check the voting page to see the results as they come in");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine("Serving Justice in Industrial Dance Since 2011,");
            //sb.AppendLine("RMW");
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine(
            //    "To unsubscribe from all future email communication, go to: http://dasklub.com/unsubscribe.aspx");
















            //UserAccountDetail uad = null;
            //foreach (UserAccount ua1 in uas)
            //{
            //    if (ua1.CreateDate > DateTime.UtcNow.AddDays(-5)) continue;

            //   // if( ContestVideo.IsUserContestVoted(ua1.UserAccountID, 9) )continue;

            //    uad = new UserAccountDetail();
            //    uad.GetUserAccountDeailForUser(ua1.UserAccountID);

            //    if (uad.EmailMessages  )
            //    {
            //        //Utilities.SendMail(ua1.EMail, ua1.UserName + ", last chance to vote, 16 hours left.", sb.ToString());
            //    }
            //}
        }
    }
}