namespace DasKlub.Lib.Interfaces
{
    /// <summary>
    ///     Implementing this means that the object outputs a ul with li's
    /// </summary>
    public interface IUnorderdList
    {
        string ToUnorderdList { get; }
    }
}