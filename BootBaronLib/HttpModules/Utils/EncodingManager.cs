//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================

#region
using System;
using System.Collections.Generic;
using System.Web;
using System.IO.Compression;
using System.IO;
using System.Text;
#endregion

namespace Miron.Web.MbCompression
{
    public sealed class EncodingManager
    {
        public enum EncodingType
        {
            None = 0,
            Gzip = 1,
            Deflate = 2
        }

        private HttpContext _currentContext;
        public EncodingManager(HttpContext context)
        {
            _currentContext = context;
            if( !string.IsNullOrEmpty(context.Request.Headers["Accept-encoding"]))
            {
                 RequestHeader = context.Request.Headers["Accept-encoding"].Replace(" ",string.Empty);
            }
            ParsePreferrdedEncoding();
        }

        #region Properties

        private string _requestHeader = string.Empty;
        public string RequestHeader
        {
            get { return _requestHeader; }
            private set
            { 
                _requestHeader = value;
            }
        }
        
        EncodingType _preferredEncodingType = EncodingType.None;
        public EncodingType PreferredEncodingType
        {
            get { return _preferredEncodingType; }
            set
            { 
                _preferredEncodingType = value;
                switch(value)
                {
                    case EncodingType.None:
                        _preferredEncoding = string.Empty;
                        break;
                    case EncodingType.Gzip:
                        _preferredEncoding = "gzip";
                        break;
                    case EncodingType.Deflate:
                        _preferredEncoding ="deflate";
                        break;
                }
            }
        }

        private string _preferredEncoding = string.Empty;
        public string PreferredEncoding
        {
            get { return _preferredEncoding; }
            private set { _preferredEncoding = value; }
        }
        public bool IsEncodingEnabled
        {
            get { return PreferredEncodingType != EncodingType.None; }
        }
        #endregion

        #region Parsing methods methods
        /// <summary>
        /// Check if the browser support compression
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsCompressionSupported()
        {
            if (_currentContext.Request.Browser == null)
                return false;

            // Check if the browser is not IE. If it is not, it safe to say it support compression
            if (!_currentContext.Request.Browser.IsBrowser("IE"))
                return true;

            // If we are here, it means the client have IE. Sometimes IE have problems with proxys that using old protocol
            // 1.0, but still sending 'Accept-encoding' header as compressible.
            if (_currentContext.Request.Params["SERVER_PROTOCOL"] != null && _currentContext.Request.Params["SERVER_PROTOCOL"].Contains("1.1"))
                return true;

            return false;
        }


        /// <summary>
        /// Find the preferred encoding
        /// </summary>
        /// <returns></returns>
        private void ParsePreferrdedEncoding()
        {
            int gzipIndex = -1;
            int deflateIndex = -1;
            float gzipQValue = 0;
            float deflateQValue = 0;
            float starQValue = 0;
            float identityQValue = 0;

            if (string.IsNullOrEmpty(_requestHeader))
            {
                PreferredEncoding = string.Empty;
                _preferredEncodingType = EncodingType.None;
                return;
            }

            string[] headersParts = _requestHeader.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < headersParts.Length; i++)
            {
                if (gzipIndex < 0 && headersParts[i].Equals("gzip",StringComparison.OrdinalIgnoreCase))
                {
                    gzipIndex = i;
                    gzipQValue = ParseQValue(headersParts[i]);
                    // We found gzip QValue = 1, skip the rest...
                    if (gzipQValue == 1)
                    {
                        if (IsCompressionSupported())
                        {
                            PreferredEncoding = "gzip";
                            _preferredEncodingType = EncodingType.Gzip;
                        }
                        else
                        {
                            PreferredEncoding = string.Empty;
                            _preferredEncodingType = EncodingType.None;
                        }
                        return;
                    }
                }
                else if (deflateIndex < 0 && headersParts[i].Equals("deflate",StringComparison.OrdinalIgnoreCase))
                {
                    deflateIndex = i;
                    deflateQValue = ParseQValue(headersParts[i]);
                    // We found deflate QValue = 1, skip the rest...
                    if (deflateQValue == 1)
                    {
                        if (IsCompressionSupported())
                        {
                            PreferredEncoding = "deflate";
                            _preferredEncodingType = EncodingType.Deflate;
                        }
                        else
                        {
                            PreferredEncoding = string.Empty;
                            _preferredEncodingType = EncodingType.None;
                        }
                        return;
                    }
                }
                else if (headersParts[i].Equals("identity", StringComparison.OrdinalIgnoreCase))
                {
                    identityQValue = ParseQValue(headersParts[i]);
                }
                else if (headersParts[i].Equals("*", StringComparison.OrdinalIgnoreCase))
                {
                    starQValue = ParseQValue(headersParts[i]);
                }
            }

