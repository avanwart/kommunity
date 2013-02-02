//  Copyright 2012 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;


namespace BootBaronLib.AppSpec.DasKlub.BLL
{
    public static class OtherExt
    {
        public static Stream ToAStream(this System.Drawing.Image image, ImageFormat formaw)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, formaw);
            stream.Position = 0;
            return stream;
        }
    }

    /// <summary>
    /// Extension methods for HttpContext.Current.Cache
    /// </summary>
    public static class CacheExtension
    {
        #region variables
        
        static CacheItemRemovedCallback onRemove = null;
        //static bool itemRemoved = false;
        static CacheItemRemovedReason reasonRemoved;

        #endregion

        #region methods


        public static bool CacheItemExists(this Cache cache, string cacheName)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Cache[cacheName] == null) return false;
                else return true;
            }
            return false;
        }

        /// <summary>
        /// Get an object from the cache by its name
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public static object GetCachedObj(this Cache cache, string cacheName)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Cache[cacheName] == null)

                    return null;
                else
                {
                    return
                        HttpContext.Current.Cache.Get(cacheName);
                }
            }
            else return null;
        }

        /// <summary>
        /// Add an object to the cache with its name (add object to cache before the datarow for the object is used because it relies
        /// on knowing only the info before the DB call to instantiate)
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="obj"></param>
        /// <param name="cacheName"></param>
        public static void AddObjToCache(this Cache cache, object obj, string cacheName)
        {
            //onRemove += new CacheItemRemovedCallback(RemovedCallback);
            onRemove = new CacheItemRemovedCallback(RemovedCallback);

            if (HttpContext.Current != null && obj != null && !string.IsNullOrEmpty(cacheName))
            {
                //HttpContext.Current.Cache.DeleteCacheObj(cacheName);

                HttpContext.Current.Cache.Add(cacheName,
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
            //onRemove += new CacheItemRemovedCallback(RemovedCallback);
            onRemove = new CacheItemRemovedCallback(RemovedCallback);

            if (HttpContext.Current != null && obj != null && !string.IsNullOrEmpty(cacheName))
            {
                //HttpContext.Current.Cache.DeleteCacheObj(cacheName);

                HttpContext.Current.Cache.Add(cacheName,
                      obj,
                      null,
                      DateTime.UtcNow.AddMinutes(minutes),
                      Cache.NoSlidingExpiration,
                      CacheItemPriority.Default,
                      onRemove);
            }
        }

        /// <summary>
        /// Removes an item from the cache by its name and removes it from any sister sites
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="keyName"></param>
        public static void DeleteCacheObj(this Cache cache, string keyName)
        {
            if (HttpContext.Current != null && 
                !string.IsNullOrEmpty(keyName) && 
                HttpContext.Current.Cache[keyName] != null)
            {
                HttpContext.Current.Cache.Remove(keyName);
                
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
        /// Removes all the cached items on the remote server, regardless of name
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