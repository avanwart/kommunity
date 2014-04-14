using System;
using System.Web.Security;
using System.Web.UI;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;

namespace DasKlub.Web
{
    public partial class AddVideo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MembershipUser mu = Membership.GetUser();

            if (mu == null ||
                string.IsNullOrEmpty(Request.QueryString["vidid"])
                ) Response.Redirect("~/account/logon");

            string vidid = Request.QueryString["vidid"];
            string vtype = Request.QueryString["vtype"];

            char vtypeAction = Convert.ToChar(vtype);

            var vid = new Video(vidid.Substring(0, 2), vidid.Substring(3, vidid.Length - 3));

            switch (vtypeAction)
            {
                case 'P':
                    // adding to playlist 
                    var plylst = new Playlist();
                    plylst.GetUserPlaylist(Convert.ToInt32(mu.ProviderUserKey));

                    var plyvid = new PlaylistVideo();

                    if (plylst.PlaylistID == 0)
                    {
                        plylst.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                        plylst.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                        plylst.Create();
                        plyvid.RankOrder = 1;
                    }
                    else
                    {
                        var plyvids = new PlaylistVideos();
                        plyvids.GetPlaylistVideosForPlaylist(plylst.PlaylistID);

                        if (mu.UserName.ToLower() != GeneralConfigs.AdminUserName.ToLower()
                            && plyvids.Count >= 25)
                        {
                            // cannot have not than 10
                            Response.Redirect("~/account/playlist/" + mu.UserName);
                        }

                        plyvid.RankOrder = plyvids.Count + 1;
                    }

                    plyvid.PlaylistID = plylst.PlaylistID;
                    plyvid.VideoID = vid.VideoID;

                    if (!PlaylistVideo.IsPlaylistVideo(plyvid.PlaylistID, vid.VideoID))
                    {
                        plyvid.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                        plyvid.Create();
                    }

                    Response.Redirect("~/account/playlist/" + mu.UserName);

                    break;
                default:

                    var uav = new UserAccountVideo();

                    uav.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                    uav.VideoID = vid.VideoID;
                    uav.VideoType = vtypeAction;

                    uav.Create();

                    Response.Redirect("~/" + mu.UserName);
                    break;
            }
        }
    }
}