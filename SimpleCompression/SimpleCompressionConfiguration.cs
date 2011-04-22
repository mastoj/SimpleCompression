using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompression
{
    public class SimpleCompressionConfiguration
    {
        private static SimpleCompressionConfiguration _defaultConfiguration;
        public static SimpleCompressionConfiguration DefaulConfiguration
        {
            get
            {
                _defaultConfiguration = _defaultConfiguration ?? new SimpleCompressionConfiguration();
                return _defaultConfiguration;
            }
        }

        public SimpleCompressionConfiguration()
        {
            if (_defaultConfiguration != null)
            {
                Disable = _defaultConfiguration.Disable;
                Compressor = _defaultConfiguration.Compressor;
                FolderForCachedResources = _defaultConfiguration.FolderForCachedResources;
                Compress = DefaulConfiguration.Compress;
            }
        }

        private bool _disable = false;
        public bool Disable
        {
            get
            {
                return _disable;
            }
            set
            {
                _disable = value;
            }
        }

        private ICompress _compressor;
        public ICompress Compressor
        {
            get
            {
                if (_compressor == null)
                    _compressor = new YUICompression();
                return _compressor;
            }
            set
            {
                _compressor = value;
            }
        }

        private string _folderForCachedResources;
        public string FolderForCachedResources
        {
            get
            {
                if (string.IsNullOrEmpty(_folderForCachedResources))
                {
                    _folderForCachedResources = "cache/";
                }
                return _folderForCachedResources;
            }
            set
            {
                if (value.EndsWith("/") == false)
                    value += "/";
                _folderForCachedResources = value;
            }
        }

        private string _clientVersionPrefix;
        public string ClientVersionPrefix
        {
            get
            {
                if (_clientVersionPrefix == null)
                {
                    _clientVersionPrefix = string.Empty;
                }
                return _clientVersionPrefix;
            }
            set
            {
                _clientVersionPrefix = value;
            }
        }

        private bool _compress = true;
        public bool Compress
        {
            get { return _compress; }
            set { _compress = value; }
        }

        public static void SetDefaultConfiguration(SimpleCompressionConfiguration defaultConfig)
        {
            _defaultConfiguration = defaultConfig;
        }
    }
}
