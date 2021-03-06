﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.BLL;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.ArtistContent
{
    public class Artist : BaseIUserLogCrud, ICacheName, IUrlTo, IDisplayName
    {
        #region properties

        private string _altName = string.Empty;
        private string _name = string.Empty;
        public int ArtistID { get; set; }

        public string AltName
        {
            get { return _altName == null ? string.Empty : _altName.Trim(); }
            set { _altName = value; }
        }

        public bool IsHidden { get; set; }


        public string Name
        {
            get
            {
                if (_name == null) return string.Empty;

                return _name.Trim();
            }
            set { _name = value; }
        }


        public string URLOfArtist
        {
            get { return Name.Replace(" ", "-").ToLower(); }
        }


        public string FullURLOfArtist
        {
            get
            {
                return
                    string.Format("{0}/{1}",
                        Utilities.URLAuthority(),
                        AltName.ToLower());
            }
        }

        public string HyperLinkToArtist
        {
            get
            {
                return
                    string.Format(@"<a href=""{0}"">{1}</a>",
                        FullURLOfArtist,
                        Name);
            }
        }

        #endregion

        #region contructors

        public Artist()
        {
        }

        public Artist(string artistName)
        {
            Name = artistName.Trim();

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistByName";

            // create a new parameter
            comm.AddParameter("name", Name);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        public Artist(int artistID)
        {
            ArtistID = artistID;

            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetArtistByID";
                // create a new parameter
                comm.AddParameter("artistID", ArtistID);

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count != 1) return;

                HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                Get(dt.Rows[0]);
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public Artist(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        public string DisplayName
        {
            get { return !string.IsNullOrEmpty(AltName) ? AltName : Name; }
        }

        public Uri UrlTo
        {
            get { return new Uri(Utilities.URLAuthority() + "/" + AltName); }
        }

        public void GetArtistByAltname(string altName)
        {
            AltName = altName;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistByAltname";
            // create a new parameter
            comm.AddParameter("altName", altName);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                ArtistID = FromObj.IntFromObj(dr["artistID"]);
                Name = FromObj.StringFromObj(dr["name"]);
                AltName = FromObj.StringFromObj(dr["altName"]);
                IsHidden = FromObj.BoolFromObj(dr["isHidden"]);
            }
            catch
            {
            }
        }

        public override int Create()
        {
            if (string.IsNullOrEmpty(Name)) return 0;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddArtist";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("isHidden", IsHidden);
            comm.AddParameter("name", Name);
            comm.AddParameter("altName", AltName);

            // the result is their ID
            // execute the stored procedure
            string result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            ArtistID = Convert.ToInt32(result);

            return ArtistID;
        }

        public override bool Update()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateArtist";

            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("isHidden", IsHidden);
            comm.AddParameter("name", Name);
            comm.AddParameter("artistID", ArtistID);
            comm.AddParameter("altName", AltName);


            int result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            return (result != -1);
        }

        #region ICacheName Members

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}",
                    GetType().FullName,
                    ArtistID.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion
    }

    public class Artists : List<Artist>, IGetAll, ICacheName
    {
        public string CacheName
        {
            get { return string.Format("{0}all", GetType().FullName); }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        public static DataSet GetArtistCloudByLetter(string letter)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistCloudByLetter";

            comm.AddParameter("firstLetter", letter);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            var ds = new DataSet();

            ds.Tables.Add(dt);

            return ds;
        }

        public static DataSet GetArtistCloudByNonLetter()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistCloudByNonLetter";
            //
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            var ds = new DataSet();

            ds.Tables.Add(dt);

            return ds;
        }

        public int GetArtistsPageWise(int pageIndex, int pageSize)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistsPageWise";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds.Tables[0].Rows.Count <= 0) return recordCount;

            foreach (Artist art in from DataRow dr
                in ds.Tables[0].Rows
                select new Artist(dr))
            {
                Add(art);
            }

            return recordCount;
        }

        #region IGetAll Members

        public void GetAll()
        {
            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllArtists";
                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt == null || dt.Rows.Count <= 0) return;

                foreach (Artist art in from DataRow dr
                    in dt.Rows
                    select new Artist(dr))
                {
                    Add(art);
                }

                HttpRuntime.Cache.AddObjToCache(this, CacheName);
            }
            else
            {
                var arts = (Artists) HttpRuntime.Cache[CacheName];

                foreach (Artist uad in arts) Add(uad);
            }
        }

        #endregion
    }
}