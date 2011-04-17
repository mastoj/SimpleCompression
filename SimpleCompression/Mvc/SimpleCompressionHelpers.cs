using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using SimpleCompression.Web;

namespace SimpleCompression.Mvc
{
    public static class SimpleCompressionHelpers
    {
        public static MvcHtmlString RegisterScripts(this HtmlHelper helper, SimpleCompressionConfiguration configuration = null, params string[] files)
        {
            return helper.RegisterResources(configuration, files, ResourceType.Javascript);
        }

        public static MvcHtmlString RegisterCssFiles(this HtmlHelper helper, SimpleCompressionConfiguration configuration = null, params string[] files)
        {
            return helper.RegisterResources(configuration, files, ResourceType.Css);
        }

        private static MvcHtmlString RegisterResources(this HtmlHelper helper, SimpleCompressionConfiguration configuration, string[] files, ResourceType resourceType)
        {
            configuration = configuration ?? SimpleCompressionConfiguration.DefaulConfiguration;

            Func<string, string> printFunc = resourceType == ResourceType.Css
                                                 ? (Func<string,string>)TagHelper.PrintCssTag
                                                 : TagHelper.PrintJavascriptTag;

            string tagString = string.Empty;
            if (configuration.Disable)
            {
                foreach (var file in files)
                {
                    tagString += helper.PrintTag(file, printFunc);
                }
            }
            else
            {

                var fileResources = CreateResources(files, configuration);

                var hashCode = fileResources.GetHashCode();
                string extension = resourceType == ResourceType.Css ? ".css" : ".js";
                string fileName = configuration.ClientVersionPrefix + hashCode + extension;
                string filePath = configuration.FolderForCachedResources + fileName;
                ResourceCacheManager.Instance.AddResourcesToCache(fileName, new Tuple<List<FileResource>, SimpleCompressionConfiguration>(fileResources, configuration));
                tagString = helper.PrintTag(filePath, printFunc);
            }
            return new MvcHtmlString(tagString);            
        }

        private static List<FileResource> CreateResources(string[] files, SimpleCompressionConfiguration configuration)
        {
            var resources = new List<FileResource>();
            foreach (var file in files)
            {
                var resource = new FileResource(file);
                resources.Add(resource);
            }
            return resources;
        }

        private static string PrintTag(this HtmlHelper helper, string file, Func<string, string> printTagFunction)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext); 
            var fileName = urlHelper.Content(file); 
            var tag = printTagFunction(fileName); 
            return tag;
        }
    }
}
