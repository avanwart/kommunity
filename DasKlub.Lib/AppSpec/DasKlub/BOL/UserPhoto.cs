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
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.AppSpec.DasKlub.BOL
{
    public class UserPhoto : BaseIUserLogCRUD
    {
        #region properties

        private string _description = string.Empty;
        private string _picURL = string.Empty;
        private string _thumbPicURL = string.Empty;
        public int UserPhotoID { get; set; }

        public int UserAccountID { get; set; }

        public string PicURL
        {
            get { return _picURL; }
            set { _picURL = value; }
        }

        public string ThumbPicURL
        {
            get { return _thumbPicURL; }
            set { _thumbPicURL = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        public int RankOrder { get; set; }

        #region non db

        public string FullProfilePicURL
        {
            get
            {
                if (string.IsNullOrEmpty(PicURL))
                {
                    return VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuser.png");
                }
                else
                {
                    return Utilities.S3ContentPath(PicURL);
                }
            }
        }


        public string FullProfilePicThumbURL
        {
            get
            {
                if (string.IsNullOrEmpty(ThumbPicURL))
                {
                    return VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuserthumb.png");
                }
                else
                {
                    return Utilities.S3ContentPath(ThumbPicURL);
                }
            }
        }

        #endregion

        #endregion

        #region constructors

        public UserPhoto()
        {
        }

        public UserPhoto(DataRow dr)
        {
            Get(dr);
        }

        public UserPhoto(int userPhotoID)
        {
            Get(userPhotoID);
        }

        #endregion

        #region methods

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                Description = FromObj.StringFromObj(dr["description"]);
                PicURL = FromObj.StringFromObj(dr["picURL"]);
                RankOrder = FromObj.IntFromObj(dr["rankOrder"]);
                ThumbPicURL = FromObj.StringFromObj(dr["thumbPicURL"]);
                UserPhotoID = FromObj.IntFromObj(dr["userPhotoID"]);
            }
            catch
            {
            }
        }

        public override void Get(int userPhotoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserPhotoByID";

            comm.AddParameter("userPhotoID", userPhotoID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        public override bool Delete()
        {
            if (UserPhotoID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserPhotoByID";

            comm.AddParameter("userPhotoID", UserPhotoID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserPhoto";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("picURL", PicURL);
            comm.AddParameter("thumbPicURL", ThumbPicURL);
            comm.AddParameter("description", Description);
            comm.AddParameter("rankOrder", RankOrder);


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
                UserPhotoID = Convert.ToInt32(result);

                return UserPhotoID;
            }
        }

        public override bool Update()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserPhoto";

            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("picURL", PicURL);
            comm.AddParameter("thumbPicURL", ThumbPicURL);
            comm.AddParameter("description", Description);
            comm.AddParameter("rankOrder", RankOrder);
            comm.AddParameter("userPhotoID", UserPhotoID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            return (result != -1);
        }

        #endregion
    }


    public class UserPhotos : List<UserPhoto>
    {
        public void GetUserPhotos(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserPhotos";

            comm.AddParameter("userAccountID", userAccountID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                UserPhoto up = null;

                foreach (DataRow dr in dt.Rows)
                {
                    up = new UserPhoto(dr);
                    Add(up);
                }
            }

            Sort(delegate(UserPhoto p1, UserPhoto p2) { return p1.RankOrder.CompareTo(p2.RankOrder); });
        }
    }
}