using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;

namespace SimpleCompression
{
    internal class ResourceCacheManager
    {

        private static ResourceCacheManager _instance;
        public static ResourceCacheManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ResourceCacheManager();
                return _instance;
            }
        }

        private ResourceCacheManager()
        {
        }

        internal void AddResourcesToCache(string cacheKey, Tuple<List<FileResource>, SimpleCompressionConfiguration> resourcesAndConfiguration)
        {
            cacheKey = "res_" + cacheKey;
            HttpContext.Current.Cache.Add(cacheKey, resourcesAndConfiguration, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 9), CacheItemPriority.NotRemovable, null);
        }

        internal Tuple<List<FileResource>, SimpleCompressionConfiguration> GetResourcesFromCache(string cacheKey)
        {
            cacheKey = "res_" + cacheKey;
            return HttpContext.Current.Cache[cacheKey] as Tuple<List<FileResource>, SimpleCompressionConfiguration>;
        }

        internal void AddFileToCache(string cacheKey, string file)
        {
            cacheKey = "file_" + cacheKey;
            HttpContext.Current.Cache.Add(cacheKey, file, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 9), CacheItemPriority.NotRemovable, null);            
        }

        internal string GetFileFromCache(string cacheKey)
        {
            cacheKey = "file_" + cacheKey;
            return HttpContext.Current.Cache[cacheKey] as string;
        }
    }
}
