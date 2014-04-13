namespace DasKlub.Lib.Interfaces
{
    public interface ICacheName
    {
        string CacheName { get; }

        void RemoveCache();
    }
}