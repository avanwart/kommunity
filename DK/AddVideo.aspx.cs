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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;

namespace DasKlub
{
    public partial class AddVideo : System.Web.UI.Page
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

            Video vid = new Video(vidid.Substring(0, 2), vidid.Substring(3, vidid.Length - 3));

            switch (vtypeAction)
            {
                case 'P':
                    // adding to playlist 
                    Playlist plylst = new Playlist();
                    plylst.GetUserPlaylist(Convert.ToInt32(mu.ProviderUserKey));

                    PlaylistVideo plyvid = new PlaylistVideo();

                    if (plylst.PlaylistID == 0)
                    {
                        plylst.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                        plylst.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                        plylst.Create();
                        plyvid.RankOrder = 1;
                    }
                    else
                    {
                        PlaylistVideos plyvids = new PlaylistVideos();
                        plyvids.GetPlaylistVideosForPlaylist(plylst.PlaylistID);

                        if (mu.UserName.ToLower() != BootBaronLib.Configs.GeneralConfigs.AdminUserName.ToLower()
                             && plyvids.Count >= 25)
                        {
                            // cannot have not than 10
                            Response.Redirect("~/account/playlist/" + mu.UserName);
                        }

                        plyvid.RankOrder = plyvids.Count + 1 ;
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

                    UserAccountVideo uav = new UserAccountVideo();

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