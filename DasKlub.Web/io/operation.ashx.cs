using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Services;
using DasKlub.Lib.Values;
using LitS3;

namespace DasKlub.Web.io
{
    public class operation : IHttpHandler
    {
        private IMailService _mail;

        public operation()
        {
            _mail = new MailService(); // TODO: FIX NINJECT, FAILING TO WORK HERE
        }

        public void ProcessRequest(HttpContext context)
        {
            if (string.IsNullOrEmpty(context.Request.QueryString[SiteEnums.QueryStringNames.param_type.ToString()]))
                return;

            var ptyc = (SiteEnums.QueryStringNames) Enum.Parse(typeof (SiteEnums.QueryStringNames),
                context.Request.QueryString[SiteEnums.QueryStringNames.param_type.ToString()]);

            MembershipUser mu;

            switch (ptyc)
            {
                case SiteEnums.QueryStringNames.status_update:

                    #region status_update

                    string key = context.Request.QueryString[SiteEnums.QueryStringNames.status_update_id.ToString()];

                    if (string.IsNullOrEmpty(key))
                    {
                        key =
                            context.Request.QueryString[
                                SiteEnums.QueryStringNames.most_applauded_status_update_id.ToString()];
                    }

                    int statusUpdateID = Convert.ToInt32(key);

                    StatusUpdate statup;

                    if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.stat_update_rsp.ToString()]))
                    {
                        mu = Membership.GetUser();

                        if (mu == null) return;
                        var ack = new Acknowledgement
                        {
                            CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey),
                            UserAccountID = Convert.ToInt32(mu.ProviderUserKey),
                            AcknowledgementType = Convert.ToChar(
                                context.Request.QueryString[SiteEnums.QueryStringNames.stat_update_rsp.ToString()]),
                            StatusUpdateID = statusUpdateID
                        };

                        statup = new StatusUpdate(statusUpdateID);

                        if (!Acknowledgement.IsUserAcknowledgement(statusUpdateID, Convert.ToInt32(mu.ProviderUserKey)))
                        {
                            ack.Create();

                            var sun = new StatusUpdateNotification();

                            if (ack.AcknowledgementType == Convert.ToChar(SiteEnums.ResponseType.A.ToString()))
                            {
                                sun.GetStatusUpdateNotificationForUserStatus(statup.UserAccountID, statusUpdateID,
                                    SiteEnums.ResponseType.A);
                            }

                            if (Convert.ToInt32(mu.ProviderUserKey) != statup.UserAccountID)
                            {
                                sun.UserAccountID = statup.UserAccountID;

                                SiteEnums.ResponseType rspType;

                                if (ack.AcknowledgementType == Convert.ToChar(SiteEnums.ResponseType.A.ToString()))
                                {
                                    rspType = SiteEnums.ResponseType.A;
                                    sun.ResponseType = Convert.ToChar(rspType.ToString());
                                }
                                else
                                {
                                    rspType = SiteEnums.ResponseType.B;
                                    sun.ResponseType = Convert.ToChar(rspType.ToString());
                                }

                                sun.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                                sun.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);

                                if (sun.StatusUpdateNotificationID == 0)
                                {
                                    sun.IsRead = false;
                                    sun.Create();
                                }
                                else
                                {
                                    sun.IsRead = false;
                                    sun.Update();
                                }

                                SendNotificationEmail(statup.UserAccountID, Convert.ToInt32(mu.ProviderUserKey), rspType,
                                    sun.StatusUpdateID);
                            }

