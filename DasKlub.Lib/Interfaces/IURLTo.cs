using System;

namespace DasKlub.Lib.Interfaces
{
    internal interface IUrlTo
    {
        /// <summary>
        ///     Returns a full url to navigate to
        /// </summary>
        Uri UrlTo { get; }
    }
}