using System;

namespace DasKlub.Lib.Interfaces
{
    /// <summary>
    ///     Represents ways to know when an action was taken
    ///     to create the data, update it, and who was responsible
    /// </summary>
    public interface IUserLog
    {
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
        int CreatedByUserID { get; set; }
        int UpdatedByUserID { get; set; }
    }
}