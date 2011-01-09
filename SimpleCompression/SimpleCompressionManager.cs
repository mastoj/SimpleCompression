using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimpleCompression
{
    public class SimpleCompressionManager : ISimpleCompressionManager
    {
        private ICompress _compressionEngine = null;

        public SimpleCompressionManager(ICompress compressionEngine)
        {
            _compressionEngine = compressionEngine;
        }

        public string CombineFiles(params string[] files)
        {
            StringBuilder combinedOutPut = new StringBuilder();
            foreach (var filePath in files)
            {
                var file = new FileInfo(filePath);
                var fileReader = file.OpenRead();
                var streamReader = new StreamReader(fileReader);
                combinedOutPut.Append(streamReader.ReadToEnd());
            }
            return combinedOutPut.ToString();
        }

        public string CombineStrings(params string[] strings)
        {
            return String.Concat(strings);
        }

        public string CombineAndCompressFiles(params string[] files)
        {
            throw new NotImplementedException();
        }

        public string CompressJavascriptString(string javascript)
        {
            throw new NotImplementedException();
        }

        public string CompressCssString(string cssString)
        {
            throw new NotImplementedException();
        }
    }
}
