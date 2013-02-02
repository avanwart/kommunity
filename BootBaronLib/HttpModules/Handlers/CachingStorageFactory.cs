//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================

using System;

namespace Miron.Web.MbCompression
{
	internal sealed class CachingStorageFactory
	{
        /// <summary>
        /// Get the caching storage object by name
        /// </summary>
        /// <param name="storageName"></param>
        /// <returns></returns>
        internal static ICachingStorage GetStorage(string storageName)
        {
            if (string.IsNullOrEmpty(storageName))
            {
                //return the default storage
                return new OutputCacheStorage();
            }
            storageName = storageName.ToUpperInvariant();
            switch(storageName)
            {
                case "FILESYSTEM":
                    return new FilesystemStorage();
                case "OUTPUTCACHE":
                    return new OutputCacheStorage();
                default:
                    throw new ArgumentException(SR.GetString(SR.ICachingStorage_Not_Valid));
            }
        }
	}
}