            // gzip if preferred
            if (gzipQValue > deflateQValue && gzipQValue > identityQValue)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = "gzip";
                    _preferredEncodingType = EncodingType.Gzip;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    _preferredEncodingType = EncodingType.None;
                }
                return;
            }
            // deflate is preferred
            else if (deflateQValue > gzipQValue && deflateQValue > identityQValue)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = "deflate";
                    _preferredEncodingType = EncodingType.Deflate;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    _preferredEncodingType = EncodingType.None;
                }
                return;
            }

            // identity (no compression) is preferred
            else if (identityQValue > gzipQValue && identityQValue > deflateQValue)
            {
                PreferredEncoding = string.Empty;
                _preferredEncodingType = EncodingType.None;
                return;
            }

            // They both have the same QValue that bogger than 0, so the preferred is the first one to apear
            else if (gzipQValue == deflateQValue && gzipQValue > 0)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = gzipIndex < deflateIndex ? "gzip" : "deflate";
                    _preferredEncodingType = gzipIndex < deflateIndex ? EncodingType.Gzip : EncodingType.Deflate;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    _preferredEncodingType = EncodingType.None;
                }
                return;
            }

            // Any encoding is accepteble. We will use deflate as the default
            else if (starQValue > 0)
            {
                if (IsCompressionSupported())
                {
                    PreferredEncoding = "deflate";
                    _preferredEncodingType = EncodingType.Deflate;
                }
                else
                {
                    PreferredEncoding = string.Empty;
                    _preferredEncodingType = EncodingType.None;
                }
                return;
            }
            // No encoding is supported
            else
            {
                PreferredEncoding = string.Empty;
                _preferredEncodingType = EncodingType.None;
                return;
            }
        }

        /// <summary>
        /// Get the QValue from the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static float ParseQValue(string value)
        {
            int qIndex = value.IndexOf("q=", StringComparison.OrdinalIgnoreCase);

            if (qIndex >= 0)
            {
                float result;
                if (float.TryParse(value.Substring(qIndex + 2), out result))
                {
                    return result;
                }
                return 0;
            }
            else
            {
                return 1;
            }
        }
        #endregion

        #region Compression methods
        /// <summary>
        /// Add a compression filter to the response
        /// </summary>
        public void CompressResponse()
        {
            if (_preferredEncodingType == EncodingType.Deflate)
            {
                _currentContext.Response.Filter = new DeflateStream(_currentContext.Response.Filter, CompressionMode.Compress);
            }
            else if (_preferredEncodingType == EncodingType.Gzip)
            {
                _currentContext.Response.Filter = new GZipStream(_currentContext.Response.Filter, CompressionMode.Compress);
            }
        }

        /// <summary>
        /// Compress a given string using the preffered algorithm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingType"></param>
        /// <returns></returns>
        public byte[] CompressString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            return CompressBytes(Util.StringToBytes(input));
        }

        /// <summary>
        /// Compress a given byte[] the preferred algorithm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] CompressBytes(byte[] buffer)
        {
            if (!IsEncodingEnabled)
            {
                throw new NotSupportedException(SR.GetString(SR.CompressionNotSupported));
            }
            if (buffer != null && buffer.Length > 0)
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    if (PreferredEncodingType == EncodingType.Gzip)
                    {
                        using (GZipStream compressStream = new GZipStream(memStream, CompressionMode.Compress))
                        {
                            compressStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        using (DeflateStream compressStream = new DeflateStream(memStream, CompressionMode.Compress))
                        {
                            compressStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    return memStream.ToArray();
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Set the encoding type for the current response
        /// </summary>
        public void SetResponseEncodingType()
        {
            if (IsEncodingEnabled)
            {
                _currentContext.Response.AppendHeader("Content-encoding", this.PreferredEncoding);
            }
        }  
    }
}
