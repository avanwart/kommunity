using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Services;
using DasKlub.Lib.Values;
using DasKlub.Models;
using DasKlub.Models.Forum;
using DasKlub.Models.Models;
using DasKlub.Lib.Resources;
using UserAccount = DasKlub.Lib.BOL.UserAccount;

namespace DasKlub.Web.Controllers
{
    public class ForumController : Controller
    {
        public const int PageSize = 10;
        public const int SubCatPageSize = 50;
        private readonly IForumCategoryRepository _forumcategoryRepository;
        private readonly MembershipUser _mu;
        private readonly IMailService _mail;
        private readonly UserAccount _ua;

        public ForumController()
            : this(new ForumCategoryRepository(), new MailService())
        {
        }


        private ForumController(IForumCategoryRepository forumcategoryRepository, IMailService mail)
        {
            ViewBag.IsAdmin = false;
            _mail = mail;
            _forumcategoryRepository = forumcategoryRepository;
            _mu = Membership.GetUser();
            if (_mu != null)
            {
                _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));
            }
        }



        public ActionResult Index()
        {
            using (var context = new DasKlubDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;

                var forumCategory = context.ForumCategory
                                                           .OrderBy(x => x.CreateDate)
                                                           .ToList();

                foreach (var category in forumCategory)
                {
                    var lastPostForum = new ForumPost();

                    var category1 = category;
                    var subForums =
                        context.ForumSubCategory.Where(x => x.ForumCategoryID == category1.ForumCategoryID);


                    foreach (var forumSubCategory in subForums)
                    {
                        category.TotalPosts++;

                        var lastPost =
                            context.ForumPost.Where(x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID)
                                   .OrderByDescending(x => x.CreateDate)
                                   .FirstOrDefault();

                        if (lastPost == null) continue;

                        var forumSubPostCount =
                            context.ForumPost.Count(x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID);

                        var pageCount = (forumSubPostCount + PageSize - 1) / PageSize;

                        lastPost.ForumPostURL =
                            new Uri(string.Format("{0}/{1}#{2}", forumSubCategory.SubForumURL, ((pageCount > 1)
                                    ? pageCount.ToString(CultureInfo.InvariantCulture)
                                    : string.Empty), lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture)));

                        lastPost.UserAccount = new UserAccount(lastPost.CreatedByUserID);

                        if (lastPost.CreateDate <= lastPostForum.CreateDate) continue;

                        if (_mu != null)
                        {
                            var userID = Convert.ToInt32(_mu.ProviderUserKey);

                            var isNew =
                                context.ForumPostNotification.FirstOrDefault(
                                    x =>
                                    x.ForumSubCategoryID == lastPost.ForumSubCategoryID &&
                                    x.UserAccountID == userID);

                            if (isNew != null && !isNew.IsRead)
                            {
                                lastPost.IsNewPost = true;
                            }
                        }
                        lastPostForum = lastPost;
                    }

                    category.LatestForumPost = lastPostForum;
                }

                return View(forumCategory);
            }
        }

        private void GetValue(string key, string subKey, DasKlubDbContext context)
        {
            var forum = context.ForumCategory.First(x => x.Key == key);
            ViewBag.Forum = forum;

            var subForum = context.ForumSubCategory.First(x => x.Key == subKey);
            ViewBag.SubForum = subForum;
        }

        #region sub forum

        public ActionResult SubCategory(string key, int pageNumber = 1)
        {
            using (var context = new DasKlubDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;

                var forum = context.ForumCategory.First(x => x.Key == key);
                ViewBag.Title = forum.Title;
                ViewBag.Forum = forum;

                var forumSubCategory = context.ForumSubCategory
                                                                 .Where(x => x.ForumCategoryID == forum.ForumCategoryID)
                                                                 .OrderByDescending(x => x.CreateDate)
                                                                 .Skip(SubCatPageSize * (pageNumber - 1))
                                                                 .Take(SubCatPageSize).ToList();

                var forumCategory = context.ForumCategory.First(x => x.Key == key);

                ViewBag.Forum = forumCategory;

                var totalCount = context.ForumSubCategory.Count(x => x.ForumCategoryID == forum.ForumCategoryID);

                ViewBag.PageCount = (totalCount + SubCatPageSize - 1) / SubCatPageSize;

                ViewBag.PageNumber = pageNumber;

                foreach (var thread in forumSubCategory)
                {
                    var lastPostForum = new ForumPost();

                    var thread1 = thread;
                    var subForums = context.ForumSubCategory.Where(x => x.ForumSubCategoryID == thread1.ForumSubCategoryID);

                    foreach (var forumPost in subForums)
                    {
                        var lastPost =
                            context.ForumPost.Where(x => x.ForumSubCategoryID == forumPost.ForumSubCategoryID)
                                   .OrderByDescending(x => x.CreateDate)
                                   .FirstOrDefault();

                        if (lastPost == null) continue;
                        if (lastPostForum.ForumPostID != 0 && (lastPost.CreateDate <= lastPostForum.CreateDate))
                            continue;

                        var forumSubPostCount = context.ForumPost.Count(x => x.ForumSubCategoryID == forumPost.ForumSubCategoryID);

                        thread.TotalPosts += forumSubPostCount;

                        var pageCount = (forumSubPostCount + SubCatPageSize - 1) / SubCatPageSize;

                        lastPost.ForumPostURL =
                            new Uri(forumPost.SubForumURL + "/" +
                                    ((pageCount > 1)
                                         ? pageCount.ToString(CultureInfo.InvariantCulture)
                                         : string.Empty) + "#" +
                                    lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture));

                        lastPost.UserAccount = new UserAccount(lastPost.CreatedByUserID);

                        if (_mu != null)
                        {
                            var userID = Convert.ToInt32(_mu.ProviderUserKey);

                            var isNew =
                                context.ForumPostNotification.FirstOrDefault(
                                    x =>
                                    x.ForumSubCategoryID == lastPost.ForumSubCategoryID &&
                                    x.UserAccountID == userID);

                            if (isNew != null && !isNew.IsRead)
                            {
                                lastPost.IsNewPost = true;
                            }
                        }

                        lastPostForum = lastPost;
                    }

                    thread.LatestForumPost = lastPostForum;
                }

                return View(forumSubCategory);
            }
        }

        [Authorize]
        public ActionResult CreateSubCategory(string key)
        {
            ViewBag.Forum = _forumcategoryRepository.Find(key);

            return View();
        }

        [Authorize]
        public ActionResult EditSubCategory(string key, string subKey)
        {
            var model = new ForumSubCategory();

            using (var context = new DasKlubDbContext())
            {
                var forum = context.ForumCategory.First(x => x.Key == key);
                ViewBag.Forum = forum;

                var subForum = context.ForumSubCategory.First(x => x.Key == subKey);
                ViewBag.SubForum = subForum;

                if (_ua.UserAccountID != subForum.CreatedByUserID)
                {
                    throw new UnauthorizedAccessException();
                }

                model = subForum;
            }

            return View(model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult EditSubCategory(ForumSubCategory model)
        {
            using (var context = new DasKlubDbContext())
            {
                var subForum = context.ForumSubCategory.First(
                    currentForumSubCategory => 
                        currentForumSubCategory.ForumSubCategoryID == model.ForumSubCategoryID);

                if (_ua.UserAccountID != subForum.CreatedByUserID)
                {
                    throw new UnauthorizedAccessException();
                }
                subForum.UpdatedByUserID = _ua.UserAccountID;

                subForum.Title = model.Title;
                subForum.Description = model.Description;

                context.Entry(subForum).State = EntityState.Modified;
                context.SaveChanges();

                model.Key = subForum.Key;
                model.ForumCategory =
                    context.ForumCategory.First(forum => forum.ForumCategoryID == subForum.ForumCategoryID);
            }

            return new RedirectResult(model.SubForumURL.ToString());
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateSubCategory(ForumSubCategory model)
        {
            if (_mu != null)
            {
                var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

                using (var context = new DasKlubDbContext())
                {
                    if (model == null) return new EmptyResult();

                    var forum = context.ForumCategory.First(x => x.Key == model.Key);

                    ViewBag.Forum = forum;

                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }

                    model.ForumCategoryID = forum.ForumCategoryID;
                    model.CreatedByUserID = ua.UserAccountID;
                    model.Key = FromString.URLKey(model.Title);
                    model.Title = model.Title.Trim();
                    model.Description = model.Description.Trim();

                    context.ForumSubCategory.Add(model);

                    var notification = new ForumPostNotification
                    {
                        CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey),
                        IsRead = true,
                        UserAccountID = Convert.ToInt32(_mu.ProviderUserKey),
                        ForumSubCategoryID = model.ForumSubCategoryID
                    };

                    context.ForumPostNotification.Add(notification);

                    context.SaveChanges();

                    return RedirectToAction("ForumPost", "Forum", new { subkey = model.Key,  key = forum.Key });
                }
            }

            return new EmptyResult();
        }


        [Authorize]
        public ActionResult DeleteSubForum(int forumSubCategoryID)
        {
            using (var context = new DasKlubDbContext())
            {
                var forumPost = context.ForumSubCategory.First(x => x.ForumSubCategoryID == forumSubCategoryID);

                if (Convert.ToInt32(_mu.ProviderUserKey) != forumPost.CreatedByUserID && !_ua.IsAdmin)
                    return RedirectToAction("Index");
                context.ForumSubCategory.Remove(forumPost);

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        #endregion

        #region forum

        [Authorize]
        public ActionResult CreateForum()
        {
            if (_mu != null)
            {
                var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));
                if (!ua.IsAdmin) return new EmptyResult();
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateForum(ForumCategory model)
        {
            if (_mu != null)
            {
                var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

                if (!ua.IsAdmin) return new EmptyResult();

                using (var context = new DasKlubDbContext())
                {
                    if (model != null)
                    {
                        model.CreatedByUserID = ua.UserAccountID;
                        model.Key = FromString.URLKey(model.Title);
                        model.Title = model.Title.Trim();
                        model.Description = model.Description.Trim();

                        context.ForumCategory.Add(model);

                        context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult CreateForumPost(string key, string subKey)
        {
            using (var context = new DasKlubDbContext())
            {
                GetValue(key, subKey, context);
            }

            return View();
        }

        #endregion

        #region forum post

        public ActionResult ForumPost(string key, string subKey, int pageNumber = 1)
        {
            using (var context = new DasKlubDbContext())
            {
                ViewBag.UserID = (_mu == null) ? 0 : Convert.ToInt32(_mu.ProviderUserKey);

                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;

                var forum = context.ForumCategory.FirstOrDefault(x => x.Key == key);

                if (forum == null || forum.ForumCategoryID == 0)
                    return RedirectPermanent("~/forum");// it's gone

                ViewBag.Forum = forum;

                var subForum = context.ForumSubCategory.FirstOrDefault(x => x.Key == subKey);

                if (subForum == null || subForum.ForumSubCategoryID == 0)
                    return RedirectPermanent("~/forum");// it's gone

                subForum.UserAccount = new UserAccount(subForum.CreatedByUserID);

                ViewBag.SubForum = subForum;

                var forumPost = context.ForumPost
                                       .Where(x => x.ForumSubCategoryID == subForum.ForumSubCategoryID)
                                       .OrderBy(x => x.CreateDate)
                                       .Skip(PageSize*(pageNumber - 1))
                                       .Take(PageSize).ToList();



                if (_mu != null)
                {
                    var userID = Convert.ToInt32(_mu.ProviderUserKey);
                    var ua = new UserAccount(userID);
                    ViewBag.IsAdmin = ua.IsAdmin;

                    var forumPostNotification =
                        context.ForumPostNotification.FirstOrDefault(
                            x =>
                            x.ForumSubCategoryID == subForum.ForumSubCategoryID &&
                            x.UserAccountID == userID);

                    if (forumPostNotification != null)
                    {
                        forumPostNotification.IsRead = true;
                        context.Entry(forumPostNotification).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                var totalCount = context.ForumPost.Count(x => x.ForumSubCategoryID == subForum.ForumSubCategoryID);

                ViewBag.PageCount = (totalCount + PageSize - 1)/PageSize;

                ViewBag.PageNumber = pageNumber;

                ViewBag.PageTitle = subForum.Title;
                ViewBag.Title = string.Format("{0} | page {1}", subForum.Title, pageNumber);

                foreach (var post in forumPost)
                {
                    post.UserAccount = new UserAccount(post.CreatedByUserID);
                }

                ViewBag.MetaDescription = FromString.GetFixedLengthString(subForum.Description, 160);

                return View(forumPost);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateForumPost(ForumPost model, int forumSubCategoryID)
        {
            using (var context = new DasKlubDbContext())
            {
                var currentLang = Utilities.GetCurrentLanguageCode();

                var subForum = context.ForumSubCategory
                                      .First(x => x.ForumSubCategoryID == forumSubCategoryID);

                if (!ModelState.IsValid)
                {
                    ViewBag.SubForum = subForum;

                    return View(model);
                }

                var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

                model.ForumSubCategoryID = forumSubCategoryID;
                model.CreatedByUserID = ua.UserAccountID;
                context.ForumPost.Add(model);

                var currentUserNotification = context.ForumPostNotification
                                                     .FirstOrDefault(x => x.ForumSubCategoryID == forumSubCategoryID && x.UserAccountID == ua.UserAccountID);

                if (currentUserNotification == null || currentUserNotification.ForumPostNotificationID == 0)
                {
                    var notification = new ForumPostNotification
                    {
                        CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey),
                        IsRead = true,
                        UserAccountID = Convert.ToInt32(_mu.ProviderUserKey),
                        ForumSubCategoryID = forumSubCategoryID
                    };

                    context.ForumPostNotification.Add(notification);
                }

                var allUserNotifications = context.ForumPostNotification.Where(x => x.ForumSubCategoryID == forumSubCategoryID).ToList();
              
                subForum.ForumCategory = context.ForumCategory.First(x => x.ForumCategoryID == subForum.ForumCategoryID);

                if (context.ForumPost.FirstOrDefault(
                    x =>
                    x.ForumSubCategoryID == forumSubCategoryID && x.Detail == model.Detail &&
                    x.CreatedByUserID == ua.UserAccountID) == null)
                {
                    Thread.CurrentThread.CurrentUICulture =
                        CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
                    Thread.CurrentThread.CurrentCulture =
                        CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

                    foreach (
                        var forumPostNotification in
                            allUserNotifications.Where(
                                forumPostNotification => forumPostNotification.UserAccountID != ua.UserAccountID))
                    {
                        forumPostNotification.IsRead = false;
                        forumPostNotification.UpdatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                        context.Entry(forumPostNotification).State = EntityState.Modified;

                        var notifiedUser = new UserAccount(forumPostNotification.UserAccountID);
                        var notifiedUserDetails = new UserAccountDetail();
                        notifiedUserDetails.GetUserAccountDeailForUser(forumPostNotification.UserAccountID);

                        if (!notifiedUserDetails.EmailMessages) continue;

                        var title = ua.UserName + " => " + subForum.Title;
                        var body = new StringBuilder(100);
                        body.Append(Messages.New);
                        body.Append(": ");
                        body.Append(subForum.SubForumURL);
                        body.AppendLine();
                        body.AppendLine();
                        body.Append(model.Detail);
                        body.AppendLine();
                        body.AppendLine();
                        body.Append(Messages.Reply);
                        body.Append(": ");
                        body.AppendFormat("{0}/create", subForum.SubForumURL);

                        _mail.SendMail(Lib.Configs.AmazonCloudConfigs.SendFromEmail, notifiedUser.EMail, title, body.ToString());
                    }

                    context.SaveChanges();

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

                    Response.Redirect(subForum.SubForumURL.ToString());
                }

                return new EmptyResult();
            }
        }

        [Authorize]
        public ActionResult DeleteForumPost(int forumPostID)
        {
            using (var context = new DasKlubDbContext())
            {
                var forumPost = context.ForumPost.First(x => x.ForumPostID == forumPostID);
                
                if (Convert.ToInt32(_mu.ProviderUserKey) == forumPost.CreatedByUserID || _ua.IsAdmin)
                {
                    context.ForumPost.Remove(forumPost);

                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}