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
    public class RelationshipStatus : BaseIUserLogCRUD, ICacheName, ILocalizedName
    {
        #region properties

        private string _name = string.Empty;
        private char _typeLetter = char.MinValue;
        public int RelationshipStatusID { get; set; }

        public char TypeLetter
        {
            get { return _typeLetter; }
            set { _typeLetter = value; }
        }


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

        public RelationshipStatus()
        {
        }

        public RelationshipStatus(int relationshipStatusID)
        {
            Get(relationshipStatusID);
        }

        public RelationshipStatus(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        public string CacheName
        {
            get
            {
                return GetType().FullName +
                       "-" + RelationshipStatusID.ToString();
            }
        }


        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        public string LocalizedName
        {
            get { return Utilities.ResourceValue(Name); }
        }

        public override void Get(int uniqueID)
        {
            RelationshipStatusID = uniqueID;

            if (HttpContext.Current == null || HttpContext.Current.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetRelationshipStatus";
                // create a new parameter
                comm.AddParameter(StaticReflection.GetMemberName<string>(x => RelationshipStatusID),
                                  RelationshipStatusID);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current != null) HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], CacheName);

                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpContext.Current.Cache[CacheName]);
            }
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            RelationshipStatusID =
                FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => RelationshipStatusID)]);
            Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Name)]);
            TypeLetter = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => TypeLetter)]);
        }
    }

    public class RelationshipStatuses : List<RelationshipStatus>, IGetAll
    {
        public void GetAll()
        {
            if (HttpContext.Current == null || HttpContext.Current.Cache[GetType().FullName] == null)
            {
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllRelationshipStatus";

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    RelationshipStatus art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new RelationshipStatus(dr);
                        Add(art);
                    }

                    HttpContext.Current.Cache.AddObjToCache(dt, GetType().FullName);
                }
            }
            else
            {
                var dt = (DataTable) HttpContext.Current.Cache[GetType().FullName];

                if (dt != null && dt.Rows.Count > 0)
                {
                    RelationshipStatus art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new RelationshipStatus(dr);
                        Add(art);
                    }
                }
            }
        }
    }
}