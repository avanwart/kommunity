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
using BootBaronLib.Configs;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;



namespace BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class Artist : BaseIUserLogCRUD, ICacheName, IURLTo, IDisplayName
    {
        #region properties

        private int _artistID = 0;

        public int ArtistID
        {
            get { return _artistID; }
            set { _artistID = value; }
        }


        private string _altName = string.Empty;

        public string AltName
        {
            get {
                if (_altName == null) return string.Empty;
                
                return _altName.Trim(); }
            set { _altName = value; }
        }

        private bool _isHidden = false;

        public bool IsHidden
        {
            get { return _isHidden; }
            set { _isHidden = value; }
        }
        private string _name = string.Empty;


        public string Name
        {
            get {
                if (_name == null) return string.Empty;
                
                return _name.Trim(); }
            set { _name = value; }
        }


        public string URLOfArtist
        {
            get { return this.Name.Replace(" ", "-").ToLower(); }
        }


        public string FullURLOfArtist
        {
            get { return  Utilities.URLAuthority() + "/" + this.AltName.ToLower(); }
        }

        public string HyperLinkToArtist
        {
            get
            {

                return @"<a href=""" + FullURLOfArtist + @""">" + this.Name + @"</a>";
                //if (string.IsNullOrEmpty(this.AltName))
                //{
                //    return @"<a href=""" + FullURLOfArtist + @""">" + this.Name + @"</a>";
                //}
                //else
                //{
                //    return @"<a href=""" + FullURLOfArtist + @""">" + this.AltName + @"</a>";
                //}
            }
        }


        #endregion

      
        #region contructors

        public Artist() { }

        public Artist(string artistName)
        {
            this.Name = artistName.Trim();

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistByName";
            
            // create a new parameter
            ADOExtenstion.AddParameter(comm, "name", Name);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        public Artist(int artistID)
        {
            this.ArtistID = artistID;

            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetArtistByID";
                // create a new parameter

                ADOExtenstion.AddParameter(comm, "artistID", ArtistID);

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

        public Artist(DataRow dr)
        {
            Get(dr);
        }


        #endregion


        public   void GetArtistByAltname(string altName)
        {
            this.AltName = altName;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistByAltname";

            // create a new parameter
            ADOExtenstion.AddParameter(comm, "altName", altName);

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

                this.ArtistID = FromObj.IntFromObj(dr["artistID"]);
                this.Name = FromObj.StringFromObj(dr["name"]);
                this.AltName = FromObj.StringFromObj(dr["altName"]);
                this.IsHidden = FromObj.BoolFromObj(dr["isHidden"]);
                
            }
            catch
            {
                //Utilities.LogError(ex);
            }
        }

        public override int Create()
        {
            if (string.IsNullOrEmpty(this.Name)) return 0;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddArtist";

            ADOExtenstion.AddParameter(comm, "createdByUserID",  CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "isHidden",  IsHidden);
            ADOExtenstion.AddParameter(comm, "name",  Name);
            ADOExtenstion.AddParameter(comm, "altName", AltName);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            else
            {
                this.ArtistID = Convert.ToInt32(result);

                return this.ArtistID;
            }



        }

        public override bool Update()
        {

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateArtist";

            ADOExtenstion.AddParameter(comm, "updatedByUserID",  UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "isHidden",  IsHidden);
            ADOExtenstion.AddParameter(comm, "name",   Name);
            ADOExtenstion.AddParameter(comm, "artistID",  ArtistID);
            ADOExtenstion.AddParameter(comm, "altName", AltName);


            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            return (result != -1);
        }

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}-{1}", this.GetType().FullName, this.ArtistID.ToString()); }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }

        #endregion

        public Uri UrlTo
        {
            get { return new  Uri( Utilities.URLAuthority() + "/" +  this.AltName ); }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AltName)) return this.AltName;
                else return this.Name;
            }
        }
    }

    public class Artists : List<Artist>, IGetAll, ICacheName
    {
        public static DataSet GetArtistCloudByLetter(string letter)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistCloudByLetter";
            
            ADOExtenstion.AddParameter(comm, "firstLetter",  letter);
            
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            return ds;
        }

        public static DataSet  GetArtistCloudByNonLetter( )
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistCloudByNonLetter";
            //
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            return ds;
        }

        #region IGetAll Members

        public void GetAll()
        {
            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetAllArtists";

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    Artist art = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        art = new Artist(dr);
                        this.Add(art);
                    }

                    HttpContext.Current.Cache.AddObjToCache(this, this.CacheName);
                }
            }
            else
            {
                Artists arts = (Artists)HttpContext.Current.Cache[this.CacheName];

                foreach (Artist uad in arts) this.Add(uad);
            }
        }

        #endregion


        public string CacheName
        {
            get { return string.Format("{0}all", this.GetType().FullName); }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
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

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Artist art = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    art = new Artist(dr);
                    this.Add(art);
                }
            }

            return recordCount;
        }
    }

}
