using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompression
{
    public static class SimpleCompressionConfiguration
    {
        private static ICompress compressorToUse;
        public static ICompress CompressorToUse
        {
            get
            {
                if (compressorToUse == null)
                    compressorToUse = new YUICompression();
                return compressorToUse;
            }
            set
            {
                compressorToUse = value;
            }
        }

        private static string folderForCachedResources;
        public static string FolderForCachedResources
        {
            get
            {
                if (string.IsNullOrEmpty(folderForCachedResources))
                {
                    folderForCachedResources = "/cache/";
                }
                return folderForCachedResources;
            }
            set
            {
                if (value.EndsWith("/") == false)
                    value += "/";
                folderForCachedResources = value;
            }
        }

        private static string clientVersionPrefix;
        public static string ClientVersionPrefix
        {
            get
            {
                if (clientVersionPrefix == null)
                {
                    clientVersionPrefix = string.Empty;
                }
                return clientVersionPrefix;
            }
            set
            {
                clientVersionPrefix = value;
            }
        }
    }
}
