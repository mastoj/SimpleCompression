using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace SimpleCompression.Mvc
{
    public static class SimpleCompressionHelpers
    {
        public static MvcHtmlString RegisterScript(this HtmlHelper helper, string file, string resourceGroup = "", int priority = 0, bool compress = true, bool ignore = false)
        {
            if (ignore || SimpleCompressionConfiguration.Disable)
            {
                return helper.PrintRegularScriptTag(file);
            }
            var fileResource = new FileResource() { FilePath = file, Priority = priority, Compress = compress };
            ResourceManager.Instance.RegisterJavascriptFile(fileResource, resourceGroup);
            return new MvcHtmlString(string.Empty);
        }

        private static MvcHtmlString PrintRegularScriptTag(this HtmlHelper helper, string file)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var fileName = urlHelper.Content(file);
            var scriptTag = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>" + Environment.NewLine, fileName);
            return new MvcHtmlString(scriptTag);
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
            if (ignore || SimpleCompressionConfiguration.Disable)
            {
                return helper.PrintRegularCssTag(file);
            }
            var fileResource = new FileResource() { FilePath = file, Priority = priority, Compress = compress };
            ResourceManager.Instance.RegisterCssFile(fileResource, resourceGroup);
            return new MvcHtmlString(string.Empty);
        }

        private static MvcHtmlString PrintRegularCssTag(this HtmlHelper helper, string file)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var fileName = urlHelper.Content(file);
            var cssTag = string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />" + Environment.NewLine, fileName);
            return new MvcHtmlString(cssTag);
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
