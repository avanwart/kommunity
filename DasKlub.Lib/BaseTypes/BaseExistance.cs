using System;

namespace DasKlub.Lib.BaseTypes
{
    public abstract class BaseExistance
    {
        private DateTime _createDate = DateTime.MinValue;
        public int CreatedByUserID { get; set; }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public virtual int Create()
        {
            throw new NotImplementedException();
        }
    }
}