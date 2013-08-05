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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Configs;
using BootBaronLib.Operational;
using BootBaronLib.Resources;
using BootBaronLib.Values;
using LitS3;

namespace DasKlub.Web.io
{
    public class operation : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (string.IsNullOrEmpty(context.Request.QueryString[SiteEnums.QueryStringNames.param_type.ToString()]))
                return;

            var ptyc = (SiteEnums.QueryStringNames) Enum.Parse(typeof (SiteEnums.QueryStringNames),
                                                               context.Request.QueryString[
                                                                   SiteEnums.QueryStringNames.param_type.ToString()]);

            StringBuilder sb;
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

                    var statusUpdateID = Convert.ToInt32(key);

                    StatusUpdate statup;

                    if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[SiteEnums.QueryStringNames.stat_update_rsp.ToString()]))
                    {
                        mu = Membership.GetUser();

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
                                //  sun.GetStatusUpdateNotificationForUserStatus(Convert.ToInt32(mu.ProviderUserKey), statusUpdateID, SiteEnums.ResponseType.A);
                                sun.GetStatusUpdateNotificationForUserStatus(statup.UserAccountID, statusUpdateID,
                                                                             SiteEnums.ResponseType.A);
                            }
                            else if (ack.AcknowledgementType == Convert.ToChar(SiteEnums.ResponseType.B.ToString()))
                            {
                                //sun.GetStatusUpdateNotificationForUserStatus(Convert.ToInt32(mu.ProviderUserKey), statusUpdateID, SiteEnums.ResponseType.B);
                                sun.GetStatusUpdateNotificationForUserStatus(statup.UserAccountID, statusUpdateID,
                                                                             SiteEnums.ResponseType.B);
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

                                SendNotificationEmail(statup.UserAccountID, Convert.ToInt32(mu.ProviderUserKey), rspType, sun.StatusUpdateID);
                            }

                            context.Response.Write(@"{""StatusAcks"": """ +
                                                   HttpUtility.HtmlEncode(statup.StatusAcknowledgements) + @"""}");
                        }
                        else
                        {
                            // reverse 

                            ack.GetAcknowledgement(statusUpdateID, Convert.ToInt32(mu.ProviderUserKey));

                            ack.Delete();

                            // TODO: DELETE NOTIFICATION

                            context.Response.Write(@"{""StatusAcks"": """ +
                                                   HttpUtility.HtmlEncode(statup.StatusAcknowledgements) + @"""}");
                        }
                    }
                    else if (
                        !string.IsNullOrEmpty(
                            context.Request.QueryString[
                                SiteEnums.QueryStringNames.stat_update_comment_rsp.ToString()]))
                    {
                        mu = Membership.GetUser();

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

                        if (
                            !StatusCommentAcknowledgement.IsUserCommentAcknowledgement(statcomup.StatusCommentID,
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


                                SendNotificationEmail(statup.UserAccountID, Convert.ToInt32(mu.ProviderUserKey), rspType, sun.StatusUpdateID);
                            }

                            context.Response.Write(@"{""StatusAcks"": """ +
                                                   HttpUtility.HtmlEncode(
                                                       statcomup.StatusCommentAcknowledgementsOptions) + @"""}");
                        }
                        else
                        {
                            // reverse 

                            ack.GetCommentAcknowledgement(statusUpdateID, Convert.ToInt32(mu.ProviderUserKey));

                            ack.Delete();
                            // TODO: DELETE NOTIFICATION

                            context.Response.Write(@"{""StatusAcks"": """ +
                                                   HttpUtility.HtmlEncode(
                                                       statcomup.StatusCommentAcknowledgementsOptions) + @"""}");
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
                        if (statCom.StatusCommentID == 0)
                        {
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
                            StatusUpdateNotification sun = null;

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

                                SendNotificationEmail(statup.UserAccountID, Convert.ToInt32(mu.ProviderUserKey), SiteEnums.ResponseType.C, sun.StatusUpdateID);
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

                        context.Response.Write(@"{""StatusUpdateID"": """ +
                                               statCom.StatusUpdateID.ToString() + @"""}");
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

                        sb = new StringBuilder(100);

                        sb.Append(statComs.ToUnorderdList);

                        context.Response.Write(@"{""StatusComs"": """ +
                                               HttpUtility.HtmlEncode(sb.ToString()) + @"""}");
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
                                !BlockedUser.IsBlockingUser(Convert.ToInt32(mu.ProviderUserKey), su1.UserAccountID)));

                        statups.IncludeStartAndEndTags = false;

                        context.Response.Write(@"{""StatusUpdates"": """ +
                                               HttpUtility.HtmlEncode(statups.ToUnorderdList) + @"""}");
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

                    // menu updates

                    // get count in video room
                    int userCountChat = 0;

                    // get new mail
                    int userMessages = 0;

                    // get new users
                    int unconfirmedUsers = 0;

                    // status notifications
                    int notifications = 0;

                    if (mu != null)
                    {
                        // log off users who are offline

                        var uasOffline = new UserAccounts();
                        uasOffline.GetWhoIsOffline(true);

                        UserAccount offlineUser = null;

                        foreach (UserAccount uaoff1 in uasOffline)
                        {
                            var cru = new ChatRoomUser();
                            cru.GetChatRoomUserByUserAccountID(uaoff1.UserAccountID);

                            if (cru.ChatRoomUserID > 0)
                            {
                                cru.DeleteChatRoomUser();
                            }

                            offlineUser = new UserAccount(uaoff1.UserAccountID);
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
                case SiteEnums.QueryStringNames.begindate:

                    #region begindate

                    //string[] dates = HttpUtility.UrlDecode(
                    //    context.Request.QueryString[SiteEnums.QueryStringNames.begindate.ToString()]
                    //    ).Split('G');


                    DateTime dtBegin =
                        Convert.ToDateTime(context.Request.QueryString[SiteEnums.QueryStringNames.begindate.ToString()]);

                    dtBegin = new DateTime(dtBegin.Year, dtBegin.Month, 1);

                    DateTime dtEnd = dtBegin.AddMonths(1).AddDays(-1);
                    var tds = new Events();

                    tds.GetEventsForLocation(
                        dtBegin, dtEnd,
                        context.Request.QueryString[SiteEnums.QueryStringNames.country_iso.ToString()],
                        context.Request.QueryString[SiteEnums.QueryStringNames.region.ToString()],
                        context.Request.QueryString[SiteEnums.QueryStringNames.city.ToString()]);

                    CalendarItems citms = GetCitms(tds, dtBegin, dtEnd, true);

                    //[ 100, 500, 300, 200, 400 ]
                    sb = new StringBuilder();

                    sb.Append("[");

                    int processed = 1;

                    foreach (CalendarItem ci1 in citms)
                    {
                        if (processed == citms.Count)
                        {
                            sb.Append(ci1.StartDate.Day);
                        }
                        else
                        {
                            sb.Append(ci1.StartDate.Day);
                            sb.Append(", ");
                        }

                        processed++;
                    }

                    sb.Append("]");

                    context.Response.Write(sb.ToString());

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
        /// Sends email notification for status updates
        /// </summary>
        /// <param name="userTo"></param>
        /// <param name="userFrom"></param>
        /// <param name="rsp"></param>
        /// <param name="statusUpdateID"></param>
        private static void SendNotificationEmail(int userTo, int userFrom, SiteEnums.ResponseType rsp, int statusUpdateID)
        {
            var uaTo = new UserAccount(userTo);
            var uaFrom = new UserAccount(userFrom);

            var uad = new UserAccountDetail();

            uad.GetUserAccountDeailForUser(uaTo.UserAccountID);

            var language = Utilities.GetCurrentLanguageCode();
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

            var statupMess = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", 
                typeGiven,
                Environment.NewLine + Environment.NewLine, 
                GeneralConfigs.SiteDomain, 
                    VirtualPathUtility.ToAbsolute("~/account/statusupdate/"), 
                    statusUpdateID,
                Environment.NewLine + Environment.NewLine,
                Messages.From + Environment.NewLine + Environment.NewLine,
                uaFrom.UrlTo
                );

            var title = string.Format("{0} -> {1}", uaFrom.UserName, typeGiven);

            if (uad.EmailMessages)
            {
                Utilities.SendMail(uaTo.EMail, title, statupMess);
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }


        private CalendarItems GetCitms(IEnumerable<Event> tds, DateTime dtBegin, DateTime dtEnd, bool isMonth)
        {
            var citms = new CalendarItems();

            // need to display the whole month so that it can get the reoccuring ones
            var dtBeginMonth = new DateTime(dtBegin.Year, dtBegin.Month, 1);
            var dtEndMonth = dtBegin.AddMonths(1).AddDays(-1);


            foreach (var td in tds)
            {
                CalendarItem citm = null;
                if (td.IsEnabled && !td.IsReoccuring)
                {
                    citm = new CalendarItem(td);
                    citms.Add(citm);
                }
                else if (td.IsEnabled && td.IsReoccuring)
                {
                    var evcyc = new EventCycle(td.EventCycleID);

                    var amt = 0;

                    switch (evcyc.EventCode)
                    {
                        case "SFF":
                            for (DateTime date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (date.DayOfWeek == DayOfWeek.Friday)
                                {
                                    amt++;

                                    if (amt == 2 || amt == 4)
                                    {
                                        if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Friday &&
                                            dtBegin.Day == date.Day)
                                        {
                                            td.LocalTimeBegin = date;
                                            citm = new CalendarItem(td);
                                            citms.Add(citm);
                                            break;
                                        }
                                        else if (isMonth)
                                        {
                                            td.LocalTimeBegin = date;
                                            citm = new CalendarItem(td);
                                            citms.Add(citm);
                                        }
                                    }
                                }
                            }
                            break;

                        case "FFR":
                            for (DateTime date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (date.DayOfWeek != DayOfWeek.Friday) continue;
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Friday &&
                                    dtBegin.Day == date.Day)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                if (isMonth)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                }
                                break;
                            }

                            break;
                        case "SFR":

                            for (var date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (amt < 1 && date.DayOfWeek == DayOfWeek.Friday)
                                {
                                    amt++;
                                }
                                else if (amt == 1 && date.DayOfWeek == DayOfWeek.Friday)
                                {
                                    if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Friday &&
                                        dtBegin.Day == date.Day)
                                    {
                                        td.LocalTimeBegin = date;
                                        citm = new CalendarItem(td);
                                        citms.Add(citm);
                                        break;
                                    }
                                    if (isMonth)
                                    {
                                        td.LocalTimeBegin = date;
                                        citm = new CalendarItem(td);
                                        citms.Add(citm);
                                    }
                                    break;
                                }
                            }

                            break;

                            #region weekly

                        case "MON":

                            for (var date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Monday)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                if (!isMonth) continue;
                                if (date.DayOfWeek != DayOfWeek.Monday) continue;
                                td.LocalTimeBegin = date;
                                citm = new CalendarItem(td);
                                citms.Add(citm);
                            }

                            break;


                        case "TUE":

                            for (var date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                if (!isMonth) continue;
                                if (date.DayOfWeek != DayOfWeek.Tuesday) continue;
                                td.LocalTimeBegin = date;
                                citm = new CalendarItem(td);
                                citms.Add(citm);
                            }

                            break;


                        case "WED":

                            for (var date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Wednesday)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                else if (isMonth)
                                {
                                    if (date.DayOfWeek == DayOfWeek.Wednesday)
                                    {
                                        td.LocalTimeBegin = date;
                                        citm = new CalendarItem(td);
                                        citms.Add(citm);
                                    }
                                }
                            }

                            break;


                        case "THU":

                            for (DateTime date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Thursday)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                else if (isMonth)
                                {
                                    if (date.DayOfWeek == DayOfWeek.Thursday)
                                    {
                                        td.LocalTimeBegin = date;
                                        citm = new CalendarItem(td);
                                        citms.Add(citm);
                                    }
                                }
                            }

                            break;


                        case "FRI":

                            for (DateTime date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Friday)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                else if (isMonth)
                                {
                                    if (date.DayOfWeek == DayOfWeek.Friday)
                                    {
                                        td.LocalTimeBegin = date;
                                        citm = new CalendarItem(td);
                                        citms.Add(citm);
                                    }
                                }
                            }

                            break;


                        case "SAT":

                            for (DateTime date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                else if (isMonth)
                                {
                                    if (date.DayOfWeek == DayOfWeek.Saturday)
                                    {
                                        td.LocalTimeBegin = date;
                                        citm = new CalendarItem(td);
                                        citms.Add(citm);
                                    }
                                }
                            }

                            break;


                        case "SUN":

                            for (DateTime date = dtBeginMonth; date <= dtEndMonth; date = date.AddDays(1))
                            {
                                if (!isMonth && dtBegin.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    td.LocalTimeBegin = date;
                                    citm = new CalendarItem(td);
                                    citms.Add(citm);
                                    break;
                                }
                                else if (isMonth)
                                {
                                    if (date.DayOfWeek == DayOfWeek.Sunday)
                                    {
                                        td.LocalTimeBegin = date;
                                        citm = new CalendarItem(td);
                                        citms.Add(citm);
                                    }
                                }
                            }

                            break;

                            #endregion

                        default:
                            break;
                    }
                }
            }

            return citms;
        }
    }
}
