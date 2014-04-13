namespace DasKlub.Lib.Interfaces
{
    /// <summary>
    ///     Implementing this means that all operations are possible
    /// </summary>
    public interface IBaseCRUD : IGet
    {
        /// <summary>
        ///     Add row
        /// </summary>
        /// <returns></returns>
        int Create();

        /// <summary>
        ///     Update object
        /// </summary>
        /// <returns></returns>
        bool Update();

        /// <summary>
        ///     Delete the object
        /// </summary>
        /// <returns></returns>
        bool Delete();
    }
}