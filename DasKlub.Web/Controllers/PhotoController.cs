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
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using LitS3;

namespace DasKlub.Web.Controllers
{
    public class PhotosController : Controller
    {
        //
        // GET: /Photo/

        private const int PageSize = 25;
        private MembershipUser _mu;
        private PhotoItem _pitm;
        private PhotoItems _pitms;

        public ActionResult Index()
        {
            _pitms = new PhotoItems {UseThumb = true, ShowTitle = false};
            var cacheName = _pitm + "1";
            if (HttpRuntime.Cache[cacheName] == null)
            {
                ViewBag.TotalPhotos = _pitms.GetPhotoItemsPageWise(1, PageSize);
                HttpRuntime.Cache.AddObjToCache(_pitms, cacheName);
            }
            else
            {
                _pitms = (PhotoItems)HttpRuntime.Cache[cacheName];
                ViewBag.TotalPhotos = PageSize; //lie
            }

            return View(_pitms);
        }

        public ActionResult Detail(int photoItemID)
        {
            _pitm = new PhotoItem(photoItemID);

            var sus = new StatusUpdates();

            var su = new StatusUpdate();

            su.GetStatusUpdateByPhotoID(photoItemID);

            su.PhotoDisplay = true;
            sus.Add(su);

            if (string.IsNullOrWhiteSpace(_pitm.Title))
            {
                _pitm.Title = String.Format("{0:u}", _pitm.CreateDate);
            }

            sus.IncludeStartAndEndTags = false;
            ViewBag.StatusUpdateList = @"<ul id=""status_update_list_items"">" + sus.ToUnorderdList + @"</ul>";

            var pitm2 = new PhotoItem();

            pitm2.GetPreviousPhoto(_pitm.CreateDate);
            if (pitm2.PhotoItemID > 0)
            {
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.PreviousPhoto = pitm2;
            }

            pitm2 = new PhotoItem();
            pitm2.GetNextPhoto(_pitm.CreateDate);

            if (pitm2.PhotoItemID > 0)
            {
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.NextPhoto = pitm2;
            }

            return View(_pitm);
        }


        public ActionResult Delete(int photoItemID)
        {
            _mu = Membership.GetUser();

            _pitm = new PhotoItem(photoItemID);

            if (_mu != null && _pitm.CreatedByUserID == Convert.ToInt32(_mu.ProviderUserKey))
            {
                var s3 = new S3Service
                    {
                        AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                        SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                    };


                _pitm.Delete();


                if (!string.IsNullOrEmpty(_pitm.FilePathStandard))
                {
                    // delete the existing photos
                    try
                    {
                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, _pitm.FilePathStandard))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, _pitm.FilePathStandard);
                        }

                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, _pitm.FilePathRaw))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, _pitm.FilePathRaw);
                        }

                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, _pitm.FilePathThumb))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, _pitm.FilePathThumb);
                        }
                    }
                    catch (Exception)
                    {
                        // whatever
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public JsonResult PhotoItems(int pageNumber)
        {
            _pitms = new PhotoItems();
            _pitms.GetPhotoItemsPageWise(pageNumber, PageSize);

            _pitms.ShowTitle = false;
            _pitms.UseThumb = true;
            _pitms.IncludeStartAndEndTags = false;

            return Json(new
                {
                    ListItems = _pitms.ToUnorderdList
                });
        }
    }
}
 