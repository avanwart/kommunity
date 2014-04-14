namespace DasKlub.Lib.Interfaces
{
    /// <summary>
    ///     Creates or updates the record depending on if it was created or not
    /// </summary>
    public interface ISet
    {
        /// <summary>
        ///     Creates or updates the record
        /// </summary>
        /// <returns></returns>
        bool Set();
    }
}