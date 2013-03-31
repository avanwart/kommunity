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
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;


namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class EventCycle : BaseIUserLogCRUD, ICacheName 
    {
        #region contstructor

        public EventCycle(int eventCycleID)
        {
            this.EventCycleID = eventCycleID;

            if (HttpContext.Current.Cache[this.CacheName] == null)
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
                    HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
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

        private int _eventCycleID = 0;

        public int EventCycleID
        {
            get { return _eventCycleID; }
            set { _eventCycleID = value; }
        }

        private string _cycleName = string.Empty;

        public string CycleName
        {
            get { return _cycleName; }
            set { _cycleName = value; }
        }

        private string _eventCode = string.Empty;
  
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

                this.EventCycleID = FromObj.IntFromObj(dr["eventCycleID"]);
                this.CycleName = FromObj.StringFromObj(dr["cycleName"]);
                this.EventCode = FromObj.StringFromObj(dr["eventCode"]);
                
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
            get
            {
                return this.GetType().FullName + "-" + this.EventCycleID;
            }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }

        #endregion
 
    }

    public class EventCycles : List<EventCycle>, IGetAll
    {
        public EventCycles() { }


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
                    this.Add(envtcyc);
                }
            }
        }

        #endregion
    }

}
