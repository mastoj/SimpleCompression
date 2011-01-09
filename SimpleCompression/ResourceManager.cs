using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;

namespace SimpleCompression
{
    internal class ResourceManager
    {
        #region static variables
        private static Dictionary<ResourceType, Dictionary<string, List<FileResource>>> resourceDictionary;
        private static Dictionary<string, string> resourceCache;
        #endregion

        private static ResourceManager instance;
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResourceManager();
                return instance;
            }
        }

        private ResourceManager()
        {
            resourceDictionary = new Dictionary<ResourceType, Dictionary<string, List<FileResource>>>();
            resourceDictionary.Add(ResourceType.Css, new Dictionary<string, List<FileResource>>());
            resourceDictionary.Add(ResourceType.javascript, new Dictionary<string, List<FileResource>>());
            resourceCache = new Dictionary<string, string>();
        }

        internal void RegisterCssFile(FileResource file, string resourceGroup = "")
        {
            RegisterResourceFile(ResourceType.Css, file, resourceGroup);
        }

        internal void RegisterJavascriptFile(FileResource file, string resourceGroup)
        {
            RegisterResourceFile(ResourceType.javascript, file, resourceGroup);
        }

        private void RegisterResourceFile(ResourceType resourceType, FileResource file, string resourceGroup)
        {
            if(resourceDictionary[resourceType].ContainsKey(resourceGroup))
                resourceDictionary[resourceType][resourceGroup].Add(file);
            else
                resourceDictionary[resourceType].Add(resourceGroup, new List<FileResource>() { file });
        }

        internal string PrintScriptTags()
        {
            string scriptTags = string.Empty;
            foreach (var resourceGroup in resourceDictionary[ResourceType.javascript].Keys)
            {
                scriptTags += PrintScriptTagForResourceGroup(resourceGroup) + Environment.NewLine;
            }
            return scriptTags;
        }

        internal string PrintScriptTagForResourceGroup(string resourceGroup)
        {
            var fileResources = resourceDictionary[ResourceType.javascript][resourceGroup];
            var hashCode = fileResources.GetHashCode();
            string fileName = SimpleCompressionConfiguration.ClientVersionPrefix + "_js_" + hashCode + ".js";
            string filePath = SimpleCompressionConfiguration.FolderForCachedResources + fileName;
            AddResourcesToCache(fileName, fileResources);
            var scriptTag = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>" + Environment.NewLine, filePath);
            return scriptTag;
        }

        internal string PrintCssTags()
        {
            string cssTags = string.Empty;
            foreach (var resourceGroup in resourceDictionary[ResourceType.Css].Keys)
            {
                cssTags += PrintCssTagForResourceGroup(resourceGroup) + Environment.NewLine;
            }
            return cssTags;
        }

        internal string PrintCssTagForResourceGroup(string resourceGroup)
        {
            var fileResources = resourceDictionary[ResourceType.Css][resourceGroup];
            var hashCode = fileResources.GetHashCode();
            string fileName = SimpleCompressionConfiguration.ClientVersionPrefix + "_css_" + hashCode + ".css";
            string filePath = SimpleCompressionConfiguration.FolderForCachedResources + fileName;
            AddResourcesToCache(fileName, fileResources);
            var cssTag = string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />" + Environment.NewLine, filePath);
            return cssTag;
        }

        private void AddResourcesToCache(string cacheKey, List<FileResource> fileResources)
        {
            HttpContext.Current.Cache.Add(cacheKey, fileResources, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        internal List<FileResource> GetResourcesFromCache(string cacheKey)
        {
            return (List<FileResource>)HttpContext.Current.Cache[cacheKey];
        }
    }

    class FileResource
    {
        public string FilePath { get; set; }
        public int Priority { get; set; }
        public bool Compress { get; set; }
    }

    enum ResourceType
    {
        Css,
        javascript
    }
}
