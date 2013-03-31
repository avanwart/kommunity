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

using System.Data.Common;
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
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