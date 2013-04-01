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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.Operational;
using BootBaronLib.Values;
using IntrepidStudios;

namespace DasKlub.Controllers
{
    public class NewsController : Controller
    {
        private const int PageSize = 5;

        public JsonResult Items(int pageNumber)
        {
            var model = new Contents();

            model.GetContentPageWiseAll(pageNumber, PageSize);

            var sb = new StringBuilder();

            foreach (var cnt in model)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
                {
                    ListItems = sb.ToString()
                });
        }


        public JsonResult LangItems(int pageNumber, string lang)
        {
            ViewBag.Lang = lang;

            var model = new Contents();

            model.GetContentPageWise(pageNumber, PageSize, lang);

            var sb = new StringBuilder();

            foreach (var cnt in model)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
                {
                    ListItems = sb.ToString()
                });
        }

        public JsonResult TagItems(int pageNumber, string key)
        {
            var model = new Contents();

            model.GetContentAllPageWiseKey(pageNumber, PageSize, key.Replace("-", " "));

            if (model.Count == 0)
            {
                // this might have had a dash in it
                model.GetContentAllPageWiseKey(1, PageSize, ViewBag.KeyName);

                if (model.Count == 0)
                {
                    // TODO: combination of with and without, deal with it
                }
            }


            var sb = new StringBuilder();

            foreach (var cnt in model)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
                {
                    ListItems = sb.ToString()
                });
        }


        public ActionResult Lang(string lang)
        {
            ViewBag.Lang = lang;

            var model = new Contents();

            model.GetContentPageWise(1, PageSize, lang);

            LoadTagCloud();

            LoadLang();

            ViewBag.EnableLoadingMore = (model.Count >= PageSize);

            return View(model);
        }

        public ActionResult Index()
        {
            var model = new Contents();

            //model.GetContentPageWise(1, pageSize, Utilities.GetCurrentLanguageCode());
            model.GetContentPageWiseAll(1, PageSize);

            LoadTagCloud();

            LoadLang();

            return View(model);
        }

        private void LoadLang()
        {
            Dictionary<string, string> dict = Contents.GetDistinctNewsLanguages();

            if (dict == null) return;

            Dictionary<string, string> sortedDict =
                (from entry in dict orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key,
                                                                                             pair => pair.Value);

            ViewBag.Langs = sortedDict;
        }

        private void LoadTagCloud()
        {
            const string cacheName = "ArticleTagCloud";
            string htmlCloud;

            if (HttpContext.Cache[cacheName] == null)
            {
                var cloud1 = new Cloud
                    {
                        DataIDField = "keyword_id",
                        DataKeywordField = "keyword_value",
                        DataCountField = "keyword_count",
                        DataURLField = "keyword_url"
                    };

                DataSet theDs = Contents.GetContentTagsAll();

                cloud1.DataSource = theDs;
                cloud1.MinFontSize = 14;
                cloud1.MaxFontSize = 30;
                cloud1.FontUnit = @"px";

                htmlCloud = cloud1.HTML();

                HttpContext.Cache.AddObjToCache(htmlCloud, cacheName);
            }
            else
            {
                htmlCloud = (string) HttpContext.Cache[cacheName];
            }

            ViewBag.CloudTags = htmlCloud;
        }


        public ActionResult VideoLog(int contentID)
        {
            // TODO: don't use referrer with HTTPS
            if (Request.UrlReferrer != null)
                HostedVideoLog.AddHostedVideoLog(Request.UrlReferrer.ToString(), Request.UserHostAddress, 0, "NW");

            return new JsonResult
                {
                    Data = new
                        {
                            Success = true,
                            ContentID = contentID
                        }
                };
        }


        [HttpGet]
        public ActionResult Detail(string key)
        {
            ViewBag.VideoHeight = (Request.Browser.IsMobileDevice) ? 190 : 400;
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ? 285 : 600;

            var model = new Content(key);


            ViewBag.ThumbIcon = Utilities.S3ContentPath(model.ContentPhotoThumbURL);


            LoadTagCloud();


            var otherNews = new Content();

            otherNews.GetPreviousNews(model.ReleaseDate);
            if (otherNews.ContentID > 0)
            {
                ViewBag.PreviousNews = otherNews.ToUnorderdListItem;
            }

            otherNews = new Content();
            otherNews.GetNextNews(model.ReleaseDate);

            if (otherNews.ContentID > 0)
            {
                ViewBag.NextNews = otherNews.ToUnorderdListItem;
            }


            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteComment(int commentID)
        {
            var mu = Membership.GetUser();

            var model = new ContentComment(commentID);

            var content = new Content(model.ContentID);

            if (mu == null || model.CreatedByUserID != Convert.ToInt32(mu.ProviderUserKey)) return new EmptyResult();

            model.Delete();

            return RedirectToAction("Detail", new {@key = content.ContentKey});
        }


        [Authorize]
        [HttpPost]
        public ActionResult Detail(FormCollection fc, int contentID)
        {
            var mu = Membership.GetUser();

            LoadTagCloud();

            var model = new Content(contentID)
                {
                    Reply = new ContentComment {StatusType = Convert.ToChar(SiteEnums.CommentStatus.C.ToString())}
                };

            if (mu != null) model.Reply.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
            model.Reply.ContentID = contentID;
            model.Reply.IpAddress = Request.UserHostAddress;

            TryUpdateModel(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (BlackIPs.IsIPBlocked(Request.UserHostAddress))
            {
                return View(model);
            }

            var hasBeenSaid = false;

            foreach (var cmt in model.Comments.Where(cmt => cmt.CreatedByUserID == model.Reply.CreatedByUserID &&
                                                                       cmt.Detail == model.Reply.Detail))
            {
                hasBeenSaid = true;
            }

            if (!hasBeenSaid) model.Reply.Create();

            return View(model);
        }

        public ActionResult Tag(string key)
        {
            if (string.IsNullOrEmpty(key)) return new EmptyResult();

            ViewBag.KeyName = key;

            key = key.Replace("-", " ");

            var model = new Contents();

            model.GetContentAllPageWiseKey(1, PageSize, key);

            if (model.Count == 0)
            {
                // this might have had a dash in it
                model.GetContentAllPageWiseKey(1, PageSize, ViewBag.KeyName);

                if (model.Count == 0)
                {
                    // TODO: combination of with and without, deal with it
                }
            }

            ViewBag.EnableLoadingMore = (model.Count < PageSize);

            ViewBag.TagName = key;

            LoadTagCloud();

            return View(model);
        }
    }
}