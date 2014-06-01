using System;
using System.Data;
using System.Text.RegularExpressions;

namespace DasKlub.Lib.Operational
{
    /// <summary>
    ///     Converts objects to various types
    /// </summary>
    public static class FromObj
    {
        public static char CharFromObj(object value)
        {
            char rslt;

            return char.TryParse(Convert.ToString(value), out rslt) ? rslt : char.MinValue;
        }

        public static DateTime? DateNullableFromObj(object p)
        {
            if (p == null || p == DBNull.Value) return null;
            return Convert.ToDateTime(p);
        }

        #region Public Static Methods

        /// <summary>
        ///     Take an object and make it a string, useful for null db values
        /// </summary>
        /// <returns></returns>
        public static string StringFromObj(object value)
        {
            if (value != DBNull.Value && value != null)
                return Convert.ToString(value);
            return string.Empty;
        }

        /// <summary>
        ///     Convert the object to a date from the database and set it to 1/1/1900
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime DateFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToDateTime(value);
            return DateTime.MinValue;
        }

        /// <summary>
        ///     Convert an object to an integer from the database
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IntFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToInt32(value);
            return 0;
        }

        public static decimal? DecimalNullableFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToDecimal(value);
            if (value == DBNull.Value)
                return null;
            return 0;
        }

        public static int? IntNullableFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToInt32(value);
            if (value == DBNull.Value)
                return null;
            return 0;
        }

        /// <summary>
        ///     Convert the object to a boolean if possible, false otherwise
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool BoolFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToBoolean(value);
            return false;
        }

        /// <summary>
        ///     Convert an object to double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double DoubleFromObj(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            double dbl;

            return double.TryParse(Convert.ToString(value), out dbl) ? dbl : 0;
        }

        public static float FloatFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return (float) Convert.ToDouble(value);
            return 0;
        }

        /// <summary>
        ///     Convert an object to a decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal DecimalFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToDecimal(value);
            return 0;
        }

        #endregion
    }

    public static class FromDataRow
    {
        internal static int IntFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.IntFromObj(dr[columnName]);
            return 0;
        }

        internal static DateTime DateTimeFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.DateFromObj(dr[columnName]);
            return DateTime.MinValue;
        }
    }

    /// <summary>
    ///     Convert from a string for the database
    /// </summary>
    public static class FromString
    {
        #region public static methods

        public static string QuoteSafe(string input)
        {
            return input.Replace("\"", @"'").Replace("“", "'").Replace("”", "'");
        }

        public static string ReplaceNewLineSingleWithHtml(string s)
        {
            return s.Replace("\n", "<br />");
        }

        public static string ReplaceNewLineWithHtml(string s)
        {
            return s.Replace(Environment.NewLine, "<br />");
        }


        /// <summary>
        ///     Given a string of text and length return a string padded and left aligned with white space
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFixedLengthString(string input, int length)
        {
            return GetFixedLengthString(input, length, " ", true);
        }


        /// <summary>
        ///     Get a substring of the first N characters.
        ///     If the length specified is not a space, continue to loop until it is and then append ...
        ///     at the end (this makes sure the end is a word and not just some part of the word).
        ///     If the string is shorter then the length specified, just return it.
        /// </summary>
        public static string Truncate(string source, int length)
        {
            if (source.Length <= length) return source;
            if (source.Length > length && source.Substring(length, 1) != string.Empty)
            {
                while (length < source.Length && source.Substring(length, 1) != " ")
                {
                    length++;
                }
                source = string.Format("{0}...", source.Substring(0, length));
            }
            else
            {
                source = source.Substring(0, length);
            }
            return source;
        }

        public static string SeoText(string input, int maxlength)
        {
            if (input.Length < maxlength) return input;

            var myString = input.Substring(0, maxlength);
            var index = myString.LastIndexOf(' ');
            var outputString = myString.Substring(0, index);

            return outputString;
        }

        #region fixed length string

        /// <summary>
        ///     Given a string of text, length and fill type, return a string padded and aligned specified
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="fillWith"></param>
        /// <param name="isLeftAligned"></param>
        /// <returns></returns>
        private static string GetFixedLengthString(string input, int length, string fillWith, bool isLeftAligned)
        {
            input = input ?? fillWith;
 
            input = input.Length > length ? input.Substring(0, length) : input;

            return isLeftAligned ? 
                string.Format("{0,-" + length + "}", input).Replace(" ", fillWith) : 
                string.Format("{0," + length + "}", input).Replace(" ", fillWith);
        }

        #endregion

        #endregion

        /// <summary>
        ///     Gets a URL accessible dash spaced string
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string UrlKey(string p)
        {
            var pname = Regex.Replace(p, @"[\W_-[#]]+", " ");
            return pname.Trim().Replace("  ", " ").Replace(" ", "-").Replace("%", string.Empty).ToLower();
        }
    }

    /// <summary>
    ///     Convert from a date for the database
    /// </summary>
    public static class FromDate
    {
        /// <summary>
        ///     YYYY-MM-DD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateToYYYY_MM_DD(DateTime value)
        {
            return value == DateTime.MinValue ? string.Empty : string.Format("{0:yyyy-MM-dd}", value);
        }
    }
}