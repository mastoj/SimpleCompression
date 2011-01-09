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
        public static MvcHtmlString RegisterScript(this HtmlHelper helper, string file, string resourceGroup = "", int priority = 0, bool compress = true, bool ignore = false)
        {
            return helper.RegisterResource(file, resourceGroup, priority, compress, ignore, TagHelper.PrintJavascriptTag, ResourceManager.Instance.RegisterJavascriptFile);
        }

        public static MvcHtmlString CreateScriptTags(this HtmlHelper helper)
        {
            var tagString = ResourceManager.Instance.PrintScriptTags();
            return new MvcHtmlString(tagString);
        }

        public static MvcHtmlString CreateScriptTags(this HtmlHelper helper, string resourceGroup)
        {
            var tagString = ResourceManager.Instance.PrintScriptTagForResourceGroup(resourceGroup);
            return new MvcHtmlString(tagString);
        }

        public static MvcHtmlString RegisterCss(this HtmlHelper helper, string file, string resourceGroup = "", int priority = 0, bool compress = true, bool ignore = false)
        {
            return helper.RegisterResource(file, resourceGroup, priority, compress, ignore, TagHelper.PrintCssTag, ResourceManager.Instance.RegisterCssFile);
        }

        private static MvcHtmlString RegisterResource(this HtmlHelper helper, string file, string resourceGroup, int priority, bool compress, bool ignore, 
            Func<string, string> printTagFunction, Action<FileResource, string> registerResourceFunction)
        {
            if (ignore || SimpleCompressionConfiguration.Disable)
            {
                return helper.PrintTag(file, TagHelper.PrintCssTag);
            }
            var fileResource = new FileResource() { FilePath = file, Priority = priority, Compress = compress };
            registerResourceFunction(fileResource, resourceGroup);
            return new MvcHtmlString(string.Empty);        
        }

        private static MvcHtmlString PrintTag(this HtmlHelper helper, string file, Func<string, string> printTagFunction)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var fileName = urlHelper.Content(file);
            var tag = printTagFunction(fileName);
            return new MvcHtmlString(tag);
        }

        public static MvcHtmlString CreateCssTags(this HtmlHelper helper, string resourceGroup)
        {
            var tagString = ResourceManager.Instance.PrintCssTagForResourceGroup(resourceGroup);
            return new MvcHtmlString(tagString);
        }

        public static MvcHtmlString CreateCssTags(this HtmlHelper helper)
        {
            var tagString = ResourceManager.Instance.PrintCssTags();
            return new MvcHtmlString(tagString);
        }
    }
}
