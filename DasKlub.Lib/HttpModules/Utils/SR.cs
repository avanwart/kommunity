//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================

using System;
using System.Globalization;

namespace Miron.Web.MbCompression
{
    internal static class SR
    {
        internal static string GetString(string strString)
        {
            return strString;
        }
        internal static string GetString(string strString, string param1)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1);
        }
        internal const string CREDIT_STRING = "/* This file was compressed using MbCompression library. http://blog.mironabramson.com */ ";

        internal const string WebResourceCompressionModule_InvalidRequest = "This is an invalid webresource request.";
        internal const string WebResourceCompressionModule_AssemblyNotFound = "Assembly {0} not found.";
        internal const string WebResourceCompressionModule_ResourceNotFound = "Resource {0} not found in assembly.";
        internal const string WebResourceCompressionModule_ReflectionNotAllowd = "Your server does not allow using reflection from your code. (Method: System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters not allowed) Add the attribute 'reflectionAlloweded=\"false\"' to your web.config in the CompressorSettings section.";
        internal const string WebResourceCompressionModule_MachineKeyMissing = "Because your server does not support reflection or you set the attribute 'reflectionAlloweded=\"false\"', You must specify a non-autogenerated machine key in your web.config to compress Webresource.axd";
        internal const string File_FileNotFound = "/* The requested file '{0}' was not found */";
        internal const string ICachingStorage_Not_Valid = "Caching storage not valid. Must be one of the following: FileSystem or OutputCache";
        internal const string CompressionNotSupported = "The current request not support any encoding";
    }
}
