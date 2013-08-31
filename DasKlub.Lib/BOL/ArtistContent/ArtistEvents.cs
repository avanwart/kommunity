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
using System.Web;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.ArtistContent
{
    public class ArtistEvent : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        public ArtistEvent(DataRow dr)
        {
            Get(dr);
        }


        public ArtistEvent()
        {
            // TODO: Complete member initialization
        }

        public int ArtistID { get; set; }

        public int EventID { private get; set; }

        public int RankOrder { private get; set; }

        #endregion

        public override sealed void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                ArtistID = FromObj.IntFromObj(dr["artistID"]);
                EventID = FromObj.IntFromObj(dr["eventID"]);
                RankOrder = FromObj.IntFromObj(dr["rankOrder"]);
            }
            catch // (Exception ex)
            {
                //Utilities.LogError(ex);
            }
        }

        public override bool Delete()
        {
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteEventByID";

            comm.AddParameter("eventID", EventID);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddArtistEvent";


            comm.AddParameter("artistID", ArtistID);
            comm.AddParameter("eventID", EventID);
            comm.AddParameter("rankOrder", RankOrder);

            return DbAct.ExecuteNonQuery(comm);
        }

        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }


        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion
    }


    public class ArtistEvents : List<ArtistEvent>, IGetAll
    {
        public void GetArtistsForEvent(int eventID)
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistsForEvent";

            comm.AddParameter("eventID", eventID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (ArtistEvent atd in from DataRow dr in dt.Rows select new ArtistEvent(dr))
            {
                Add(atd);
            }
        }

        #region IGetAll Members

        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllArtistEvents";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ArtistEvent td = null;
                foreach (DataRow dr in dt.Rows)
                {
                    td = new ArtistEvent(dr);
                    Add(td);
                }
            }
        }

        #endregion
    }
}