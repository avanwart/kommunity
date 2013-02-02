//  Copyright 2012 
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
using System.Data;
using BootBaronLib.DAL;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent
{
    public class ArtistProperty : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private int _artistPropertyID = 0;

        public int ArtistPropertyID
        {
            get { return _artistPropertyID; }
            set { _artistPropertyID = value; }
        }

        private int _artistID = 0;

        public int ArtistID
        {
            get { return _artistID; }
            set { _artistID = value; }
        }

        private string _propertyContent = string.Empty;

        public string PropertyContent
        {
            get { return _propertyContent; }
            set { _propertyContent = value; }
        }

        private string _propertyType = string.Empty;

        /// <summary>
        /// Options are: 
        /// MD = meta description
        /// LD = long description
        /// PH = photo
        /// </summary>
        public string PropertyType
        {
            get { return _propertyType; }
            set { _propertyType = value; }
        }

        #endregion


        #region constants

        public const string ARTISTIMAGEPREFIX = "~/Content/artists/";

        #endregion


        #region constructors


        public ArtistProperty() { }

        #endregion

        #region methods

        public void GetArtistPropertyForTypeArtist(int artistID, string propertyType)
        {
            this.ArtistID = artistID;
            this.PropertyType = propertyType;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetArtistPropertyForTypeArtist";

            ADOExtenstion.AddParameter(comm, "artistID", ArtistID);
            ADOExtenstion.AddParameter(comm, "propertyType", PropertyType);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }



        public override bool Update()
        {
            if (this.ArtistPropertyID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateArtistProperty";

            // create a new parameter
            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "artistID", ArtistID);
            ADOExtenstion.AddParameter(comm, "propertyContent", PropertyContent);
            ADOExtenstion.AddParameter(comm, "propertyType", PropertyType);
            ADOExtenstion.AddParameter(comm, "artistPropertyID", ArtistPropertyID);

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            RemoveCache();

            return result;
        }



        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddArtistProperty";

            // create a new parameter
            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "artistID", ArtistID);
            ADOExtenstion.AddParameter(comm, "propertyContent", PropertyContent);
            ADOExtenstion.AddParameter(comm, "propertyType", PropertyType);
 
            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.ArtistPropertyID = Convert.ToInt32(result);

            return this.ArtistPropertyID;
        }


        public override bool Delete()
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteArtistProperty";

            ADOExtenstion.AddParameter(comm, "artistPropertyID", this.ArtistPropertyID);

            RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;

        }





        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                this.ArtistID = FromObj.IntFromObj(dr["artistID"]);
                this.PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);
                this.PropertyType = FromObj.StringFromObj(dr["propertyType"]);
                this.ArtistPropertyID = FromObj.IntFromObj(dr["artistPropertyID"]);
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
            // TODO: USE THIS
            return;
        }

        #endregion
    }
}