                            context.Response.Write(string.Format(@"{{""StatusAcks"": ""{0}""}}",
                                HttpUtility.HtmlEncode(statup.StatusAcknowledgements)));
                        }
                        else
                        {
                            // reverse 

                            ack.GetAcknowledgement(statusUpdateID, Convert.ToInt32(mu.ProviderUserKey));

                            ack.Delete();

                            // TODO: DELETE NOTIFICATION

                            context.Response.Write(string.Format(@"{{""StatusAcks"": ""{0}""}}",
                                HttpUtility.HtmlEncode(statup.StatusAcknowledgements)));
                        }
                    }
                    else if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[
                                SiteEnums.QueryStringNames.stat_update_comment_rsp.ToString()]))
                    {
                        mu = Membership.GetUser();

                        if (mu == null) return;
                        var ack = new StatusCommentAcknowledgement
                        {
                            CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey),
                            UserAccountID = Convert.ToInt32(mu.ProviderUserKey),
                            AcknowledgementType = Convert.ToChar(
                                context.Request.QueryString[
                                    SiteEnums.QueryStringNames.stat_update_comment_rsp.ToString()]),
                            StatusCommentID = statusUpdateID
                        };

                        var statcomup = new StatusComment(statusUpdateID);

                        statup = new StatusUpdate(statcomup.StatusUpdateID);

                        if (!StatusCommentAcknowledgement.IsUserCommentAcknowledgement(statcomup.StatusCommentID,
                            Convert.ToInt32(
                                mu.ProviderUserKey)))
                        {
                            ack.Create();

                            var sun = new StatusUpdateNotification();

                            sun.GetStatusUpdateNotificationForUserStatus(statcomup.UserAccountID,
                                statcomup.StatusUpdateID,
                                ack.AcknowledgementType ==
                                Convert.ToChar(
                                    SiteEnums.ResponseType.A.ToString())
                                    ? SiteEnums.ResponseType.A
                                    : SiteEnums.ResponseType.B);

                            if (Convert.ToInt32(mu.ProviderUserKey) != statcomup.UserAccountID)
                            {
                                sun.UserAccountID = statcomup.UserAccountID;

                                SiteEnums.ResponseType rspType;

                                if (ack.AcknowledgementType == Convert.ToChar(SiteEnums.ResponseType.A.ToString()))
                                {
                                    rspType = SiteEnums.ResponseType.A;
                                    sun.ResponseType = Convert.ToChar(rspType.ToString());
                                }
                                else
                                {
                                    rspType = SiteEnums.ResponseType.B;
                                    sun.ResponseType = Convert.ToChar(rspType.ToString());
                                }

                                if (sun.StatusUpdateNotificationID == 0)
                                {
                                    sun.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                                    sun.IsRead = false;
                                    sun.Create();
                                }
                                else
                                {
                                    sun.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                                    sun.IsRead = false;
                                    sun.Update();
                                }


                                SendNotificationEmail(statup.UserAccountID, Convert.ToInt32(mu.ProviderUserKey), rspType,
                                    sun.StatusUpdateID);
                            }

                            context.Response.Write(string.Format(@"{{""StatusAcks"": ""{0}""}}", HttpUtility.HtmlEncode(
                                statcomup.StatusCommentAcknowledgementsOptions)));
                        }
                        else
                        {
                            // reverse 

                            ack.GetCommentAcknowledgement(statusUpdateID, Convert.ToInt32(mu.ProviderUserKey));

                            ack.Delete();
                            // TODO: DELETE NOTIFICATION

                            context.Response.Write(string.Format(@"{{""StatusAcks"": ""{0}""}}", HttpUtility.HtmlEncode(
                                statcomup.StatusCommentAcknowledgementsOptions)));
                        }
                    }
                    else if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.comment_msg.ToString()]) &&
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.comment_msg.ToString()])
                        )
                    {
                        mu = Membership.GetUser();

                        if (mu == null) return;

                        var statCom = new StatusComment
                        {
                            CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey),
                            Message = HttpUtility.HtmlEncode(
                                context.Request.QueryString[SiteEnums.QueryStringNames.comment_msg.ToString()]),
                            StatusUpdateID = statusUpdateID,
                            UserAccountID = Convert.ToInt32(mu.ProviderUserKey)
                        };

                        // TODO: CHECK IF THERE IS A RECENT MESSAGE THAT IS THE SAME
                        if (statCom.StatusCommentID != 0) return;
                        //BUG: THERE IS AN EVENT HANDLER THAT HAS QUEUED UP TOO MANY
                        var suLast = new StatusUpdate();
                        suLast.GetMostRecentUserStatus(Convert.ToInt32(mu.ProviderUserKey));

                        if (suLast.Message.Trim() != statCom.Message.Trim() ||
                            (suLast.Message.Trim() == statCom.Message.Trim() &&
                             suLast.StatusUpdateID != statCom.StatusUpdateID))
                        {
                            statCom.Create();
                        }

                        statup = new StatusUpdate(statusUpdateID);

                        // create a status update notification for the post maker and all commenters
                        StatusUpdateNotification sun;

                        if (Convert.ToInt32(mu.ProviderUserKey) != statup.UserAccountID)
                        {
                            sun = new StatusUpdateNotification();

                            sun.GetStatusUpdateNotificationForUserStatus(statup.UserAccountID,
                                statusUpdateID,
                                SiteEnums.ResponseType.C);

                            if (sun.StatusUpdateNotificationID == 0)
                            {
                                sun.ResponseType = Convert.ToChar(SiteEnums.ResponseType.C.ToString());
                                sun.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                                sun.IsRead = false;
                                sun.StatusUpdateID = statup.StatusUpdateID;
                                sun.UserAccountID = statup.UserAccountID;
                                sun.Create();
                            }
                            else
                            {
                                sun.IsRead = false;
                                sun.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                                sun.Update();
                            }

                            SendNotificationEmail(statup.UserAccountID, Convert.ToInt32(mu.ProviderUserKey),
                                SiteEnums.ResponseType.C, sun.StatusUpdateID);
                        }

                        var statComs = new StatusComments();

                        statComs.GetAllStatusCommentsForUpdate(statusUpdateID);

                        foreach (StatusComment sc1 in statComs)
                        {
                            sun = new StatusUpdateNotification();

                            sun.GetStatusUpdateNotificationForUserStatus(statup.UserAccountID,
                                statusUpdateID,
                                SiteEnums.ResponseType.C);

                            if (Convert.ToInt32(mu.ProviderUserKey) == sc1.UserAccountID ||
                                Convert.ToInt32(mu.ProviderUserKey) == statup.UserAccountID) continue;

                            if (sun.StatusUpdateNotificationID == 0)
                            {
                                sun.IsRead = false;
                                sun.StatusUpdateID = statusUpdateID;
                                sun.UserAccountID = sc1.UserAccountID;
                                sun.Create();
                            }
                            else
                            {
                                sun.IsRead = false;
                                sun.Update();
                            }
                        }
                        context.Response.Write(@"{""StatusAcks"": """ + @"""}");
                    }
                    else if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.act_type.ToString()]) &&
                        context.Request.QueryString[SiteEnums.QueryStringNames.act_type.ToString()] == "P"
                        )
                    {
                        // delete post
                        statup = new StatusUpdate(statusUpdateID);

                        StatusUpdateNotifications.DeleteNotificationsForStatusUpdate(statup.StatusUpdateID);
                        Acknowledgements.DeleteStatusAcknowledgements(statup.StatusUpdateID);

                        var statComs = new StatusComments();
                        statComs.GetAllStatusCommentsForUpdate(statup.StatusUpdateID);

                        foreach (StatusComment sc1 in statComs)
                        {
                            StatusCommentAcknowledgements.DeleteStatusCommentAcknowledgements(
                                sc1.StatusCommentID);
                        }
                        StatusComments.DeleteStatusComments(statup.StatusUpdateID);

                        statup.Delete();

                        if (statup.PhotoItemID != null)
                        {
                            var pitm = new PhotoItem(Convert.ToInt32(statup.PhotoItemID));

                            var s3 = new S3Service
                            {
                                AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                                SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                            };

                            if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathRaw))
                            {
                                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathRaw);
                            }

                            if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathStandard))
                            {
                                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathStandard);
                            }

                            if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathThumb))
                            {
                                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathThumb);
                            }

                            pitm.Delete();
                        }
                        context.Response.Write(@"{""StatusAcks"": """ + @"""}");
                    }
                    else if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.act_type.ToString()]) &&
                        context.Request.QueryString[SiteEnums.QueryStringNames.act_type.ToString()] ==
                        "C"
                        )
                    {
                        // delete comment

                        var statCom = new StatusComment(
                            Convert.ToInt32(
                                context.Request.QueryString[
                                    SiteEnums.QueryStringNames.status_com_id.ToString()]));

                        StatusCommentAcknowledgements.DeleteStatusCommentAcknowledgements(
                            statCom.StatusCommentID);

                        statCom.Delete();

                        context.Response.Write(string.Format(@"{{""StatusUpdateID"": ""{0}""}}", statCom.StatusUpdateID));
                    }
                    else if (!string.IsNullOrEmpty(
                        context.Request.QueryString[SiteEnums.QueryStringNames.all_comments.ToString()]))
                    {
                        mu = Membership.GetUser();

                        if (mu == null) return;

                        var preFilter = new StatusComments();

                        preFilter.GetAllStatusCommentsForUpdate(statusUpdateID);

                        var statComs = new StatusComments();
                        statComs.AddRange(
                            preFilter.Where(
                                su1 =>
                                    !BlockedUser.IsBlockingUser(Convert.ToInt32(mu.ProviderUserKey), su1.UserAccountID)));

                        statComs.IncludeStartAndEndTags = true;

                        var sb = new StringBuilder(100);

                        sb.Append(statComs.ToUnorderdList);

                        context.Response.Write(string.Format(@"{{""StatusComs"": ""{0}""}}",
                            HttpUtility.HtmlEncode(sb.ToString())));
                    }
                    else if (!string.IsNullOrEmpty(
                        context.Request.QueryString[SiteEnums.QueryStringNames.comment_page.ToString()]))
                    {
                        int pcount =
                            Convert.ToInt32(
                                context.Request.QueryString[
                                    SiteEnums.QueryStringNames.comment_page.ToString()]);

                        var statups = new StatusUpdates();

                        pcount = pcount + 10;

                        var preFilter = new StatusUpdates();

                        preFilter.GetStatusUpdatesPageWise(pcount, 1);


                        mu = Membership.GetUser();

                        statups.AddRange(
                            preFilter.Where(
                                su1 =>
                                    mu != null &&
                                    !BlockedUser.IsBlockingUser(Convert.ToInt32(mu.ProviderUserKey), su1.UserAccountID)));

                        statups.IncludeStartAndEndTags = false;

                        context.Response.Write(string.Format(@"{{""StatusUpdates"": ""{0}""}}",
                            HttpUtility.HtmlEncode(statups.ToUnorderdList)));
                    }

                    #endregion

                    break;
                case SiteEnums.QueryStringNames.begin_playlist:

                    #region begin_playlist

                    context.Response.Write(
                        PlaylistVideo.GetFirstVideo(Convert.ToInt32(context.Request.QueryString[
                            SiteEnums.QueryStringNames.playlist.ToString()])));

                    #endregion

                    break;
                case SiteEnums.QueryStringNames.menu:

                    #region menu

                    mu = Membership.GetUser();

                    int userCountChat = 0;
                    int userMessages = 0;
                    int unconfirmedUsers = 0;
                    int notifications = 0;

                    if (mu != null)
                    {
                        // log off users who are offline

                        var uasOffline = new UserAccounts();
                        uasOffline.GetWhoIsOffline(true);

                        foreach (UserAccount uaoff1 in uasOffline)
                        {
                            var cru = new ChatRoomUser();
                            cru.GetChatRoomUserByUserAccountID(uaoff1.UserAccountID);

                            if (cru.ChatRoomUserID > 0)
                            {
                                cru.DeleteChatRoomUser();
                            }

                            var offlineUser = new UserAccount(uaoff1.UserAccountID);
                            offlineUser.RemoveCache();
                        }


                        userCountChat = ChatRoomUsers.GetChattingUserCount();

                        userMessages = DirectMessages.GetDirectMessagesToUserCount(mu);
                        unconfirmedUsers =
                            UserConnections.GetCountUnconfirmedConnections(Convert.ToInt32(mu.ProviderUserKey));
                    }

                    // get users online
                    int onlineUsers = UserAccounts.GetOnlineUserCount();

                    if (mu != null)
                    {
                        notifications =
                            StatusUpdateNotifications.GetStatusUpdateNotificationCountForUser(
                                Convert.ToInt32(mu.ProviderUserKey));
                    }

                    string timedMessge = string.Format(
                        @"{{""UserCountChat"": ""{0}"",
               ""UserMessages"": ""{1}"", 
               ""OnlineUsers"": ""{2}"",
               ""Notifications"": ""{3}"",
               ""UnconfirmedUsers"": ""{4}""}}", userCountChat, userMessages, onlineUsers, notifications,
                        unconfirmedUsers);

                    context.Response.Write(timedMessge);

                    #endregion

                    break;
                case SiteEnums.QueryStringNames.random:

                    #region random

                    if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.currentvidid.ToString()]))
                    {
                        context.Response.Write(Video.GetRandomJSON(
                            context.Request.QueryString[SiteEnums.QueryStringNames.currentvidid.ToString()]));
                    }
                    else
                    {
                        context.Response.Write(Video.GetRandomJSON());
                    }

                    #endregion

                    break;
                case SiteEnums.QueryStringNames.video_playlist:

                    #region video_playlist

                    if (!string.IsNullOrEmpty(
                        context.Request.QueryString[SiteEnums.QueryStringNames.currentvidid.ToString()]))
                    {
                        context.Response.Write(
                            PlaylistVideo.GetNextVideo(
                                Convert.ToInt32(
                                    context.Request.QueryString[SiteEnums.QueryStringNames.playlist.ToString()]),
                                context.Request.QueryString[SiteEnums.QueryStringNames.currentvidid.ToString()]));
                    }
                    else if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.begin_playlist.ToString()]))
                    {
                        context.Response.Write(
                            PlaylistVideo.GetFirstVideo(
                                Convert.ToInt32(
                                    context.Request.QueryString[SiteEnums.QueryStringNames.playlist.ToString()])));
                    }
                    else
                    {
                        context.Response.Write(
                            PlaylistVideo.CurrentVideoInPlaylist(
                                Convert.ToInt32(
                                    context.Request.QueryString[SiteEnums.QueryStringNames.playlist.ToString()])
                                ));
                    }

                    #endregion

                    break;
                case SiteEnums.QueryStringNames.video:

                    #region video

                    var vid = new Video("YT", context.Request.QueryString[SiteEnums.QueryStringNames.vid.ToString()]);

                    VideoLog.AddVideoLog(vid.VideoID, context.Request.UserHostAddress);

                    context.Response.Write(
                        Video.GetVideoJSON(context.Request.QueryString[SiteEnums.QueryStringNames.vid.ToString()]));

                    #endregion

                    break;

                case SiteEnums.QueryStringNames.playlist:

                    #region playlist

                    if (!string.IsNullOrEmpty(
                        context.Request.QueryString[SiteEnums.QueryStringNames.currentvidid.ToString()]))
                    {
                        context.Response.Write(
                            PlaylistVideo.GetNextVideo(
                                Convert.ToInt32(
                                    context.Request.QueryString[SiteEnums.QueryStringNames.playlist.ToString()]),
                                context.Request.QueryString[SiteEnums.QueryStringNames.currentvidid.ToString()]));
                    }
                    else if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.begin_playlist.ToString()]))
                    {
                        context.Response.Write(
                            PlaylistVideo.GetFirstVideo(
                                Convert.ToInt32(
                                    context.Request.QueryString[SiteEnums.QueryStringNames.playlist.ToString()])));
                    }
                    else
                    {
                        context.Response.Write(
                            PlaylistVideo.CurrentVideoInPlaylist(
                                Convert.ToInt32(
                                    context.Request.QueryString[SiteEnums.QueryStringNames.playlist.ToString()])
                                ));
                    }

                    #endregion

                    break;
            }
        }


        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        ///     Sends email notification for status updates
        /// </summary>
        /// <param name="userTo"></param>
        /// <param name="userFrom"></param>
        /// <param name="rsp"></param>
        /// <param name="statusUpdateID"></param>
        private void SendNotificationEmail(int userTo, int userFrom, SiteEnums.ResponseType rsp, int statusUpdateID)
        {
            var uaTo = new UserAccount(userTo);
            var uaFrom = new UserAccount(userFrom);
            var uad = new UserAccountDetail();

            uad.GetUserAccountDeailForUser(uaTo.UserAccountID);

            string language = Utilities.GetCurrentLanguageCode();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);
            string typeGiven;

            switch (rsp)
            {
                case SiteEnums.ResponseType.A:
                    typeGiven = Messages.Applauded;
                    break;
                case SiteEnums.ResponseType.B:
                    typeGiven = Messages.BeatenDown;
                    break;
                case SiteEnums.ResponseType.C:
                    typeGiven = Messages.Commented;
                    break;
                default:
                    typeGiven = Messages.Unknown;
                    break;
            }

            string statupMess = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                typeGiven,
                string.Format("{0}{0}", Environment.NewLine),
                GeneralConfigs.SiteDomain,
                VirtualPathUtility.ToAbsolute("~/account/statusupdate/"),
                statusUpdateID,
                Environment.NewLine + Environment.NewLine,
                Messages.From + Environment.NewLine + Environment.NewLine,
                uaFrom.UrlTo
                );

            string title = string.Format("{0} -> {1}", uaFrom.UserName, typeGiven);

            if (uad.EmailMessages)
            {
                if (_mail == null)
                {
                    _mail = new MailService();
                }
                _mail.SendMail(AmazonCloudConfigs.SendFromEmail, uaTo.EMail, title, statupMess);
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }
}