using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompression.Web
{
    public static class TagHelper
    {
        public static string PrintCssTag(string path)
        {
            var cssTag = string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />" + Environment.NewLine, path);
            return cssTag;
        }

        public static string PrintJavascriptTag(string path)
        {
            var scriptTag = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>" + Environment.NewLine, path);
            return scriptTag;
        }
    }
}
