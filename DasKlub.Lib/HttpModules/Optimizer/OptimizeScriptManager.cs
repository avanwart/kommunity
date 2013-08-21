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
using System.IO;
using System.Reflection;
using System.Text;
using AdShoreLib.AspNetPerformanceOptimizer.ConfigurationSections;

namespace AdShoreLib.AspNetPerformanceOptimizer.ScriptOptimizer
{
    public class OptimizeScriptManager : ScriptManager
    {
        private const string HANDLER_PATH = "~/ClientScriptCombiner.aspx?keys=";
        private const string BLOCKED_HANDLER_PATH = HANDLER_PATH + "-1";
        private Dictionary<string, ScriptReference> _scripts = new Dictionary<string, ScriptReference>();
        private List<ScriptReference> _profilerScripts = null;
        private Dictionary<string, string> _scriptContents = new Dictionary<string, string>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (OptimizerConfig.EnableProfiler) _profilerScripts = new List<ScriptReference>();
        }
        protected override void OnResolveScriptReference(ScriptReferenceEventArgs e)
        {
            try
            {
                base.OnResolveScriptReference(e);

                #region Profiling scripts
                if (OptimizerConfig.EnableProfiler)
                {
                    bool isFound = false;
                    foreach (ScriptReference reference in _profilerScripts)
                    {
                        if (reference.Assembly == e.Script.Assembly && reference.Name == e.Script.Name && reference.Path == e.Script.Path)
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        ScriptReference objScrRef = new ScriptReference(e.Script.Name, e.Script.Assembly);
                        if (!string.IsNullOrEmpty(e.Script.Name) && string.IsNullOrEmpty(e.Script.Assembly))
                        {
                            //TODO: if resource belongs to System.Web.Extensions.dll, it does not provide assembly info that's why hard-coded assembly name is written to get it in profiler
                            objScrRef.Assembly = "System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
                        }
                        objScrRef.Path = e.Script.Path;
                        objScrRef.IgnoreScriptPath = e.Script.IgnoreScriptPath;
                        objScrRef.NotifyScriptLoaded = e.Script.NotifyScriptLoaded;
                        objScrRef.ResourceUICultures = e.Script.ResourceUICultures;
                        objScrRef.ScriptMode = e.Script.ScriptMode;
                        _profilerScripts.Add(objScrRef);
                        objScrRef = null;
                    }
                }
                #endregion

                #region Combining Client Scripts

                bool isAssemblyBased = ((e.Script.Assembly.Length > 0) ? true : false);
                bool isPathBased = ((e.Script.Path.Length > 0) ? true : false);
                bool isNameBased = ((e.Script.Path.Length == 0 && e.Script.Assembly.Length == 0 && e.Script.Name.Length > 0) ? true : false);

                if (OptimizerConfig.Enable && (isAssemblyBased || isPathBased || isNameBased))
                {
                    ScriptElement element = null;
                    try
                    {
                        if (isAssemblyBased)
                            element = OptimizerConfig.GetScriptByResource(e.Script.Name, e.Script.Assembly);
                        else if (isPathBased)
                        {
                            element = OptimizerConfig.GetScriptByPath(e.Script.Path);
                            if (null != element)
                            {
                                if (!OptimizerHelper.IsValidExtension(element, ".js"))
                                {
                                    element = null;
                                }
                                else if (!OptimizerHelper.IsAbsolutePathExists(element))
                                {
                                    string absolutePath = OptimizerHelper.GetAbsolutePath(element);
                                    element = null;
                                }
                            }
                        }
                        else if (isNameBased)
                            element = OptimizerConfig.GetScriptByName(e.Script.Name);
                    }
                    catch (Exception exc)
                    {
                        element = null;
                    }

                    if (element != null)
                    {
                        if (!_scriptContents.ContainsKey(element.Key))
                        {
                            _scriptContents.Add(element.Key, element.GetContents());
                        }
                        if (!_scripts.ContainsKey(element.Key))
                        {
                            try
                            {
                                _scripts.Add(element.Key, e.Script);
                                e.Script.Assembly = string.Empty;
                                e.Script.Name = string.Empty;

                                StringBuilder objStrBuilder = new StringBuilder();
                                objStrBuilder.Append(HANDLER_PATH);

                                foreach (KeyValuePair<string, ScriptReference> script in _scripts)
                                {
                                    objStrBuilder.Append(script.Key + ".");
                                }
                                string strPath = objStrBuilder.ToString();
                                objStrBuilder = null;

                                foreach (KeyValuePair<string, ScriptReference> script in _scripts)
                                {
                                    script.Value.Path = strPath;
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            e.Script.Assembly = string.Empty;
                            e.Script.Name = string.Empty;
                            e.Script.Path = BLOCKED_HANDLER_PATH;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Page.Response.Write(ex.ToString().Replace("\n", "<br>"));
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                HttpContext.Current.Cache.Insert("ScriptContents", _scriptContents);
                
                #region Writing profiled scripts on the browser
                if (OptimizerConfig.EnableProfiler && _profilerScripts != null)
                {
                    StringBuilder builder = new StringBuilder();
                    int index = 1;
                    foreach (ScriptReference script in _profilerScripts)
                    {
                        builder.Append("&lt;add key=\"" + index++ + "\" name=\"" + script.Name + "\" assembly=\"" + script.Assembly + "\" path=\"" + script.Path + "\" /&gt;<br>");
                    }
                    writer.WriteLine("<pre>");
                    writer.WriteLine(builder.ToString());
                    writer.WriteLine("</pre>");
                    builder = null;
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Page.Response.Write(ex.ToString().Replace("\n", "<br>"));
            }
            finally
            {
                base.Render(writer);
            }
        }
    }
}