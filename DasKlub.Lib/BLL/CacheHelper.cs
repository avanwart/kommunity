using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace DasKlub.Lib.BLL
{
    public static class OtherExt
    {
        public static Stream ToAStream(this Image image, ImageFormat formaw)
        {
            var stream = new MemoryStream();
            image.Save(stream, formaw);
            stream.Position = 0;
            return stream;
        }
    }

    /// <summary>
    ///     Extension methods for HttpRuntime.Cache
    /// </summary>
    public static class CacheExtension
    {
        #region variables

        private static CacheItemRemovedCallback onRemove;
        private static CacheItemRemovedReason reasonRemoved;

        #endregion

        #region methods

        public static bool CacheItemExists(this Cache cache, string cacheName)
        {
            if (HttpContext.Current != null)
            {
                if (HttpRuntime.Cache[cacheName] == null) return false;
                else return true;
            }
            return false;
        }

        /// <summary>
        ///     Get an object from the cache by its name
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public static object GetCachedObj(this Cache cache, string cacheName)
        {
            if (HttpContext.Current != null)
            {
                return HttpRuntime.Cache[cacheName] == null ? null : HttpRuntime.Cache.Get(cacheName);
            }
            return null;
        }

        /// <summary>
        ///     Add an object to the cache with its name (add object to cache before the datarow for the object is used because it relies
        ///     on knowing only the info before the DB call to instantiate)
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="obj"></param>
        /// <param name="cacheName"></param>
        public static void AddObjToCache(this Cache cache, object obj, string cacheName)
        {
            onRemove = RemovedCallback;

            if (HttpContext.Current != null && obj != null && !string.IsNullOrEmpty(cacheName))
            {
                HttpRuntime.Cache.Add(cacheName,
                                              obj,
                                              null,
                                              Cache.NoAbsoluteExpiration,
                                              new TimeSpan(0, 5, 0),
                                              CacheItemPriority.Default,
                                              onRemove);
            }
        }


        public static void AddObjToCache(this Cache cache, object obj, string cacheName, int minutes)
        {
            onRemove = RemovedCallback;

            if (HttpContext.Current != null && obj != null && !string.IsNullOrEmpty(cacheName))
            {
                HttpRuntime.Cache.Add(cacheName,
                                              obj,
                                              null,
                                              DateTime.UtcNow.AddMinutes(minutes),
                                              Cache.NoSlidingExpiration,
                                              CacheItemPriority.Default,
                                              onRemove);
            }
        }

        /// <summary>
        ///     Removes an item from the cache by its name and removes it from any sister sites
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="keyName"></param>
        public static void DeleteCacheObj(this Cache cache, string keyName)
        {
            if (HttpContext.Current != null &&
                !string.IsNullOrEmpty(keyName) &&
                HttpRuntime.Cache[keyName] != null)
            {
                HttpRuntime.Cache.Remove(keyName);
            }
        }

        public static void RemoveExternalCache(string keyName)
        {
            //// delete on all remote systems
            //foreach (string partner in GeneralConfigs.TwinSites)
            //{
            //    if (!string.IsNullOrEmpty(partner))
            //    {
            //        ItemizerRequest.ClearCache(partner, keyName);
            //    }
            //}
        }


        /// <summary>
        ///     Removes all the cached items on the remote server, regardless of name
        /// </summary>
        public static void RemoveExternalCache()
        {
            //// delete on all remote systems
            //foreach (string partner in GeneralConfigs.TwinSites)
            //{
            //    if (!string.IsNullOrEmpty(partner))
            //    {
            //        ItemizerRequest.ClearCache(partner);
            //    }
            //}
        }

        #endregion

        #region events

        public static void RemovedCallback(String keyName, Object obj, CacheItemRemovedReason reason)
        {
            //itemRemoved = true;
            //reason = reason;
            reasonRemoved = reason;

            RemoveExternalCache(keyName);
        }

        #endregion
    }
}