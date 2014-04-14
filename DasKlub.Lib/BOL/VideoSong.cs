using System.Data.Common;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class VideoSong
    {
        public int VideoID { get; set; }

        public int SongID { get; set; }


        public int RankOrder { get; set; }


        public static bool AddVideoSong(int songID, int videoID, int rankOrder)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVideoSong";

            comm.AddParameter("videoID", videoID);
            comm.AddParameter("songID", songID);
            comm.AddParameter("rankOrder", rankOrder);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            return true; // this isn't really true
        }

        public static bool DeleteSongsForVideo(int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteSongsForVideo";

            comm.AddParameter("videoID", videoID);

            // execute the stored procedure
            int result = DbAct.ExecuteNonQuery(comm);

            return result > 0;
        }
    }
}