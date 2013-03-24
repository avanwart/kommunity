//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
 
using BootBaronLib.Enums;



namespace BootBaronLib.Operational.Converters
{

    /// <summary>
    /// Exports a gridview to an excel file
    /// </summary>
    public class FromGridView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="gv"></param>
        public static void Export(string fileName, GridView gv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a table to contain the grid
                    Table table = new Table();

                    //  include the gridline settings
                    table.GridLines = gv.GridLines;

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        FromGridView.PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        FromGridView.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        FromGridView.PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        /// <summary>
        /// Replace any of the contained controls with literals
        /// </summary>
        /// <param name="control"></param>
        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    FromGridView.PrepareControlForExport(current);
                }
            }
        }
    }


    /// <summary>
    /// <para>Various extension methods.</para>
    /// </summary>
    /// Sample of using ToCSV
    /// <example>
    /// DataTable table = dv.Table;
    /// // Assumes table is a DataTable
    /// string result = table.ToCSV(true);
    /// System.IO.File.WriteAllText(@"C:\sample.csv", result);
    /// System.Diagnostics.Process proc = new System.Diagnostics.Process();
    /// proc.StartInfo.FileName = @"C:\sample.csv";
    /// proc.StartInfo.UseShellExecute = true;
    /// proc.Start();
    /// </example>
    public static class FromDataTable
    {
        /// <summary>
        /// Convert the datatable to a CSV that shows up in the response, downloadable as an excel file
        /// </summary>
        /// <param name="table"></param>
        public static void ResponseToCSV(this DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", Guid.NewGuid() + ".csv"));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            //  render the htmlwriter into the response
            HttpContext.Current.Response.Write(FromDataTable.ToCSV(table));
            HttpContext.Current.Response.End();

        }

        /// <summary>
        /// Converts the passed in data table to a CSV-style string.      
        /// </summary>
        /// <param name="table">Table to convert</param>
        /// <returns>Resulting CSV-style string</returns>
        public static string ToCSV(this DataTable table)
        {

            return ToCSV(table, ",", true);

        }

        /// <summary>

        /// Converts the passed in data table to a CSV-style string.

        /// </summary>

        /// <param name="table">Table to convert</param>

        /// <param name="includeHeader">true - include headers<br/>

        /// false - do not include header column</param>

        /// <returns>Resulting CSV-style string</returns>
        public static string ToCSV(this DataTable table, bool includeHeader)
        {
            return ToCSV(table, ",", includeHeader);

        }



        /// <summary>
        /// Converts the passed in data table to a CSV-style string.
        /// </summary>
        /// <param name="table">Table to convert</param>
        /// <param name="delimiter">Delimiter used to separate fields</param>
        /// <param name="includeHeader">true - include headers<br/>
        /// false - do not include header column</param>
        /// <returns>Resulting CSV-style string</returns>
        public static string ToCSV(this DataTable table, string delimiter, bool includeHeader)
        {
            StringBuilder result = new StringBuilder();

            if (includeHeader)
            {
                foreach (DataColumn column in table.Columns)
                {
                    result.Append(column.ColumnName);
                    result.Append(delimiter);
                }

                result.Remove(--result.Length, 0);
                result.Append(Environment.NewLine);
            }

            foreach (DataRow row in table.Rows)
            {
                foreach (object item in row.ItemArray)
                {
                    if (item is System.DBNull)
                        result.Append(delimiter);
                    else
                    {
                        string itemAsString = item.ToString();
                        // Double up all embedded double quotes
                        itemAsString = itemAsString.Replace("\"", "\"\"");

                        // To keep things simple, always delimit with double-quotes
                        // so we don't have to determine in which cases they're necessary
                        // and which cases they're not.
                        itemAsString = "\"" + itemAsString + "\"";
                        result.Append(itemAsString + delimiter);
                    }
                }

                result.Remove(--result.Length, 0);
                result.Append(Environment.NewLine);
            }
            return result.ToString();

        }

    }


    /// <summary>
    /// Converts objects to various types
    /// </summary>
    public class FromObj
    {
        private T Call<T>(Uri url, SiteEnums.HTTPTypes methodType) where T : class
        {
            T result;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = methodType.ToString();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                string jsonData = reader.ReadToEnd();
                result = (T)jsSerializer.Deserialize<T>(jsonData);
            }

            return result;
        }


        #region Public Static Methods


       

        /// <summary>
        /// An XML string representation of this object
        /// </summary>
        public static string XMLString(object objk)
        {

            // Prepare XML serializer
            XmlSerializer serializer = new XmlSerializer(objk.GetType());

            // Serialize into StringBuilder
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, objk);
            sw.Flush();

            // Replace UTF-16 encoding with UTF-8 encoding
            string xml = sb.ToString();
            xml = xml.Replace("utf-16", "utf-8");
            return xml;

        }


        /// <summary>
        /// Convert the object to an xml document
        /// </summary>
        /// <param name="obj"></param>
        /// <see cref=">http://geekswithblogs.net/TimH/archive/2006/02/09/68857.aspx"/>
        /// <returns></returns>
        public static XmlDocument ConvertObjectToXML(object obj)
        {
            //XmlDocument doc = new XmlDocument();
            
            //try
            //{
            //    // NOTE: for some reason aka: http://social.msdn.microsoft.com/Forums/en-US/asmxandxml/thread/9f0c169f-c45e-4898-b2c4-f72c816d4b55/ 
            //    //  there is an exception thrown but just ignore it because it apparently doesn't matter
            //    XmlSerializer ser = new XmlSerializer(obj.GetType());
            //    StringBuilder sb = new StringBuilder();
            //    StringWriter writer = new StringWriter(sb);

            //    ser.Serialize(writer, obj);
            //    doc.LoadXml(sb.ToString());
            //}
            //catch (Exception ex)
            //{
            //    Utilities.LogError("EXCEPTION, FAILED TO CONVERT OBJECT TO XML: ", ex);
            //}

            //return doc;

            return ConvertObjectToXML(obj, false);
        }



        public static XmlDocument   ConvertObjectToXML(object obj, bool removeXMLDeclarationAndNameSpace)
        {
            if (obj == null) return null;

            XmlDocument doc = new XmlDocument();

            try
            {
                // NOTE: for some reason aka: http://social.msdn.microsoft.com/Forums/en-US/asmxandxml/thread/9f0c169f-c45e-4898-b2c4-f72c816d4b55/ 
                //  there is an exception thrown but just ignore it because it apparently doesn't matter

                XmlSerializer ser = new XmlSerializer(obj.GetType());
                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);

                ser.Serialize(writer, obj);

                // you must do this because SQL says: XML parsing: ... unable to switch the encoding
              //  doc.LoadXml(sb.ToString().Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", @"<?xml version=""1.0"" encoding=""UTF-8""?>"));

                if (!removeXMLDeclarationAndNameSpace)
                {
                    doc.LoadXml(sb.ToString());
                }
                else
                {
                    // either replace it or make it UTF-8
                    doc.LoadXml(sb.ToString()
                        .Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", string.Empty)
                        .Replace(@"xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", string.Empty)); // remove namespace
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError("EXCEPTION, FAILED TO CONVERT OBJECT TO XML: ", ex);
            }

            return doc;
        }


        /// <summary>
        /// Take an object and make it a string, useful for null db values
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string StringFromObj(object value)
        {
            if (value != DBNull.Value && value != null)
                return Convert.ToString(value);
            else
                return string.Empty;
        }

    

        /// <summary>
        /// Take object, return guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid GuidFromObject(object value)
        {
            Guid g = Guid.Empty;

            if (Utilities.IsGuid(Convert.ToString(value)))
            {
                g = new Guid(Convert.ToString(value));
            }

            return g;

        }

        /// <summary>
        /// Convert the object to a date from the database and set it to 1/1/1900
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DateTime DateFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToDateTime(value);
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// Convert an object to an integer from the database
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int IntFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToInt32(value);
            else
                return 0;
        }


        public static decimal? DecimalNullableFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToDecimal(value);
            else if (value == DBNull.Value)
                return null;
            else
                return 0;
        }


        public static int? IntNullableFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToInt32(value);
            else if (value == DBNull.Value)
                return null;
            else
                return 0;
        }



        public static long LongFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToInt64(value);
            else
                return 0;
        }

        /// <summary>
        /// Convert the object to a boolean if possible, false otherwise
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool BoolFromObj(object value)
        {

            if (value != null && value != DBNull.Value)
                return Convert.ToBoolean(value);
            else
                return false;

        }

        /// <summary>
        /// Convert an object to double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double DoubleFromObj(object value)
        {

            if (value != null && value != DBNull.Value)
            {
                double dbl = 0;

                if (double.TryParse(Convert.ToString(value), out dbl))
                {
                    return dbl;
                }
            }

            return 0;
        }




        public static float FloatFromObj(object value)
        {

            if (value != null && value != DBNull.Value)
                return (float) Convert.ToDouble(value);
            else
                return 0;

        }

        /// <summary>
        /// Convert an object to a decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal DecimalFromObj(object value)
        {
            if (value != null && value != DBNull.Value)
                return Convert.ToDecimal(value);
            else
                return 0;
        }

        /// <summary>
        /// Convert object to string and then to XML
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlDocument XMLFromObj(object value)
        {
            if (value != null  && value != DBNull.Value)
            {
                XmlDocument xd = new XmlDocument();
                try
                {
                    xd.LoadXml(Convert.ToString(value));
                }
                catch 
                {
                    return null;
                }
                return xd;
            }
            else
                return null;

        }

        #endregion

        public static char CharFromObj(object value)
        {
            char rslt = char.MinValue;

            if (char.TryParse(Convert.ToString(value), out rslt))
                return rslt;
            else return char.MinValue;
        }

        public static DateTime? DateNullableFromObj(object p)
        {
            if (p == null || p == DBNull.Value) return null;
            else return Convert.ToDateTime(p);
        }
    }

    public class FromDataRow
    {
        internal static string StringFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.StringFromObj(dr[columnName]);
            else 
                return string.Empty;
        }

        internal static bool BoolFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.BoolFromObj(dr[columnName]);
            else
                return false;
        }

        internal static int IntFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.IntFromObj(dr[columnName]);
            else
                return 0;
        }

        internal static decimal DecimalFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.DecimalFromObj(dr[columnName]);
            else
                return 0;
        }

        internal static double DoubleFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.DoubleFromObj(dr[columnName]);
            else
                return 0;
        }

        internal static DateTime DateTimeFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.DateFromObj(dr[columnName]);
            else
                return DateTime.MinValue;
        }

        internal static Guid GuidFromDataRow(DataRow dr, string columnName)
        {
            if (dr != null && dr.Table.Columns.Contains(columnName))
                return FromObj.GuidFromObject(dr[columnName]);
            else
                return Guid.Empty;
        }
    }

    /// <summary>
    /// Convert from a string for the database
    /// </summary>
    public class FromString
    {

        #region public static methods


        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }


        public static string EncodeJsString(string s)
        {
            s = s.Replace("\r\n", "\n");

            return Regex.Replace(s, "(['\"\\\\])", @"\$1");
        }

        public static string EncodeStringFromJs(string s)
        {
            //"this is the best confirm\\'s change asdfasdf"
            return s.Replace("\\'", "'").Replace("\n", "\r\n");
        }



        public static string ReplaceNewLineSingleWithHTML(string s)
        {
            return s.Replace("\n", "<br />");
        }

        public static string ReplaceNewLineWithHTML(string s)
        {
            return s.Replace("\r\n", "<br />");
        }


        public static string ReplaceHTMLWithNewLine(string s)
        {
            return s.Replace("<br />", "\r\n");
        }
  


        /// <summary>
        /// Given the string, only get back at most the max length of characters
        /// </summary>
        /// <param name="input"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetMaxLength(string input, int maxLength)
        {
            if (input.Length > maxLength)
            {
                return input.Substring(0, maxLength);
            }
            else return input;
        }


        /// <summary>
        /// Replaces all non-alphanumeric characters with an empty string, leaves white space though
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveNonAlphaNumeric(string input)
        {
            //return Regex.Replace(input, @"[\W]", string.Empty); removes white space

            return Regex.Replace(input, @"\W(?<!\s)", string.Empty);
        }

        #region fixed length string

        /// <summary>
        /// Given a string of text, length and fill type, return a string padded and aligned specified
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="fillWith"></param>
        /// <returns></returns>
        public static string GetFixedLengthString(string input, int length, string fillWith, bool isLeftAligned)
        {
            input = input ?? fillWith; // what? http://stackoverflow.com/questions/446835/what-do-two-question-marks-together-mean-in-c
            input = input.Length > length ? input.Substring(0, length) : input;

            if (isLeftAligned)
                return string.Format("{0,-" + length + "}", input).Replace(" ", fillWith); // align the string to the left
            else
                return string.Format("{0," + length + "}", input).Replace(" ", fillWith); // align the string to the right
        }

        /// <summary>
        /// Given a string of text, length and fill type, return a string padded and left aligned
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="fillWith"></param>
        /// <returns></returns>
        public static string GetFixedLengthString(string input, int length, string fillWith)
        {
            return GetFixedLengthString(input, length, fillWith, true);
        }

        #endregion

        /// <summary>
        /// Given a string of text and length return a string padded and left aligned with white space 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFixedLengthString(string input, int length)
        {
            return GetFixedLengthString(input, length, " ");
        }


        /// <summary>
        /// Convert a string to a type of object, useful for converting 
        /// XML responses to object instances (note: requires
        /// casting after the object has been returned as in: (myclassname)result)
        /// </summary>
        /// <param name="str">The string to convert to an object from XML string (note:
        /// this is used mostly for responses from a web server where the response
        /// is a string)</param>
        /// <param name="typ">The typeof object, example: typeof(myclassname)</param>
        /// <returns></returns>
        public static object ConvertXMLStringToObject(string str, Type typ)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typ);

                StringReader sr = new StringReader(str);

                if (sr != null)
                    return serializer.Deserialize(sr);
                else
                    return null;
            }
            catch (Exception ex)
            {
                Utilities.LogError("EXCEPTION WITH STRING TO OBJECT DESERIALIZATION FOR TYPE: " + typ.ToString(), ex);
                return null;
            }
        }


        /// <summary>
        /// Convert a string to an XML document
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlDocument StringToXML(string value)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(value);
                return xd;
            }
            catch
            {
                return null;
            }

        }

 


        /// <summary>
        /// Make a string have the 1st letter uppercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UpperCaseFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))

                return string.Empty;
            return char.ToUpper(s[0]) + s.Substring(1);

        }

        /// <summary>
        /// Get a substring of the first N characters.
        /// If the length specified is not a space, continue to loop until it is and then append ...
        /// at the end (this makes sure the end is a word and not just some part of the word).  
        /// If the string is shorter then the length specified, just return it.
        /// </summary>
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {

                if (source.Length > length && source.Substring(length, 1) != string.Empty)
                {
                    while (length < source.Length && source.Substring(length, 1) != " ")
                    {
                        length++;
                    }
                    source = source.Substring(0, length) + "..."; ;
                }
                else
                {
                    source = source.Substring(0, length);
                }
            }
            return source;
        }

        /// <summary>
        /// Process string for database
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object StringToDBNull(string value)
        {
            if (string.IsNullOrEmpty(value)) return DBNull.Value;
            if (value.Trim() == string.Empty)
            { return DBNull.Value; }
            else return value.Trim();
        }

        /// <summary>
        /// Return a space if it's empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringToSpace(string value)
        {
            if (value == string.Empty)
            {
                return string.Empty;
            }
            else return value;

        }

        /// <summary>
        /// Turn a string into a decimal value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal StringToDecimal(string value)
        {
            if (value == string.Empty)
            {
                return 0;
            }
            else return Convert.ToDecimal(value);
        }

        /// <summary>
        /// Turn a string into a int value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int StringToInt(string value)
        {
            if (value == string.Empty)
            {
                return 0;
            }
            else return Convert.ToInt32(value);
        }




        public static string StringToBase64String(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            byte[] ToByte = System.Text.Encoding.Default.GetBytes(input);
            return System.Convert.ToBase64String(ToByte);
        }


        public static string StringFromBase64String(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            byte[] b = Convert.FromBase64String(input);
            return (System.Text.Encoding.UTF8.GetString(b));

            // return  Convert.FromBase64String(input);
        }

        #endregion

        public static string XMLUnicode(string p)
        {
            string input = HttpUtility.HtmlEncode( p );
            StringBuilder output = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '&')
                {
                    int startOfEntity = i; // just for easier reading
                    int endOfEntity = input.IndexOf(';', startOfEntity);
                    string entity = input.Substring(startOfEntity, endOfEntity - startOfEntity);
                    int unicodeNumber = (int)(HttpUtility.HtmlDecode(entity)[0]);
                    output.Append("&#" + unicodeNumber + ";");
                    i = endOfEntity; // continue parsing after the end of the entity
                }
                else
                    output.Append(input[i]);
            }
            return output.ToString();
        }

        /// <summary>
        /// Gets a URL accessible dash spaced string 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string URLKey(string p)
        {
            string pname = Regex.Replace(p, @"[\W_-[#]]+", " ");
            return pname.Trim().Replace("  ", " ").Replace(" ", "-").Replace("%", string.Empty).ToLower(); // TODO: FULL REGEX
        }
    }

    /// <summary>
    /// Convert from a date for the database
    /// </summary>
    public class FromDate
    {



        /// <summary>
        /// Convert the date to a user friendly display
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateToString(DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                return string.Empty;
            }
            else return value.ToShortDateString();
        }

        /// <summary>
        /// If the date is the min
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object DateToDBNull(DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                return DBNull.Value;
            }
            else return value;
        }

        /// <summary>
        /// Returns the MMDDYY of the date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateToMMDDYY(DateTime value)
        {
            if (value == DateTime.MinValue)
                return string.Empty;
            else
                return string.Format("{0:MMddyy}", value);

        }

        /// <summary>
        /// YYYYMMDD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateToYYYYMMDD(DateTime value)
        {
            if (value == DateTime.MinValue)
                return string.Empty;
            else
                return string.Format("{0:yyyyMMdd}", value);
        }

        /// <summary>
        /// YYYY-MM-DD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateToYYYY_MM_DD(DateTime value)
        {
            if (value == DateTime.MinValue)
                return string.Empty;
            else
                return string.Format("{0:yyyy-MM-dd}", value);

        }


    }

    /// <summary>
    /// From Decimal to database
    /// </summary>
    public class FromDecimal
    {
        /// <summary>
        /// If it's null, set to db null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object DecimalToDBNull(decimal value)
        {
            if (value == 0)
            {
                return DBNull.Value;
            }
            else return value;
        }

        public static string DecimalToStandardPercisionString(decimal value)
        {
            return String.Format("{0:0.00}", value);
        }

        /// <summary>
        /// Return an empty string if it's 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecimalToEmptyString(decimal value)
        {
            if (value == 0)
            {
                return string.Empty;
            }
            else return value.ToString();
        }


        /// <summary>
        /// Return an empty string if it's 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DecimalToFormattedCurrency(decimal value, SiteEnums.CurrencyTypes curType)
        {
            switch (curType)
            {
                case SiteEnums.CurrencyTypes.CAD:
                case SiteEnums.CurrencyTypes.USD:
                case SiteEnums.CurrencyTypes.AUD:
                case SiteEnums.CurrencyTypes.MXN:
                case SiteEnums.CurrencyTypes.BRL:
                case SiteEnums.CurrencyTypes.EUR:
                case SiteEnums.CurrencyTypes.GBP:
                    return Utilities.GetEnumDescription(curType) + String.Format("{0:0.00}", value);
                default:
                    return String.Format("{0:0.00}", value);
            }
        }


    }

    /// <summary>
    /// Convert to user friendly True or False
    /// </summary>
    public class FromBool
    {
        /// <summary>
        /// Convert boolan to a human-friendly message
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BoolToMessage(bool value)
        {
            if (value == true)
                return "Yes";
            else
                return "No";
        }
    }

    /// <summary>
    /// Int for the database
    /// </summary>
    public class FromInt
    {
        /// <summary>
        /// If it's null, set to db null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object IntToDBNull(int value)
        {
            if (value == 0)
            {
                return DBNull.Value;
            }
            else return value;
        }

        /// <summary>
        /// Return an empty string if it's 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntToEmptyString(int value)
        {
            if (value == 0)
            {
                return string.Empty;
            }
            else return value.ToString();
        }

    }

    public class FromXML
    {
        /// <summary>
        /// Turn XML to JSON
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        /// <see cref=">http://www.phdcc.com/xml2json.htm"/>
        public  static string XmlToJSON(XmlDocument xmlDoc)
        {
            StringBuilder sbJSON = new StringBuilder();
            sbJSON.Append("{ ");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
            sbJSON.Append("}");
            return sbJSON.ToString();
        }


        /// <summary>
        ///  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array
        /// </summary>
        /// <param name="sbJSON"></param>
        /// <param name="node"></param>
        /// <param name="showNodeName"></param>
        /// <see cref=">http://www.phdcc.com/xml2json.htm"/>
        private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            try
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
                sbJSON.Append("{");
                // Build a sorted list of key-value pairs
                //  where   key is case-sensitive nodeName
                //          value is an ArrayList of string or XmlElement
                //  so that we know whether the nodeName is an array or not.
                SortedList childNodeNames = new SortedList();

                //  Add in all node attributes
                if (node.Attributes != null)
                    foreach (XmlAttribute attr in node.Attributes)
                        StoreChildNode(childNodeNames, attr.Name, attr.InnerText);

                //  Add in all nodes
                foreach (XmlNode cnode in node.ChildNodes)
                {
                    if (cnode is XmlText)
                        StoreChildNode(childNodeNames, "value", cnode.InnerText);
                    else if (cnode is XmlElement)
                        StoreChildNode(childNodeNames, cnode.Name, cnode);
                }

                // Now output all stored info
                foreach (string childname in childNodeNames.Keys)
                {
                    ArrayList alChild = (ArrayList)childNodeNames[childname];
                    if (alChild.Count == 1)
                        OutputNode(childname, alChild[0], sbJSON, true);
                    else
                    {
                        sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                        foreach (object Child in alChild)
                            OutputNode(childname, Child, sbJSON, false);
                        sbJSON.Remove(sbJSON.Length - 2, 2);
                        sbJSON.Append(" ], ");
                    }
                }
                sbJSON.Remove(sbJSON.Length - 2, 2);
                sbJSON.Append(" }");
            }
            catch { return; }
        }


        /// <summary>
        /// StoreChildNode: Store data associated with each nodeName 
        /// so that we know whether the nodeName is an array or not.
        /// </summary>
        /// <param name="childNodeNames"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeValue"></param>
        private static void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
        {
            // Pre-process contraction of XmlElement-s
            if (nodeValue is XmlElement)
            {
                // Convert  <aa></aa> into "aa":null
                //          <aa>xx</aa> into "aa":"xx"
                XmlNode cnode = (XmlNode)nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    XmlNodeList children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
            // Add nodeValue to ArrayList associated with each nodeName
            // If nodeName doesn't exist then add it
            object oValuesAL = childNodeNames[nodeName];
            ArrayList ValuesAL;
            if (oValuesAL == null)
            {
                ValuesAL = new ArrayList();
                childNodeNames[nodeName] = ValuesAL;
            }
            else
                ValuesAL = (ArrayList)oValuesAL;
            ValuesAL.Add(nodeValue);
        }


        private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                string sChild = (string)alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
                XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
            sbJSON.Append(", ");
        }


        /// <summary>
        /// Make a string safe for JSON
        /// </summary>
        /// <param name="sIn"></param>
        /// <returns></returns>
        private static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }


    }

    public class FromDouble
    {

        /// <summary>
        /// Return an empty string if it's 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DoubleToFormattedCurrency(double value, SiteEnums.CurrencyTypes curType)
        {
            switch (curType)
            {
                case SiteEnums.CurrencyTypes.CAD:
                case SiteEnums.CurrencyTypes.USD:
                case SiteEnums.CurrencyTypes.AUD:
                    return "$" + String.Format("{0:0.00}", value);
                case SiteEnums.CurrencyTypes.EUR:
                    return "€" + String.Format("{0:0,00}", value);
                case SiteEnums.CurrencyTypes.GBP:
                    return "£" + String.Format("{0:0.00}", value);
                default:
                    return string.Empty;
            }
        }
    }


    /// <summary>
    ///  http://pietschsoft.com/post/2008/02/NET-35-JSON-Serialization-using-the-DataContractJsonSerializer.aspx
    /// </summary>
    public class FromJSON
    {


        public static string Serialize<T>(T obj)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.Default.GetString(ms.ToArray());
            ms.Dispose();
            return retVal;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }
    }

    public class FromGUID
    {

        public static string FromGUIDToOrangeGUID(Guid input)
        {
            return input.ToString().Replace("-", "").ToUpper();

        }
    }

}