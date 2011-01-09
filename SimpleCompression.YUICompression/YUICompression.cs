using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yahoo.Yui.Compressor;

namespace SimpleCompression.YUICompression
{
    public class YUICompression : ICompress
    {
        public string CompressJavascriptString(string exampleJSScript)
        {
            var compressedString = JavaScriptCompressor.Compress(exampleJSScript);
            return compressedString;
        }

        public string CompressCssString(string cssString)
        {
            var compressor = CssCompressor.Compress(cssString);
            return compressor;
        }
    }
}
