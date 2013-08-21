//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 19-08-08
//===========================================================================================

#region Using
using System;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Generic;
#endregion

namespace Miron.Web.MbCompression
{
    internal sealed class Settings
    {
        private static readonly object _msAjaxLock = new object();

        #region // Constants
        // Name of the settings section in the web.config file
        private const string CONFIGURATION_SECTION_NAME = "Miron.web/CompressorSettings";

        // Configuration attributes
        private const string CONFIGURATION_EXCLUDE_TYPES_NODE_NAME = "excludeTypes";
        private const string CONFIGURATION_EXCLUDE_PATHS_NODE_NAME = "excludePaths";

        private const string CONFIGURATION_COMPRESS_CSS_ATTRIBUTE = "compressCSS";
        private const string CONFIGURATION_COMPRESS_JS_ATTRIBUTE = "compressJavaScript";
        private const string CONFIGURATION_COMPRESS_PAGE_ATTRIBUTE = "compressPage";
        private const string CONFIGURATION_CACHEING_STORAGE_NAME = "cachingStorage";
        private const string CONFIGURATION_COMPRESS_WEBRESOURCE_ATTRIBUTE = "compressWebResource";
        private const string CONFIGURATION_REFLECTION_ALLOWEDED_ATTRIBUTE = "reflectionAlloweded";
        private const string CONFIGURATION_PARSE_THIRD_PARTY_SCRIPTS = "compressThirdParityScripts";
        private const string CONFIGURATION_MINIFY_CONTENT = "minifyContent";
        private const string CONFIGURATION_AUTOMODE = "autoMode";
        private const string CONFIGURATION_CSS_VERSION = "cssVersion";
        private const string CONFIGURATION_SCRIPTS_VERSION = "scriptsVersion";

        #endregion


        #region // Private members
        private Dictionary<string,sbyte> _excludeTypes;
        private Dictionary<string, sbyte> _excludePathes;
        private static readonly Settings settings = new Settings();
        private double _daysCache = 365;        // Default: 365 days
        private bool _compressCSS = true;
        private bool _compressJavaScript = true;
        private bool _compressPage = true;
        private bool _compressWebResource = true;
        private bool _reflectionAlloweded = true;
        private bool _minifyContent = true;
        private ICachingStorage _storage;
        private bool _autoMode = true;
        private string _cssVersion = string.Empty;
        private string _jsVersion = string.Empty;
        private double _msAjaxVersion = -1;
        #endregion


        #region // Properties

        internal double DaysInCache
        {
            get { return _daysCache; }
        }
        internal bool CompressCSS
        {
            get { return _compressCSS; }
        }
        internal bool CompressJavaScript
        {
            get { return _compressJavaScript; }
        }
        internal bool CompressPage
        {
            get { return _compressPage; }
        }
        internal bool CompressWebResource
        {
            get { return _compressWebResource; }
        }
        internal bool ReflectionAlloweded
        {
            get { return _reflectionAlloweded; }
            set { _reflectionAlloweded = value; }
        }
        internal ICachingStorage CurrentStorage
        {
            get 
            {
                if (_storage == null)
                {
                    _storage = CachingStorageFactory.GetStorage("OutputCache"); // Default
                }
                return _storage;
            }
        }
        internal bool MinifyContent
        {
            get { return _minifyContent; }
        }
        internal bool AutoMode
        {
            get { return _autoMode; }
        }
        internal string CssVersion
        {
            get { return _cssVersion; }
        }
        internal string JsVersion
        {
            get { return _jsVersion; }
        }
        internal double MSAjaxVersion
        {
            get
            {
                if (_msAjaxVersion < 0)
                {
                    lock (_msAjaxLock)
                    {
                        if (_msAjaxVersion < 0)
                        {
                            try
                            {
                                _msAjaxVersion = Util.GetMsAjaxVersion();
                            }
                            catch (Exception)
                            {
                                _msAjaxVersion = 0;
                            }
                        }
                    }
                }
                return _msAjaxVersion;
            }
        }
        #endregion


        #region // Constractors

        /// <summary>
        /// Private constructor
        /// </summary>
        private Settings()
        {
            Initialize();
        }


        #endregion


        // Create singleton object of the settings
        internal static Settings Instance
        {
            get
            {
                return settings;
            }
        }


        #region // Initialize

