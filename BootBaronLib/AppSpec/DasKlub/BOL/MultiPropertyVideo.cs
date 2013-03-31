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
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class MultiPropertyVideo
    {
        public MultiPropertyVideo(DataRow dr)
        {
            Get(dr);
        }


        public void Get(DataRow dr)
        {
            MultiPropertyID = FromObj.IntFromObj(dr["multiPropertyID"]);
            ProductID = FromObj.IntFromObj(dr["productID"]);
        }

        public static bool AddMultiPropertyVideo(int multiPropertyID, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddMultiPropertyVideo";

            comm.AddParameter("multiPropertyID", multiPropertyID);
            comm.AddParameter("videoID", videoID);

            return Convert.ToInt32(DbAct.ExecuteScalar(comm)) > 0;
        }

        public static bool DeleteMultiPropertyVideo(int multiPropertyID, int videoID)
        {
            if (multiPropertyID == 0 || videoID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteMultiPropertyVideo";

            comm.AddParameter("multiPropertyID", multiPropertyID);
            comm.AddParameter("videoID", videoID);

            return Convert.ToInt32(DbAct.ExecuteNonQuery(comm)) > 0;
        }

        #region properties

        public int MultiPropertyID { get; set; }

        public int ProductID { get; set; }

        #endregion

        //public static bool DeletePropertyTypeForVideo(int propertyTypeID, int videoID)
        //{
        //    if (propertyTypeID == 0 || videoID == 0) return false;

        //    // get a configured DbCommand object
        //    DbCommand comm = DbAct.CreateCommand();
        //    // set the stored procedure name
        //    comm.CommandText = "up_DeleteMultiPropertyVideo";
        //    // create a new parameter
        //    DbParameter param = comm.CreateParameter();

        //    param.ParameterName = "@multiPropertyID";
        //    param.Value = propertyTypeID;
        //    param.DbType = DbType.Int32;
        //    comm.Parameters.Add(param);
        //    //
        //    param = comm.CreateParameter();
        //    param.ParameterName = "@videoID";
        //    param.Value = videoID;
        //    param.DbType = DbType.Int32;
        //    comm.Parameters.Add(param);

        //    return Convert.ToInt32(DbAct.ExecuteNonQuery(comm)) > 0;
        //}
    }

    public class MultiPropertyVideos : List<MultiPropertyVideo>
    {
        public void GetMultiPropertyVideoForProduct(int productID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMultiPropertyVideoForProduct";

            comm.AddParameter("productID", productID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);
            MultiPropertyVideo pd = null;
            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    pd = new MultiPropertyVideo(dr);
                    Add(pd);
                }
            }
        }
    }
}