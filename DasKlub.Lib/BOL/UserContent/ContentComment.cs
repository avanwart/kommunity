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
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using System.Web.Security;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL.UserContent
{
    public class ContentComment : BaseIUserLogCRUD, IUnorderdListItem, IURLTo
    {
        #region properties

        private string _detail = string.Empty;
        private string _fromEmail = string.Empty;
        private string _fromName = string.Empty;
        private string _ipAddress = string.Empty;
        private char _statusType = char.MinValue;
        public int ContentCommentID { get; set; }

        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof (Messages))]
        public char StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Message")]
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }


        public string FromName
        {
            get { return _fromName; }
            set { _fromName = value; }
        }


        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "IpAddress")]
        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }


        [RegularExpression(@".+\@.+\..+", ErrorMessageResourceName = "IncorrectFormat", ErrorMessageResourceType =
            typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "EMail")]
        public string FromEmail
        {
            get { return _fromEmail; }
            set { _fromEmail = value; }
        }

        public int ContentID { get; set; }

        #endregion

        #region constructors

        public ContentComment(DataRow dr)
        {
            Get(dr);
        }

        public ContentComment()
        {
            
        }


        public ContentComment(int p)
        {
            Get(p);
        }

        #endregion

        public string StatusTypeName
        {
            get
            {
                var theCO = SiteEnums.CommentStatus.U;

                if (Enum.TryParse(StatusType.ToString(), out theCO))
                {
                    return Utilities.ResourceValue(Utilities.GetEnumDescription(theCO));
                }

                return string.Empty;
            }
        }

        public string ToUnorderdListItem
        {
            get
            {
                if (ContentCommentID == 0) return string.Empty;

                var sb = new StringBuilder(100);

                sb.Append(@"<li>");

                sb.Append(@"<hr />");


                sb.Append(@"<br />");

                sb.Append(@"<i>");
                sb.Append(Utilities.TimeElapsedMessage(CreateDate));
                sb.Append(@"</i>");
                sb.Append(@"<br />");

                if (CreatedByUserID > 0)
                {
                    var ua = new UserAccount(CreatedByUserID);

                    var uad = new UserAccountDetail();

                    uad.GetUserAccountDeailForUser(ua.UserAccountID);

                    sb.Append(uad.SmallUserIcon);
                }

                sb.Append(@"<br />");

                sb.Append(@"<p>");
                sb.Append(FromString.ReplaceNewLineWithHTML(HttpUtility.HtmlEncode(Detail)));
                sb.Append(@"</p>");

                MembershipUser mu = Membership.GetUser();

                if (mu != null && CreatedByUserID == Convert.ToInt32(mu.ProviderUserKey))
                {
                    sb.AppendFormat(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(
                        "~/news/DeleteComment?commentID=" + ContentCommentID.ToString()), Messages.Delete);
                }

                sb.Append(@"</li>");

                return sb.ToString();
            }
        }

        public Uri UrlTo
        {
            get
            {
                var cnt = new Content(ContentID);
                return cnt.UrlTo;
            }
        }

        public override void Get(int uniqueID)
        {
            ContentCommentID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentComment";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentCommentID), ContentCommentID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateContentComment";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UpdatedByUserID), UpdatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusType), StatusType);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Detail), Detail);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentID), ContentID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FromName), FromName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FromEmail), FromEmail);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IpAddress), IpAddress);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentCommentID), ContentCommentID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);


            return (result != -1);
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddContentComment";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusType), StatusType);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Detail), Detail);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentID), ContentID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FromName), FromName);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FromEmail), FromEmail);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IpAddress), IpAddress);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            else
            {
                ContentCommentID = Convert.ToInt32(result);

                return ContentCommentID;
            }
        }


        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                ContentCommentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentCommentID)]);
                StatusType = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => StatusType)]);
                ContentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentID)]);
                Detail = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Detail)]);
                FromEmail = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => FromEmail)]);
                FromName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => FromName)]);
                IpAddress = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => IpAddress)]);
            }
            catch
            {
            }
        }


        public override bool Delete()
        {
            if (ContentCommentID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteContentComment";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentCommentID), ContentCommentID);

            //RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }


    public class ContentComments : List<ContentComment>, IUnorderdList
    {
        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }


        public string ToUnorderdList
        {
            get
            {
                if (Count == 0) return string.Empty;

                //MembershipUser mu = Membership.GetUser();

                //int membID = (mu != null) ? Convert.ToInt32(mu.ProviderUserKey) : 0;

                var sb = new StringBuilder(100);

                //if (IncludeStartAndEndTags) sb.Append(@"<ul>");

                //sb.Append(@"<form method=""post"" action=""/reviews/commentdelete"" name=""delete_post"">");

                //UserAccount ua = null;

                //int i = 0;

                //Control ctrl = new Control();

                //foreach (ContentComment su in this)
                //{
                //    i++;

                //    ua = new UserAccount(su.CreatedByUserID);

                //    if (i % 2 == 1)
                //    {
                //        sb.Append(@"<li class=""status_post"">");
                //    }
                //    else
                //    {
                //        sb.Append(@"<li class=""status_post alternate_post"">");
                //    }


                //    sb.Append(@"<table>");
                //    sb.Append(@"<tr>");
                //    sb.Append(@"<td>");

                //    sb.Append(@"<ul>");
                //    sb.Append(ua.ToUnorderdListItem);
                //    sb.Append(@"</ul>");

                //    sb.Append(@"</td>");
                //    sb.Append(@"<td>");

                //    sb.Append(@"<ul><li>");

                //    sb.Append(@"<ul>");
                //    sb.Append(su.ToUnorderdListItem);
                //    sb.Append(@"</ul>");

                //    //sb.Append(@"<span class=""status_count"">");
                //    //sb.Append(Acknowledgements.GetAcknowledgementCount(su.StatusUpdateID));
                //    //sb.Append(@"</span>");
                //    //sb.Append("<br />");

                //    //if (mu != null &&
                //    //    Acknowledgement.IsUserAcknowledgement(
                //    //    su.StatusUpdateID, Convert.ToInt32(mu.ProviderUserKey)))
                //    //{
                //    //    sb.Append(@"<img alt=""you gave applause!"" title=""you gave applause!""");
                //    //    sb.Append(@" src=""");
                //    //    sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/content/images/clap_check.png"));
                //    //    sb.Append(@""" />");
                //    //}
                //    //else
                //    //{
                //    //    sb.Append(@"<button title=""applaud post"" name=""status_update_id"" class=""applaud_status"" type=""submit"" value=""" + su.StatusUpdateID.ToString() + @""">Applaud</button>");
                //    //}


                //    if (membID != 0 && ua.UserAccountID == membID)
                //    {
                //        sb.Append("<br />");
                //        sb.Append(@"<button title=""delete your post"" name=""delete_status_id"" class=""delete_icon"" type=""submit"" value=""" + su.ContentCommentID.ToString() + @""">Delete</button>");
                //        //sb.Append(@"<a title=""delete your post"" class=""delete_icon"" href=""#"" onclick=""document.delete_post.submit();return false"">&nbsp;</a>");
                //    }

                //    sb.Append(@"</div>");
                //    sb.Append(@"</li></ul>");
                //    sb.Append(@"</td>");

                //    sb.Append(@"</tr>");
                //    sb.Append(@"</table>");


                //    sb.Append(@"</li>");
                //}

                //sb.Append(@"</form>");

                //if (IncludeStartAndEndTags) sb.Append(@"</ul>");


                return sb.ToString();
            }
        }


        public void GetUserContentComments(int createdByUserID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserContentComments";

            comm.AddParameter("createdByUserID", createdByUserID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ContentComment ccomm = null;

                foreach (DataRow dr in dt.Rows)
                {
                    ccomm = new ContentComment(dr);
                    Add(ccomm);
                }
            }
        }


        public void GetCommentsForContent(int contentID, SiteEnums.CommentStatus status)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentComments";

            comm.AddParameter("contentID", contentID);
            comm.AddParameter("statusType", status.ToString());

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ContentComment ccomm = null;

                foreach (DataRow dr in dt.Rows)
                {
                    ccomm = new ContentComment(dr);
                    Add(ccomm);
                }
            }
        }


        public int GetCommentsPageWise(int pageIndex, int pageSize)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentCommentsPageWise";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ContentComment content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new ContentComment(dr);
                    Add(content);
                }
            }

            return recordCount;
        }
    }
}