using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;
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
            string cacheName = _pitm + "1";
            if (HttpRuntime.Cache[cacheName] == null)
            {
                ViewBag.TotalPhotos = _pitms.GetPhotoItemsPageWise(1, PageSize);
                HttpRuntime.Cache.AddObjToCache(_pitms, cacheName);
            }
            else
            {
                _pitms = (PhotoItems) HttpRuntime.Cache[cacheName];
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
            ViewBag.StatusUpdateList = string.Format(@"<ul id=""status_update_list_items"">{0}</ul>", sus.ToUnorderdList);

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

            if (_mu == null || _pitm.CreatedByUserID != Convert.ToInt32(_mu.ProviderUserKey))
                return RedirectToAction("Index");

            var s3 = new S3Service
            {
                AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
            };


            _pitm.Delete();


            if (string.IsNullOrEmpty(_pitm.FilePathStandard)) return RedirectToAction("Index");
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
            catch
            {
                // whatever
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