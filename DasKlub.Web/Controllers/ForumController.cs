﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Services;
using DasKlub.Lib.Values;
using DasKlub.Models;
using DasKlub.Models.Forum;
using DasKlub.Models.Models;

namespace DasKlub.Web.Controllers
{
    public class ForumController : Controller
    {
        public const int PageSizeForumPost = 50;
        public const int SubCatPageSize = 50;
        private readonly IForumCategoryRepository _forumcategoryRepository;
        private readonly IMailService _mail;
        private readonly MembershipUser _mu;
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

                List<ForumCategory> forumCategory = context.ForumCategory
                    .OrderBy(x => x.CreateDate)
                    .ToList();

                foreach (ForumCategory category in forumCategory)
                {
                    var lastPostForum = new ForumPost();

                    ForumCategory category1 = category;
                    IQueryable<ForumSubCategory> subForums =
                        context.ForumSubCategory.Where(x => x.ForumCategoryID == category1.ForumCategoryID);

                    using (var context2 = new DasKlubDbContext())
                    {
                        foreach (ForumSubCategory forumSubCategory in subForums)
                        {
                            category.TotalPosts++;

                            ForumPost lastPost =
                                context2.ForumPost.Where(
                                    x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID)
                                    .OrderByDescending(x => x.CreateDate)
                                    .FirstOrDefault();

                            if (lastPost == null) continue;

                            int forumSubPostCount =
                                context2.ForumPost.Count(
                                    x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID);

                            int pageCount = (forumSubPostCount + PageSizeForumPost - 1)/PageSizeForumPost;

                            lastPost.ForumPostURL =
                                new Uri(string.Format("{0}/{1}#{2}", forumSubCategory.SubForumURL, ((pageCount > 1)
                                    ? pageCount.ToString(CultureInfo.InvariantCulture)
                                    : string.Empty), lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture)));

                            lastPost.UserAccount = new UserAccount(lastPost.CreatedByUserID);

                            if (lastPost.CreateDate <= lastPostForum.CreateDate) continue;

                            if (_mu != null)
                            {
                                int userId = Convert.ToInt32(_mu.ProviderUserKey);

                                ForumPostNotification isNew =
                                    context2.ForumPostNotification.FirstOrDefault(
                                        x =>
                                            x.ForumSubCategoryID == lastPost.ForumSubCategoryID &&
                                            x.UserAccountID == userId);

                                if (isNew != null && !isNew.IsRead)
                                {
                                    lastPost.IsNewPost = true;
                                }
                            }
                            lastPostForum = lastPost;
                        }

                        category.LatestForumPost = lastPostForum;
                    }
                }

