using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class UserPhoto : BaseIUserLogCrud
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
                return string.IsNullOrEmpty(PicURL)
                    ? VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuser.png")
                    : Utilities.S3ContentPath(PicURL);
            }
        }


        public string FullProfilePicThumbURL
        {
            get
            {
                return string.IsNullOrEmpty(ThumbPicURL)
                    ? VirtualPathUtility.ToAbsolute("~/content/images/users/defaultuserthumb.png")
                    : Utilities.S3ContentPath(ThumbPicURL);
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
            UserPhotoID = Convert.ToInt32(result);

            return UserPhotoID;
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

            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (var up in from DataRow dr in dt.Rows select new UserPhoto(dr))
                {
                    Add(up);
                }
            }

            Sort((p1, p2) => p1.RankOrder.CompareTo(p2.RankOrder));
        }
    }
}