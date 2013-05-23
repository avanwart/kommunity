//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using DasKlub.Web.Models.Forum;
//using DasKlub.Web.Models.Models;

//namespace DasKlub.Web.Models.Controllers
//{   
//    public class ForumCategoriesController : Controller
//    {
//        private readonly IForumCategoryRepository forumcategoryRepository;

//        // If you are using Dependency Injection, you can delete the following constructor
//        public ForumCategoriesController() : this(new ForumCategoryRepository())
//        {
//        }

//        public ForumCategoriesController(IForumCategoryRepository forumcategoryRepository)
//        {
//            this.forumcategoryRepository = forumcategoryRepository;
//        }

//        //
//        // GET: /ForumCategories/

//        public ViewResult Index()
//        {
//            return View(forumcategoryRepository.All);
//        }

//        //
//        // GET: /ForumCategories/Details/5

//        public ViewResult Details(int id)
//        {
//            return View(forumcategoryRepository.Find(id));
//        }

//        //
//        // GET: /ForumCategories/Create

//        public ActionResult Create()
//        {
//            return View();
//        } 

//        //
//        // POST: /ForumCategories/Create

//        [HttpPost]
//        public ActionResult Create(ForumCategory forumcategory)
//        {
//            if (ModelState.IsValid) {
//                forumcategoryRepository.InsertOrUpdate(forumcategory);
//                forumcategoryRepository.Save();
//                return RedirectToAction("Index");
//            } else {
//                return View();
//            }
//        }
        
//        //
//        // GET: /ForumCategories/Edit/5
 
//        public ActionResult Edit(int id)
//        {
//             return View(forumcategoryRepository.Find(id));
//        }

//        //
//        // POST: /ForumCategories/Edit/5

//        [HttpPost]
//        public ActionResult Edit(ForumCategory forumcategory)
//        {
//            if (ModelState.IsValid) {
//                forumcategoryRepository.InsertOrUpdate(forumcategory);
//                forumcategoryRepository.Save();
//                return RedirectToAction("Index");
//            } else {
//                return View();
//            }
//        }

//        //
//        // GET: /ForumCategories/Delete/5
 
//        public ActionResult Delete(int id)
//        {
//            return View(forumcategoryRepository.Find(id));
//        }

//        //
//        // POST: /ForumCategories/Delete/5

//        [HttpPost, ActionName("Delete")]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            forumcategoryRepository.Delete(id);
//            forumcategoryRepository.Save();

//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing) {
//                forumcategoryRepository.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}

