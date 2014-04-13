namespace DasKlub.Lib.Interfaces
{
    /// <summary>
    ///     Implementing this means that the object can be output to an li string
    /// </summary>
    public interface IUnorderdListItem
    {
        string ToUnorderdListItem { get; }
    }
}