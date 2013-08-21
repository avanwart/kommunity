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
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class Song : BaseIUserLogCRUD
    {
        public Song(int artistID, string songName)
        {
            ArtistID = artistID;
            Name = songName;


            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetSongByArtistIDName";

            comm.AddParameter("artistID", artistID);
            comm.AddParameter("name", songName);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        public Song(DataRow dr)
        {
            Get(dr);
        }

        #region properties

        private string _name = string.Empty;
        private string _songKey = string.Empty;
        public int SongID { get; set; }

        public int ArtistID { get; set; }

        public bool IsHidden { get; set; }


        public string Name
        {
            get
            {
                if (_name != null) _name = _name.Trim();
                return _name;
            }
            set { _name = value; }
        }

        public string SongKey
        {
            get { return _songKey; }
            set { _songKey = value; }
        }

        #endregion

        /* justric in the hands of chaos */

        public int RankOrder { get; set; }

        #region methods

        public override sealed void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                SongID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => SongID)]);
                ArtistID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ArtistID)]);
                Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Name)]);
                SongKey = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => SongKey)]);
            }
            catch
            {
            }
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddSong";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ArtistID), ArtistID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsHidden), IsHidden);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Name), Name);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => SongKey), SongKey);


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
                SongID = Convert.ToInt32(result);

                return SongID;
            }
        }

        public override bool Update()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateSong";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UpdatedByUserID), UpdatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ArtistID), ArtistID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsHidden), IsHidden);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Name), Name);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => SongID), SongID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => SongKey), SongKey);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            return (result != -1);

            #endregion
        }
    }

    public class Songs : List<Song>
    {
        public void GetSongsForArtist(int artistID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetSongsForArtist";

            comm.AddParameter("artistID", artistID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var sng in from DataRow dr in dt.Rows select new Song(dr))
            {
                Add(sng);
            }
        }


        public void GetSongsForVideo(int videoID)
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetSongsForVideo";

            comm.AddParameter("videoID", videoID);

            // execute the stored procedure
            var dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;
            foreach (var sng in from DataRow dr in dt.Rows select new Song(dr) {RankOrder = FromObj.IntFromObj(dr["rankOrder"])})
            {
                Add(sng);
            }
        }
    }
}