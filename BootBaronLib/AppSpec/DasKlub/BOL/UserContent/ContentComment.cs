//  Copyright 2012 
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
using System.Text;
using System.Web.UI;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational.Converters;
using System.Web.Security;

using System.Linq;
using System.Text.RegularExpressions;
using BootBaronLib.Operational;
using System.ComponentModel.DataAnnotations;
using BootBaronLib.Enums;
using System.Web;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL.UserContent
{
    public class ContentComment : BaseIUserLogCRUD, IUnorderdListItem, IURLTo
    {
        #region properties

        private int _contentCommentID = 0;

        public int ContentCommentID
        {
            get { return _contentCommentID; }
            set { _contentCommentID = value; }
        }

        private char _statusType = char.MinValue;

        [Required(ErrorMessageResourceName = "Required",
ErrorMessageResourceType = typeof(Resources.Messages))]
 
        public char StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        private string _detail = string.Empty;

        [Required(ErrorMessageResourceName = "Required",
        ErrorMessageResourceType = typeof(Resources.Messages))]
        [Display(ResourceType = typeof(Resources.Messages), Name = "Message")]
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        private string _fromName = string.Empty;

 
        public string FromName
        {
            get { return _fromName; }
            set { _fromName = value; }
        }


        private string _ipAddress = string.Empty;

        [Required(ErrorMessageResourceName = "Required",
       ErrorMessageResourceType = typeof(Resources.Messages))]
        [Display(ResourceType = typeof(Resources.Messages), Name = "IpAddress")]
        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }



        private string _fromEmail = string.Empty;

        [RegularExpression(@".+\@.+\..+", ErrorMessageResourceName = "IncorrectFormat", ErrorMessageResourceType =
typeof(Resources.Messages))]
        [Display(ResourceType = typeof(Resources.Messages), Name = "EMail")]
        public string FromEmail
        {
            get { return _fromEmail; }
            set { _fromEmail = value; }
        }

        private int _contentID = 0;

        public int ContentID
        {
            get { return _contentID; }
            set { _contentID = value; }
        }

        #endregion

        #region constructors

        public ContentComment(DataRow dr)
        {
            Get(dr);
        }

        public ContentComment()
        {
            // TODO: Complete member initialization
        }


        public ContentComment(int p)
        {
            Get(p);
        }

        #endregion

        public override void Get(int uniqueID)
        {
            this.ContentCommentID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentComment";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentCommentID), ContentCommentID);

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

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusType), StatusType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Detail), Detail);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentID), ContentID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FromName), FromName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FromEmail), FromEmail);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IpAddress), IpAddress);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentCommentID), ContentCommentID);

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

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusType), StatusType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Detail), Detail);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentID), ContentID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FromName), FromName);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FromEmail), FromEmail);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IpAddress), IpAddress);

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
                this.ContentCommentID = Convert.ToInt32(result);

                return this.ContentCommentID;
            }
        }


        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.ContentCommentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentCommentID)]);
                this.StatusType = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.StatusType)]);
                this.ContentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentID)]);
                this.Detail = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Detail)]);
                this.FromEmail = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FromEmail)]);
                this.FromName = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FromName)]);
                this.IpAddress = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.IpAddress)]);
            }
            catch { }
        }


        public string ToUnorderdListItem
        {
            get
            {
                if (this.ContentCommentID == 0) return string.Empty;

                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<li>");

                sb.Append(@"<hr />");


                sb.Append(@"<br />");

                sb.Append(@"<i>");
                sb.Append(Utilities.TimeElapsedMessage(CreateDate));
                sb.Append(@"</i>");
                sb.Append(@"<br />");

                if (this.CreatedByUserID > 0)
                {
                    UserAccount ua = new UserAccount(this.CreatedByUserID);

                    UserAccountDetail uad = new UserAccountDetail();

                    uad.GetUserAccountDeailForUser(ua.UserAccountID);

                    sb.Append(uad.SmallUserIcon);
                }

                sb.Append(@"<br />");

                sb.Append(@"<p>");
                sb.Append(FromString.ReplaceNewLineWithHTML( this.Detail));
                sb.Append(@"</p>");

                MembershipUser mu = Membership.GetUser();

                if (mu != null && this.CreatedByUserID == Convert.ToInt32(mu.ProviderUserKey))
                {
                    sb.AppendFormat(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(
                        "~/news/DeleteComment?commentID=" + this.ContentCommentID.ToString()), Messages.Delete);
                }

                sb.Append(@"</li>");

                return sb.ToString();
            }
        }


        public override bool Delete()
        {
            if (this.ContentCommentID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteContentComment";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentCommentID), ContentCommentID);

            //RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public string StatusTypeName
        {
            get
            {
                SiteEnums.CommentStatus theCO = SiteEnums.CommentStatus.U;

                if (Enum.TryParse(this.StatusType.ToString(), out theCO))
                {
                    return Utilities.ResourceValue(Utilities.GetEnumDescription(theCO));
                }

                return string.Empty;
            }
        }


        public Uri UrlTo
        {
            get
            {
                Content cnt = new Content(this.ContentID);
                return cnt.UrlTo;
            }
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
                if (this.Count == 0) return string.Empty;

                //MembershipUser mu = Membership.GetUser();

                //int membID = (mu != null) ? Convert.ToInt32(mu.ProviderUserKey) : 0;

                StringBuilder sb = new StringBuilder(100);

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





        public void  GetUserContentComments(int createdByUserID)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserContentComments";

            ADOExtenstion.AddParameter(comm, "createdByUserID", createdByUserID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ContentComment ccomm = null;

                foreach (DataRow dr in dt.Rows)
                {
                    ccomm = new ContentComment(dr);
                    this.Add(ccomm);
                }
            }

        }


        public void GetCommentsForContent(int contentID, SiteEnums.CommentStatus status)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentComments";

            ADOExtenstion.AddParameter(comm, "contentID", contentID);
            ADOExtenstion.AddParameter(comm, "statusType", status.ToString());

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ContentComment ccomm = null;

                foreach (DataRow dr in dt.Rows)
                {
                    ccomm = new ContentComment(dr);
                    this.Add(ccomm);
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

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ContentComment content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new ContentComment(dr);
                    this.Add(content);
                }
            }

            return recordCount;
        }



    }
}
