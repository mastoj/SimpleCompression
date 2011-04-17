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
            HttpContext.Current.Cache.Add(cacheKey, resourcesAndConfiguration, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        internal Tuple<List<FileResource>, SimpleCompressionConfiguration> GetResourcesFromCache(string cacheKey)
        {
            return HttpContext.Current.Cache[cacheKey] as Tuple<List<FileResource>, SimpleCompressionConfiguration>;
        }
    }
}
