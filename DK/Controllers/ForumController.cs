using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Operational;
using DasKlub.Models;
using DasKlub.Models.Forum;


namespace DasKlub.Controllers
{
    public class ForumController : Controller
    {
        //
        // GET: /Forum/

        private const int PageSize = 10;
        private readonly MembershipUser _mu;


        public ForumController()
        {
            _mu = Membership.GetUser();
        }

        public ActionResult Index()
        {
            using (var context = new DasKlubDBContext())
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
                    var subForums = context.ForumSubCategory.Where(x => x.ForumCategoryID == category1.ForumCategoryID);


                    foreach (var forumSubCategory in subForums)
                    {
                        category.TotalPosts++;

                        var lastPost =
                               context.ForumPost.Where(x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID)
                                      .OrderByDescending(x => x.CreateDate)
                                      .FirstOrDefault();

                        if (lastPost == null) continue;

                        var forumSubPostCount = context.ForumPost.Count(x => x.ForumSubCategoryID == forumSubCategory.ForumSubCategoryID);



                        var pageCount = (forumSubPostCount + PageSize - 1)/PageSize;

                        lastPost.ForumPostURL =
                            new Uri(forumSubCategory.SubForumURL + "/" +
                                    ((pageCount > 1 )
                                         ? pageCount.ToString(CultureInfo.InvariantCulture)
                                         : string.Empty) + "#" + lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture));

                        lastPost.UserAccount = new UserAccount(lastPost.CreatedByUserID);

                        if (lastPost.CreateDate <= lastPostForum.CreateDate) continue;
                        lastPostForum = lastPost;
                    }

                    category.LatestForumPost = lastPostForum;
                }

