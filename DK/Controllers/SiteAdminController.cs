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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.DomainConnection;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.Configs;
using BootBaronLib.Operational;
using BootBaronLib.Values;
using DasKlub.Models;
using LitS3;

namespace DasKlub.Controllers
{
    [Authorize (Roles = "admin")]
    public class SiteAdminController : Controller
    {
        MembershipUser mu = null;

        public ActionResult SiteBranding()
        {
            SiteDomains siteDomains = new SiteDomains();

            siteDomains.GetAll();

            return View(siteDomains);
        }


        #region user management

        private void LoadAllRoles()
        {
            string[] allRoles = BootBaronLib.AppSpec.DasKlub.BOL.Role.GetAllRoles();

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
            UserAccount ua = new UserAccount();
            ua.GetUserAccountByEmail(email);

            if (ua.UserAccountID > 0)
            {
                ViewBag.SelectedUser = ua;
                UserAccountDetail uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                ViewBag.UserAccountDetail = uad;
            }

            LoadAllRoles();

            return View("UserManagement");
        }

        [HttpPost]
        public ActionResult FindByUsername(string username)
        {
            UserAccount ua = new UserAccount(username);

            if (ua.UserAccountID > 0)
            {
                ViewBag.SelectedUser = ua;
                UserAccountDetail uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                ViewBag.UserAccountDetail = uad;
            }

            LoadAllRoles();

            return View("UserManagement");
        }


        [HttpPost]
        public ActionResult UpdateRoles(int userAccountID, string[] roleOption)
        {
            UserAccount ua = new UserAccount(userAccountID);

            // delete all their roles
            UserAccountRole.DeleteUserRoles(userAccountID);

            foreach (string newRole in roleOption)
            {
               Role thenewRole = new Role(newRole);

               UserAccountRole.AddUserToRole(userAccountID, thenewRole.RoleID);
            }

            if (ua.UserAccountID > 0)
            {
                ViewBag.SelectedUser = ua;
                UserAccountDetail uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                ViewBag.UserAccountDetail = uad;
            }

            LoadAllRoles();


            return View("UserManagement");
        }

         

        #endregion

        [HttpGet]
        public ActionResult DeleteSiteDomain(int? siteDomainID)
        {
            
            if (siteDomainID != null)
            {
                SiteDomain siteDomain = new SiteDomain(Convert.ToInt32(siteDomainID));

                siteDomain.Delete();
            }

            return RedirectToAction("SiteBranding");
        }