        private void Initialize()
        {
            _excludeTypes = new Dictionary<string, sbyte>(StringComparer.OrdinalIgnoreCase);
            _excludePathes = new Dictionary<string, sbyte>(StringComparer.OrdinalIgnoreCase);

            // Load the configuration settings from the web.config
            SettingsConfigSection configSection = ConfigurationManager.GetSection(CONFIGURATION_SECTION_NAME) as SettingsConfigSection;

            // No configuration was setting
            if (configSection == null)
            {
                return;
            }

            // Load the extra properties for the compressos
            LoadCompressorProperties(configSection.Config);

            // Add the exluded mime types from the configSection
            LoadExcludedTypes(configSection.Config.SelectSingleNode("excludeTypes"));

            // Add the exluded paths from the configSection
            LoadExcludedPaths(configSection.Config.SelectSingleNode("excludePaths"));
        }


        /// <summary>
        /// Add the excluded mime types from the configSection
        /// </summary>
        /// <param name="node"></param>
        private void LoadExcludedTypes(XPathNavigator node)
        {
            if (node == null)
                return;

            XPathNodeIterator childrens = node.SelectChildren(XPathNodeType.All);
            foreach (XPathNavigator child in childrens)
            {
                if (!string.IsNullOrEmpty(child.GetAttribute("key", string.Empty)))
                    _excludeTypes.Add(child.GetAttribute("key", string.Empty), 0);
            }
        }

        /// <summary>
        /// Add the excluded paths from the configSection
        /// </summary>
        /// <param name="node"></param>
        private void LoadExcludedPaths(XPathNavigator node)
        {
            if (node == null)
                return;

            XPathNodeIterator childrens = node.SelectChildren(XPathNodeType.All);
            foreach (XPathNavigator child in childrens)
            {
                if (!string.IsNullOrEmpty(child.GetAttribute("key", string.Empty)))
                    _excludePathes.Add(child.GetAttribute("key", string.Empty), 0);
            }
        }

        /// <summary>
        /// Load the extra properties for the compressos
        /// </summary>
        /// <param name="node"></param>
        private void LoadCompressorProperties(XPathNavigator node)
        {
            if (node == null)
                return;

            bool tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_CSS_ATTRIBUTE, string.Empty), out tmpBool))
                _compressCSS = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_JS_ATTRIBUTE, string.Empty), out tmpBool))
                _compressJavaScript = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_PAGE_ATTRIBUTE, string.Empty), out tmpBool))
                _compressPage = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_WEBRESOURCE_ATTRIBUTE, string.Empty), out tmpBool))
                _compressWebResource = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_REFLECTION_ALLOWEDED_ATTRIBUTE, string.Empty), out tmpBool))
                _reflectionAlloweded = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_MINIFY_CONTENT, string.Empty), out tmpBool))
                _minifyContent = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_AUTOMODE, string.Empty), out tmpBool))
                _autoMode = tmpBool;

            _cssVersion = string.IsNullOrEmpty(node.GetAttribute(CONFIGURATION_CSS_VERSION, string.Empty))
                ? string.Empty 
                : "&v=" + Util.UrlEncode(node.GetAttribute(CONFIGURATION_CSS_VERSION, string.Empty));

            _jsVersion = string.IsNullOrEmpty(node.GetAttribute(CONFIGURATION_SCRIPTS_VERSION, string.Empty))
                ? string.Empty
                : "&v=" + Util.UrlEncode(node.GetAttribute(CONFIGURATION_SCRIPTS_VERSION, string.Empty));

            _storage = CachingStorageFactory.GetStorage(node.GetAttribute(CONFIGURATION_CACHEING_STORAGE_NAME, string.Empty));
        }

        #endregion


        // Check if the gives type is valid to be compressed
        internal bool IsValidType(string type)
        {
            return !_excludeTypes.ContainsKey(type);
        }


        // Check if the gives path is valid to be compressed
        internal bool IsValidPath(string path)
        {
            return !_excludePathes.ContainsKey(path);
        }
    }

    #region // Class to hold the configuration section from the web.config
    public class SettingsConfigSection : IConfigurationSectionHandler
    {
        private XPathNavigator _config;

        public XPathNavigator Config
        {
            get { return _config; }
        }

        protected SettingsConfigSection() { }

        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section == null)
            {
                return null;
            }
            _config = section.CreateNavigator();
            return this;
        }
    }
    #endregion
}