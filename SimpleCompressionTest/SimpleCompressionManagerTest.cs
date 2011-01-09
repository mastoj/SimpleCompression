using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCompression;
using NUnit.Framework;

namespace SimpleCompression.SimpleCompressionTest
{
    [TestFixture]
    public class SimpleCompressionManagerTest
    {
        [Test]
        public void Combine_Strings()
        {
            var strings = new string[] { "1", "2", "3" };
            var expectedResult = "123";

            var target = new SimpleCompressionManager(null);
            var result = target.CombineStrings(strings);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Combine_Files_With_Relative_Path()
        {
            var files = new string[] { ".\\ExampleFiles\\File1.js", ".\\ExampleFiles\\File2.js" };
            var expectedResult = "function hej() { }function hej2() { }";

            var target = new SimpleCompressionManager(null);
            var result = target.CombineFiles(files);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void Combine_Files_Throws_FileNotFoundExpcetion_If_Not_Found()
        {
            var files = new string[] { ".\\ExampleFiles\\File1.js", ".\\ExampleFiles\\File3.js" };
            var expectedResult = "function hej() { }function hej2() { }";

            var target = new SimpleCompressionManager(null);
            var result = target.CombineFiles(files);

            Assert.AreEqual(expectedResult, result);
        }


    }
}
