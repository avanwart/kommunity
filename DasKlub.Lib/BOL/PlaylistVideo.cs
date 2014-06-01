using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.BOL.ArtistContent;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class PlaylistVideo : BaseIUserLogCrud
    {
        #region properties 

        public PlaylistVideo(DataRow dr)
        {
            Get(dr);
        }

        public PlaylistVideo()
        {
        }

        public int PlaylistVideoID { get; set; }

        public int PlaylistID { get; set; }

        public int VideoID { get; set; }

        public int RankOrder { get; set; }

        #endregion

        public PlaylistVideo(int playlistVideoID)
        {
            Get(playlistVideoID);
        }

        #region methods

        public void Get(int playlistID, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPlaylistVideoByIDs";

            comm.AddParameter("playlistID", playlistID);
            comm.AddParameter("videoID", videoID);

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

            comm.AddParameter("playlistVideoID", playlistVideoID);

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

                PlaylistID = FromObj.IntFromObj(dr["playlistID"]);
                PlaylistVideoID = FromObj.IntFromObj(dr["playlistVideoID"]);
                RankOrder = FromObj.IntFromObj(dr["rankOrder"]);
                VideoID = FromObj.IntFromObj(dr["videoID"]);
            }
            catch
            {
            }
        }

        public static string CurrentVideoInPlaylist(int playlistID)
        {
            var ply = new Playlist(playlistID);
            var plvids = new PlaylistVideos();

            plvids.GetPlaylistVideosForPlaylist(ply.PlaylistID);

            int secondsElapsed = 0;

            if (plvids.Count == 0) return string.Empty;

            DateTime now = DateTime.UtcNow;
            TimeSpan elapsedTime = now - ply.PlaylistBegin;
            int totalSecondsElapsed = Convert.ToInt32(elapsedTime.TotalSeconds);

            Video v1 = null;
            SongRecord sngr = null;
            int cliplength = 0;

            foreach (PlaylistVideo plv in plvids)
            {
                v1 = new Video(plv.VideoID);

                if (!v1.IsEnabled) continue;

                cliplength = v1.ClipLength;

                if (totalSecondsElapsed <= v1.ClipLength)
                {
                    v1.Intro += totalSecondsElapsed;
                    sngr = new SongRecord(v1);
                    sngr.GetRelatedVideos = false;
                    return sngr.JSONResponse;
                }
                if (totalSecondsElapsed <= (secondsElapsed + v1.ClipLength))
                {
                    v1.Intro += (totalSecondsElapsed - secondsElapsed);
                    sngr = new SongRecord(v1);
                    sngr.GetRelatedVideos = false;
                    return sngr.JSONResponse;
                }

                secondsElapsed += cliplength;
            }


            // just do it over
            ply.PlaylistBegin = DateTime.UtcNow;
            ply.Update();
            return CurrentVideoInPlaylist(1);
        }


        public static string GetFirstVideo(int playlistID)
        {
            var ply = new Playlist(playlistID);
            var plvids = new PlaylistVideos();

            plvids.GetPlaylistVideosForPlaylist(ply.PlaylistID);

            Video v1 = null;
            SongRecord sngr = null;

            foreach (PlaylistVideo plv in plvids)
            {
                v1 = new Video(plv.VideoID);

                if (!v1.IsEnabled) continue;
                v1 = new Video(plvids[0].VideoID);
                sngr = new SongRecord(v1);
                sngr.GetRelatedVideos = false;
                return sngr.JSONResponse;
            }

            return string.Empty;
        }


        public static string GetNextVideo(int playlistID, string currentVideo)
        {
            var ply = new Playlist(playlistID);
            var plvids = new PlaylistVideos();

            plvids.GetPlaylistVideosForPlaylist(ply.PlaylistID);

            int indx = 0;

            foreach (PlaylistVideo plv in plvids)
            {
                var v1 = new Video(plv.VideoID);

                if (!v1.IsEnabled) continue;

                indx++;

                if (currentVideo != v1.ProviderKey) continue;

                SongRecord sngr;
                if (plvids.Count > (indx))
                {
                    v1 = new Video(plvids[indx].VideoID);
                    sngr = new SongRecord(v1) {GetRelatedVideos = false};
                    return sngr.JSONResponse;
                }
                ply.PlaylistBegin = DateTime.UtcNow;
                ply.Update();
                v1 = new Video(plvids[0].VideoID);
                sngr = new SongRecord(v1) {GetRelatedVideos = false};
                return sngr.JSONResponse;
            }

            return string.Empty;
        }

        public static bool IsPlaylistVideo(int playlistID, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsPlaylistVideo";

            comm.AddParameter("playlistID", playlistID);
            comm.AddParameter("videoID", videoID);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }

        public override bool Update()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdatePlaylistVideo";

            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("playlistID", PlaylistID);
            comm.AddParameter("videoID", VideoID);
            comm.AddParameter("rankOrder", RankOrder);
            comm.AddParameter("playlistVideoID", PlaylistVideoID);

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

            comm.AddParameter("playlistVideoID", PlaylistVideoID);

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

            comm.AddParameter("playlistID", playlistID);
            comm.AddParameter("videoID", videoID);

            //RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddPlaylistVideo";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("videoID", VideoID);
            comm.AddParameter("playlistID", PlaylistID);
            comm.AddParameter("rankOrder", RankOrder);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            PlaylistVideoID = Convert.ToInt32(result);

            return PlaylistVideoID;
        }

        #endregion
    }

    public class PlaylistVideos : List<PlaylistVideo>
    {
        #region constructors 

        #endregion

        public static int GetCountOfVideosInPlaylist(int playlistID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCountOfVideosInPlaylist";

            comm.AddParameter("playlistID", playlistID);

            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str)) return 0;
            return Convert.ToInt32(str);
        }

        public void GetPlaylistVideosForPlaylist(int playlistID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPlaylistVideosForPlaylist";

            comm.AddParameter("playlistID", playlistID);

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

                    if (vid.IsEnabled) Add(plv);
                }
            }


            // sort by order rank
            Sort(delegate(PlaylistVideo p1, PlaylistVideo p2) { return p1.RankOrder.CompareTo(p2.RankOrder); });
        }

        /// <summary>
        ///     same thing as GetPlaylistVideosForPlaylist but adds to list regardless
        /// </summary>
        /// <param name="playlistID"></param>
        public void GetPlaylistVideosForPlaylistAll(int playlistID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPlaylistVideosForPlaylist";

            comm.AddParameter("playlistID", playlistID);

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

                    Add(plv);
                }
            }


            // sort by order rank
            Sort(delegate(PlaylistVideo p1, PlaylistVideo p2) { return p1.RankOrder.CompareTo(p2.RankOrder); });
        }
    }
}