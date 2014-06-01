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
    public abstract class BaseIUserLogCrud : BaseExistance, ICRUD, IUserLog
    {
        #region properties

        #region IUserLog Members

        private DateTime _updateDate = DateTime.MinValue;

        public DateTime UpdateDate
        {
            get { return _updateDate; }
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