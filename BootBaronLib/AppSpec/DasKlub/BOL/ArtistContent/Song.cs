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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class Song : BaseIUserLogCRUD
    {
        public Song(int artistID, string songName)
        {
            this.ArtistID = artistID;
            this.Name = songName;


            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetSongByArtistIDName";

            ADOExtenstion.AddParameter(comm, "artistID",   artistID);
            ADOExtenstion.AddParameter(comm, "name",  songName);

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

        private int _songID = 0;

        public int SongID
        {
            get { return _songID; }
            set { _songID = value; }
        }

        private int _artistID = 0;

        public int ArtistID
        {
            get { return _artistID; }
            set { _artistID = value; }
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

                if (_name != null) _name = _name.Trim();
                return _name; }
            set { _name = value; }
        }

        private string _songKey = string.Empty;

        public string SongKey
        {
            get { return _songKey; }
            set { _songKey = value; }
        }

        #endregion

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.SongID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.SongID)]);
                this.ArtistID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ArtistID)]);
                this.Name = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Name)]);
                this.SongKey = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.SongKey)]);

            
            }
            catch { }
        }

        /* justric in the hands of chaos */

        private int _rankOrder = 0;

        public int RankOrder
        {
            get { return _rankOrder; }
            set { _rankOrder = value; }
        }



        #region methods

        public override int Create()
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddSong";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ArtistID), ArtistID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IsHidden), IsHidden);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Name), Name);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.SongKey), SongKey);

 
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
                this.SongID = Convert.ToInt32(result);

                return this.SongID;
            }
        }

        public override bool Update()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateSong";
 
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ArtistID), ArtistID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IsHidden), IsHidden);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Name), Name);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.SongID), SongID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.SongKey), SongKey);

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

            ADOExtenstion.AddParameter(comm, "artistID",  artistID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Song sng = null;

                foreach (DataRow dr in dt.Rows)
                {
                    sng = new Song(dr);
                    this.Add(sng);
                }
            }

        }


        public void GetSongsForVideo(int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetSongsForVideo";

            ADOExtenstion.AddParameter(comm, "videoID",  videoID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Song sng = null;

                foreach (DataRow dr in dt.Rows)
                {
                    sng = new Song(dr);
                    sng.RankOrder = FromObj.IntFromObj(dr["rankOrder"]);

                    this.Add(sng);
                }
            }
        }


    }
}