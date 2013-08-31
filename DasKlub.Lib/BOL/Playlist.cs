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
using DasKlub.Lib.BLL;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class Playlist : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private string _playListName = string.Empty;
        private DateTime _playlistBegin = DateTime.MinValue;
        public int PlaylistID { get; set; }

        public DateTime PlaylistBegin
        {
            get { return _playlistBegin; }
            set { _playlistBegin = value; }
        }

        public string PlayListName
        {
            get { return _playListName; }
            set { _playListName = value; }
        }


        public int UserAccountID { get; set; }

        public bool AutoPlay { get; set; }

        #endregion

        #region constructors

        public Playlist()
        {
        }

        public Playlist(int playlistID)
        {
            Get(playlistID);
        }

        #endregion

        #region methods

        public override bool Delete()
        {
            if (PlaylistID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeletePlaylist";

            comm.AddParameter("playlistID", PlaylistID);

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

            comm.AddParameter("userAccountID", userAccountID);

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

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("playlistBegin", PlaylistBegin);
            comm.AddParameter("playListName", PlayListName);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("autoPlay", AutoPlay);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            PlaylistID = Convert.ToInt32(result);

            return PlaylistID;
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

            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("playListName", PlayListName);
            comm.AddParameter("playlistID", PlaylistID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("autoPlay", AutoPlay);
            comm.AddParameter("playlistBegin", PlaylistBegin);


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
                PlaylistBegin = FromObj.DateFromObj(dr["playlistBegin"]);
                PlaylistID = FromObj.IntFromObj(dr["playlistID"]);
                PlayListName = FromObj.StringFromObj(dr["playListName"]);
                UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                AutoPlay = FromObj.BoolFromObj(dr["autoPlay"]);
            }
            catch
            {
            }
        }

        public override void Get(int playlistID)
        {
            PlaylistID = playlistID;

            if (HttpRuntime.Cache[CacheName] == null)
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
                    HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return string.Format("{0}-{1}", GetType().FullName, PlaylistID.ToString()); }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);
        }

        #endregion
    }
}