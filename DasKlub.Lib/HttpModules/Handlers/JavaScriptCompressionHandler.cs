//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================

#region Using

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Caching;

#endregion

namespace Miron.Web.MbCompression
{
    /// <summary>
    /// 
    /// </summary>
    public class JavaScriptCompressionHandler : CompressionHandlerBase
    {
       
        public override void ProcessRequest(HttpContext context)
        {
            if (context == null || context.Request == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(context.Request.QueryString["d"]))
            {
                context.Response.ContentType = "text/javascript";
                base.ProcessRequest(context);
            }
        }


        /// <summary>
        /// Strips the whitespace from any .js file.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override string Minify(StreamReader reader)
        {
            if (Settings.Instance.MinifyContent)
            {
                JavaScriptMinifier _Minifier = new JavaScriptMinifier();
                return _Minifier.Minify(reader);
            }
            else
            {
                return base.Minify(reader);
            }
        }

        /// <summary>
        /// Verify that the requested file is a javascript file
        /// </summary>
        /// <param name="file"></param>
        protected override void VerifyPermission(string file)
        {
            if (!file.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
            {
                throw new System.IO.FileLoadException("File type error");
            }
        }

        /// <summary>
        /// Determinate if the current response content should be compressed
        /// </summary>
        /// <returns></returns>
        protected override bool IsCompressContent()
        {
            return Settings.Instance.CompressJavaScript;
        }
    }
}