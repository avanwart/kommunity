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
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using DasKlub.Lib.AppSpec.DasKlub.BLL;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.AppSpec.DasKlub.BOL
{
    public class StatusUpdate : BaseIUserLogCRUD, ICacheName, IUnorderdListItem
    {
        #region constructors

        public StatusUpdate(DataRow dr)
        {
            Get(dr);
        }

        public StatusUpdate()
        {
        }

        public StatusUpdate(int statusUpdateID)
        {
            Get(statusUpdateID);
        }

        #endregion

        #region properties

        private string _message = string.Empty;
        private char _statusType = char.MinValue;
        public int StatusUpdateID { get; private set; }


        public bool IsMobile { get; set; }

        public int? PhotoItemID { get; set; }

        public int? ZoneID { get; set; }

        public int UserAccountID { get; set; }

        public string Message
        {
            get {
                return _message == null ? _message : _message.Trim();
            }
            set { _message = value; }
        }


        public char StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        #endregion

        public string StatusAcknowledgements
        {
            get
            {
                var sb = new StringBuilder(100);
                Acknowledgement ack = null;

                MembershipUser mu = Membership.GetUser();

                if (mu == null) return string.Empty;


                var acks = new Acknowledgements();
                acks.GetAcknowledgementsForStatus(StatusUpdateID);

                var uaApplauds = new UserAccounts();
                var uaBeats = new UserAccounts();
                UserAccount uaRsp = null;

                foreach (Acknowledgement ack1 in acks)
                {
                    uaRsp = new UserAccount(ack1.CreatedByUserID);

                    if (ack1.AcknowledgementType == Convert.ToChar(SiteEnums.AcknowledgementType.A.ToString()))
                    {
                        uaApplauds.Add(uaRsp);
                    }
                    else if (ack1.AcknowledgementType == Convert.ToChar(SiteEnums.AcknowledgementType.B.ToString()))
                    {
                        uaBeats.Add(uaRsp);
                    }
                }


                if (mu != null &&
                    Acknowledgement.IsUserAcknowledgement(StatusUpdateID, Convert.ToInt32(mu.ProviderUserKey)))
                {
                    sb.Append(@"<div class=""left_float"">");

                    var sbApplaud = new StringBuilder(100);

                    int i = 0;

                    foreach (UserAccount uar1 in uaApplauds)
                    {
                        i++;

                        if (i == uaApplauds.Count)
                        {
                            sbApplaud.AppendFormat("{0}", uar1.UserName);
                        }
                        else
                        {
                            sbApplaud.AppendFormat("{0}, ", uar1.UserName);
                        }
                    }

                    sb.AppendFormat(@"<span class=""status_count_applaud"" title=""{0}"">", sbApplaud);
                    sb.Append(Acknowledgements.GetAcknowledgementCount(StatusUpdateID,
                                                                       Convert.ToChar(
                                                                           SiteEnums.AcknowledgementType.A.ToString())));
                    sb.Append(@"</span>");

                    ack = new Acknowledgement();
                    ack.GetAcknowledgement(StatusUpdateID, Convert.ToInt32(mu.ProviderUserKey));

                    if (ack.AcknowledgementID > 0 &&
                        ack.AcknowledgementType == Convert.ToChar(SiteEnums.AcknowledgementType.A.ToString()))
                    {
                        sb.AppendFormat(@"<button title=""{0}"" name=""status_update_id_applaud""",
                                        Messages.YouResponded);
                        sb.Append(@" class=""applaud_status_complete""  type=""button"" value=""");
                        sb.Append(StatusUpdateID.ToString());
                        sb.AppendFormat(@""">{0}</button>", Messages.Applaud);
                    }
                    else
                    {
                        sb.AppendFormat(@"<button title=""{0}"" name=""status_update_id_applaud""",
                                        Messages.YouResponded);
                        sb.Append(@" disabled=""disabled"" class=""applaud_status""  type=""button"" value=""");
                        sb.Append(StatusUpdateID.ToString());
                        sb.AppendFormat(@""">{0}</button>", Messages.Applaud);
                    }

                    sb.Append(@"</div>");

                    sb.Append(@"<div class=""left_float"">");

                    var sbBeatDowns = new StringBuilder(100);

                    foreach (UserAccount uar1 in uaBeats)
                    {
                        i++;

                        if (i == uaBeats.Count)
                        {
                            sbBeatDowns.AppendFormat("{0}", uar1.UserName);
                        }
                        else
                        {
                            sbBeatDowns.AppendFormat("{0}, ", uar1.UserName);
                        }
                    }

                    sb.AppendFormat(@"<span class=""status_count_beatdown"" title=""{0}"">", sbBeatDowns);
                    sb.Append(Acknowledgements.GetAcknowledgementCount(StatusUpdateID,
                                                                       Convert.ToChar(
                                                                           SiteEnums.AcknowledgementType.B.ToString())));
                    sb.Append(@"</span>");

                    if (ack.AcknowledgementID > 0 &&
                        ack.AcknowledgementType == Convert.ToChar(SiteEnums.AcknowledgementType.B.ToString()))
                    {
                        sb.AppendFormat(@"<button title=""{0}""", Messages.YouResponded);
                        sb.Append(
                            @" class=""beat_status_complete"" name=""status_update_id_beat"" type=""button"" value=""");
                        sb.Append(StatusUpdateID.ToString());
                        sb.AppendFormat(@""">{0}</button>", Messages.BeatDown);
                    }
                    else
                    {
                        sb.AppendFormat(@"<button title=""{0}""", Messages.YouResponded);
                        sb.Append(
                            @" class=""beat_status"" disabled=""disabled"" name=""status_update_id_beat"" type=""button"" value=""");
                        sb.Append(StatusUpdateID.ToString());
                        sb.AppendFormat(@""">{0}</button>", Messages.BeatDown);
                    }

                    sb.Append(@"</div>");
                }
                else
                {
                    sb.Append(@"<div class=""left_float"">");


                    var sbApplaud = new StringBuilder(100);

                    int i = 0;

                    foreach (UserAccount uar1 in uaApplauds)
                    {
                        i++;

                        if (i == uaApplauds.Count)
                        {
                            sbApplaud.AppendFormat("{0}", uar1.UserName);
                        }
                        else
                        {
                            sbApplaud.AppendFormat("{0}, ", uar1.UserName);
                        }
                    }

                    sb.AppendFormat(@"<span class=""status_count_applaud"" title=""{0}"">", sbApplaud);
                    sb.Append(Acknowledgements.GetAcknowledgementCount(StatusUpdateID,
                                                                       Convert.ToChar(
                                                                           SiteEnums.AcknowledgementType.A.ToString())));
                    sb.Append(@"</span>");
                    sb.AppendFormat(@"<button title=""{0}"" name=""status_update_id_applaud""", Messages.Applaud);
                    sb.Append(@" class=""applaud_status"" type=""button"" value=""");
                    sb.Append(StatusUpdateID.ToString());
                    sb.AppendFormat(@""">{0}</button>", Messages.Applaud);

                    sb.Append(@"</div>");

                    sb.Append(@"<div class=""left_float"">");

                    var sbBeatDowns = new StringBuilder(100);

                    i = 0; // reset count

                    foreach (UserAccount uar1 in uaBeats)
                    {
                        i++;

                        if (i == uaBeats.Count)
                        {
                            sbBeatDowns.AppendFormat("{0}", uar1.UserName);
                        }
                        else
                        {
                            sbBeatDowns.AppendFormat("{0}, ", uar1.UserName);
                        }
                    }

                    sb.AppendFormat(@"<span class=""status_count_beatdown"" title=""{0}"">", sbBeatDowns);


                    sb.Append(Acknowledgements.GetAcknowledgementCount(StatusUpdateID,
                                                                       Convert.ToChar(
                                                                           SiteEnums.AcknowledgementType.B.ToString())));
                    sb.Append(@"</span>");
                    sb.AppendFormat(@"<button title=""{0}"" name=""status_update_id_beat""", Messages.BeatDown);
                    sb.Append(@" class=""beat_status"" type=""button"" value=""");
                    sb.Append(StatusUpdateID.ToString());
                    sb.AppendFormat(@""">{0}</button>", Messages.BeatDown);

                    sb.Append(@"</div>");
                }

                return sb.ToString();
            }
        }

        public bool PhotoDisplay { private get; set; }

        public string JSONResponse
        {
            get { return @"{""StatusMessage"": """ + HttpUtility.HtmlEncode(ToUnorderdListItem) + @"""}"; }
        }

        public string ToUnorderdListItem
        {
            get
            {
                if (StatusUpdateID == 0 || UserAccountID == 0) return string.Empty;

                var sb = new StringBuilder(100);
                var ua = new UserAccount(UserAccountID);
                var isUsersPost = false;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    var currentUser = new UserAccount(HttpContext.Current.User.Identity.Name);

                    if (currentUser.UserAccountID != 0 && ua.UserAccountID == currentUser.UserAccountID)
                    {
                        isUsersPost = true;
                    }
                }

                sb.AppendFormat(@"<li class=""status_post"" id=""status_update_id_{0}"">", StatusUpdateID);

                if (PhotoItemID != null)
                {
                    var pitem = new PhotoItem(Convert.ToInt32(PhotoItemID));

                    sb.AppendFormat(@"<div class=""row"">
                                      <div class=""span6"">
                        <a class=""m_over"" href=""{0}"" target=""_blank""><img src=""{1}"" alt=""{2}"" title=""{2}"" /></a>
                                       ",
                                    Utilities.S3ContentPath(pitem.FilePathRaw),
                                    Utilities.S3ContentPath(pitem.FilePathStandard),
                                    Messages.SourceFile);


                    if (isUsersPost)
                    {
                        if (PhotoItemID != null && PhotoItemID > 0)
                        {
                            sb.AppendFormat(@"<br /><span class=""rotate_photo""><a href=""{0}"">{1}</a></span>",
                                            VirtualPathUtility.ToAbsolute(
                                                "~/account/RotateStatusImage?statusUpdateID=" +
                                                StatusUpdateID),
                                            Messages.RotatePhoto);
                        }
                    }

                    sb.Append(@"</div></div>");
                }

                sb.AppendFormat(@"<div>");

                #region user icon

                if (!PhotoDisplay)
                {
                    var uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(ua.UserAccountID);

                    sb.AppendFormat(@"<div class=""user_account_thumb"">{0}</div>", uad.SmallUserIcon);
                }
                else
                {
                    sb.AppendFormat(@"<div>{0}: <a href=""{1}"">{2}</a></div>", Messages.Uploader,
                                    VirtualPathUtility.ToAbsolute("~/" + ua.UserName), ua.UserName);
                }

                #endregion

                #region acknowledgements

                if (!PhotoDisplay)
                {
                    sb.AppendFormat(@"<div class=""acknowlege_options""><div id=""status_ack_{0}"">{1}</div></div>",
                                    StatusUpdateID, StatusAcknowledgements);
                }
                else
                {
                    sb.AppendFormat(@"<div>{0}: {1}</div>",
                                    Messages.Applauded,
                                    Acknowledgements.GetAcknowledgementCount(StatusUpdateID,
                                                                             Convert.ToChar(
                                                                                 SiteEnums.AcknowledgementType.A
                                                                                          .ToString())));

                    sb.AppendFormat(@"<div>{0}: {1}</div>",
                                    Messages.BeatenDown,
                                    Acknowledgements.GetAcknowledgementCount(StatusUpdateID,
                                                                             Convert.ToChar(
                                                                                 SiteEnums.AcknowledgementType.B
                                                                                          .ToString())));
                }

                #endregion

                sb.AppendFormat(@"</div>");

                sb.Append(@"<div class=""clear""></div>");

                #region message

                if (IsMobile)
                {
                    sb.AppendFormat(@"<img src=""{0}"" alt=""{1}"" title=""{1}"" />&nbsp;",
                                    VirtualPathUtility.ToAbsolute("~/content/images/icons/icon_mobile.png"),
                                    Messages.FromMobile);
                }
                else
                {
                    sb.AppendFormat(@"<img src=""{0}"" alt=""{1}"" title=""{1}"" />&nbsp;",
                                    VirtualPathUtility.ToAbsolute("~/content/images/icons/icon_desktop.png"),
                                    Messages.FromDesktop);
                }

                var timeElapsed = Utilities.TimeElapsedMessage(CreateDate);


                sb.AppendFormat(@"<i title=""{1}"">{0}</i>", timeElapsed, CreateDate.ToString("o"));


                if (!string.IsNullOrWhiteSpace(Message))
                {
                    sb.Append("<br />");
                }
                sb.Append("<br />");

                sb.AppendFormat(@"<div class=""post_content"">{0}</div>",
                                PhotoItemID == null
                                    ? Video.IFrameVideo(FromString.ReplaceNewLineWithHTML(Message))
                                    : Utilities.MakeLink(FromString.ReplaceNewLineWithHTML(Message)));

                #endregion

                sb.Append(@"<br />");

                #region comments

                sb.Append(@"<div class=""row"">");

                sb.Append(@"<div class=""span6"">");

                // begin: comments
                var statcoms = new StatusComments();
                statcoms.GetAllStatusCommentsForUpdate(StatusUpdateID);


                sb.Append(@"<div class=""status_accordion_child"">");

                sb.AppendFormat(
                    @"{1}: <span class=""status_comment_count"" id=""status_comments_count_{0}"">{2}</span>",
                    StatusUpdateID, Messages.Comments, StatusComments.GetStatusCommentCount(StatusUpdateID));


                sb.AppendFormat(@"<div class=""status_comment_content""  id=""status_comments_{1}"">{0}</div>",
                                statcoms.ToUnorderdList, StatusUpdateID);


                // end: comments


                sb.Append(@"<div class=""status_comment_outer"">");

                sb.Append("<br />");


                sb.AppendFormat(
                    @"<textarea class=""status_comment input-large expand50-200"" name=""status_comment_{0}"" id=""status_comment_{0}""></textarea>",
                    StatusUpdateID);

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    sb.AppendFormat(@"<button   title=""{0}"" name=""comment_status_id"" 
                                class=""btn btn-success comment_on_status""  type=""button"" value=""{1}"">{0}</button>",
                                    Messages.Comment, StatusUpdateID);
                }
                else
                {
                    sb.AppendFormat(@"<button disabled=""disabled"" title=""{0}"" name=""comment_status_id"" 
                                class=""btn btn-success comment_on_status""  type=""button"" value=""{1}"">{0}</button> ",
                                    Messages.Comment, StatusUpdateID);

                    sb.AppendFormat(@" &nbsp;<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute("~/account/logon"),
                                    Messages.SignIn);
                    ;
                }


                sb.Append(@"</div>");


                sb.Append(@"</div>");

                MembershipUser mu = Membership.GetUser();

                if (mu != null)
                {
                    var ua1 = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

                    if (isUsersPost || (ua != null && ua1.IsAdmin))
                    {
                        sb.AppendFormat(@"<button title=""{0}"" name=""delete_status_id"" 
                    class=""delete_icon btn btn-danger btn-mini"" type=""button"" value=""{1}"">{0}</button>",
                                        Messages.Delete, StatusUpdateID);
                    }
                }


                sb.Append(@"</div>");

                sb.Append(@"</div>");

                #endregion

                sb.Append(@"</li>");

                return sb.ToString();
            }
        }

        public void GetMostAcknowledgedStatus(int daysBack, SiteEnums.AcknowledgementType acknowledgementType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostAcknowledgedStatus";

            comm.AddParameter("daysBack", daysBack);
            comm.AddParameter("acknowledgementType", Convert.ToChar(acknowledgementType.ToString()));


            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(FromObj.IntFromObj(dt.Rows[0]["statusUpdateID"]));
            }
        }

        public override void Get(int statusUpdateID)
        {
            StatusUpdateID = statusUpdateID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdateByID";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateID), StatusUpdateID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);


                Message = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Message)]);
                StatusType = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => StatusType)]);
                StatusUpdateID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => StatusUpdateID)]);
                UserAccountID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => UserAccountID)]);
                ZoneID = FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => ZoneID)]);
                PhotoItemID = FromObj.IntNullableFromObj(dr[StaticReflection.GetMemberName<string>(x => PhotoItemID)]);
                IsMobile = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => IsMobile)]);
            }
            catch
            {
            }
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddStatusUpdate";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Message), Message);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusType), StatusType);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PhotoItemID), PhotoItemID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ZoneID), ZoneID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsMobile), IsMobile);

            var result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            StatusUpdateID = Convert.ToInt32(result);

            return StatusUpdateID;
        }

        public override bool Delete()
        {
            if (StatusUpdateID == 0) return false;

            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_DeleteStatusUpdate";
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusUpdateID), StatusUpdateID);
            RemoveCache();

            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public void GetMostRecentUserStatus(int userAccountID)
        {
            UserAccountID = userAccountID;

            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetMostRecentUserStatus";
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);

            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public void GetStatusUpdateByPhotoID(int photoItemID)
        {
            PhotoItemID = photoItemID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdateByPhotoID";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PhotoItemID), PhotoItemID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
        }

        #endregion
    }


    public class StatusUpdates : List<StatusUpdate>, IUnorderdList
    {
        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            private get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }


        public string ToUnorderdList
        {
            get
            {
                if (Count == 0) return string.Empty;

                var sb = new StringBuilder(100);

                if (IncludeStartAndEndTags) sb.Append(@"<ul>");

                foreach (var su in this)
                {
                    sb.Append(su.ToUnorderdListItem);
                }

                if (IncludeStartAndEndTags) sb.Append(@"</ul>");

                return sb.ToString();
            }
        }

        public static string MostFrequentStatusMessages()
        {
            string output = null;

            const string cacheName = "MostFrequentStatusMessages";

            if (HttpContext.Current != null && HttpRuntime.Cache[cacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_MostFrequentStatusMessages";

                DataTable dt = DbAct.ExecuteSelectCommand(comm);


                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    var bandCount = new Dictionary<string, int>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        string[] bandsList = FromObj.StringFromObj(dr["message"]).Split(' ');

                        foreach (string sss in bandsList)
                        {
                            if (string.IsNullOrWhiteSpace(sss)) continue;

                            if (bandCount.ContainsKey(sss.Trim().ToLower()))
                            {
                                bandCount[sss.Trim().ToLower()] = bandCount[sss.Trim().ToLower().ToLower()] + 1;
                            }
                            else
                            {
                                bandCount[sss.Trim().ToLower()] = 1;
                            }
                            //  dt.Rows.Add(new object[] { sss.Trim() });
                        }
                    }

                    var myList = bandCount.ToList();

                    myList.Sort(
                        (firstPair, nextPair) => nextPair.Value.CompareTo(firstPair.Value)
                        );

                    var sb = new StringBuilder();

                    sb.Append("<ol>");

                    int counter = 0;

                    while (counter < 100)
                    {
                        counter++;
                        // do something with entry.Value or entry.Key\
                        // Response.Write(entry.
                        sb.AppendFormat("<li>{0} : <i>{1}</i></li>", myList[counter].Key, myList[counter].Value);
                    }

                    sb.Append("</ol>");

                    output = sb.ToString();

                    HttpRuntime.Cache.AddObjToCache(output, cacheName);
                }
            }
            else
            {
                output = (string) HttpRuntime.Cache[cacheName];
            }


            return output;
        }

        public void GetMostAcknowledgedStatus(int daysBack, SiteEnums.AcknowledgementType acknowledgementType)
        {
            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetMostAcknowledgedStatus";
            comm.AddParameter("daysBack", daysBack);
            comm.AddParameter("acknowledgementType", Convert.ToChar(acknowledgementType.ToString()));
            
            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var su in from DataRow dr in dt.Rows select new StatusUpdate(FromObj.IntFromObj(dr["statusUpdateID"])))
            {
                Add(su);
            }
        }


        public int GetStatusUpdatesPageWise(int pageIndex, int pageSize)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetStatusUpdatesPageWise";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (var statup in from DataRow dr in ds.Tables[0].Rows select new StatusUpdate(dr))
                {
                    Add(statup);
                }
            }

            return recordCount;
        }


        public static bool DeleteAllStatusUpdates(int userAccountID)
        {
            if (userAccountID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteAllStatusUpdates";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public void GetAllUserStatusUpdates(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllUserStatusUpdates";

            comm.AddParameter("userAccountID", userAccountID);


            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var art in from DataRow dr in dt.Rows select new StatusUpdate(dr))
            {
                Add(art);
            }
        }


        public void GetMostRecentStatusUpdates()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostRecentStatusUpdates";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var art in from DataRow dr in dt.Rows select new StatusUpdate(dr))
            {
                Add(art);
            }
        }

        public void GetRecentStatusUpdates()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRecentStatusUpdates";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;
            
            foreach (var art in from DataRow dr in dt.Rows select new StatusUpdate(dr))
            {
                Add(art);
            }
        }
    }
}