        [HttpGet]
        public ActionResult EditSiteDomain(int? siteDomainID)
        {
            SiteDomainModel model = new SiteDomainModel();

            if (siteDomainID != null)
            {
                SiteDomain siteDomain = new SiteDomain(Convert.ToInt32( siteDomainID));

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
                SiteDomain siteDomain = new SiteDomain();

                siteDomain.Description = model.Description;
                siteDomain.Language = ( model.Language == null) ? string.Empty : model.Language;
                siteDomain.PropertyType = model.PropertyType;
                siteDomain.SiteDomainID = model.SiteDomainID;

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
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment();

            if (contentCommentID != null && contentCommentID > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment(
                    Convert.ToInt32(contentCommentID));
            }

            TryUpdateModel(model);

            if (ModelState.IsValid)
            {
                model.Update();

                return RedirectToAction("ArticleComments");
            }
            else
            {
                return View(model);
            }
        }


        [Authorize]
        [HttpGet]
        public ActionResult BlockIP(int? id)
        {
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment();

            if (id != null && id > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment(
                    Convert.ToInt32(id));

                model.Delete();

                BlackIP bip = new BlackIP();
                bip.IpAddress = model.IpAddress;
                bip.Create();
            }

            return RedirectToAction("ArticleComments");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditArticleComment(int? id)
        {
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment();

            if (id != null && id > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment(
                    Convert.ToInt32(id));
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ArticleComments()
        {

            int totalRecords = 0;
            int pageSize = 10;
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComments();

            if (string.IsNullOrEmpty(Request.QueryString[
                SiteEnums.QueryStringNames.pg.ToString()]))
            {
                totalRecords = model.GetCommentsPageWise(1, pageSize);
            }
            else
            {
                int pageNumber = Convert.ToInt32(Request.QueryString[
                    SiteEnums.QueryStringNames.pg.ToString()]);

                totalRecords = model.GetCommentsPageWise(pageNumber, pageSize);
            }

            ViewBag.PageCount = (totalRecords + pageSize - 1) / pageSize;

            return View(model);



        }


        [Authorize]
        [HttpGet]
        public ActionResult DeleteComment(int? id)
        {
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment();

            if (id != null && id > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.ContentComment(
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
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();

            if (id != null && id > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(
                    Convert.ToInt32(id));

                ContentComments concoms = new ContentComments();
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
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();

            if (model.ReleaseDate == DateTime.MinValue)
            {
                model.ReleaseDate = DateTime.UtcNow;
            }

            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditArticle(
            FormCollection fc,
            int? contentID,
            HttpPostedFileBase imageFile,
            HttpPostedFileBase videoFile)
        {

            mu = Membership.GetUser();
            // TODO: SAVE VIDEO

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();

            if (contentID != null && contentID > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(
                    Convert.ToInt32(contentID));
            }

            TryUpdateModel(model);

            ////// begin: amazon
            var acl = CannedAcl.PublicRead;

            S3Service s3 = new S3Service();

            s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
            s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;


            if (ModelState.IsValid)
            {
                model.ContentKey = FromString.URLKey(model.Title);

                if (model.ContentID == 0)
                {
                    mu = Membership.GetUser();
                    model.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);

                    if (model.ReleaseDate == DateTime.MinValue)
                    {
                        model.ReleaseDate = DateTime.UtcNow;
                    }
                }

                if (model.Set() <= 0) return View(model);

                if (imageFile != null)
                {

                    Bitmap b = new Bitmap(imageFile.InputStream);

                    System.Drawing.Image fullPhoto = (System.Drawing.Image)b;

                    string fileNameFull = Utilities.CreateUniqueContentFilename(imageFile);

                    System.Drawing.Image imgPhoto = ImageResize.FixedSize(b, 600, 400, System.Drawing.Color.Black);

                    Stream maker = imgPhoto.ToAStream(ImageFormat.Jpeg);

                    s3.AddObject(
                        maker,
                        maker.Length,
                        AmazonCloudConfigs.AmazonBucketName,
                        fileNameFull,
                        imageFile.ContentType,
                        acl);
                    
                    if (!string.IsNullOrWhiteSpace(model.ContentPhotoThumbURL))
                    {
                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentPhotoURL))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentPhotoURL);
                        }
                    }

                    model.ContentPhotoURL = fileNameFull;

                    // resized

                    string fileNameThumb = Utilities.CreateUniqueContentFilename(imageFile);

                    System.Drawing.Image imgPhotoThumb = ImageResize.FixedSize(b, 350, 250, System.Drawing.Color.Black);

                    maker = imgPhotoThumb.ToAStream(ImageFormat.Jpeg);

                    s3.AddObject(
                        maker,
                        maker.Length,
                        AmazonCloudConfigs.AmazonBucketName,
                        fileNameThumb,
                        imageFile.ContentType,
                        acl);

                    if (!string.IsNullOrWhiteSpace(model.ContentPhotoThumbURL))
                    {
                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentPhotoThumbURL))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentPhotoThumbURL);
                        }
                    }

                    model.ContentPhotoThumbURL = fileNameThumb;

                    model.Set();


                }



                if (videoFile != null)
                {
                    // full
                    try
                    {
                        string fileName3 = Utilities.CreateUniqueContentFilename(videoFile);

                        // full
                        fileName3 = Utilities.CreateUniqueContentFilename(videoFile);

                        s3.AddObject(
                         videoFile.InputStream,
                         videoFile.ContentLength,
                         AmazonCloudConfigs.AmazonBucketName,
                         fileName3,
                         videoFile.ContentType,
                         acl);


                        if (!string.IsNullOrWhiteSpace(model.ContentVideoURL))
                        {
                            if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL))
                            {
                                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL);
                            }
                        }

                        model.ContentVideoURL = fileName3;

                        model.Set();

                    }
                    catch { }

                }

                return RedirectToAction("Articles");
            }
            else
            {
                return View(model);
            }
        }



        [HttpGet]
        public ActionResult EditArticle(int? id)
        {
            ViewBag.VideoHeight = (Request.Browser.IsMobileDevice) ? 160 : 360;
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ? 285 : 640;

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();

            if (id != null && id > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(
                    Convert.ToInt32(id));
            }


            return View(model);
        }



        public ActionResult Articles()
        {
            int totalRecords = 0;
            int pageSize = 10;
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            if (string.IsNullOrEmpty(Request.QueryString[SiteEnums.QueryStringNames.pg.ToString()]))
            {
                totalRecords = model.GetContentPageWiseAll(1, pageSize);
            }
            else
            {
                int pageNumber = Convert.ToInt32(Request.QueryString[SiteEnums.QueryStringNames.pg.ToString()]);

                totalRecords = model.GetContentPageWiseAll(pageNumber, pageSize);
            }

            ViewBag.PageCount = (totalRecords + pageSize - 1) / pageSize;

            return View(model);
        }


        #endregion

    }
}
