using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.ArtistContent
{
    public class SongProperty : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private string _propertyContent = string.Empty;
        private string _propertyType = string.Empty;
        public int SongPropertyID { get; set; }

        public int SongID { get; set; }


        public string PropertyContent
        {
            get { return _propertyContent; }
            set { _propertyContent = value; }
        }

        public string PropertyType
        {
            get { return _propertyType; }
            set { _propertyType = value; }
        }

        #endregion

        #region methods

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddSongProperty";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("songID", SongID);
            comm.AddParameter("propertyContent", PropertyContent);
            comm.AddParameter("propertyType", PropertyType);

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
                SongPropertyID = Convert.ToInt32(result);

                return SongPropertyID;
            }
        }


        public void GetSongPropertySongIDTypeID(int songID, string propertyType)
        {
            SongID = songID;
            PropertyType = propertyType;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored up_GetSongPropertySongIDTypeID name
            comm.CommandText = "up_GetSongPropertySongIDTypeID";

            comm.AddParameter("propertyType", PropertyType);
            comm.AddParameter("songID", SongID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]); // should only be 1
            }
        }


        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                SongPropertyID = FromObj.IntFromObj(dr["songPropertyID"]);
                SongID = FromObj.IntFromObj(dr["songID"]);
                PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);
                PropertyType = FromObj.StringFromObj(dr["propertyType"]);
            }
            catch
            {
            }
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        #endregion

        public enum SPropType
        {
            UN,
            AM,
            IT,
        }

        public static string GetPropertyTypeLink(int vidID)
        {
            var sngs = new Songs();

            sngs.GetSongsForVideo(vidID);

            var sb = new StringBuilder();

            var sp = new SongProperty();

            foreach (Song sng in sngs)
            {
                sp = new SongProperty();

                sp.GetSongPropertySongIDTypeID(sng.SongID, SPropType.IT.ToString());

                if (!string.IsNullOrEmpty(sp.PropertyContent))
                {
                    sb.Append(@"<a target=""_blank"" class=""info"" href=""" + sp.PropertyContent + @""">");
                    sb.Append(sng.Name);
                    sb.Append(@"</a>");
                }
            }

            return HttpUtility.HtmlEncode(sb.ToString().Trim());
        }
    }
}