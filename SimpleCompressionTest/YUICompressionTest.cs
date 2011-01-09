using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCompression;
using NUnit.Framework;

namespace SimpleCompression.SimpleCompressionTest
{
    [TestFixture]
    public class YUICompressionTest
    {
        [Test]
        public void YUICompressionConstructTest()
        {
            var target = new YUICompression();

            Assert.IsNotNull(target);
        }

        [Test]
        public void Compress_Javascript_String()
        {
            var exampleJSScript = @"function myfunction(txt) 
{ 
alert(txt);
} ";
            var target = new YUICompression();

            var result = target.CompressJavascriptString(exampleJSScript);

            Assert.IsTrue(result.Length < exampleJSScript.Length);
        }

        [Test]
        public void Compress_Css_String()
        {
            var exampleCss = @"body {
    line-height: 1;
    text-align: left;
}";
            var target = new YUICompression();

            var result = target.CompressCssString(exampleCss);

            Assert.IsTrue(result.Length < exampleCss.Length);
        }
    }
}
