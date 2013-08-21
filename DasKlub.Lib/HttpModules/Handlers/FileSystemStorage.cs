//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================

using System;
using System.Web;
using System.IO;
using System.Text;

namespace Miron.Web.MbCompression
{
	internal class FilesystemStorage : ICachingStorage
	{
        private const string cReferencesCache = "ReferencesCache\\";
        private static readonly object _syncObject = new object();
        
        /// <summary>
        /// Excute the request and response the needes content
        /// <para>Save the response in the file system for next requests</para>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="absoluteFiles"></param>
        /// <param name="minify"></param>
        /// <param name="versionHash"></param>
        /// <param name="encodingMgr"></param>
        public void Excute(HttpContext context, string[] absoluteFiles, Minifier minify, int versionHash, EncodingManager encodingMgr)
        {
            string phisicalRoot = context.Server.MapPath("~/") + cReferencesCache;

            string fullPath = string.Empty;
            if (encodingMgr.IsEncodingEnabled)
            {
                fullPath = phisicalRoot + "comp_" + encodingMgr.PreferredEncoding + "_" + versionHash + (minify.Target is JavaScriptCompressionHandler ? ".js" : ".css");
                encodingMgr.SetResponseEncodingType();
            }
            else
            {
                fullPath = phisicalRoot + versionHash + (minify.Target is JavaScriptCompressionHandler ? ".js" : ".css");
            }

            SetResponseCacheSettings(context, null);

            if (!File.Exists(fullPath))
            {
                lock (_syncObject)
                {
                    if (!File.Exists(fullPath))
                    {
                        if (!Directory.Exists(phisicalRoot))
                        {
                            Directory.CreateDirectory(phisicalRoot);
                        }
                        if (encodingMgr.IsEncodingEnabled)
                        {
                            StringBuilder contentSb = new StringBuilder(SR.GetString(SR.CREDIT_STRING));
                            foreach (string fileName in absoluteFiles)
                            {
                                if (fileName.StartsWith("NOT_FOUND",StringComparison.Ordinal))
                                {
                                    contentSb.AppendFormat(SR.File_FileNotFound + Environment.NewLine, Path.GetFileName(fileName.Split('|')[1]));
                                }
                                else
                                {
                                    using (StreamReader reader = new StreamReader(fileName))
                                    {
                                        contentSb.AppendLine(minify(reader));
                                    }
                                }
                            }

                            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                            {
                                byte[] compressedContent = encodingMgr.CompressString( contentSb.ToString() );
                                fs.Write(compressedContent, 0, compressedContent.Length);
                            }
                            // Release the StringBuilder
                            contentSb.Length = 0;
                        }
                        else
                        {
                            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                string content = SR.GetString(SR.CREDIT_STRING);
                                sw.WriteLine(content);
                                foreach (string fileName in absoluteFiles)
                                {
                                    if (fileName == string.Empty)
                                    {
                                        content = string.Format(SR.File_FileNotFound, Path.GetFileName(fileName));
                                    }
                                    else
                                    {
                                        using (StreamReader reader = new StreamReader(fileName))
                                        {
                                            content = minify(reader);
                                        }
                                    }
                                    sw.WriteLine(content);
                                }
                            }
                        }
                    }
                }
            }
           
            context.Response.TransmitFile(fullPath);
            return;
        }

        /// <summary>
        /// Set the headers for the response for the cache
        /// </summary>
        /// <param name="context"></param>
        /// <param name="files"></param>
        public void SetResponseCacheSettings(HttpContext context, string[] files)
        {
            // Cache anywhere but not in the server
            context.Response.Cache.SetNoServerCaching();
        }
	}
}
