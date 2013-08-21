using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System.IO;

namespace AdShoreLib.AspNetPerformanceOptimizer.ConfigurationSections
{
    /// <summary>
    /// 
    /// </summary>
    public class OptimizerSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ScriptCollection Scripts
        {
            get { return (ScriptCollection)this[""]; }
            set { this[""] = value; }
        }

        [ConfigurationProperty("enable", IsRequired = true)]
        public bool Enable
        {
            get { return (bool)this["enable"]; }
            set { this["enable"] = value; }
        }
        [ConfigurationProperty("enableProfiler", IsRequired = true)]
        public bool EnableProfiler
        {
            get { return (bool)this["enableProfiler"]; }
            set { this["enableProfiler"] = value; }
        }
        [ConfigurationProperty("enableScriptCompression", IsRequired = true)]
        public bool EnableScriptCompression
        {
            get { return (bool)this["enableScriptCompression"]; }
            set { this["enableScriptCompression"] = value; }
        }
        [ConfigurationProperty("enableHtmlCompression", IsRequired = true)]
        public bool EnableHtmlCompression
        {
            get { return (bool)this["enableHtmlCompression"]; }
            set { this["enableHtmlCompression"] = value; }
        }
        [ConfigurationProperty("enableScriptMinification", IsRequired = true)]
        public bool EnableScriptMinification
        {
            get { return (bool)this["enableScriptMinification"]; }
            set { this["enableScriptMinification"] = value; }
        }
        [ConfigurationProperty("enableHtmlMinification", IsRequired = true)]
        public bool EnableHtmlMinification
        {
            get { return (bool)this["enableHtmlMinification"]; }
            set { this["enableHtmlMinification"] = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ScriptCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ScriptElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScriptElement)element).Key;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ScriptElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)base["key"]; }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
            set { base["assembly"] = value; }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }

        #region method added to make this solution .net 2.0 complient
        public string GetContents()
        {
            string contents = string.Empty;
            try
            {
                if (this.Path.Length > 0)
                {
                    if (OptimizerHelper.IsAbsolutePathExists(this))
                    {
                        string path = OptimizerHelper.GetAbsolutePath(this);
                        using (StreamReader reader = new StreamReader(path))
                        {
                            contents = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                }
                else if (this.Assembly.Length > 0 && this.Name.Length > 0)
                {
                    using (StreamReader reader = new StreamReader(System.Reflection.Assembly.Load(this.Assembly).GetManifestResourceStream(this.Name)))
                    {
                        contents = reader.ReadToEnd();
                        reader.Close();
                    }
                }
            }
            catch (Exception ex) { }
            return contents;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class OptimizerConfig
    {
        protected static Dictionary<string, ScriptElement> _scripts;
        protected static bool _enable;
        protected static bool _enableProfiler;
        protected static bool _enableScriptCompression;
        protected static bool _enableHtmlCompression;
        protected static bool _enableScriptMinification;
        protected static bool _enableHtmlMinification;

        /// <summary>
        /// 
        /// </summary>
        static OptimizerConfig()
        {
            _scripts = new Dictionary<string, ScriptElement>();
            OptimizerSection sec = null;
            try
            {
                //Configuration config = GetConfig(Assembly.GetAssembly(typeof(OptimizerSection)));
                sec = (OptimizerSection)System.Configuration.ConfigurationManager.GetSection("optimizerSection");
                //if (config != null) sec = config.GetSection("optimizerSection") as OptimizerSection;
                //config = null;

                foreach (ScriptElement i in sec.Scripts)
                {
                    _scripts.Add(i.Key, i);
                }
                _enable = sec.Enable;
                _enableProfiler = sec.EnableProfiler;
                _enableScriptCompression = sec.EnableScriptCompression;
                _enableHtmlCompression = sec.EnableHtmlCompression;
                _enableScriptMinification = sec.EnableScriptMinification;
                _enableHtmlMinification = sec.EnableHtmlMinification;
            }
            catch { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ScriptElement GetScriptByKey(string key)
        {
            ScriptElement objElement = null;
            try
            {
                objElement = _scripts[key];
            }
            catch { }
            return objElement;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static ScriptElement GetScriptByResource(string name, string assembly)
        {
            ScriptElement objElement = null;
            foreach (KeyValuePair<string, ScriptElement> element in _scripts)
            {
                if (element.Value.Name == name && element.Value.Assembly == assembly)
                {
                    objElement = element.Value;
                    break;
                }
            }
            return objElement;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ScriptElement GetScriptByPath(string path)
        {
            ScriptElement objElement = null;
            foreach (KeyValuePair<string, ScriptElement> element in _scripts)
            {
                if (element.Value.Path == path)
                {
                    objElement = element.Value;
                    break;
                }
            }
            return objElement;
        }
        /// <summary>
        /// This function is mainly put here to support looking up reserved names such as MicrosoftAjax.js and MicrosoftAjaxWebForms.js
        /// otherwise it's not recommended to lookup by name!!!
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ScriptElement GetScriptByName(string name)
        {
            ScriptElement objElement = null;
            foreach (KeyValuePair<string, ScriptElement> element in _scripts)
            {
                if (element.Value.Name == name)
                {
                    objElement = element.Value;
                    break;
                }
            }
            return objElement;
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool Enable
        {
            get { return _enable; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool EnableProfiler
        {
            get { return _enableProfiler; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool EnableScriptCompression
        {
            get { return _enableScriptCompression; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool EnableHtmlCompression
        {
            get { return _enableHtmlCompression; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool EnableScriptMinification
        {
            get { return _enableScriptMinification; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool EnableHtmlMinification
        {
            get { return _enableHtmlMinification; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class OptimizerHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(ScriptElement element)
        {
            string _absolutePath = string.Empty;

            if (null != element)
            {
                if (!element.Path.StartsWith("~", StringComparison.OrdinalIgnoreCase) && !element.Path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                    _absolutePath = System.Web.HttpContext.Current.Server.MapPath("~/" + element.Path);
                else
                    _absolutePath = System.Web.HttpContext.Current.Server.MapPath(element.Path);
            }

            return _absolutePath;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsAbsolutePathExists(ScriptElement element)
        {
            return System.IO.File.Exists(GetAbsolutePath(element));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAbsolutePathExists(string path)
        {
            return System.IO.File.Exists(path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool IsValidExtension(ScriptElement element, string extension)
        {
            if (null == element || string.IsNullOrEmpty(element.Path)) return false;

            string path = element.Path;
            int index = 0;
            if ((index = path.IndexOf('?')) != -1) path = path.Remove(index);
            string ext = System.IO.Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext)) return false;
            else return string.Equals(ext, extension, StringComparison.OrdinalIgnoreCase);
        }
    }
}