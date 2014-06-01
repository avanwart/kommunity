using System;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.ArtistContent
{
    public class ArtistProperty : BaseIUserLogCrud 
    {
        #region properties

        private string _propertyContent = string.Empty;
        private string _propertyType = string.Empty;
        public int ArtistPropertyID { get; private set; }

        public int ArtistID { get; private set; }

        public string PropertyContent
        {
            get { return _propertyContent; }
            set { _propertyContent = value; }
        }

        /// <summary>
        ///     Options are:
        ///     MD = meta description
        ///     LD = long description
        ///     PH = photo
        /// </summary>
        private string PropertyType
        {
            get { return _propertyType; }
            set { _propertyType = value; }
        }

        #endregion

        #region constants

        public const string Artistimageprefix = "~/content/artists/";

        #endregion

        #region constructors

        #endregion

        #region methods

        public void GetArtistPropertyForTypeArtist(int artistID, string propertyType)
        {
            ArtistID = artistID;
            PropertyType = propertyType;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistPropertyForTypeArtist";

            comm.AddParameter("artistID", ArtistID);
            comm.AddParameter("propertyType", PropertyType);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }


        public override bool Update()
        {
            if (ArtistPropertyID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateArtistProperty";

            // create a new parameter
            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("artistID", ArtistID);
            comm.AddParameter("propertyContent", PropertyContent);
            comm.AddParameter("propertyType", PropertyType);
            comm.AddParameter("artistPropertyID", ArtistPropertyID);

            // result will represent the number of changed rows
            // execute the stored procedure
            var result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));
 
            return result;
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddArtistProperty";

            // create a new parameter
            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("artistID", ArtistID);
            comm.AddParameter("propertyContent", PropertyContent);
            comm.AddParameter("propertyType", PropertyType);

            // the result is their ID
            // execute the stored procedure
            var result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            ArtistPropertyID = Convert.ToInt32(result);

            return ArtistPropertyID;
        }


        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteArtistProperty";

            comm.AddParameter("artistPropertyID", ArtistPropertyID);

   
            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                ArtistID = FromObj.IntFromObj(dr["artistID"]);
                PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);
                PropertyType = FromObj.StringFromObj(dr["propertyType"]);
                ArtistPropertyID = FromObj.IntFromObj(dr["artistPropertyID"]);
            }
            catch
            {
            }
        }

        #endregion
 
    }
}