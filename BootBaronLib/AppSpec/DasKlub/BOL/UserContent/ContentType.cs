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
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;


namespace BootBaronLib.AppSpec.DasKlub.BOL.UserContent
{
    public class ContentType : BaseIUserLogCRUD
    {
        #region properties

        private int _contentTypeID = 0;

        public int ContentTypeID
        {
            get { return _contentTypeID; }
            set { _contentTypeID = value; }
        }

        private string _contentName = string.Empty;

        public string ContentName
        {
            get { return _contentName; }
            set { _contentName = value; }
        }



        public ContentType(DataRow dr)
        {
            // TODO: Complete member initialization
            Get(dr);
        }

        public ContentType()
        {
            // TODO: Complete member initialization
        }

        public ContentType(int p)
        {
            // TODO: Complete member initialization
            Get(p);
        }

        #endregion


        public override void Get(int contentTypeID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentTypeByID";

            ADOExtenstion.AddParameter(comm, "contentTypeID",  contentTypeID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
            //   base.Get(dr);


        }

        //public void GetContentTypeByContentCode()
        //{
        //    // get a configured DbCommand object
        //    DbCommand comm = DbAct.CreateCommand();
        //    // set the stored procedure name
        //    comm.CommandText = "up_GetContentTypeByContentCode";
        //    // create a new parameter
        //    DbParameter param = comm.CreateParameter();
        //    param.ParameterName = "@contentCode";
        //    param.Value = ContentCode.ToString();
        //    param.DbType = DbType.String;
        //    comm.Parameters.Add(param);

        //    // execute the stored procedure
        //    DataTable dt = DbAct.ExecuteSelectCommand(comm);

        //    // was something returned?
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        Get(dt.Rows[0]);
        //    }
        //    //   base.Get(dr);


        //}

        public override void Get(DataRow dr)
        {


            try
            {


                base.Get(dr);




                this.ContentTypeID = FromObj.IntFromObj(dr["contentTypeID"]);

                string contentCode = FromObj.StringFromObj(dr["contentCode"]);

                //if (string.IsNullOrEmpty(contentCode))
                //    this.ContentCode = SiteEnums.ContentTypesForPages.UNKNO;
                //else

                //    this.ContentCode = (SiteEnums.ContentTypesForPages)
                //        Enum.Parse(typeof(SiteEnums.ContentTypesForPages), contentCode);



                this.ContentName = FromObj.StringFromObj(dr["contentName"]);


            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
            }



        }
    }

    public class ContentTypes : List<ContentType>, IGetAll
    {




        #region IGetAll Members

        public void GetAll()
        {



            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllContentTypes";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                    this.Add(new ContentType(dr));
            }
        }
        #endregion
    }
}
