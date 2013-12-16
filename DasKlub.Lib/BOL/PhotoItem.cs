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
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class PhotoItem : BaseIUserLogCRUD, IUnorderdListItem
    {
        private bool _showTitle = true;

        public PhotoItem()
        {
        }

        public PhotoItem(DataRow dr)
        {
            Get(dr);
        }

        public PhotoItem(int photoItemID)
        {
            Get(photoItemID);
        }

        #region properties

        private string _filePathRaw = string.Empty;
        private string _filePathStandard = string.Empty;

        private string _filePathThumb = string.Empty;
        private string _title = string.Empty;
        public int PhotoItemID { get; set; }

        public string FilePathRaw
        {
            get { return _filePathRaw; }
            set { _filePathRaw = value; }
        }

        public string FilePathThumb
        {
            get { return _filePathThumb; }
            set { _filePathThumb = value; }
        }

        public string FilePathStandard
        {
            get { return _filePathStandard; }
            set { _filePathStandard = value; }
        }

        public string Title
        {
            get
            {
                if (_title == null) return string.Empty;
                return _title.Trim();
            }
            set { _title = value; }
        }

        #endregion

        public string UploadingUserName
        {
            get
            {
                var ua = new UserAccount(CreatedByUserID);

                return ua.UserName;
            }
        }

        public bool IsUserPhoto { get; set; }


        public bool UseThumb { get; set; }

        public bool ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; }
        }

        #region methods

        public void GetPreviousPhotoForUser(DateTime createDateCurrent, int createdByUserID)
        {
            CreatedByUserID = createdByUserID;

            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPreviousPhotoForUser";

            comm.AddParameter("createdByUserID", createdByUserID);
            comm.AddParameter("createDateCurrent", createDateCurrent);

            // execute the stored procedure
            var dt = DbAct.ExecuteSelectCommand(comm);

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
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPreviousPhoto";

            comm.AddParameter("createDateCurrent", createDateCurrent);

            // execute the stored procedure
            var dt = DbAct.ExecuteSelectCommand(comm);

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
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNextPhotoForUser";

            comm.AddParameter("createdByUserID", createdByUserID);
            comm.AddParameter("createDateCurrent", createDateCurrent);

            // execute the stored procedure
            var dt = DbAct.ExecuteSelectCommand(comm);

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
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNextPhoto";

            comm.AddParameter("createDateCurrent", createDateCurrent);

            // execute the stored procedure
            var dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override int Create()
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_AddPhotoItem";

            // create a new parameter
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Title), Title);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FilePathRaw), FilePathRaw);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FilePathThumb), FilePathThumb);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FilePathStandard), FilePathStandard);

            // the result is their ID
            // execute the stored procedure
            var result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            PhotoItemID = Convert.ToInt32(result);

            return PhotoItemID;
        }

        public override bool Delete()
        {
            if (PhotoItemID == 0) return false;

            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeletePhotoItem";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PhotoItemID), PhotoItemID);

            //RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override void Get(int uniqueID)
        {
            PhotoItemID = uniqueID;

            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItem";
            
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PhotoItemID), PhotoItemID);

            // execute the stored procedure
            var dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public override void Get(DataRow dr)
        {
            base.Get(dr);

            PhotoItemID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => PhotoItemID)]);
            Title = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Title)]);
            FilePathRaw = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => FilePathRaw)]);
            FilePathThumb = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => FilePathThumb)]);
            FilePathStandard = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => FilePathStandard)]);
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_UpdatePhotoItem";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UpdatedByUserID), UpdatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => PhotoItemID), PhotoItemID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Title), Title);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FilePathRaw), FilePathRaw);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FilePathThumb), FilePathThumb);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => FilePathStandard), FilePathStandard);

            var result = DbAct.ExecuteNonQuery(comm);

            //RemoveCache();

            return (result != -1);
        }

        #endregion

        public string ToUnorderdListItem
        {
            get
            {
                var ua = new UserAccount(CreatedByUserID);

                var sb = new StringBuilder(100);

                sb.Append(@"<li>");
                if (ShowTitle)
                {
                    sb.Append(@"<span>");
                    sb.Append(Title);
                    sb.Append(@"</span>");
                    sb.Append("<br />");
                }
                sb.Append(@"<a href=""");

                sb.Append(!IsUserPhoto
                    ? VirtualPathUtility.ToAbsolute(string.Format("~/photos/{0}", PhotoItemID))
                    : VirtualPathUtility.ToAbsolute(string.Format("~/{0}/userphoto/{1}", ua.UserName, PhotoItemID)));
                sb.Append(@""">");

                sb.Append(@"<img src=""");
                sb.Append(UseThumb ? Utilities.S3ContentPath(FilePathThumb) : Utilities.S3ContentPath(FilePathStandard));
                sb.Append(@""" alt=""");
                sb.Append(Title);
                sb.Append(@"""");
                sb.Append(@" title=""");
                sb.Append(Title);
                sb.Append(@"""");
                sb.Append(@" />");
                sb.Append(@"</a> ");


                sb.Append(@"</li>");

                return sb.ToString();
            }
 
        }

    }


    public class PhotoItems : List<PhotoItem>, IUnorderdList
    {
        private bool _includeStartAndEndTags = true;

        private bool _showTitle = true;
        public bool IsUserPhoto { get; set; }

        public bool ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; }
        }

        public bool UseThumb { get; set; }

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }

        public string ToUnorderdList
        {
            get
            {
                var sb = new StringBuilder(100);

                if (IncludeStartAndEndTags)
                {
                    sb.Append(@"<ul class=""photo_item_list"">");
                }

                foreach (PhotoItem pitm in this)
                {
                    pitm.IsUserPhoto = IsUserPhoto;
                    pitm.UseThumb = UseThumb;
                    pitm.ShowTitle = ShowTitle;
                    sb.Append(pitm.ToUnorderdListItem);
                }
                if (IncludeStartAndEndTags)
                {
                    sb.Append(@"</ul>");
                }

                return sb.ToString();
            }
        }

        public static int GetPhotoItemCountForUser(int createdByUserID)
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItemCountForUser";

            comm.AddParameter("createdByUserID", createdByUserID);

            // execute the stored procedure
            var str = DbAct.ExecuteScalar(comm);

            return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
        }


        public int GetPhotoItemsPageWise(int pageIndex, int pageSize)
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItemsPageWise";

            var param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);

            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            if (ds == null) return 0;

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds.Tables[0].Rows.Count <= 0) return recordCount;
            
            foreach (var pitm in from DataRow dr in ds.Tables[0].Rows select new PhotoItem(dr))
            {
                Add(pitm);
            }

            return recordCount;
        }

        public void GetUserPhotos(int createdByUserID)
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_GetPhotoItemForUser";
            comm.AddParameter("createdByUserID", createdByUserID);

            // execute the stored procedure
            var dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var pitm in from DataRow dr in dt.Rows select new PhotoItem(dr))
            {
                Add(pitm);
            }
        }
    }
}