                return View(forumCategory);
            }
        }


        #region sub forum

        public ActionResult SubCategory(string key, int pageNumber = 1)
        {
            using (var context = new DasKlubDBContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;

                //var forumCategory = context.ForumCategory
                //        .Where(x => x.ForumSubCategory.Any())
                //        .OrderByDescending(x => x.CreateDate)
                //        .Skip(PageSize * (pageNumber - 1))
                //        .Take(PageSize).ToList();

                var forum = context.ForumCategory.First(x => x.Key == key);
                ViewBag.Title = forum.Title;
                ViewBag.Forum = forum;

                var forumSubCategory = context.ForumSubCategory
                                              .Where(x => x.ForumCategoryID == forum.ForumCategoryID)
                                              .OrderByDescending(x => x.CreateDate)
                                              .Skip(PageSize * (pageNumber - 1))
                                              .Take(PageSize).ToList();


                var forumCategory = context.ForumCategory.First(x => x.Key == key);

                ViewBag.Forum = forumCategory;

                //var forumCats = context.ForumCategory.FirstOrDefault(x => x.ForumCategoryID == 13);


                //if (forumCats != null)
                //{
                //    forumCats.Title = "555yeas ";

                //    context.ForumCategory.Remove(forumCats);
                //    var entry = context.Entry(forumCats);
                //    entry.Property(e => e.Title).IsModified = true;
                //    // other changed properties
                //    context.SaveChanges();
                //}



                var totalCount = context.ForumSubCategory.Count(x => x.ForumCategoryID == forum.ForumCategoryID);

                ViewBag.PageCount = (totalCount + PageSize - 1) / PageSize;

                ViewBag.PageNumber = pageNumber;




















                foreach (var thread in forumSubCategory)
                {
                    var lastPostForum = new ForumPost();

                    var subForums = context.ForumSubCategory.Where(x => x.ForumSubCategoryID == thread.ForumSubCategoryID);

                    foreach (ForumSubCategory forumPost in subForums)
                    {
                        var lastPost =
                            context.ForumPost.Where(x => x.ForumSubCategoryID == forumPost.ForumSubCategoryID)
                                   .OrderByDescending(x => x.CreateDate)
                                   .FirstOrDefault();

                        if (lastPost != null)
                        {
                            if (lastPostForum.ForumPostID == 0 || (lastPost.CreateDate > lastPostForum.CreateDate))
                            {
                                var forumSubPostCount =
                                    context.ForumPost.Count(
                                        x => x.ForumSubCategoryID == forumPost.ForumSubCategoryID);

                                thread.TotalPosts += forumSubPostCount;

                                var pageCount = (forumSubPostCount + PageSize - 1) / PageSize;

                                lastPost.ForumPostURL =
                                    new Uri(forumPost.SubForumURL + "/" +
                                            ((pageCount > 1)
                                                 ? pageCount.ToString(CultureInfo.InvariantCulture)
                                                 : string.Empty) + "#" + lastPost.ForumPostID.ToString(CultureInfo.InvariantCulture));

                                lastPost.UserAccount = new UserAccount(lastPost.CreatedByUserID);

                                lastPostForum = lastPost;

                            }



                        }

                    }

                    thread.LatestForumPost = lastPostForum;
                }


                return View(forumSubCategory);

            }
        }


        [Authorize]
        public ActionResult CreateSubCategory(string key)
        {
            using (var context = new DasKlubDBContext())
            {
                ViewBag.Forum = context.ForumCategory.First(x => x.Key == key);

                return View();
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult CreateSubCategory(ForumSubCategory model)
        {
            if (_mu != null)
            {
                var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

                using (var context = new DasKlubDBContext())
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
                    context.SaveChanges();

                    return RedirectToAction("SubCategory", "Forum",  model.Key);

                }
            }

            return new  EmptyResult();

        }


        [Authorize]
        public ActionResult DeleteSubForum(int forumSubCategoryID)
        {
            using (var context = new DasKlubDBContext())
            {
                var forumPost = context.ForumSubCategory.First(x => x.ForumSubCategoryID == forumSubCategoryID);

                if (Convert.ToInt32(_mu.ProviderUserKey) == forumPost.CreatedByUserID)
                {
                    context.ForumSubCategory.Remove(forumPost);

                    context.SaveChanges();
                }

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

                using (var context = new DasKlubDBContext())
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

            using (var context = new DasKlubDBContext())
            {
                GetValue(key, subKey, context);
            }

            return View();
        }

        #endregion

        #region forum post


        public ActionResult ForumPost(string key, string subKey, int pageNumber = 1)
        {
            using (var context = new DasKlubDBContext())
            {

                ViewBag.UserID = (_mu == null) ? 0 : Convert.ToInt32(_mu.ProviderUserKey);

                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;

                //var forumCategory = context.ForumCategory
                //        .Where(x => x.ForumSubCategory.Any())
                //        .OrderByDescending(x => x.CreateDate)
                //        .Skip(PageSize * (pageNumber - 1))
                //        .Take(PageSize).ToList();

                var forum = context.ForumCategory.First(x => x.Key == key);
                ViewBag.Forum = forum;

                var subForum = context.ForumSubCategory.First(x => x.Key == subKey);
                subForum.UserAccount = new UserAccount(subForum.CreatedByUserID);
                
                ViewBag.SubForum = subForum;


              

                var forumPost = context.ForumPost
                                       .Where(x => x.ForumSubCategoryID == subForum.ForumSubCategoryID)
                                       .OrderBy(x => x.CreateDate)
                                       .Skip(PageSize * (pageNumber - 1))
                                       .Take(PageSize).ToList();

                ViewBag.Title = subForum.Title;


                var totalCount = context.ForumPost.Count(x => x.ForumSubCategoryID == subForum.ForumSubCategoryID);

                ViewBag.PageCount = (totalCount + PageSize - 1) / PageSize;

                ViewBag.PageNumber = pageNumber;

                //var forumCats = context.ForumCategory.FirstOrDefault(x => x.ForumCategoryID == 13);


                //if (forumCats != null)
                //{
                //    forumCats.Title = "555yeas ";

                //    context.ForumCategory.Remove(forumCats);
                //    var entry = context.Entry(forumCats);
                //    entry.Property(e => e.Title).IsModified = true;
                //    // other changed properties
                //    context.SaveChanges();
                //}
                foreach (var post in forumPost)
                {
                    post.UserAccount = new UserAccount(post.CreatedByUserID);
                }

                return View(forumPost);

            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateForumPost(ForumPost model, int forumSubCategoryID)
        {
            using (var context = new DasKlubDBContext())
            {
                var subForum = context.ForumSubCategory.First(x => x.ForumSubCategoryID == forumSubCategoryID);

                if (!ModelState.IsValid)
                {
                    ViewBag.SubForum = subForum;

                    return View(model);
                }

                var ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

                model.ForumSubCategoryID = forumSubCategoryID;
                model.CreatedByUserID = ua.UserAccountID;
                context.ForumPost.Add(model);

                context.SaveChanges();

                subForum.ForumCategory = context.ForumCategory.First(x => x.ForumCategoryID == subForum.ForumCategoryID);

                Response.Redirect(subForum.SubForumURL.ToString());

                return new EmptyResult();
            }

        }

 
        [Authorize]
        public ActionResult DeleteForumPost(int forumPostID)
        {
            using (var context = new DasKlubDBContext())
            {
                var forumPost = context.ForumPost.First(x => x.ForumPostID == forumPostID);

                if (Convert.ToInt32(_mu.ProviderUserKey) == forumPost.CreatedByUserID)
                {
                    context.ForumPost.Remove(forumPost);

                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

        }

        #endregion


        private void GetValue(string key, string subKey, DasKlubDBContext context)
        {
            var forum = context.ForumCategory.First(x => x.Key == key);
            ViewBag.Forum = forum;

            var subForum = context.ForumSubCategory.First(x => x.Key == subKey);
            ViewBag.SubForum = subForum;
        }
    }
}
