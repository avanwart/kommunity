namespace DasKlub.Lib.Interfaces
{
    /// <summary>
    ///     Reflects that this object can get every row in the table without
    ///     any filtering
    /// </summary>
    public interface IGetAll
    {
        /// <summary>
        ///     Get every row in the table
        /// </summary>
        void GetAll();
    }
}