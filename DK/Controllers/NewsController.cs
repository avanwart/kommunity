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
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.Operational;
using System.Collections;
using System.Collections.Generic;
using BootBaronLib.Values;
using IntrepidStudios;

namespace DasKlub.Controllers
{
    public class NewsController : Controller
    {
        int pageSize = 5;

        public JsonResult Items(int pageNumber)
        {
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            model.GetContentPageWiseAll(pageNumber, pageSize );

            StringBuilder sb = new StringBuilder();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content cnt in model)
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

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            model.GetContentPageWise(pageNumber, pageSize, lang);

            StringBuilder sb = new StringBuilder();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content cnt in model)
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
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            model.GetContentAllPageWiseKey(pageNumber, pageSize, key);

            if (model.Count == 0)
            {
                // this might have had a dash in it
                model.GetContentAllPageWiseKey(1, pageSize, ViewBag.KeyName);

                if (model.Count == 0)
                {
                    // TODO: combination of with and without, deal with it
                }
            }



            StringBuilder sb = new StringBuilder();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content cnt in model)
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

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            model.GetContentPageWise(1, pageSize, lang);

            LoadTagCloud();

            LoadLang();

            ViewBag.EnableLoadingMore = (model.Count >= pageSize);

            return View(model);
        }

        public ActionResult Index()
        {
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            //model.GetContentPageWise(1, pageSize, Utilities.GetCurrentLanguageCode());
            model.GetContentPageWiseAll(1, pageSize);

            LoadTagCloud();

            LoadLang();

            return View(model);
        }

        private void LoadLang()
        {
            Dictionary<string, string> dict = Contents.GetDistinctNewsLanguages();

            if (dict == null) return;

            var sortedDict = (from entry in dict orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            ViewBag.Langs = sortedDict;
        }

        private void LoadTagCloud()
        {
            string cacheName = "ArticleTagCloud";
            string htmlCloud = string.Empty;

            if (HttpContext.Cache[cacheName] == null)
            {
                Cloud cloud1 = new Cloud();
                cloud1.DataIDField = "keyword_id";
                cloud1.DataKeywordField = "keyword_value";
                cloud1.DataCountField = "keyword_count";
                cloud1.DataURLField = "keyword_url";

                DataSet theDS = new DataSet();

                theDS = Contents.GetContentTagsAll();

                cloud1.DataSource = theDS;
                cloud1.MinFontSize = 14;
                cloud1.MaxFontSize = 30;
                cloud1.FontUnit = "px";

                htmlCloud = cloud1.HTML();

                HttpContext.Cache.AddObjToCache(htmlCloud, cacheName);
            }
            else
            {
                htmlCloud = (string)HttpContext.Cache[cacheName];
            }

            ViewBag.CloudTags = htmlCloud;

        }


        public ActionResult VideoLog(int contentID)
        {
            // TODO: don't use referrer with HTTPS
            HostedVideoLog.AddHostedVideoLog(Request.UrlReferrer.ToString(), Request.UserHostAddress, 0,  "NW");

            return new JsonResult()
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
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ?  285 : 600;

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(key);


            ViewBag.ThumbIcon = Utilities.S3ContentPath(model.ContentPhotoThumbURL);


            LoadTagCloud();

            ContentComment concom = new ContentComment();


            BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content otherNews = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();

            otherNews.GetPreviousNews(model.ReleaseDate );
            if (otherNews.ContentID > 0 )
            {
                ViewBag.PreviousNews = otherNews.ToUnorderdListItem;
            }

            otherNews = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();
            otherNews.GetNextNews(model.ReleaseDate );

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
            MembershipUser mu = Membership.GetUser();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment(commentID);

            Content content = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(model.ContentID);

            if (mu == null || model.CreatedByUserID != Convert.ToInt32(mu.ProviderUserKey)) return new EmptyResult();

            model.Delete();

            return RedirectToAction("Detail", new { @key = content.ContentKey });
        }

        

        [Authorize]
        [HttpPost]
        public ActionResult Detail(FormCollection fc, int contentID)
        {
            MembershipUser mu = Membership.GetUser();

            LoadTagCloud();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(contentID);

            model.Reply = new ContentComment();

            model.Reply.StatusType = Convert.ToChar(SiteEnums.CommentStatus.C.ToString());
            model.Reply.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
            model.Reply.ContentID = contentID;
            model.Reply.IpAddress = Request.UserHostAddress;

            TryUpdateModel(model);

            if (ModelState.IsValid)
            {
                if (BlackIPs.IsIPBlocked(Request.UserHostAddress))
                {
                    return View(model);
                }

                bool hasBeenSaid = false;

                foreach (ContentComment cmt in model.Comments)
                {
                    if (cmt.CreatedByUserID == model.Reply.CreatedByUserID &&
                        cmt.Detail == model.Reply.Detail)
                    {
                        hasBeenSaid = true;
                    }
                }
                
                if (!hasBeenSaid) model.Reply.Create();

                return View(model);
            }
            else
            {
                return View(model);
            }


        }

        public ActionResult Tag(string key)
        {
            if (string.IsNullOrEmpty(key)) return new EmptyResult();

            ViewBag.KeyName = key;

            key = key.Replace("-", " ");

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            model.GetContentAllPageWiseKey(1, pageSize, key);

            if (model.Count == 0)
            {
                // this might have had a dash in it
                model.GetContentAllPageWiseKey(1, pageSize, ViewBag.KeyName );

                if (model.Count == 0)
                {
                    // TODO: combination of with and without, deal with it
                }
            }

            ViewBag.EnableLoadingMore = (model.Count < pageSize);

            ViewBag.TagName = key;

            LoadTagCloud();

            return View(model);
        }


    }
}
