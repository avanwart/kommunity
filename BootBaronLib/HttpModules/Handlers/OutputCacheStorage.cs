//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================

#region Using
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
#endregion

namespace Miron.Web.MbCompression
{
	internal class OutputCacheStorage : ICachingStorage
	{
        /// <summary>
        /// Excute the request and response the needes content
        /// <para>Save the response in the Output cach for next requests</para>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="absoluteFiles"></param>
        /// <param name="minify"></param>
        /// <param name="versionHash"></param>
        /// <param name="encodingMgr"></param>
        public void Excute(HttpContext context, string[] absoluteFiles, Minifier minify, int versionHash, EncodingManager encodingMgr)
        {
            context.Response.Write(SR.GetString(SR.CREDIT_STRING));

            List<string> exsitingFiles = new List<string>();
            for (int i = 0; i < absoluteFiles.Length; i++)
            {
                WriteContent(context, GetFileContent(absoluteFiles[i], minify, exsitingFiles));
            }
            SetResponseCacheSettings(context, exsitingFiles.ToArray());
            if (encodingMgr.IsEncodingEnabled)
            {
                encodingMgr.CompressResponse();
                encodingMgr.SetResponseEncodingType();
            }
        }

        /// <summary>
        /// Writes the given content to the response stream.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content"></param>
        private void WriteContent(HttpContext context, string content)
        {
            context.Response.Write(content + Environment.NewLine);
        }

        /// <summary>
        /// Set the headers for the response for the cache
        /// </summary>
        /// <param name="context"></param>
        /// <param name="files"></param>
        public void SetResponseCacheSettings(HttpContext context, string[] files)
        {
            context.Response.AddFileDependencies(files);
            context.Response.Cache.SetLastModifiedFromFileDependencies();
        }

        /// <summary>
        /// Get the content of the specified file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="minify"></param>
        /// <param name="exsitingFiles"></param>
        /// <returns></returns>
        private string GetFileContent(string fileName, Minifier minify, List<string> exsitingFiles)
        {
            string content = string.Empty;
            if (fileName.StartsWith("NOT_FOUND", StringComparison.Ordinal))
            {
                content = string.Format(SR.File_FileNotFound, Path.GetFileName(fileName.Split('|')[1]));
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    content = minify(reader);
                }
                exsitingFiles.Add(fileName);
            }
            return content;
        }
	}
}
