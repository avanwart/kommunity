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
using System.Web;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class EventCycle : BaseIUserLogCRUD, ICacheName
    {
        #region contstructor

        public EventCycle(int eventCycleID)
        {
            EventCycleID = eventCycleID;

            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetEventCycleByID";
                // create a new parameter
                DbParameter param = comm.CreateParameter();

                param.ParameterName = "@eventCycleID";
                param.Value = eventCycleID;
                param.DbType = DbType.Int32;
                comm.Parameters.Add(param);

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public EventCycle()
        {
            // TODO: Complete member initialization
        }

        public EventCycle(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        #region properties

        private string _cycleName = string.Empty;

        private string _eventCode = string.Empty;
        public int EventCycleID { get; set; }

        public string CycleName
        {
            get { return _cycleName; }
            set { _cycleName = value; }
        }

        public string EventCode
        {
            get { return _eventCode; }
            set { _eventCode = value; }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                EventCycleID = FromObj.IntFromObj(dr["eventCycleID"]);
                CycleName = FromObj.StringFromObj(dr["cycleName"]);
                EventCode = FromObj.StringFromObj(dr["eventCode"]);
            }
            catch // (Exception ex)
            {
                //Utilities.LogError(ex);
            }
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return GetType().FullName + "-" + EventCycleID; }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion
    }

    public class EventCycles : List<EventCycle>, IGetAll
    {
        #region IGetAll Members

        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllEventCycles";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                EventCycle envtcyc = null;

                foreach (DataRow dr in dt.Rows)
                {
                    envtcyc = new EventCycle(dr);
                    Add(envtcyc);
                }
            }
        }

        #endregion
    }
}