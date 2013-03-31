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
using System.Linq;
using System.Text;
using BootBaronLib.Interfaces;
using BootBaronLib.BaseTypes;
using System.Data.Common;
using BootBaronLib.DAL;
using System.Data;
using System.Web;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class SongProperty : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private int _songPropertyID = 0;

        public int SongPropertyID
        {
            get { return _songPropertyID; }
            set { _songPropertyID = value; }
        }

        private int _songID = 0;

        public int SongID
        {
            get { return _songID; }
            set { _songID = value; }
        }
 

        private string _propertyContent = string.Empty;

        public string PropertyContent
        {
            get { return _propertyContent; }
            set { _propertyContent = value; }
        }

        private string _propertyType = string.Empty;

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

            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "songID", SongID);
            ADOExtenstion.AddParameter(comm, "propertyContent", PropertyContent);
            ADOExtenstion.AddParameter(comm, "propertyType", PropertyType);

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
                this.SongPropertyID = Convert.ToInt32(result);

                return this.SongPropertyID;
            }
        }


        public void GetSongPropertySongIDTypeID(int songID, string propertyType)
        {
            this.SongID = songID;
            this.PropertyType = propertyType;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored up_GetSongPropertySongIDTypeID name
            comm.CommandText = "up_GetSongPropertySongIDTypeID";

            ADOExtenstion.AddParameter(comm, "propertyType", PropertyType);
            ADOExtenstion.AddParameter(comm, "songID", SongID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);// should only be 1
            }

        }


        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.SongPropertyID = FromObj.IntFromObj(dr["songPropertyID"]);
                this.SongID = FromObj.IntFromObj(dr["songID"]);
                this.PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);
                this.PropertyType = FromObj.StringFromObj(dr["propertyType"]);
            }
            catch { }
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
             Songs sngs = new  Songs();

            sngs.GetSongsForVideo(vidID);

            StringBuilder sb = new StringBuilder();

            SongProperty sp = new SongProperty();

            foreach (Song sng in sngs)
            {
                sp = new SongProperty();

                sp.GetSongPropertySongIDTypeID(sng.SongID, SongProperty.SPropType.IT.ToString());

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
