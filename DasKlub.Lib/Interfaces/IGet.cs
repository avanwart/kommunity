using System.Data;

namespace DasKlub.Lib.Interfaces
{
    /// <summary>
    ///     Implementing this interface means you can take a datarow and
    ///     assign it to properties of this object
    /// </summary>
    public interface IGet
    {
        void Get(DataRow dr);
    }
}