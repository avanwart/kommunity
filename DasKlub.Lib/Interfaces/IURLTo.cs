using System;

namespace DasKlub.Lib.Interfaces
{
    internal interface IURLTo
    {
        /// <summary>
        ///     Returns a full url to navigate to
        /// </summary>
        Uri UrlTo { get; }
    }
}