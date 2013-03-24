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
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BootBaronLib.Operational
{
    public class XMLUtil
    {
        public XMLUtil()
        {
            //
            // TODO: Add constructor logic here
            //
        }
 
        #region XML
        /// <summary>
        /// An XML string representation of this object
        /// </summary>
        public static string XMLString(object objk)
        {
            // Prepare XML serializer
            XmlSerializer serializer = new XmlSerializer(objk.GetType());

            // Serialize into StringBuilder
            StringBuilder sb = new StringBuilder();
            
            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, objk);
                sw.Flush();
            }
            
            // Replace UTF-16 encoding with UTF-8 encoding
            string xml = sb.ToString();
            xml = xml.Replace("utf-16", "utf-8");
            return xml;
        }



        public static string XMLString(object objk, bool removeXMLheadings)
        {

            if (!removeXMLheadings) return XMLString(objk);

            // Prepare XML serializer
            XmlSerializer serializer = new XmlSerializer(objk.GetType());

            // Serialize into StringBuilder
            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, objk);
                sw.Flush();
            }

            // Replace UTF-16 encoding with UTF-8 encoding
            string xml = sb.ToString();
            // replace and remove all the default strings that are placed in the heading
            xml = xml.Replace("utf-16", "utf-8");
            xml = xml.Replace(@"<?xml version=""1.0"" encoding=""utf-8""?>" + Environment.NewLine, string.Empty);
            xml = xml.Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", string.Empty);
           
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
            XmlDocument doc = new XmlDocument();

            try
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);
                ser.Serialize(writer, obj);
                doc.LoadXml(sb.ToString());
            }
            catch (Exception ex)
            {

                Utilities.LogError("EXCEPTION, FAILED TO CONVERT OBJECT TO XML: ", ex);
            }

            return doc;
        }

        /// <summary>
        /// Post XML outbound
        /// </summary>
        /// <param name="xmlToPost"></param>
        /// <param name="urlToPostTo"></param>
        /// <returns></returns>
        public static string PostXML(string xmlToPost, Uri urlToPostTo)
        {
            // Configure HTTP Request
            HttpWebRequest httpRequest = WebRequest.Create(urlToPostTo) as HttpWebRequest;  // use the defualt URL
            httpRequest.Method = "POST";

            // Prepare correct encoding for XML serialization
            UTF8Encoding encoding = new UTF8Encoding();

            // Use Xml property to obtain serialized XML data 
            // Convert into bytes using encoding specified above and get length
            byte[] bodyBytes = encoding.GetBytes(xmlToPost);
            httpRequest.ContentLength = bodyBytes.Length;

            Stream httpRequestBodyStream = null;

            try
            {
                // Get HTTP Request stream for putting XML data into
                httpRequestBodyStream = httpRequest.GetRequestStream();

                // Fill stream with serialized XML data
                httpRequestBodyStream.Write(bodyBytes, 0, bodyBytes.Length);
                httpRequestBodyStream.Close();

                // Get HTTP Response
                HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;

                StreamReader httpResponseStream =
                  new StreamReader(httpResponse.GetResponseStream(), Encoding.ASCII);

                // Extract XML from response
                string httpResponseBody = httpResponseStream.ReadToEnd();
                httpResponseStream.Close();

                if (string.IsNullOrEmpty(httpResponseBody)) return string.Empty; // nothing in the response

                // Ignore everything that isn't XML by removing headers
                httpResponseBody = httpResponseBody.Substring(httpResponseBody.IndexOf("<?xml"));

                httpResponseStream.Dispose();

                return httpResponseBody;
            }
            catch (WebException ex)
            {
                Utilities.LogError("WebException WITH XML POST: ", ex);
                return string.Empty;
            }
            catch (ProtocolViolationException ex)
            {
                Utilities.LogError("ProtocolViolationException WITH XML POST: ", ex);
                return string.Empty;
            }
            catch (Exception ex)
            {
                Utilities.LogError("Exception WITH XML POST: ", ex);
                return string.Empty;
            }
            finally
            {
                httpRequestBodyStream.Dispose();
            }
        }

 



        /// <summary>
        /// Take a URL and a string that is XML and post it, return the response
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xmlToConvert"></param>
        /// <returns></returns>
        public static string PostXML(string url, string xmlToConvert)
        {
 

            // Configure HTTP Request
            HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
            httpRequest.Method = "POST";

            // Prepare correct encoding for XML serialization
            UTF8Encoding encoding = new UTF8Encoding();

            // Use Xml property to obtain serialized XML data
            // Convert into bytes using encoding specified above and
            // get length
            byte[] bodyBytes = encoding.GetBytes(xmlToConvert);
            httpRequest.ContentLength = bodyBytes.Length;

            try
            {
                // Get HTTP Request stream for putting XML data into
                Stream httpRequestBodyStream = httpRequest.GetRequestStream();

                // Fill stream with serialized XML data
                httpRequestBodyStream.Write(bodyBytes, 0, bodyBytes.Length);
                httpRequestBodyStream.Close();

                // Get HTTP Response
                HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;
                StreamReader httpResponseStream =
                    new StreamReader(httpResponse.GetResponseStream(),
                  System.Text.Encoding.ASCII);

                // Extract XML from response
                string httpResponseBody = httpResponseStream.ReadToEnd();
                httpResponseStream.Close();

                if (string.IsNullOrEmpty(httpResponseBody)) return string.Empty;

                // Ignore everything that isn't XML by removing headers
                httpResponseBody = httpResponseBody.Substring(httpResponseBody.IndexOf("<?xml"));

                //   Deserialize XML into DataCashResponse
                StringReader responseReader = new StringReader(httpResponseBody);
                return responseReader.ToString();
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
                return string.Empty;
            }

        }


        /// <summary>
        /// Sends an xml document over http, and returns the xml server response
        /// </summary
        /// <param name="xDoc"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static XmlDocument PostXML(XmlDocument xDoc, string URL)
        {
            XmlDocument xresp = null;

            try
            {
                // get the data from the xml document into a byte stream
                Byte[] bdata = Encoding.ASCII.GetBytes(xDoc.OuterXml);
                // instantiate a web client
                using (WebClient wc = new WebClient())
                {
                    Byte[] bresp;

                    // add appropriate headers
                    wc.Headers.Add(HttpRequestHeader.ContentType, "text/xml");
                    // send data to server, and wait for a response
                    bresp = wc.UploadData(URL, bdata);

                    // read the response
                    string resp = Encoding.ASCII.GetString(bresp);
                    xresp = new XmlDocument();
                    xresp.LoadXml(resp);
                    // return the xml document response from the server

                    if (string.IsNullOrEmpty(xresp.InnerText))
                    {
                        Utilities.LogError("XML RESPONSE: NO INNER TEXT", false);
                    }
                }
                return xresp;
            }
            catch (Exception ex)
            {
                Utilities.LogError("ERROR WITH XML: " + ex.ToString(), true);
                xDoc = new XmlDocument();
                return xresp;
            }
        }




        #endregion
    }
}