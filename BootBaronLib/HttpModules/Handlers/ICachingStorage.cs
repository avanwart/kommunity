//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================

using System;
using System.Web;
using System.IO;

namespace Miron.Web.MbCompression
{
    public delegate string Minifier(StreamReader stream);

	internal interface ICachingStorage
	{
        void Excute(HttpContext context,
            string[] filesInfo,
            Minifier minify,
            int versionHash,
            EncodingManager encodingMgr);

        void SetResponseCacheSettings(HttpContext context, string[] files);
	}
}
