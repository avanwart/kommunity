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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL.VideoContest
{
    public class Contest : BaseIUserLogCRUD
    {
        #region properties

        private int _contestID = 0;

        public int ContestID
        {
            get { return _contestID; }
            set { _contestID = value; }
        }

        private string _contestKey = string.Empty;

        public string ContestKey
        {
            get { return _contestKey; }
            set { _contestKey = value; }
        }

        private DateTime _beginDate = DateTime.MinValue;

        public DateTime BeginDate
        {
            get { return _beginDate; }
            set { _beginDate = value; }
        }

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DateTime _deadLine = DateTime.MinValue;

        public DateTime DeadLine
        {
            get { return _deadLine; }
            set { _deadLine = value; }
        }

        private string _description = string.Empty;

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
            this.Name = name;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestByName";
            // create a new parameter
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Name), Name);

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
            this.ContestKey = contestKey;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestByKey";

            // create a new parameter
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContestKey), ContestKey);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);


                this.Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Name)]);
                this.ContestID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContestID)]);
                this.DeadLine = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => this.DeadLine)]);
                this.BeginDate = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => this.BeginDate)]);
                this.Description = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Description)]);
                this.ContestKey = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContestKey)]);
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

                return (this.DeadLine > dt && this.BeginDate < dt);
            }
        }

        public static Contest GetCurrentContest()
        {
            Contests sns = new Contests();
            sns.GetAll();

            Contest cndss = new Contest();

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
            Contests sns = new Contests();
            sns.GetAll();

            Contest cndss = new Contest();

            sns.Sort(delegate(Contest p1, Contest p2)
            {
                return p2.DeadLine.CompareTo(p1.DeadLine);
            });

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

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllContests";

                // execute the stored procedure
                dt = DbAct.ExecuteSelectCommand(comm);

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.AddObjToCache(dt, this.CacheName);
                }
            }
            else
            {
                dt = (DataTable)HttpContext.Current.Cache[this.CacheName];
            }

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Contest cont = null;
                foreach (DataRow dr in dt.Rows)
                {
                    cont = new Contest(dr);
                    this.Add(cont);
                }
            }
        }

        #endregion

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}", this.GetType().FullName, "-all");
            }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }
    }
}
