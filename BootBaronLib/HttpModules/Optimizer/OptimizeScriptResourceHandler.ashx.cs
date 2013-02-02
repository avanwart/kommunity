using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Configuration;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.Handlers;
using AdShoreLib.AspNetPerformanceOptimizer.ConfigurationSections;

namespace AdShoreLib.AspNetPerformanceOptimizer.ScriptOptimizer
{
    public class OptimizeScriptResourceHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }
        public void ProcessRequest(HttpContext context)
        {
            bool shouldProcessRequest = true;
            string[] scriptKeys = null;
            string keys = context.Server.UrlDecode(context.Request.Params["keys"]);
            string scriptResourcePath = String.Empty;
            ScriptManager objScriptManager = new ScriptManager();
            StringBuilder scriptBuilder = new StringBuilder();
            IHttpHandler handler = new ScriptResourceHandler();
            ///StringCollection _gescHdrStatus = new StringCollection();

            if (String.IsNullOrEmpty(keys) || keys.Equals("-1")) shouldProcessRequest = false;

            if (shouldProcessRequest)
            {
                Dictionary<string, string> _scriptContents = null;
                try
                {
                    _scriptContents = (Dictionary<string, string>)HttpContext.Current.Cache["ScriptContents"];
                }
                catch (Exception ex) { }

                scriptKeys = keys.Split('.');
                ///_gescHdrStatus.Add("incount:" + scripts.Length.ToString()); //script count
                scriptResourcePath = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", context.Request.Url.Scheme, "://", context.Request.Url.Host, ":", context.Request.Url.Port, "/", context.Request.ApplicationPath, "/ScriptResource.axd");
                scriptResourcePath = scriptResourcePath.Replace("///", "/");
                foreach (string key in scriptKeys)
                {
                    ScriptElement element = OptimizerConfig.GetScriptByKey(key);
                    if (element == null) continue;

                    #region Generating resource url dynamically and creating WebRequest object to extract stream of script

                    if (element != null)
                    {
                        ScriptReference reference = null;
                        bool isPathBased = false;
                        if (element.Path.Length > 0)
                        {
                            reference = new ScriptReference(element.Path);
                            isPathBased = true;
                        }
                        else if (element.Assembly.Length > 0 && element.Name.Length > 0)
                        {
                            reference = new ScriptReference(element.Name, element.Assembly);
                        }
                        try
                        {
                            OptimizeScriptReference openReference = new OptimizeScriptReference(reference);
                            string url = string.Empty;
                            if (!isPathBased)
                            {
                                url = context.Request.Url.OriginalString.Replace(context.Request.RawUrl, "") + openReference.GetUrl(objScriptManager);
                                var queryStringIndex = url.IndexOf('?');
                                var queryString = url.Substring(queryStringIndex + 1);
                                var request = new HttpRequest("scriptresource.axd", scriptResourcePath, queryString);

                                using (StringWriter textWriter = new StringWriter(scriptBuilder))
                                {
                                    try
                                    {
                                        //HttpResponse response = new HttpResponse(textWriter);
                                        //HttpContext ctx = new HttpContext(request, response);
                                        //handler.ProcessRequest(ctx); //raising error in .Net 2.0
                                        if (_scriptContents.ContainsKey(key))
                                        {
                                            scriptBuilder.Append(_scriptContents[key]);
                                        }
                                    }
                                    catch (Exception ctxEx)
                                    {
                                    }
                                }
                            }
                            else
                            {
                                string absolutePath = OptimizerHelper.GetAbsolutePath(element);
                                if (OptimizerHelper.IsAbsolutePathExists(absolutePath))
                                {
                                    using (StreamReader objJsReader = new StreamReader(absolutePath, true))
                                    {
                                        scriptBuilder.Append(objJsReader.ReadToEnd());
                                    }
                                }
                            }
                            scriptBuilder.AppendLine();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    #endregion
                }
            }

            objScriptManager = null;

            #region Writing combine output scripts to the Response.OutputStream


            context.Response.Clear();
            context.Response.ContentType = "application/x-javascript";

            try
            {
                SetResponseCache(context.Response);
                scriptBuilder.AppendLine();
                //scriptBuilder.AppendLine("if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();");
                string combinedScripts = scriptBuilder.ToString();

                if (shouldProcessRequest)
                {
                    if (OptimizerConfig.EnableScriptMinification)
                    {
                        combinedScripts = JsMinifier.GetMinifiedCode(combinedScripts);
                    }

                    string encodingTypes = string.Empty;
                    string compressionType = "none";
                    if (OptimizerConfig.EnableScriptCompression)
                    {
                        encodingTypes = context.Request.Headers["Accept-Encoding"];

                        if (!string.IsNullOrEmpty(encodingTypes))
                        {
                            encodingTypes = encodingTypes.ToLower();
                            if (context.Request.Browser.Browser == "IE")
                            {
                                if (context.Request.Browser.MajorVersion < 6)
                                    compressionType = "none";
                                else if (context.Request.Browser.MajorVersion == 6 && !string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_USER_AGENT"]) && context.Request.ServerVariables["HTTP_USER_AGENT"].Contains("EV1"))
                                    compressionType = "none";
                            }
                            if ((encodingTypes.Contains("gzip") || encodingTypes.Contains("x-gzip") || encodingTypes.Contains("*")))
                                compressionType = "gzip";
                            else if (encodingTypes.Contains("deflate"))
                                compressionType = "deflate";
                        }
                    }
                    else
                    {
                        compressionType = "none";
                    }
                    if (compressionType == "gzip")
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            using (StreamWriter writer = new StreamWriter(new GZipStream(stream, CompressionMode.Compress), Encoding.UTF8))
                            {
                                writer.Write(combinedScripts);
                            }
                            byte[] buffer = stream.ToArray();
                            context.Response.AddHeader("Content-encoding", "gzip");
                            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else if (compressionType == "deflate")
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            using (StreamWriter writer = new StreamWriter(new DeflateStream(stream, CompressionMode.Compress), Encoding.UTF8))
                            {
                                writer.Write(combinedScripts);
                            }
                            byte[] buffer = stream.ToArray();
                            context.Response.AddHeader("Content-encoding", "deflate");
                            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        //no compression plain text...
                        context.Response.AddHeader("Content-Length", combinedScripts.Length.ToString());
                        context.Response.Write(combinedScripts);
                    }
                }
                scriptBuilder = null;
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.ToString().Replace("\n", "<br>"));
            }
            #endregion
        }
        private static void SetResponseCache(HttpResponse response)
        {
            HttpCachePolicy cache = response.Cache;
            DateTime _now = DateTime.Now;
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByParams["keys"] = true;
            cache.SetOmitVaryStar(true);
            cache.SetExpires(_now + TimeSpan.FromDays(365.0));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(_now);
        }
    }
}