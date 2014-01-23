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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.DomainConnection;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using DasKlub.Web.Models;
using LitS3;

namespace DasKlub.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class SiteAdminController : Controller
    {
        private MembershipUser _mu;

        public ActionResult SiteBranding()
        {
            var siteDomains = new SiteDomains();

            siteDomains.GetAll();

            return View(siteDomains);
        }

        [HttpGet]
        public ActionResult DeleteSiteDomain(int? siteDomainID)
        {
            if (siteDomainID != null)
            {
                var siteDomain = new SiteDomain(Convert.ToInt32(siteDomainID));

                siteDomain.Delete();
            }

            return RedirectToAction("SiteBranding");
        }

        [HttpGet]
        public ActionResult EditSiteDomain(int? siteDomainID)
        {
            var model = new SiteDomainModel();

            if (siteDomainID != null)
            {
                var siteDomain = new SiteDomain(Convert.ToInt32(siteDomainID));

                model.Description = siteDomain.Description;
                model.Language = siteDomain.Language;
                model.PropertyType = siteDomain.PropertyType;
                model.SiteDomainID = siteDomain.SiteDomainID;
            }

            return View(model);
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditSiteDomain(SiteDomainModel model)
        {
            TryUpdateModel(model);

            if (ModelState.IsValid)
            {
                var siteDomain = new SiteDomain
                    {
                        Description = model.Description,
                        Language = model.Language ?? string.Empty,
                        PropertyType = model.PropertyType,
                        SiteDomainID = model.SiteDomainID
                    };

                siteDomain.Set();
            }

            return RedirectToAction("SiteBranding");
        }

        #region comments

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditArticleComment(
            FormCollection fc,
            int? contentCommentID)
        {
            var model = new ContentComment();

            if (contentCommentID != null && contentCommentID > 0)
            {
                model = new ContentComment(
                    Convert.ToInt32(contentCommentID));
            }

            TryUpdateModel(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Update();

            return RedirectToAction("ArticleComments");
        }


        [Authorize]
        [HttpGet]
        public ActionResult BlockIP(int? id)
        {
            if (id != null && id > 0)
            {
                var model = new ContentComment(
                    Convert.ToInt32(id));

                model.Delete();

                var bip = new BlackIP {IpAddress = model.IpAddress};
                bip.Create();
            }

            return RedirectToAction("ArticleComments");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditArticleComment(int? id)
        {
            var model = new ContentComment();

            if (id != null && id > 0)
            {
                model = new ContentComment(
                    Convert.ToInt32(id));
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ArticleComments()
        {
            int totalRecords;
            const int pageSize = 10;
            var model = new ContentComments();

            if (string.IsNullOrEmpty(Request.QueryString[
                SiteEnums.QueryStringNames.pg.ToString()]))
            {
                totalRecords = model.GetCommentsPageWise(1, pageSize);
            }
            else
            {
                var pageNumber = Convert.ToInt32(Request.QueryString[
                    SiteEnums.QueryStringNames.pg.ToString()]);

                totalRecords = model.GetCommentsPageWise(pageNumber, pageSize);
            }

            ViewBag.PageCount = (totalRecords + pageSize - 1)/pageSize;

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public ActionResult DeleteComment(int? id)
        {
            if (id != null && id > 0)
            {
                var model = new ContentComment(
                    Convert.ToInt32(id));


                model.Delete();
            }

            return RedirectToAction("ArticleComments");
        }

        #endregion

        #region Articles

        [HttpGet]
        public ActionResult DeleteArticle(int? id)
        {
            if (id != null && id > 0)
            {
                var model = new Content(
                    Convert.ToInt32(id));

                var concoms = new ContentComments();
                concoms.GetCommentsForContent(model.ContentID, SiteEnums.CommentStatus.U);
                concoms.GetCommentsForContent(model.ContentID, SiteEnums.CommentStatus.C);

                foreach (ContentComment c1 in concoms)
                    c1.Delete();

                model.Delete();
            }

            return RedirectToAction("Articles");
        }

        [HttpGet]
        public ActionResult CreateArticle()
        {
            var model = new Content();

            if (model.ReleaseDate == DateTime.MinValue)
            {
                model.ReleaseDate = DateTime.UtcNow;
            }

            return View(model);
        }
 


        [HttpGet]
        public ActionResult EditArticle(int? id)
        {
            ViewBag.VideoHeight = (Request.Browser.IsMobileDevice) ? 160 : 360;
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ? 285 : 640;

            var model = new Content();

            if (id != null && id > 0)
            {
                model = new Content(
                    Convert.ToInt32(id));
            }


            return View(model);
        }


        public ActionResult Articles()
        {
            int totalRecords;
            const int pageSize = 10;
            var model = new Contents();

            if (string.IsNullOrEmpty(Request.QueryString[SiteEnums.QueryStringNames.pg.ToString()]))
            {
                totalRecords = model.GetContentPageWiseAll(1, pageSize);
            }
            else
            {
                var pageNumber = Convert.ToInt32(Request.QueryString[SiteEnums.QueryStringNames.pg.ToString()]);

                totalRecords = model.GetContentPageWiseAll(pageNumber, pageSize);
            }

            ViewBag.PageCount = (totalRecords + pageSize - 1)/pageSize;

            return View(model);
        }

        #endregion

        #region user management

        private void LoadAllRoles()
        {
            var allRoles = Role.GetAllRoles();

            ViewBag.AllRoles = allRoles;
        }

        [HttpGet]
        public ActionResult UserManagement()
        {
            LoadAllRoles();
            return View();
        }

        [HttpPost]
        public ActionResult FindByEmail(string email)
        {
            var ua = new UserAccount();
            ua.GetUserAccountByEmail(email);

            if (ua.UserAccountID > 0)
            {
                ViewBag.SelectedUser = ua;
                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                ViewBag.UserAccountDetail = uad;
            }

            LoadAllRoles();

            return View("UserManagement");
        }

        [HttpPost]
        public ActionResult FindByUsername(string username)
        {
            var ua = new UserAccount(username);

            if (ua.UserAccountID > 0)
            {
                ViewBag.SelectedUser = ua;
                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                ViewBag.UserAccountDetail = uad;
            }

            LoadAllRoles();

            return View("UserManagement");
        }


        [HttpPost]
        public ActionResult UpdateRoles(int userAccountID, IEnumerable<string> roleOption)
        {
            var ua = new UserAccount(userAccountID);
            ua.IsLockedOut = (Request.Form["isLockedOut"] == null) ? false : true;
            ua.Update();

            UserAccountRole.DeleteUserRoles(userAccountID);

            if (roleOption != null)
            {
                foreach (var thenewRole in roleOption.Select(newRole => new Role(newRole)))
                {
                    UserAccountRole.AddUserToRole(userAccountID, thenewRole.RoleID);
                }
            }

            if (ua.UserAccountID > 0)
            {
                ViewBag.SelectedUser = ua;
                var uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                ViewBag.UserAccountDetail = uad;
            }

            LoadAllRoles();


            return View("UserManagement");
        }

        #endregion
    }
}