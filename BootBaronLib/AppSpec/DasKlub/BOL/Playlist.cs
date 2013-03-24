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
using System.Data;
using System.Data.Common;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Playlist : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private int _playlistID = 0;

        public int PlaylistID
        {
            get { return _playlistID; }
            set { _playlistID = value; }
        }

        private DateTime _playlistBegin = DateTime.MinValue;

        public DateTime PlaylistBegin
        {
            get { return _playlistBegin; }
            set { _playlistBegin = value; }
        }

        private string _playListName = string.Empty;

        public string PlayListName
        {
            get { return _playListName; }
            set { _playListName = value; }
        }


        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }

        private bool _autoPlay = false;

        public bool AutoPlay
        {
            get { return _autoPlay; }
            set { _autoPlay = value; }
        }

        #endregion

        #region constructors

        public Playlist() { }

        public Playlist(int playlistID)
        {
            Get(playlistID);
        }

        #endregion

        #region methods

  

        public override bool Delete()
        {
            if (this.PlaylistID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeletePlaylist";

            ADOExtenstion.AddParameter(comm, "playlistID", this.PlaylistID);

            RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public void GetUserPlaylist(int userAccountID)
        {
            // this may need to be used for more than 1 playlist at some point

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUserPlaylist";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }


        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddCreatePlaylist";

            if (PlaylistBegin == DateTime.MinValue)
            {
                PlaylistBegin = new DateTime(1900, 1, 1);
            }

            ADOExtenstion.AddParameter(comm, "createdByUserID",  CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "playlistBegin",  PlaylistBegin);
            ADOExtenstion.AddParameter(comm, "playListName", PlayListName);
            ADOExtenstion.AddParameter(comm, "userAccountID",  UserAccountID);
            ADOExtenstion.AddParameter(comm, "autoPlay",  this.AutoPlay);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.PlaylistID = Convert.ToInt32(result);

            return this.PlaylistID;
        }

        public override bool Update()
        {

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdatePlaylist";

            if (PlaylistBegin == DateTime.MinValue)
            {
                PlaylistBegin = new DateTime(1900, 1, 1);
            }

            ADOExtenstion.AddParameter(comm, "updatedByUserID",  this.UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "playListName", this.PlayListName);
            ADOExtenstion.AddParameter(comm, "playlistID",  this.PlaylistID);
            ADOExtenstion.AddParameter(comm, "userAccountID",  this.UserAccountID);
            ADOExtenstion.AddParameter(comm, "autoPlay",  this.AutoPlay);
            ADOExtenstion.AddParameter(comm, "playlistBegin",  PlaylistBegin);

          
            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            return (result != -1);
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);
                this.PlaylistBegin = FromObj.DateFromObj(dr["playlistBegin"]);
                this.PlaylistID = FromObj.IntFromObj(dr["playlistID"]);
                this.PlayListName = FromObj.StringFromObj(dr["playListName"]);
                this.UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                this.AutoPlay = FromObj.BoolFromObj(dr["autoPlay"]);
            }
            catch
            {

            }
        }

        public override void Get(int playlistID)
        {
            this.PlaylistID = playlistID;

            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetPlaylist";
                // create a new parameter
                DbParameter param = comm.CreateParameter();
                param.ParameterName = "@playlistID";
                param.Value = playlistID;
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

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}-{1}", this.GetType().FullName , this.PlaylistID.ToString()); }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName); 
        }

        #endregion
    }
}
