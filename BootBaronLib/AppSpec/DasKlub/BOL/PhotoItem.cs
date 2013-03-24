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
using System.Text;
using System.Web.Security;
using System.Web.UI;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using System.Web;
using BootBaronLib.Enums;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class PhotoItem : BaseIUserLogCRUD, IUnorderdListItem
    {

        public PhotoItem() { }

        public PhotoItem(DataRow dr) { Get(dr); }

        public PhotoItem(int photoItemID) { Get(photoItemID); }



        #region properties

        private int _photoItemID = 0;

        public int PhotoItemID
        {
            get { return _photoItemID; }
            set { _photoItemID = value; }
        }

        private string _filePathRaw = string.Empty;

        public string FilePathRaw
        {
            get { return _filePathRaw; }
            set { _filePathRaw = value; }
        }

        private string _filePathThumb = string.Empty;

        public string FilePathThumb
        {
            get { return _filePathThumb; }
            set { _filePathThumb = value; }
        }

        private string _filePathStandard = string.Empty;

        public string FilePathStandard
        {
            get { return _filePathStandard; }
            set { _filePathStandard = value; }
        }

        private string _title = string.Empty;

        public string Title
        {
            get {
                if (_title == null) return string.Empty;
                return _title.Trim(); }
            set { _title = value; }
        }

        #endregion


        public string UploadingUserName
        {
            get
            {
                UserAccount ua = new UserAccount(this.CreatedByUserID);

                return ua.UserName;
            }

        }


        #region methods


        public void GetPreviousPhotoForUser(DateTime createDateCurrent, int createdByUserID)
        {
            this.CreatedByUserID = createdByUserID;

            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPreviousPhotoForUser";

            ADOExtenstion.AddParameter(comm, "createdByUserID", createdByUserID);
            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }

        }

        public void GetPreviousPhoto(DateTime createDateCurrent)
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPreviousPhoto";

            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }

        }

        public void GetNextPhotoForUser(DateTime createDateCurrent, int createdByUserID)
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNextPhotoForUser";

            ADOExtenstion.AddParameter(comm, "createdByUserID", createdByUserID);
            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }



        public void GetNextPhoto(DateTime createDateCurrent)
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNextPhoto";

            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddPhotoItem";

            // create a new parameter
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Title), Title);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FilePathRaw), FilePathRaw);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FilePathThumb), FilePathThumb);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FilePathStandard), FilePathStandard);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.PhotoItemID = Convert.ToInt32(result);

            return this.PhotoItemID;
        }

        public override bool Delete()
        {
            if (this.PhotoItemID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeletePhotoItem";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PhotoItemID), PhotoItemID);

            //RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override void Get(int uniqueID)
        {
            this.PhotoItemID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItem";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PhotoItemID), PhotoItemID);

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
            base.Get(dr);

            this.PhotoItemID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.PhotoItemID)]);
            this.Title = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Title)]);
            this.FilePathRaw = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FilePathRaw)]);
            this.FilePathThumb = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FilePathThumb)]);
            this.FilePathStandard = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.FilePathStandard)]);
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdatePhotoItem";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PhotoItemID), PhotoItemID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Title), Title);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FilePathRaw), FilePathRaw);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FilePathThumb), FilePathThumb);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.FilePathStandard), FilePathStandard);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            //RemoveCache();

            return (result != -1);

        }

        #endregion


        private bool _isUserPhoto = false;

        public bool IsUserPhoto
        {
            get { return _isUserPhoto; }
            set { _isUserPhoto = value; }
        }



        public string ToUnorderdListItem
        {
            get
            {
                UserAccount ua = new UserAccount(this.CreatedByUserID);

                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<li>");
                if (ShowTitle)
                {
                    sb.Append(@"<span>");
                    sb.Append(this.Title);
                    sb.Append(@"</span>");
                    sb.Append("<br />");
                }
                sb.Append(@"<a href=""");

                if (!IsUserPhoto)
                {
                    sb.Append(System.Web.VirtualPathUtility.ToAbsolute(string.Format( "~/photos/{0}",  this.PhotoItemID)));
                }
                else
                {
                    sb.Append(System.Web.VirtualPathUtility.ToAbsolute(string.Format( "~/{0}/userphoto/{1}", ua.UserName,  this.PhotoItemID)));
                }
                sb.Append(@""">");

                sb.Append(@"<img src=""");
                if (UseThumb)
                {
                    sb.Append(Utilities.S3ContentPath(this.FilePathThumb));
                }
                else
                {
                    sb.Append(Utilities.S3ContentPath(this.FilePathStandard));
                }
                sb.Append(@""" alt=""");
                sb.Append(this.Title);
                sb.Append(@"""");
                sb.Append(@" title=""");
                sb.Append(this.Title);
                sb.Append(@"""");
                sb.Append(@" />");
                sb.Append(@"</a> ");

             
                sb.Append(@"</li>");

                return sb.ToString();
            }


            //get
            //{
            //    UserAccount ua = new UserAccount(this.CreatedByUserID);

            //    StringBuilder sb = new StringBuilder(100);

            //    sb.Append(@"<li>");
            //    if (ShowTitle)
            //    {
            //        sb.Append(@"<span>");
            //        sb.Append(this.Title);
            //        sb.Append(@"</span>");
            //        sb.Append("<br />");
            //    }
            //    sb.Append(@"<a href=""");
            //    sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/photos/detail/" 
            //        + this.PhotoItemID.ToString()));
            //    sb.Append(@""">");

            //    sb.Append(@"<img src=""");
            //    if (UseThumb)
            //    {
            //        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(this.FilePathThumb));
            //    }
            //    else
            //    {
            //        sb.Append(System.Web.VirtualPathUtility.ToAbsolute(this.FilePathStandard));
            //    }
            //    sb.Append(@""" alt=""");
            //    sb.Append(this.Title);
            //    sb.Append(@"""");
            //    sb.Append(@" title=""");
            //    sb.Append(this.Title);
            //    sb.Append(@"""");
            //    sb.Append(@" />");
            //    sb.Append(@"</a> ");

            //    sb.Append("<br />");
            //    sb.Append(Messages.Uploader);
            //    sb.Append(@": <a href=""/");
            //    sb.Append(ua.UserName);
            //    sb.Append(@""">");
            //    sb.Append(ua.UserName);
            //    sb.Append(@"</a> ");
            //    sb.Append("<br />");
            //    sb.AppendFormat(Utilities.TimeElapsedMessage(CreateDate));

            //    sb.Append(@"</li>");

            //    return sb.ToString();
            //}
        }


        private bool _useThumb = false;

        public bool UseThumb
        {
            get { return _useThumb; }
            set { _useThumb = value; }
        }

        private bool _showTitle = true;

        public bool ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; }
        }

    }


    public class PhotoItems : List<PhotoItem>, IUnorderdList
    {

        public static int GetPhotoItemCountForUser(int createdByUserID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItemCountForUser";

            ADOExtenstion.AddParameter(comm, "createdByUserID", createdByUserID);

            // execute the stored procedure
            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            else return Convert.ToInt32(str);
        }

 
       
        public int GetPhotoItemsPageWise(int pageIndex, int pageSize)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItemsPageWise";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            if (ds == null) return 0;

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds.Tables[0].Rows.Count > 0)
            {
                PhotoItem pitm = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    pitm = new PhotoItem(dr);
                    this.Add(pitm);
                }
            }

            return recordCount;
        }

        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }



        private bool _isUserPhoto = false;

        public bool IsUserPhoto
        {
            get { return _isUserPhoto; }
            set { _isUserPhoto = value; }
        }


        public string ToUnorderdList
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                if (IncludeStartAndEndTags)
                {
                    sb.Append(@"<ul class=""photo_item_list"">");
                }

                foreach (PhotoItem pitm in this)
                {
                    pitm.IsUserPhoto = this.IsUserPhoto;
                    pitm.UseThumb = this.UseThumb;
                    pitm.ShowTitle = this.ShowTitle;
                    sb.Append(pitm.ToUnorderdListItem);
                }
                if (IncludeStartAndEndTags)
                {
                    sb.Append(@"</ul>");
                }

                return sb.ToString();

            }
        }

        private bool _showTitle = true;

        public bool ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; }
        }

        private bool _useThumb = false;

        public bool UseThumb
        {
            get { return _useThumb; }
            set { _useThumb = value; }
        }

        public void GetUserPhotos(int createdByUserID)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItemForUser";

            ADOExtenstion.AddParameter(comm, "createdByUserID", createdByUserID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                PhotoItem pitm = null;

                foreach (DataRow dr in dt.Rows)
                {
                    pitm = new PhotoItem(dr);
                    this.Add(pitm);
                }
            }

 
        }
    }
}

