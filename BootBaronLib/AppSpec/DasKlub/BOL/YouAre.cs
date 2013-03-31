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
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{

    public class YouAre : BaseIUserLogCRUD, ICacheName, ILocalizedName
    {

        #region properties

        private int _youAreID = 0;

        public int YouAreID
        {
            get { return _youAreID; }
            set { _youAreID = value; }
        }




        private char _typeLetter = char.MinValue;

        public char TypeLetter
        {
            get { return _typeLetter; }
            set { _typeLetter = value; }
        }

        private string _name = string.Empty;

        public string Name
        {
            get
            {

                if (string.IsNullOrWhiteSpace(_name)) return _name;
                return _name.Trim();
            }
            set { _name = value; }
        }

        #endregion

        #region constructors

        public YouAre() { }

        public YouAre(int youAreID) { Get(youAreID); }

        public YouAre(DataRow dr)
        {
            Get(dr);
        }
        #endregion

        public override void Get(int uniqueID)
        {
            this.YouAreID = uniqueID;

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetYouAre";
                // create a new parameter
                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.YouAreID), YouAreID);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current != null) HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);

                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            this.YouAreID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.YouAreID)]);
            this.Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Name)]);
            this.TypeLetter = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.TypeLetter)]);
        }

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}", this.GetType().FullName, this.YouAreID.ToString());
            }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        public string LocalizedName
        {
            get
            {
                return Utilities.ResourceValue(this.Name);
            }
        }
    }

    public class YouAres : List<YouAre>, IGetAll
    {


        public void GetAll()
        {
            if (HttpContext.Current == null || HttpContext.Current.Cache[this.GetType().FullName] == null)
            {
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllYouAre";

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    YouAre art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new YouAre(dr);
                        this.Add(art);
                    }

                    HttpContext.Current.Cache.AddObjToCache(dt, this.GetType().FullName);
                }
            }
            else
            {
                DataTable dt = (DataTable)HttpContext.Current.Cache[this.GetType().FullName];

                if (dt != null && dt.Rows.Count > 0)
                {
                    YouAre art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new YouAre(dr);
                        this.Add(art);
                    }
                }
            }
        }



    }
}
