using System;
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
    public class YouAre : BaseIUserLogCRUD, ICacheName, ILocalizedName
    {
        #region properties

        private string _name = string.Empty;
        private char _typeLetter = char.MinValue;
        public int YouAreID { get; set; }

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

        public YouAre()
        {
        }

        public YouAre(int youAreID)
        {
            Get(youAreID);
        }

        public YouAre(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        public string CacheName
        {
            get { return string.Format("{0}-{1}", GetType().FullName, YouAreID.ToString()); }
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
            YouAreID = uniqueID;

            if (HttpContext.Current == null || HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetYouAre";
                // create a new parameter
                comm.AddParameter(StaticReflection.GetMemberName<string>(x => YouAreID), YouAreID);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current != null) HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);

                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            YouAreID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => YouAreID)]);
            Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Name)]);
            TypeLetter = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => TypeLetter)]);
        }
    }

    public class YouAres : List<YouAre>, IGetAll
    {
        public void GetAll()
        {
            if (HttpContext.Current == null || HttpRuntime.Cache[GetType().FullName] == null)
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
                        Add(art);
                    }

                    HttpRuntime.Cache.AddObjToCache(dt, GetType().FullName);
                }
            }
            else
            {
                var dt = (DataTable) HttpRuntime.Cache[GetType().FullName];

                if (dt != null && dt.Rows.Count > 0)
                {
                    YouAre art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new YouAre(dr);
                        Add(art);
                    }
                }
            }
        }
    }
}