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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using System;
using BootBaronLib.Operational;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class PlaylistVideo: BaseIUserLogCRUD
    {
        #region properties 

        private int _playlistVideoID = 0;

        public int PlaylistVideoID
        {
            get { return _playlistVideoID; }
            set { _playlistVideoID = value; }
        }

        private int _playlistID = 0;

        public int PlaylistID
        {
            get { return _playlistID; }
            set { _playlistID = value; }
        }

        private int _videoID = 0;

        public int VideoID
        {
            get { return _videoID; }
            set { _videoID = value; }
        }

        private int _rankOrder = 0;

        public PlaylistVideo(DataRow dr)
        {
            Get(dr);
        }

        public PlaylistVideo()
        {
            // TODO: Complete member initialization
        }

        public int RankOrder
        {
            get { return _rankOrder; }
            set { _rankOrder = value; }
        }

        #endregion

  

        public PlaylistVideo(int playlistVideoID) { Get(playlistVideoID); }

        #region methods


        public void Get(int playlistID, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPlaylistVideoByIDs";

            ADOExtenstion.AddParameter(comm, "playlistID",  playlistID);
            ADOExtenstion.AddParameter(comm, "videoID",  videoID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(int playlistVideoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPlaylistVideoByID";

            ADOExtenstion.AddParameter(comm, "playlistVideoID",  playlistVideoID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.PlaylistID = FromObj.IntFromObj(dr["playlistID"]);
                this.PlaylistVideoID = FromObj.IntFromObj(dr["playlistVideoID"]);
                this.RankOrder = FromObj.IntFromObj(dr["rankOrder"]);
                this.VideoID = FromObj.IntFromObj(dr["videoID"]);
            }
            catch
            {

            }
        }

        public static string CurrentVideoInPlaylist(int playlistID)
        {
            Playlist ply = new Playlist(playlistID);
            PlaylistVideos plvids = new PlaylistVideos();

            plvids.GetPlaylistVideosForPlaylist(ply.PlaylistID);

            int secondsElapsed = 0;


            if (plvids.Count == 0) return string.Empty;

            //Videos vids = new Videos();

            DateTime now = BootBaronLib.Operational.Utilities.GetDataBaseTime();
            TimeSpan elapsedTime = now - ply.PlaylistBegin;
            int totalSecondsElapsed = Convert.ToInt32(elapsedTime.TotalSeconds);

            Video v1 = null;
            SongRecord sngr = null;
            int cliplength = 0;

            foreach (PlaylistVideo plv in plvids)
            {
                v1 = new Video(plv.VideoID);

                if (!v1.IsEnabled) continue;

                //secondsElapsed += v1.ClipLength;
                //vids.Add(v1);
                cliplength = v1.ClipLength;

                if (totalSecondsElapsed <= v1.ClipLength)
                {
                    v1.Intro += totalSecondsElapsed;// +(secondsElapsed - totalSecondsElapsed);
                    sngr = new SongRecord(v1);
                    sngr.GetRelatedVideos = false;
                    return sngr.JSONResponse;
                }
                else if ( totalSecondsElapsed <= (secondsElapsed + v1.ClipLength) )
                {
                    v1.Intro +=  (totalSecondsElapsed - secondsElapsed);
                    sngr = new SongRecord(v1);
                    sngr.GetRelatedVideos = false;
                    return sngr.JSONResponse;
                }

                secondsElapsed += cliplength;

            }


            // just do it over
            ply.PlaylistBegin = BootBaronLib.Operational.Utilities.GetDataBaseTime();
            ply.Update();
            return CurrentVideoInPlaylist(1);

            //return string.Empty;
        }


        public static string GetFirstVideo(int playlistID)
        {
            Playlist ply = new Playlist(playlistID);
            PlaylistVideos plvids = new PlaylistVideos();

            plvids.GetPlaylistVideosForPlaylist(ply.PlaylistID);

            Video v1 = null;
            SongRecord sngr = null;

            foreach (PlaylistVideo plv in plvids)
            {
                v1 = new Video(plv.VideoID);

                if (!v1.IsEnabled) continue;
                else
                {
                    v1 = new Video(plvids[0].VideoID);
                    sngr = new SongRecord(v1);
                    sngr.GetRelatedVideos = false;
                    return sngr.JSONResponse;
                }

            }

            return string.Empty;
        }



        public static string GetNextVideo(int playlistID, string currentVideo)
        {
            Playlist ply = new Playlist(playlistID);
            PlaylistVideos plvids = new PlaylistVideos();

            plvids.GetPlaylistVideosForPlaylist(ply.PlaylistID);
 
            Video v1 = null;
            SongRecord sngr = null;
            int indx = 0;

            foreach (PlaylistVideo plv in plvids)
            {
                v1 = new Video(plv.VideoID);

                if (!v1.IsEnabled) continue;

                indx++;

                if (currentVideo == v1.ProviderKey)
                {
                    if (plvids.Count > (indx))
                    {
                        v1 = new Video(plvids[indx].VideoID);
                        sngr = new SongRecord(v1);
                        sngr.GetRelatedVideos = false;
                        return sngr.JSONResponse;
                    }
                    else  
                    {
                        ply.PlaylistBegin = BootBaronLib.Operational.Utilities.GetDataBaseTime();
                        ply.Update();
                        v1 = new Video(plvids[0].VideoID);
                        sngr = new SongRecord(v1);
                        sngr.GetRelatedVideos = false;
                        return sngr.JSONResponse;
                    }
                }
 
                
            }

            return string.Empty;
        }

        public static bool IsPlaylistVideo(int playlistID, int videoID)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsPlaylistVideo";

            ADOExtenstion.AddParameter(comm, "playlistID",   playlistID);
            ADOExtenstion.AddParameter(comm, "videoID", videoID);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }

        public override bool Update()
        {

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdatePlaylistVideo";

            ADOExtenstion.AddParameter(comm, "updatedByUserID",   UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "playlistID",  PlaylistID);
            ADOExtenstion.AddParameter(comm, "videoID",   this.VideoID);
            ADOExtenstion.AddParameter(comm, "rankOrder",  RankOrder);
            ADOExtenstion.AddParameter(comm, "playlistVideoID",  PlaylistVideoID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);


            return (result != -1);
        }

        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeletePlaylistVideoByID";

            ADOExtenstion.AddParameter(comm, "playlistVideoID",  PlaylistVideoID);

            //RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;

        }

        public static bool Delete(int playlistID, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeletePlaylistVideo";

            ADOExtenstion.AddParameter(comm, "playlistID",  playlistID);
            ADOExtenstion.AddParameter(comm, "videoID",   videoID);

            //RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddPlaylistVideo";

            ADOExtenstion.AddParameter(comm, "createdByUserID",   CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "videoID",  VideoID);
            ADOExtenstion.AddParameter(comm, "playlistID",  PlaylistID);
            ADOExtenstion.AddParameter(comm, "rankOrder",  RankOrder);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.PlaylistVideoID = Convert.ToInt32(result);

            return this.PlaylistVideoID;
        }

        #endregion


    }

    public class PlaylistVideos : List<PlaylistVideo>
    {
        #region constructors 

        public PlaylistVideos() { }

        #endregion


        public static int GetCountOfVideosInPlaylist(int playlistID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCountOfVideosInPlaylist";

            ADOExtenstion.AddParameter(comm, "playlistID", playlistID);

            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str)) return 0;
            else return Convert.ToInt32(str);
        }

        public void GetPlaylistVideosForPlaylist(int playlistID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPlaylistVideosForPlaylist";

            ADOExtenstion.AddParameter(comm, "playlistID",  playlistID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                PlaylistVideo plv = null;
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    plv = new PlaylistVideo(dr);
                    vid = new Video(plv.VideoID);

                    if (vid.IsEnabled) this.Add(plv);
                }
            }


            // sort by order rank
            this.Sort(delegate(PlaylistVideo p1, PlaylistVideo p2)
            {
                return p1.RankOrder.CompareTo(p2.RankOrder);
            });
        }

        /// <summary>
        /// same thing as GetPlaylistVideosForPlaylist but adds to list regardless
        /// </summary>
        /// <param name="playlistID"></param>
        public void GetPlaylistVideosForPlaylistAll(int playlistID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPlaylistVideosForPlaylist";

            ADOExtenstion.AddParameter(comm, "playlistID", playlistID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                PlaylistVideo plv = null;
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    plv = new PlaylistVideo(dr);
                    vid = new Video(plv.VideoID);

                     this.Add(plv);
                }
            }


            // sort by order rank
            this.Sort(delegate(PlaylistVideo p1, PlaylistVideo p2)
            {
                return p1.RankOrder.CompareTo(p2.RankOrder);
            });
        }

    }
}
