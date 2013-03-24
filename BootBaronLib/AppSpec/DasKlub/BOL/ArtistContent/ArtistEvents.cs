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
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class ArtistEvent : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private int _artistID = 0;

        public int ArtistID
        {
            get { return _artistID; }
            set { _artistID = value; }
        }

        private int _eventID = 0;

        public int EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }

        private int _rankOrder = 0;
        
        public ArtistEvent(DataRow dr)
        {
            Get(dr);
        }



        public ArtistEvent()
        {
            // TODO: Complete member initialization
        }

        public int RankOrder
        {
            get { return _rankOrder; }
            set { _rankOrder = value; }
        }

        #endregion

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.ArtistID = FromObj.IntFromObj(dr["artistID"]);
                this.EventID = FromObj.IntFromObj(dr["eventID"]);
                this.RankOrder = FromObj.IntFromObj(dr["rankOrder"]);
            }
            catch // (Exception ex)
            {
                //Utilities.LogError(ex);
            }
        }


        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }


        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }

        #endregion


        public override bool Delete()
        {

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteEventByID";

            ADOExtenstion.AddParameter(comm, "eventID", EventID);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddArtistEvent";


            ADOExtenstion.AddParameter(comm, "artistID", ArtistID);
            ADOExtenstion.AddParameter(comm, "eventID", EventID);
            ADOExtenstion.AddParameter(comm, "rankOrder", RankOrder);

            return DbAct.ExecuteNonQuery(comm);
        }

    }


    public class ArtistEvents : List<ArtistEvent>, IGetAll
    {
        public void GetArtistsForEvent(int eventID)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistsForEvent";

            ADOExtenstion.AddParameter(comm, "eventID", eventID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ArtistEvent atd = null;

                foreach (DataRow dr in dt.Rows)
                {
                    atd = new ArtistEvent(dr);

                    this.Add(atd);
                }
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
                    this.Add(td);
                }
            }
        }

        #endregion
    }
}