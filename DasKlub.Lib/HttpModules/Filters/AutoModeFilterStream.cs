///////////////////////////////////////////////////////////////////////
//                PrepareScriptsToComnpressionStream                 //
//             Written by: Miron Abramson. Date: 20-07-08            //
//   Parse the response, and convert any injected javascript include //
//   to format that can be parsed and compress by the js compressor  //
//                    Last updated: 27-08-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Globalization;
using System.Diagnostics;
#endregion

namespace Miron.Web.MbCompression
{
    internal class AutoModeFilterStream : Stream
    {
        private static readonly Regex REGEX_SCRIPT = new Regex("<script\\s*[^<]*src=\"((?=(?!http:|https:)[^\"]*)[^\"]*)\"\\s*[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase);
        private static readonly Regex REGEX_STYLE = new Regex("<link\\s*[^<]*href=\"((?=(?!http:|https:)[^\"]*)[^\"]*)\"\\s*[^>]*>[^<]*(?:>)?", RegexOptions.IgnoreCase);

        public AutoModeFilterStream(Stream stream,Encoding currentEncoding,bool processScripts,bool processStyles)
        {
            _baseStream = stream;
            _currentEncoding = currentEncoding;
            _processScripts = processScripts;
            _processStyles = processStyles;
        }

        private Stream _baseStream;
        private Encoding _currentEncoding;
        private bool _processScripts;
        private bool _processStyles;

        #region Properites

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        private long _position;
        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion

        #region Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Close()
        {
            _baseStream.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!_processStyles && !_processScripts)
            {
                _baseStream.Write(buffer, offset, count);
                return;
            }

            string html = _currentEncoding.GetString(buffer, offset, count);

            if (_processScripts)
            {
                html = FixScriptsUrl(html);
            }
            if (_processStyles)
            {
                html = FixStylesUrl(html);
            }

            byte[] finalBuffer = _currentEncoding.GetBytes(html);

            _baseStream.Write(finalBuffer, 0, finalBuffer.Length);
        }

        #region Fix style urls
        /// <summary>
        /// Fix the styles url that it can be compressed
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string FixStylesUrl(string html)
        {
            return REGEX_STYLE.Replace(html, new MatchEvaluator(StyleFound));
        }

        /// <summary>
        /// Find the source
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string StyleFound(Match m)
        {
            if (m.Groups[1].Value.IndexOf(".axd", StringComparison.OrdinalIgnoreCase) > -1 ||
                m.Value.IndexOf("rel=\"stylesheet\"",StringComparison.OrdinalIgnoreCase) < 1 )
            {
                return m.Value;
            }
            int index = m.Groups[1].Value.LastIndexOf('/');
            if (index >= 0)
            {
                index = m.Groups[1].Index - m.Index + index + 1;
                return m.Value.Insert(index, "css.axd?d=").Insert(m.Groups[1].Index - m.Index + m.Groups[1].Length + 10, Settings.Instance.CssVersion);
            }
            else
            {
                index = m.Groups[1].Index - m.Index;
                return m.Value.Insert(index, "css.axd?d=").Insert(index + m.Groups[1].Length + 10, Settings.Instance.CssVersion);
            }
        }

        #endregion

        #region Fix script urls

        /// <summary>
        /// Fix the scripts url that it can be compressed
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string FixScriptsUrl(string html)
        {
            return REGEX_SCRIPT.Replace(html, new MatchEvaluator(ScriptFound));
        }

        /// <summary>
        /// Find the source
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string ScriptFound(Match m)
        {
            if (m.Groups[1].Value.IndexOf(".axd", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return m.Value;
            }
            int index = m.Groups[1].Value.LastIndexOf('/');
            if (index >= 0)
            {
                index = m.Groups[1].Index - m.Index + index + 1;
                return m.Value.Insert(index, "jslib.axd?d=").Insert(m.Groups[1].Index - m.Index + m.Groups[1].Length + 12, Settings.Instance.JsVersion);
            }
            else
            {
                index = m.Groups[1].Index - m.Index;
                return m.Value.Insert(index, "jslib.axd?d=").Insert(index + m.Groups[1].Length + 12, Settings.Instance.JsVersion);
            }       
        }
        #endregion

        #endregion
    }
}
