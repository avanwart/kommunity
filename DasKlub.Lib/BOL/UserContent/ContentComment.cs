using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
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
    public class ContentComment : BaseIUserLogCrud, IUnorderdListItem, IURLTo
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
                var theCo = SiteEnums.CommentStatus.U;

                return Enum.TryParse(StatusType.ToString(), out theCo)
                    ? Utilities.ResourceValue(Utilities.GetEnumDescription(theCo))
                    : string.Empty;
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
                sb.Append(Utilities.ConvertTextToHtml(Detail));
                sb.Append(@"</p>");

                MembershipUser mu = Membership.GetUser();

                if (mu != null)
                {
                    var user = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

                    if (CreatedByUserID == Convert.ToInt32(mu.ProviderUserKey) || user.IsAdmin)
                    {
                        sb.AppendFormat(@"<a href=""{0}"">{1}</a>", VirtualPathUtility.ToAbsolute(
                            string.Format("~/news/deletecomment?commentID={0}", ContentCommentID)), Messages.Delete);
                    }
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
            ContentCommentID = Convert.ToInt32(result);

            return ContentCommentID;
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


    public class ContentComments : List<ContentComment>
    {
        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
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
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (ContentComment ccomm in from DataRow dr in dt.Rows select new ContentComment(dr))
            {
                Add(ccomm);
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
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (ContentComment ccomm in from DataRow dr in dt.Rows select new ContentComment(dr))
            {
                Add(ccomm);
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

            if (ds == null || ds.Tables[0].Rows.Count <= 0) return recordCount;

            foreach (ContentComment content in from DataRow dr in ds.Tables[0].Rows select new ContentComment(dr))
            {
                Add(content);
            }

            return recordCount;
        }
    }
}