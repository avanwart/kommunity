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

namespace DasKlub.Lib.BOL.VideoContest
{
    public class Contest : BaseIUserLogCRUD
    {
        #region properties

        private DateTime _beginDate = DateTime.MinValue;
        private string _contestKey = string.Empty;
        private DateTime _deadLine = DateTime.MinValue;
        private string _description = string.Empty;
        private string _name = string.Empty;
        public int ContestID { get; set; }

        public string ContestKey
        {
            get { return _contestKey; }
            set { _contestKey = value; }
        }

        public DateTime BeginDate
        {
            get { return _beginDate; }
            set { _beginDate = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public DateTime DeadLine
        {
            get { return _deadLine; }
            set { _deadLine = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        #region constructor

        public Contest(DataRow dr)
        {
            Get(dr);
        }

        public Contest()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region methods

        public void GetContestByName(string name)
        {
            Name = name;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestByName";
            // create a new parameter
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Name), Name);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public void GetContestByKey(string contestKey)
        {
            ContestKey = contestKey;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestByKey";

            // create a new parameter
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContestKey), ContestKey);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override sealed void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);


                Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Name)]);
                ContestID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ContestID)]);
                DeadLine = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => DeadLine)]);
                BeginDate = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => BeginDate)]);
                Description = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Description)]);
                ContestKey = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ContestKey)]);
            }
            catch
            {
            }
        }

        #endregion

        public bool IsHappening
        {
            get
            {
                DateTime dt = Utilities.GetDataBaseTime();

                return (DeadLine > dt && BeginDate < dt);
            }
        }

        public static Contest GetCurrentContest()
        {
            var sns = new Contests();
            sns.GetAll();

            var cndss = new Contest();

            foreach (Contest c1 in sns)
            {
                if (DateTime.UtcNow > c1.BeginDate && DateTime.UtcNow < c1.DeadLine)
                {
                    return c1;
                }
            }
            return null;
        }

        public static Contest GetLastContest()
        {
            var sns = new Contests();
            sns.GetAll();

            var cndss = new Contest();

            sns.Sort(delegate(Contest p1, Contest p2) { return p2.DeadLine.CompareTo(p1.DeadLine); });

            if (sns.Count > 0)
                return sns[0];
            else
                return null;
        }
    }

    public class Contests : List<Contest>, IGetAll, ICacheName
    {
        #region IGetAll Members

        public void GetAll()
        {
            DataTable dt = null;

            if (HttpContext.Current == null || HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllContests";

                // execute the stored procedure
                dt = DbAct.ExecuteSelectCommand(comm);

                if (HttpContext.Current != null)
                {
                    HttpRuntime.Cache.AddObjToCache(dt, CacheName);
                }
            }
            else
            {
                dt = (DataTable) HttpRuntime.Cache[CacheName];
            }

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;
            
            foreach (Contest cont in from DataRow dr in dt.Rows select new Contest(dr))
            {
                Add(cont);
            }
        }

        #endregion

        public string CacheName
        {
            get { return string.Format("{0}-{1}", GetType().FullName, "-all"); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }
    }
}