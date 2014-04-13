using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using IntrepidStudios.SearchCloud;

namespace DasKlub.Web.Controllers
{
    public class NewsController : Controller
    {
        private const int PageSize = 5;
        private readonly MembershipUser _mu;

        public NewsController()
        {
            LoadTagCloud();

            LoadLang();

            _mu = Membership.GetUser();
        }

        public JsonResult Items(int pageNumber)
        {
            var model = new Contents();

            model.GetContentPageWiseReleaseAll(pageNumber, PageSize);

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

            model.GetContentPageWiseRelease(pageNumber, PageSize, lang);

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

        [HttpPost]
        public ActionResult TagItems(string key, int pageNumber = 1)
        {
            var model = new Contents();

            model.GetContentAllPageWiseKey(pageNumber, PageSize, key.Replace("-", " "));

            if (model.Count == 0)
            {
                // this might have had a dash in it
                model.GetContentPageWiseKeyRelease(1, PageSize, ViewBag.KeyName);

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
                    ListItems = sb.ToString(),
                    JsonRequestBehavior.AllowGet
                });
        }

        public ActionResult Lang(string lang)
        {
            ViewBag.Lang = lang;

            var model = new Contents();

            var total = model.GetContentPageWiseRelease(1, PageSize, lang);

            ViewBag.EnableLoadingMore = (PageSize < total);

            return View(model);
        }

        public ActionResult Index()
        {
            var model = new Contents();

            model.GetContentPageWiseReleaseAll(1, PageSize);

            return View(model);
        }

        private void LoadLang()
        {
            var dict = Contents.GetDistinctNewsLanguages();

            if (dict == null) return;

            var sortedDict = (from entry in dict orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            ViewBag.Langs = sortedDict;
        }

        private void LoadTagCloud()
        {
            const string cacheName = "ArticleTagCloud";
            string htmlCloud;

            if (HttpRuntime.Cache[cacheName] == null)
            {
                var cloud1 = new Cloud
                    {
                        DataIDField = "keyword_id",
                        DataKeywordField = "keyword_value",
                        DataCountField = "keyword_count",
                        DataURLField = "keyword_url"
                    };

                var theDs = Contents.GetContentTagsAll();

                cloud1.DataSource = theDs;
                cloud1.MinFontSize = 14;
                cloud1.MaxFontSize = 30;
                cloud1.FontUnit = @"px";

                htmlCloud = cloud1.HTML();

                HttpRuntime.Cache.AddObjToCache(htmlCloud, cacheName);
            }
            else
            {
                htmlCloud = (string) HttpRuntime.Cache[cacheName];
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
            
            if (!string.IsNullOrWhiteSpace(model.ContentVideoURL2) && 
                string.IsNullOrWhiteSpace(model.ContentVideoURL))
            {
                // TODO: parse just the key, it's currently requiring the embed
                model.ContentVideoURL2 = string.Concat(model.ContentVideoURL2, "?rel=0");
                ViewBag.VideoWidth = "100%";
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteComment(int commentID)
        {
            var model = new ContentComment(commentID);

            var content = new Content(model.ContentID);

            var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            if (_mu == null || ( model.CreatedByUserID != Convert.ToInt32(_mu.ProviderUserKey) && !ua.IsAdmin)) return new EmptyResult();

            model.Delete();

            return RedirectToAction("Detail", new {@key = content.ContentKey});
        }


        [Authorize]
        [HttpPost]
        public ActionResult Detail(FormCollection fc, int contentID)
        {
            var model = new Content(contentID)
                {
                    Reply = new ContentComment {StatusType = Convert.ToChar(SiteEnums.CommentStatus.C.ToString())}
                };

            if (_mu != null) model.Reply.CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
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

            Response.Redirect(model.UrlTo.ToString() + "#content_comments");

            return new EmptyResult();
        }

        public ActionResult Tag(string key)
        {
            if (string.IsNullOrEmpty(key)) return new EmptyResult();

            ViewBag.KeyName = key;

            key = key.Replace("-", " ");

            var model = new Contents();

            var total = model.GetContentPageWiseKeyRelease(1, PageSize, key);

            if (model.Count == 0)
            {
                // this might have had a dash in it
                model.GetContentPageWiseKeyRelease(1, PageSize, ViewBag.KeyName);

                if (model.Count == 0)
                {
                    // TODO: combination of with and without, deal with it
                }
            }

            ViewBag.EnableLoadingMore = (PageSize < total);

            ViewBag.TagName = key;
 
            return View(model);
        }
    }
}