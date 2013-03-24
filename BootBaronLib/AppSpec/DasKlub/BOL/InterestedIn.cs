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
using System.Linq;
using System.Text;
using BootBaronLib.Interfaces;
using BootBaronLib.BaseTypes;
using System.Data.Common;
using BootBaronLib.DAL;
using BootBaronLib.AppSpec.DasKlub.BLL;
using System.Data;
using System.Web;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class InterestedIn : BaseIUserLogCRUD, ICacheName, ILocalizedName
    {

        #region properties

        private int _interestedInID = 0;

        public int InterestedInID
        {
            get { return _interestedInID; }
            set { _interestedInID = value; }
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

        public InterestedIn() { }

        public InterestedIn(int interestedInID) {
            Get(interestedInID); 
        }

        public InterestedIn(DataRow dr)
        {
            Get(dr);
        }
        #endregion

        public override void Get(int uniqueID)
        {
            this.InterestedInID = uniqueID;

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetInterestedIn";
                // create a new parameter
                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.InterestedInID), InterestedInID);

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

            this.InterestedInID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.InterestedInID)]);
            this.Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Name)]);
            this.TypeLetter = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.TypeLetter)]);
        }

        public string CacheName
        {
            get
            {
                return this.GetType().FullName +
                    "-" + this.InterestedInID.ToString();
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

    public class InterestedIns : List<InterestedIn>, IGetAll
    {

        public void GetAll()
        {
            if (HttpContext.Current == null || HttpContext.Current.Cache[this.GetType().FullName] == null)
            {
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllInterestedIn";

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    InterestedIn art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new InterestedIn(dr);
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
                    InterestedIn art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new InterestedIn(dr);
                        this.Add(art);
                    }
                }
            }
        }
    }
}
