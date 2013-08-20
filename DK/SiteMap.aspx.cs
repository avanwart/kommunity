//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com | http://dasklub.com

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
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;
using DasKlub.Lib.AppSpec.DasKlub.BOL;
using DasKlub.Lib.AppSpec.DasKlub.BOL.ArtistContent;
using DasKlub.Lib.AppSpec.DasKlub.BOL.UserContent;
using DasKlub.Web.Controllers;
using DasKlub.Web.Models;

namespace DasKlub.Web
{
    public partial class SiteMap : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";

            var writer = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);

            writer.WriteStartDocument();
            writer.WriteStartElement("urlset");
            writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            writer.WriteString("\r\n"); //newline 

            // home
            writer.WriteStartElement("url");
            writer.WriteElementString("loc", "http://dasklub.com/");
            writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", DateTime.UtcNow));
            writer.WriteElementString("changefreq", "weekly");
            writer.WriteElementString("priority", "1.0");
            writer.WriteEndElement();
            writer.WriteString("\r\n"); //newline 



            using (var context = new DasKlubDBContext())
            {

                var forumCategory = context.ForumCategory
                                           .OrderBy(x => x.CreateDate)
                                           .ToList();

                foreach (var category in forumCategory)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", category.ForumURL.ToString());
                    writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", category.CreateDate));
                    writer.WriteElementString("changefreq", "weekly");
                    writer.WriteElementString("priority", "0.8");
                    writer.WriteEndElement();
                    writer.WriteString("\r\n"); //newline 

                    var category1 = category;
                    var subForums = context.ForumSubCategory.Where(x => x.ForumCategoryID == category1.ForumCategoryID);

                    foreach (var forumPost in subForums)
                    {
                        writer.WriteStartElement("url");
                        writer.WriteElementString("loc", forumPost.SubForumURL.ToString());
                        writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", forumPost.CreateDate));
                        writer.WriteElementString("changefreq", "weekly");
                        writer.WriteElementString("priority", "0.8");
                        writer.WriteEndElement();
                        writer.WriteString("\r\n"); //newline 

                        var totalCount =
                            context.ForumPost.Count(x => x.ForumSubCategoryID == forumPost.ForumSubCategoryID);
                        var pageCount = (totalCount + ForumController.PageSize - 1) / ForumController.PageSize;
                        
                        if (pageCount <= 1) continue;

                        for (var i = 2; i <= pageCount; i++)
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", string.Format("{0}/{1}", forumPost.SubForumURL, i));
                            writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", forumPost.CreateDate));
                            writer.WriteElementString("changefreq", "weekly");
                            writer.WriteElementString("priority", "0.8");
                            writer.WriteEndElement();
                            writer.WriteString("\r\n"); //newline 
                        }
                    }
                }
            }

            var artis = new Artists();
            artis.GetAll();

            foreach (Artist app in artis)
            {
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "http://dasklub.com/" + app.URLOfArtist);
                writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", app.CreateDate));
                writer.WriteElementString("changefreq", "weekly");
                writer.WriteElementString("priority", "0.8");
                writer.WriteEndElement();
                writer.WriteString("\r\n"); //newline 
            }

            ArrayList userAccounts = Videos.GetDistinctUsers();

            foreach (string app in userAccounts)
            {
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "http://dasklub.com/" + app);
                writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", DateTime.UtcNow));
                writer.WriteElementString("changefreq", "weekly");
                writer.WriteElementString("priority", "0.7");
                writer.WriteEndElement();
                writer.WriteString("\r\n"); //newline 
            }

            // users
            var uas = new UserAccounts();
            uas.GetAll();

            foreach (UserAccount ua1 in uas)
            {
                //foreach (string a1 in userAccounts)
                //{
                //  if (a1 == ua1.UserName) continue;// already added as a video

                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "http://dasklub.com/" + ua1.UserName.Replace(" ", "-"));
                writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", ua1.LastActivityDate));
                writer.WriteElementString("changefreq", "weekly");
                writer.WriteElementString("priority", "0.7");
                writer.WriteEndElement();
                writer.WriteString("\r\n"); //newline 
                //}
            }


            /// content

            var cnts = new Contents();
            cnts.GetAllActiveContent();


            foreach (Content c1 in cnts)
            {
                //foreach (string a1 in userAccounts)
                //{
                //  if (a1 == ua1.UserName) continue;// already added as a video

                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "http://dasklub.com/news/" + c1.ContentKey);
                writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", c1.ReleaseDate));
                writer.WriteElementString("changefreq", "weekly");
                writer.WriteElementString("priority", "0.8");
                writer.WriteEndElement();
                writer.WriteString("\r\n"); //newline 
                //}
            }


            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            Response.End();
        }
    }
}