                return View(forumCategory);
            }
        }

        private void GetValue(string key, string subKey, DasKlubDbContext context)
        {
            ForumCategory forum = context.ForumCategory.First(existingForum => existingForum.Key == key);
            ViewBag.Forum = forum;

            ForumSubCategory subForum = context.ForumSubCategory
                .First(
                    existingSubForum =>
                        existingSubForum.Key == subKey && existingSubForum.ForumCategoryID == forum.ForumCategoryID);
            ViewBag.SubForum = subForum;
        }

        #region sub forum

        public ActionResult SubCategory(string key, int pageNumber = 1)
        {
            using (var context = new DasKlubDbContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;

                ForumCategory forum = context.ForumCategory.First(x => x.Key == key);
                ViewBag.Title = forum.Title;
                ViewBag.Forum = forum;

                List<ForumSubCategory> forumSubCategory = context.ForumSubCategory
                    .Where(x => x.ForumCategoryID == forum.ForumCategoryID)
                    .OrderByDescending(x => x.CreateDate)
                    .Skip(SubCatPageSize*(pageNumber - 1))
                    .Take(SubCatPageSize).ToList();

                ForumCategory forumCategory = context.ForumCategory.First(x => x.Key == key);

                ViewBag.Forum = forumCategory;

                int totalCount = context.ForumSubCategory.Count(x => x.ForumCategoryID == forum.ForumCategoryID);

                ViewBag.PageCount = (totalCount + SubCatPageSize - 1)/SubCatPageSize;

                ViewBag.PageNumber = pageNumber;

                foreach (ForumSubCategory thread in forumSubCategory)
                {
                    var lastPostForum = new ForumPost();

                    ForumSubCategory thread1 = thread;
                    IQueryable<ForumSubCategory> subForums =
                        context.ForumSubCategory.Where(x => x.ForumSubCategoryID == thread1.ForumSubCategoryID);

                    using (var context2 = new DasKlubDbContext())
                    {
                        foreach (ForumSubCategory forumPost in subForums)
                        {
                            ForumPost lastPost =
                                context2.ForumPost.Where(x => x.ForumSubCategoryID == forumPost.ForumSubCategoryID)
                                    .OrderByDescending(x => x.CreateDate)
                                    .FirstOrDefault();

                            if (lastPost == null) continue;
                            if (lastPostForum.ForumPostID != 0 && (lastPost.CreateDate <= lastPostForum.CreateDate))
                                continue;

                            int forumSubPostCount =
                                context2.ForumPost.Count(x => x.ForumSubCategoryID == forumPost.ForumSubCategoryID);

                            thread.TotalPosts += forumSubPostCount;

                            int pageCount = (forumSubPostCount + SubCatPageSize - 1)/SubCatPageSize;

                            lastPost.ForumPostURL =
                                new Uri(forumPost.SubForumURL + "/" +
                                        ((pageCount > 1)
                                            ? pageCount.ToString(CultureInfo.InvariantCulture)
                                            : string.Empty) + "#" +
                                        lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture));

                            lastPost.UserAccount = new UserAccount(lastPost.CreatedByUserID);

                            if (_mu != null)
                            {
                                int userID = Convert.ToInt32(_mu.ProviderUserKey);

                                ForumPostNotification isNew =
                                    context2.ForumPostNotification.FirstOrDefault(
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
            ForumSubCategory model;

            using (var context = new DasKlubDbContext())
            {
                ForumCategory forum = context.ForumCategory.First(existingForum => existingForum.Key == key);
                ViewBag.Forum = forum;

                ForumSubCategory subForum = context.ForumSubCategory
                    .First(
                        existingSubForum =>
                            existingSubForum.Key == subKey && existingSubForum.ForumCategoryID == forum.ForumCategoryID);
                ViewBag.SubForum = subForum;

                if (_ua.UserAccountID != subForum.CreatedByUserID && !_ua.IsAdmin)
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
                ForumSubCategory subForum = context.ForumSubCategory.First(
                    currentForumSubCategory =>
                        currentForumSubCategory.ForumSubCategoryID == model.ForumSubCategoryID);

                if (_ua.UserAccountID != subForum.CreatedByUserID && !_ua.IsAdmin)
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
            if (_mu == null) return new EmptyResult();

            var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            using (var context = new DasKlubDbContext())
            {
                if (model == null) return new EmptyResult();

                ForumCategory forum = context.ForumCategory.First(x => x.Key == model.Key);

                ViewBag.Forum = forum;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model.ForumCategoryID = forum.ForumCategoryID;
                model.CreatedByUserID = ua.UserAccountID;
                model.Key = FromString.UrlKey(model.Title);
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

                return RedirectToAction("ForumPost", "Forum", new {subkey = model.Key, key = forum.Key});
            }
        }

        [Authorize]
        public ActionResult DeleteSubForum(int forumSubCategoryID)
        {
            using (var context = new DasKlubDbContext())
            {
                ForumSubCategory forumPost =
                    context.ForumSubCategory.First(x => x.ForumSubCategoryID == forumSubCategoryID);

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
            if (_mu == null) return View();

            var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));
            if (!ua.IsAdmin) return new EmptyResult();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateForum(ForumCategory model)
        {
            if (_mu == null) return RedirectToAction("Index");

            var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            if (!ua.IsAdmin) return new EmptyResult();

            using (var context = new DasKlubDbContext())
            {
                if (model == null) return RedirectToAction("Index");

                model.CreatedByUserID = ua.UserAccountID;
                model.Key = FromString.UrlKey(model.Title);
                model.Title = model.Title.Trim();
                model.Description = model.Description.Trim();

                context.ForumCategory.Add(model);

                context.SaveChanges();
            }

            return RedirectToAction("Index");
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

                ForumCategory forum = context.ForumCategory.FirstOrDefault(forumPost => forumPost.Key == key);

                if (forum == null || forum.ForumCategoryID == 0)
                    return RedirectPermanent("~/forum"); // it's gone

                ViewBag.Forum = forum;

                ForumSubCategory subForum = context.ForumSubCategory.FirstOrDefault(forumPost =>
                    forumPost.Key == subKey &&
                    forumPost.ForumCategoryID == forum.ForumCategoryID);

                if (subForum == null || subForum.ForumSubCategoryID == 0)
                    return RedirectPermanent("~/forum"); // it's gone

                // TODO: USE LINQ
                DbCommand comm = DbAct.CreateCommand();
                comm.CommandText = string.Format(@"
                    select temp.CreatedByUserID, count(*) as 'count'
                    from (
                        select CreatedByUserID, ForumSubCategoryID from ForumSubCategories with (nolock)
                        union all
                        select CreatedByUserID, ForumSubCategoryID from ForumPosts with (nolock)
                    ) as temp
                    where CreatedByUserID in (select CreatedByUserID from ForumPosts with (nolock) where ForumSubCategoryID = {0})
                    group by CreatedByUserID
                    order by CreatedByUserID ", subForum.ForumSubCategoryID);

                comm.CommandType = CommandType.Text;

                DataTable userPostCounts = DbAct.ExecuteSelectCommand(comm);

                Dictionary<int, int> userPostCountList = userPostCounts.Rows
                    .Cast<DataRow>()
                    .ToDictionary(
                        row => Convert.ToInt32(row["CreatedByUserID"]),
                        row => Convert.ToInt32(row["count"]));

                ViewBag.UserPostCounts = userPostCountList;

                subForum.UserAccount = new UserAccount(subForum.CreatedByUserID);

                ViewBag.SubForum = subForum;

                List<ForumPost> forumPostDisplay = context.ForumPost
                    .Where(x => x.ForumSubCategoryID == subForum.ForumSubCategoryID)
                    .OrderBy(x => x.CreateDate)
                    .Skip(PageSizeForumPost*(pageNumber - 1))
                    .Take(PageSizeForumPost).ToList();

                if (_mu != null)
                {
                    int userId = Convert.ToInt32(_mu.ProviderUserKey);
                    var ua = new UserAccount(userId);
                    ViewBag.IsAdmin = ua.IsAdmin;

                    ForumPostNotification forumPostNotification =
                        context.ForumPostNotification.FirstOrDefault(
                            x =>
                                x.ForumSubCategoryID == subForum.ForumSubCategoryID &&
                                x.UserAccountID == userId);

                    if (forumPostNotification != null)
                    {
                        forumPostNotification.IsRead = true;
                        context.Entry(forumPostNotification).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                int totalCount = context.ForumPost.Count(x => x.ForumSubCategoryID == subForum.ForumSubCategoryID);

                ViewBag.PageCount = (totalCount + PageSizeForumPost - 1)/PageSizeForumPost;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageTitle = subForum.Title;
                ViewBag.Title = pageNumber == 1
                    ? string.Format("{0}", subForum.Title)
                    : string.Format("{0} | page {1}", subForum.Title, pageNumber);

                foreach (ForumPost post in forumPostDisplay)
                {
                    post.UserAccount = new UserAccount(post.CreatedByUserID);
                }

                ViewBag.MetaDescription = FromString.GetFixedLengthString(subForum.Description, 160);

                return View(forumPostDisplay);
            }
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


        [Authorize]
        public ActionResult EditForumPost(string key, string subKey, int forumPostID)
        {
            ForumPost model;

            using (var context = new DasKlubDbContext())
            {
                model = context.ForumPost.First(existingForumPost => existingForumPost.ForumPostID == forumPostID);

                if (_ua.UserAccountID != model.CreatedByUserID && !_ua.IsAdmin)
                {
                    throw new UnauthorizedAccessException();
                }

                ForumSubCategory subCategory = context.ForumSubCategory.First(forumSubCategory =>
                    forumSubCategory.ForumSubCategoryID == model.ForumSubCategoryID);
                ViewBag.SubForum = subCategory;
                ViewBag.Forum =
                    context.ForumCategory.First(forum => forum.ForumCategoryID == subCategory.ForumCategoryID);
            }

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public ActionResult CreateForumPost(ForumPost model, int forumSubCategoryID)
        {
            using (var context = new DasKlubDbContext())
            {
                string currentLang = Utilities.GetCurrentLanguageCode();

                ForumSubCategory subForum = context.ForumSubCategory
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

                ForumPostNotification currentUserNotification = context.ForumPostNotification
                    .FirstOrDefault(
                        x => x.ForumSubCategoryID == forumSubCategoryID && x.UserAccountID == ua.UserAccountID);

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

                List<ForumPostNotification> allUserNotifications =
                    context.ForumPostNotification.Where(x => x.ForumSubCategoryID == forumSubCategoryID).ToList();

                subForum.ForumCategory = context.ForumCategory.First(x => x.ForumCategoryID == subForum.ForumCategoryID);

                if (context.ForumPost.FirstOrDefault(
                    x =>
                        x.ForumSubCategoryID == forumSubCategoryID && x.Detail == model.Detail &&
                        x.CreatedByUserID == ua.UserAccountID) != null) return new EmptyResult();
                Thread.CurrentThread.CurrentUICulture =
                    CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
                Thread.CurrentThread.CurrentCulture =
                    CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

                foreach (
                    ForumPostNotification forumPostNotification in
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

                    string title = ua.UserName + " => " + subForum.Title;
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

                    _mail.SendMail(AmazonCloudConfigs.SendFromEmail, notifiedUser.EMail, title, body.ToString());
                }

                context.SaveChanges();

                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

                Response.Redirect(subForum.SubForumURL.ToString());

                return new EmptyResult();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditForumPost(ForumPost model)
        {
            using (var context = new DasKlubDbContext())
            {
                ForumPost forumPost = context.ForumPost.First(
                    currentForumPost => currentForumPost.ForumPostID == model.ForumPostID);

                if (_ua.UserAccountID != forumPost.CreatedByUserID && !_ua.IsAdmin)
                {
                    throw new UnauthorizedAccessException();
                }
                forumPost.UpdatedByUserID = _ua.UserAccountID;

                forumPost.Detail = model.Detail;

                context.Entry(forumPost).State = EntityState.Modified;
                context.SaveChanges();

                ForumSubCategory subForum = context.ForumSubCategory.First(
                    currentForumSubCategory =>
                        currentForumSubCategory.ForumSubCategoryID == model.ForumSubCategoryID);

                subForum.ForumCategory =
                    context.ForumCategory.First(forum => forum.ForumCategoryID == subForum.ForumCategoryID);

                return new RedirectResult(subForum.SubForumURL.ToString());
            }
        }

        [Authorize]
        public ActionResult DeleteForumPost(int forumPostID)
        {
            using (var context = new DasKlubDbContext())
            {
                ForumPost forumPost = context.ForumPost.First(x => x.ForumPostID == forumPostID);

                if (Convert.ToInt32(_mu.ProviderUserKey) != forumPost.CreatedByUserID && !_ua.IsAdmin)
                    return RedirectToAction("Index");

                context.ForumPost.Remove(forumPost);

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}