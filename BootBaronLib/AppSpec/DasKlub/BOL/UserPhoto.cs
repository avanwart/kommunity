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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Operational.Converters;
using System;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserPhoto : BaseIUserLogCRUD
    {
        #region properties

        private int _userPhotoID = 0;

        public int UserPhotoID
        {
            get { return _userPhotoID; }
            set { _userPhotoID = value; }
        }

        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }

        private string _picURL = string.Empty;

        public string PicURL
        {
            get { return _picURL; }
            set { _picURL = value; }
        }

        private string _thumbPicURL = string.Empty;

        public string ThumbPicURL
        {
            get { return _thumbPicURL; }
            set { _thumbPicURL = value; }
        }

        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private int _rankOrder = 0;
   

        public int RankOrder
        {
            get {
                
                return _rankOrder; }
            set { _rankOrder = value; }
        }


        #region non db

        public string FullProfilePicURL
        {
            get
            {

                if (string.IsNullOrEmpty(this.PicURL))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuser.png");
                }
                else
                {
                    return Utilities.S3ContentPath(this.PicURL); 
                }
            }
        }


        public string FullProfilePicThumbURL
        {
            get
            {
                if (string.IsNullOrEmpty(this.ThumbPicURL))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuserthumb.png");
                }
                else
                {
                    return Utilities.S3ContentPath(this.ThumbPicURL); 
                }
            }
        }

        #endregion


        #endregion

        #region constructors

        public UserPhoto() { }

        public UserPhoto(DataRow dr) { Get(dr); }

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

                this.UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                this.Description = FromObj.StringFromObj(dr["description"]);
                this.PicURL = FromObj.StringFromObj(dr["picURL"]);
                this.RankOrder = FromObj.IntFromObj(dr["rankOrder"]);
                this.ThumbPicURL = FromObj.StringFromObj(dr["thumbPicURL"]);
                this.UserPhotoID = FromObj.IntFromObj(dr["userPhotoID"]);
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

            ADOExtenstion.AddParameter(comm, "userPhotoID", userPhotoID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        public override bool Delete()
        {
            if (this.UserPhotoID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserPhotoByID";

            ADOExtenstion.AddParameter(comm, "userPhotoID", UserPhotoID);
            
            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserPhoto";

            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);
            ADOExtenstion.AddParameter(comm, "picURL", PicURL);
            ADOExtenstion.AddParameter(comm, "thumbPicURL", ThumbPicURL);
            ADOExtenstion.AddParameter(comm, "description", Description);
            ADOExtenstion.AddParameter(comm, "rankOrder",  RankOrder);


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
                this.UserPhotoID = Convert.ToInt32(result);

                return this.UserPhotoID;
            }



        }

        public override bool Update()
        {

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateUserPhoto";

            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "userAccountID",  UserAccountID);
            ADOExtenstion.AddParameter(comm, "picURL",  PicURL);
            ADOExtenstion.AddParameter(comm, "thumbPicURL", ThumbPicURL);
            ADOExtenstion.AddParameter(comm, "description", Description);
            ADOExtenstion.AddParameter(comm, "rankOrder", RankOrder);
            ADOExtenstion.AddParameter(comm, "userPhotoID", UserPhotoID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            return (result != -1);
        }

        #endregion
    }


    public class UserPhotos : List<UserPhoto>
    {
        public UserPhotos() { }

        public void GetUserPhotos(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserPhotos";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);
             
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                UserPhoto up = null;

                foreach (DataRow dr in dt.Rows)
                {
                    up = new UserPhoto(dr);
                    this.Add(up);
                }
            }

            this.Sort(delegate(UserPhoto p1, UserPhoto p2)
            {
                return p1.RankOrder.CompareTo(p2.RankOrder);
            });

        }
    }
}
