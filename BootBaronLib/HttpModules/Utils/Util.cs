///////////////////////////////////////////////////////////////////////
//                                   Util                            //
//             Written by: Miron Abramson. Date: 04-10-07            //
//                    Last updated: 06-09-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using

using System;
using System.Web;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Web.UI;
using System.Collections.Generic;

using System.Configuration;
using System.Text;
using System.Web.Configuration;
#endregion

namespace Miron.Web.MbCompression
{
    internal class Util
    {
        private static MethodInfo _decryptString;
        private static readonly Object _getMethodLock = new Object();
        private static readonly Dictionary<string, sbyte> _compressibleTypes = new Dictionary<string, sbyte>(5, StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, sbyte> _UIPageTypes = new Dictionary<string, sbyte>(2,StringComparer.OrdinalIgnoreCase);


        static Util()
        {
            if (_compressibleTypes.Count < 1)
            {
                _compressibleTypes.Add("text/css", 0);
                _compressibleTypes.Add("application/x-javascript", 0);
                _compressibleTypes.Add("text/javascript", 0);
                _compressibleTypes.Add("text/html", 0);
                _compressibleTypes.Add("text/plain", 0);
            }
            if (_UIPageTypes.Count < 1)
            {
                _UIPageTypes.Add("text/html", 0);
                _UIPageTypes.Add("text/plain", 0);
            }
        }


        /// <summary>
        /// Check if the current request is an AsyncCall by MS-AJAX framework
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsMsAjaxRequest(HttpContext context)
        {
            return context.Request.Headers["X-MicrosoftAjax"] != null;
        }

        /// <summary>
        /// Get the current "System.Web.Extensions" assembly version.
        /// </summary>
        /// <returns><para>0.0 - the assembly was not loaded</para>
        /// <para>bigger that 0.0 - the assembly version that loaded</para></returns>
        internal static double GetMsAjaxVersion()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName.StartsWith("System.Web.Extensions", StringComparison.Ordinal))
                {
                    int versionIndex = (asm.FullName.IndexOf("Version=", StringComparison.OrdinalIgnoreCase));
                    // Not suppose to happen, but just to be sure..
                    if (versionIndex < 0)
                    {
                        return 0.0;
                    }
                    versionIndex += "Version=".Length;
                    int commaIndex = (asm.FullName.IndexOf(',', versionIndex));
                    string ver = asm.FullName.Substring(versionIndex, commaIndex - versionIndex);
                    while (ver.IndexOf('.') < ver.LastIndexOf('.'))
                    {
                        ver = ver.Remove(ver.LastIndexOf('.'), 1);
                    }
                    return Convert.ToDouble(ver);
                }
            }
            return 0.0;
        }


        /// <summary>
        /// Check if a specific content type is compressible
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsContentTypeCompressible(string contentType)
        {
            return _compressibleTypes.ContainsKey(contentType);
        }

        /// <summary>
        /// Check if the specified content type is System.Web.UI.Page
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsUIPageContentType(string contentType)
        {
            return _UIPageTypes.ContainsKey(contentType);
        }

        /// <summary>
        /// Decript a string using MachineKey
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [
        ReflectionPermission(SecurityAction.Assert, Unrestricted = true),
        SecurityCritical,
        SecurityTreatAsSafe
        ]
        internal static string DecryptString(string input)
        {
            if (Settings.Instance.ReflectionAlloweded)
            {
                try
                {
                    if (_decryptString == null)
                    {
                        lock (_getMethodLock)
                        {
                            if (_decryptString == null)
                            {
                                _decryptString = typeof(Page).GetMethod("DecryptString", BindingFlags.Static | BindingFlags.NonPublic);
                            }
                        }
                    }
                    return (string)_decryptString.Invoke(null, new object[] { input });
                }
                // Reflection is not allowded!
                catch (MethodAccessException)
                {
                    Settings.Instance.ReflectionAlloweded = false;
                    return EmptyMembership.Instance.DecryptString(input);
                }
                catch (TargetInvocationException)
                {
                    Settings.Instance.ReflectionAlloweded = false;
                    return EmptyMembership.Instance.DecryptString(input);
                }
            }
            else
            {
                return EmptyMembership.Instance.DecryptString(input);
            }
        }



        /// <summary>
        /// copy one stream to another
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        internal static void StreamCopy(Stream input, Stream output)
        {
            byte[] buffer = new byte[1024];
            int read;
            do
            {
                read = input.Read(buffer, 0, buffer.Length);
                output.Write(buffer, 0, read);
            } while (read > 0);
        }



        /// <summary>
        /// Combine two hash codes (From class: 'HashCodeCombiner' in the assembly: 'System.Web.Util')
        /// </summary>
        /// <param name="hash1"></param>
        /// <param name="hash2"></param>
        /// <returns></returns>
        internal static int CombineHashCodes(int hash1, int hash2)
        {
            if (hash2 == 0)
                return hash1;
            return (((hash1 << 5) + hash1) ^ hash2);
        }

        /// <summary>
        /// Get the file name from a path string
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static string GetFileName(string url)
        {
            return System.IO.Path.GetFileName(url);
        }


        /// <summary>
        /// Get the extension of the specified filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string GetFileExtension(string fileName)
        {
            return System.IO.Path.GetExtension(fileName);
        }

        /// <summary>
        /// Convert string to byte[]
        /// <para>Faster than the built-in method, and prevent encoding problems</para>
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static byte[] StringToBytes(string value)
        {
            int length = value.Length;
            byte[] resultBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                resultBytes[i] = (byte)value[i];
            }
            return resultBytes;
        }



        /// <summary>
        /// Get the current folder of the current request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static string GetCurrentPath(HttpContext context)
        {
            int index = context.Request.CurrentExecutionFilePath.LastIndexOf('/');
            if (index > -1)
            {
                return context.Request.CurrentExecutionFilePath.Substring(0, index + 1);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Encode giben string for using in the querystring
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }
    }
}

