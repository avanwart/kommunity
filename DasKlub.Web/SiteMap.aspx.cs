using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.ArtistContent;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Models;
using DasKlub.Web.Controllers;

namespace DasKlub.Web
{
    public partial class SiteMap : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var minWordCount = 300;
            var siteDomain = string.Format("{0}/", Lib.Configs.GeneralConfigs.SiteDomain);
            Response.Clear();
            Response.ContentType = "text/xml";

            var writer = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);

            writer.WriteStartDocument();
            writer.WriteStartElement("urlset");
            writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            writer.WriteString("\r\n"); //newline 

            // home
            writer.WriteStartElement("url");
            writer.WriteElementString("loc", siteDomain);
            writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", DateTime.UtcNow));
            writer.WriteElementString("changefreq", "daily");
            writer.WriteElementString("priority", "1.0");
            writer.WriteEndElement();
            writer.WriteString("\r\n"); //newline 

            // news
            var contents = new Contents();
            contents.GetContentPageWiseReleaseAll(1, 10000);

            foreach (var c1 in contents)
            {
                var text = c1.ContentDetail;

                if (text.Split(' ').Length < minWordCount) continue;

                writer.WriteStartElement("url");
                writer.WriteElementString("loc", string.Format("{0}news/{1}", siteDomain, c1.ContentKey.ToLower()));
                var lastmod = c1.ReleaseDate;
                if (c1.Comments != null && c1.Comments.Count > 0)
                {
                    lastmod = c1.Comments[c1.Comments.Count - 1].CreateDate;
                }
                writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", lastmod));
                writer.WriteElementString("changefreq", "monthly");
                writer.WriteElementString("priority", "0.8");
                writer.WriteEndElement();
                writer.WriteString("\r\n"); //newline 
            }

            using (var context = new DasKlubDbContext())
            {
                var forumCategory = context.ForumCategory
                                           .OrderBy(x => x.CreateDate)
                                           .ToList();

                foreach (var category in forumCategory)
                {
                    var category1 = category;
                    var subForums = context.ForumSubCategory.Where(x => x.ForumCategoryID == category1.ForumCategoryID);

                    using (var context2 = new DasKlubDbContext())
                    {
                        foreach (var thread in subForums)
                        {
                            var text = thread.Description;
                            var allPosts = context2.ForumPost.Where(x => x.ForumSubCategoryID == thread.ForumSubCategoryID);

                            var allPostText = new StringBuilder();

                            foreach(var item in allPosts)
                            {
                                allPostText.Append(item.Detail);
                            }

                            allPostText.Append(text);

                            if (allPostText.ToString().Split(' ').Length < minWordCount) { continue;  }

                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", thread.SubForumURL.ToString().ToLower());

                            var thread1 = thread;
                            var lastPost = context2.ForumPost
                                .Where(post => post.ForumSubCategoryID == thread1.ForumSubCategoryID)
                                .OrderByDescending(post => post.CreateDate).FirstOrDefault();

                            writer.WriteElementString("lastmod",
                                lastPost != null
                                    ? String.Format("{0:yyyy-MM-dd}", lastPost.CreateDate)
                                    : String.Format("{0:yyyy-MM-dd}", thread.CreateDate));
                            writer.WriteElementString("changefreq", "daily");
                            writer.WriteElementString("priority", "0.8");
                            writer.WriteEndElement();
                            writer.WriteString("\r\n"); //newline 

                            var totalCount =
                                context2.ForumPost.Count(x => x.ForumSubCategoryID == thread.ForumSubCategoryID);
                            var pageCount = (totalCount + ForumController.PageSizeForumPost - 1) / ForumController.PageSizeForumPost;

                            if (pageCount <= 1) continue;

                            for (var i = 2; i <= pageCount; i++)
                            {
                                writer.WriteStartElement("url");
                                writer.WriteElementString("loc", string.Format("{0}/{1}", thread.SubForumURL, i).ToLower());
                                writer.WriteElementString("lastmod", String.Format("{0:yyyy-MM-dd}", thread.CreateDate));
                                writer.WriteElementString("changefreq", "weekly");
                                writer.WriteElementString("priority", "0.8");
                                writer.WriteEndElement();
                                writer.WriteString("\r\n"); //newline 
                            }
                        }
                    }
                }
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            Response.End();
        }
    }
}