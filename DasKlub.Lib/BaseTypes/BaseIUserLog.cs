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
using System.Data;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BaseTypes
{
    /// <summary>
    ///     This is used so that all the CRUD and logging actions
    ///     can be performed through inheritance
    /// </summary>
    public abstract class BaseIUserLogCRUD : BaseExistance, ICRUD, IUserLog
    {
        #region properties

        #region IUserLog Members

        private DateTime _updateDate = DateTime.MinValue;

        public DateTime UpdateDate
        {
            get
            {
                //if (this._updateDate == DateTime.MinValue) return this.CreateDate;

                return _updateDate;
            }
            set { _updateDate = value; }
        }


        public int UpdatedByUserID { get; set; }

        #endregion

        #endregion

        #region methods

        #region IBaseCRUD Members

        public virtual void Get(DataRow dr)
        {
            try
            {
                //IUserLog
                CreateDate = FromDataRow.DateTimeFromDataRow(dr, "createDate");
                UpdateDate = FromDataRow.DateTimeFromDataRow(dr, "updateDate");
                CreatedByUserID = FromDataRow.IntFromDataRow(dr, "createdByUserID");
                UpdatedByUserID = FromDataRow.IntFromDataRow(dr, "updatedByUserID");
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
            }
        }

        public virtual bool Update()
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICRUD Members

        public virtual void Get(int uniqueID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}