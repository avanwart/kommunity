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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.AppSpec.DasKlub.BOL.VideoContest;
using BootBaronLib.Configs;
using BootBaronLib.Enums;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Resources;
using BootBaronLib.Values;
using DasKlub.Models;
using LitS3;

namespace DasKlub.Controllers
{
    public class AccountController : Controller
    {
        #region variables

        MembershipUser mu = null;
        UserAccountDetail uad = null;
        UserAccounts uas = null;
        UserAccount ua = null;
        UserAccounts contacts = null;
        UserConnections ucons = null;
        UserConnection ucon = null;
        UserPhotos ups = null;
        static int pageSize = 5;

        #endregion

        #region Articles

        [HttpGet]
        [Authorize]
        public ActionResult DeleteArticle(int? id)
        {
            mu = Membership.GetUser();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();

            if (id != null && id > 0)
            {
                model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(
                    Convert.ToInt32(id));

                ContentComments concoms = new ContentComments();
                concoms.GetCommentsForContent(model.ContentID, SiteEnums.CommentStatus.U);
                concoms.GetCommentsForContent(model.ContentID, SiteEnums.CommentStatus.C);

                if (model.CreatedByUserID == Convert.ToInt32(mu.ProviderUserKey))
                {
                    // security check
                    foreach (ContentComment c1 in concoms)
                        c1.Delete();

                    model.Delete();
                }

            }

            return RedirectToAction("Articles");
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateArticle()
        {
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content();

            if (model.ReleaseDate == DateTime.MinValue)
            {
                model.ReleaseDate = DateTime.UtcNow;
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult NotAllowed()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditArticle(
            FormCollection fc,
            int? contentID,
            HttpPostedFileBase imageFile,
            HttpPostedFileBase videoFile,
            HttpPostedFileBase videoFile2)
        {
            mu = Membership.GetUser();

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

            if (string.IsNullOrWhiteSpace(model.Detail) )
            {
                ModelState.AddModelError(Messages.Required, Messages.Required + ": " + Messages.Details);
            }

            if (imageFile == null && string.IsNullOrWhiteSpace(model.ContentPhotoURL))
            {
                ModelState.AddModelError(Messages.Required, Messages.Required + ": " + Messages.Photo);
            }

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



                    if (!string.IsNullOrWhiteSpace(model.ContentPhotoURL))
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



                if (videoFile2 != null)
                {
                    // full
                    try
                    {
                        string fileName3 = Utilities.CreateUniqueContentFilename(videoFile2);

                        // full
                        fileName3 = Utilities.CreateUniqueContentFilename(videoFile2);

                        s3.AddObject(
                         videoFile2.InputStream,
                         videoFile2.ContentLength,
                         AmazonCloudConfigs.AmazonBucketName,
                         fileName3,
                         videoFile2.ContentType,
                         acl);


                        if (!string.IsNullOrWhiteSpace(model.ContentVideoURL2))
                        {

                            if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL2))
                            {
                                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL2);
                            }
                        }

                        model.ContentVideoURL2 = fileName3;

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


        [Authorize]
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


        [Authorize]
        public ActionResult Articles()
        {
            mu = Membership.GetUser();

            int totalRecords = 0;
            int pageSize = 10;
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Contents();

            if (string.IsNullOrEmpty(Request.QueryString[SiteEnums.QueryStringNames.pg.ToString()]))
            {
                //totalRecords = 
                model.GetContentForUser(Convert.ToInt32(mu.ProviderUserKey));

                model.Sort(delegate(Content p1, Content p2)
                {
                    return p2.ReleaseDate.CompareTo(p1.ReleaseDate);
                });
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


        #region sign in/ out


        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool remember = Convert.ToBoolean(model.RememberMe);

                if (string.IsNullOrWhiteSpace(model.UserName) ||
                    string.IsNullOrWhiteSpace(model.Password))
                {
                    return View(model);
                }

                ua = new UserAccount(model.UserName);

                if (ua.UserAccountID == 0)
                {
                    ua = new UserAccount();
                    ua.GetUserAccountByEmail(model.UserName);

                    if (ua.UserAccountID > 0)
                    {
                        // they were stupid and put their email not username
                        model.UserName = ua.UserName;
                    }
                }

                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.RedirectFromLoginPage(model.UserName, remember);

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ua = new UserAccount(model.UserName);

                        if (ua.IsLockedOut)
                        {
                            ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.IsLockedOut);
                            return View(model);
                        }

                        ua.LastLoginDate = DateTime.UtcNow;
                        ua.FailedPasswordAttemptCount = 0;
                        ua.IsOnLine = true;
                        ua.SigningOut = false;
                        ua.Update();

                        UserAccountDetail uad = new UserAccountDetail();
                        uad.GetUserAccountDeailForUser(ua.UserAccountID);
                        //uad.DefaultLanguage = Utilities.GetCurrentLanguageCode();
                        //uad.Update();

                        if (!string.IsNullOrWhiteSpace(uad.DefaultLanguage))
                        {
                            HttpCookie hc =
                                new HttpCookie(SiteEnums.CookieName.usersetting.ToString(), uad.DefaultLanguage);

                            NameValueCollection nvc = new NameValueCollection();
                            nvc.Add(SiteEnums.CookieValue.language.ToString(), uad.DefaultLanguage);

                            Utilities.CookieMaker(SiteEnums.CookieName.usersetting, nvc);

                            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);
                            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);
                        }

                        return RedirectToAction("Home", "Account");
                    }
                }
                else
                {
                    // it updates as online, make off
                    ua = new UserAccount(model.UserName);

                    if (ua.UserAccountID > 0)
                    {
                        ua.IsOnLine = false;
                        ua.SigningOut = true;
                        ua.Update();
                    }
                }
            }

            ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.LoginUnsuccessfulPleaseCorrect);

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize]
        public ActionResult LogOff()
        {
            if (Membership.GetUser() == null)
            {
                FormsAuthentication.SignOut();

                return RedirectToAction("Index", "Home");
            }


            ua = new UserAccount(Membership.GetUser().UserName);
            ua.IsOnLine = false;
            ua.SigningOut = true;
            ua.Update();
            ua.RemoveCache();

            ChatRoomUser cru = new ChatRoomUser();

            cru.GetChatRoomUserByUserAccountID(ua.UserAccountID);
            cru.DeleteChatRoomUser();

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOn()
        {
            return View();
        }


        #endregion

        #region video vote

        private void LoadContestResults()
        {
            Contest contest = Contest.GetLastContest();
            ContestResults cResults = new ContestResults();
            cResults.GetContestVideosForContest(contest.ContestID);
            ViewBag.ContestResults = cResults;
        }

        [Authorize]
        [HttpPost]
        public ActionResult VideoVote(int video_vote)
        {
            mu = Membership.GetUser();

            Contest contest = Contest.GetLastContest();

            ViewBag.Contest = contest;

            if (ContestVideo.IsUserContestVoted(Convert.ToInt32(mu.ProviderUserKey), contest.ContestID))
            {
                LoadContestResults();
                return View();
            }

            ContestVideo convid = new ContestVideo();

            convid.GetContestVideoForContestAndVideo(video_vote, contest.ContestID);

            ContestVideoVote cvv = new ContestVideoVote();

            cvv.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);
            cvv.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
            cvv.ContestVideoID = convid.ContestVideoID;

            cvv.Create();

            LoadContestResults();

            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult VideoVote()
        {
            Contest contest = Contest.GetLastContest();

            ViewBag.Contest = contest;

            mu = Membership.GetUser();
            
            if (ContestVideo.IsUserContestVoted(Convert.ToInt32(mu.ProviderUserKey), contest.ContestID)  && contest.DeadLine.AddHours(72) > DateTime.UtcNow)
            {
                LoadContestResults();

                return View();
            }


            ContestVideos cvids = new ContestVideos();

            cvids.GetContestVideosForContest(contest.ContestID);


            Videos vidsInContest = new Videos();
            Video vid2 = null;

            foreach (ContestVideo cv1 in cvids)
            {
                vid2 = new Video(cv1.VideoID);
                vidsInContest.Add(vid2);

            }


            vidsInContest.Sort(delegate(Video p1, Video p2)
                            {
                                return p1.PublishDate.CompareTo(p2.PublishDate);
                            });

            ViewBag.ContestVideos = vidsInContest;

            //SongRecords sngrcds3 = new SongRecords();
            //SongRecord sng3 = new SongRecord();

            //foreach (Video v1 in vidsInContest)
            //{
            //    sng3 = new SongRecord(v1);

            //    sngrcds3.Add(sng3);
            //}

            //ViewBag.ContestVideoList = sngrcds3.VideosList();

            return View();
        }

        #endregion

        #region user deletion

        [Authorize]
        [HttpGet]
        public ActionResult UserDeletion()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserDeletion(NameValueCollection postedData)
        {
            mu = Membership.GetUser();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

            if (ua.IsAdmin)
            {
                // admin should not die
                return View();
            }

            if (ua.Delete(true))
            {
                FormsAuthentication.SignOut();

                return RedirectToAction("Register", "Account");
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region contact request

        [Authorize]
        [HttpPost]
        public ActionResult ContactRequest(NameValueCollection postedData)
        {
            NameValueCollection qury = Request.QueryString;
            UserAccount userToBe = new UserAccount(qury["username"]);
            UserConnection ucToSet = new UserConnection();
            mu = Membership.GetUser();

            ucToSet.GetUserToUserConnection(
                Convert.ToInt32(Membership.GetUser().ProviderUserKey), userToBe.UserAccountID);

            if (ucToSet.UserConnectionID == 0 && qury["rslt"] == "0")
            {
                // they never set anything up and are ending the request
                Response.Redirect("~/" + userToBe.UserName);
            }
            else if (ucToSet.UserConnectionID != 0 && qury["rslt"] == "0")
            {
                if (Convert.ToInt32(mu.ProviderUserKey) != ucToSet.CreatedByUserID)
                {
                    // one user sent a request and now it's rejected
                    ucToSet.StatusType = 'L';
                    ucToSet.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                    ucToSet.IsConfirmed = true;
                    ucToSet.Update();
                    Response.Redirect("/" + Membership.GetUser().UserName);
                }
                else
                {
                    // this is assumed to be not a declude because it is a reject by the issuer
                    Response.Redirect("/" + userToBe.UserName);
                }
            }
            else if (ucToSet.UserConnectionID == 0 && qury["rslt"] == "1")
            {
                // they are not yet connected and are requesting to begin a connection
                ucToSet.IsConfirmed = false;
                ucToSet.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                ucToSet.FromUserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                ucToSet.ToUserAccountID = userToBe.UserAccountID;
                ucToSet.StatusType = Convert.ToChar(qury["contacttype"]);
                ucToSet.Create();


                Response.Redirect("/" + userToBe.UserName);
            }
            else if (ucToSet.UserConnectionID > 0 && qury["rslt"] == "1")
            {
                // there was a request that is being confirmed 

                if (ucToSet.StatusType == Convert.ToChar(qury["contacttype"]))
                {
                    ucToSet.IsConfirmed = true;
                }
                else
                {
                    // upgrading the request
                    ucToSet.IsConfirmed = false;

                    if (Convert.ToInt32(mu.ProviderUserKey) == ucToSet.ToUserAccountID)
                    {
                        // the opposite this time
                        ucToSet.FromUserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                        ucToSet.ToUserAccountID = userToBe.UserAccountID;
                    }
                }
                ucToSet.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                ucToSet.StatusType = Convert.ToChar(qury["contacttype"]);
                ucToSet.Update();

                switch (Convert.ToChar(qury["contacttype"]))
                {
                    case 'R':
                        if (ucToSet.IsConfirmed)
                        {
                            Response.Redirect("~/account/irlcontacts/" + mu.UserName);
                        }
                        else
                        {
                            Response.Redirect("/" + userToBe.UserName);
                        }
                        break;
                    case 'C':
                        if (ucToSet.IsConfirmed)
                        {
                            Response.Redirect("~/account/CyberAssociates/" + mu.UserName);
                        }
                        else
                        {
                            Response.Redirect("~/" + userToBe.UserName);
                        }
                        break;
                    case 'L':
                        Response.Redirect("~/" + mu.UserName);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // ???
            }



            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ContactRequest()
        {
            NameValueCollection nvc = Request.QueryString;

            ua = new UserAccount(nvc["username"]);
            uas = new UserAccounts();

            uas.Add(ua);

            ViewBag.UserTo = uas.ToUnorderdList;
            ViewBag.ContactType = nvc["contacttype"];
            ViewBag.UserNameTo = ua.UserName;

            char contype = Convert.ToChar(nvc["contacttype"]);

            switch (contype)
            {
                case 'R':
                    ViewBag.HeaderRequest = string.Format("{0} : {1}", ua.UserName, BootBaronLib.Resources.Messages.HaveYouMetInRealLife);
                    ViewBag.HeaderRequest += string.Format(
                        @"<img src=""{0}"" />",
                        VirtualPathUtility.ToAbsolute("~/content/images/userstatus/handprint_small.png"));
                    break;
                case 'C':
                    ViewBag.HeaderRequest = string.Format("{0} : {1}", ua.UserName, BootBaronLib.Resources.Messages.WouldYouLikeToBeCyberAssociates);
                    ViewBag.HeaderRequest +=
    string.Format(
    @"<img src=""{0}"" />",
    VirtualPathUtility.ToAbsolute("~/content/images/userstatus/keyboard_small.png"));
                    break;
                default:
                    break;
            }

            return View();
        }

        #endregion

        #region contacts and visitors


        [Authorize]
        [HttpGet]
        public ActionResult DeleteContact(int userConnectionID)
        {
            mu = Membership.GetUser();

            ucon = new UserConnection(userConnectionID);

            char conType = ucon.StatusType;

            if (ucon.IsConfirmed)
            {
                ucon.Delete();
            }

            if (conType == 'R')
            {
                return RedirectToAction("irlcontacts");
            }
            else if (conType == 'C')
            {
                return RedirectToAction("CyberAssociates");
            }
            else
            {
                Utilities.LogError("deleted no existing contact type");
                // something went wrong 
                return new EmptyResult();
            }
        }


        //[Authorize]
        //[HttpPost]
        //public ActionResult DeleteContact(int contact_to_delete)
        //{
        //    mu = Membership.GetUser();

        //    ucon = new UserConnection(contact_to_delete);

        //    char conType = ucon.StatusType;

        //    if (ucon.IsConfirmed)
        //    {
        //        ucon.Delete();
        //    }

        //    if (conType == 'R')
        //    {
        //        return RedirectToAction("irlcontacts");
        //    }
        //    else if (conType == 'C')
        //    {
        //        return RedirectToAction("CyberAssociates");
        //    }

        //    return new EmptyResult();
        //}


        [Authorize]
        [HttpPost]
        public ActionResult ManageVideos(NameValueCollection nvc)
        {

            mu = Membership.GetUser();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));
            ViewBag.UserName = ua.UserName;



            UserAccountVideos plyvids = new UserAccountVideos();
            plyvids.GetVideosForUserAccount(ua.UserAccountID, 'U');

            nvc = Request.Form;

            //video_delete_id
            //video_down_id
            //video_up_id

            if (nvc["video_delete_id"] != null)
            {
                UserAccountVideo.DeleteVideoForUser(ua.UserAccountID, Convert.ToInt32(nvc["video_delete_id"]));
            }

            return View("ManageVideos");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ManageVideos()
        {
            mu = Membership.GetUser();

            UserAccountVideos uavs = new UserAccountVideos();
            uavs.GetVideosForUserAccount(Convert.ToInt32(mu.ProviderUserKey), 'U');

            if (uavs.Count > 0)
            {
                Videos favvids = new Videos();
                Video f1 = new Video();

                foreach (UserAccountVideo uav1 in uavs)
                {
                    f1 = new Video(uav1.VideoID);
                    if (f1.IsEnabled) favvids.Add(f1);
                }

                SongRecord sng1 = null;
                SongRecords sngrcds2 = new SongRecords();

                foreach (Video v1 in favvids)
                {
                    sng1 = new SongRecord(v1);
                    sngrcds2.Add(sng1);
                }

                sngrcds2.IsUserSelected = true;
                sngrcds2.EnableChangeOrder = false;

                ViewBag.UserUploaded = sngrcds2.VideoPlaylist();
            }

            return View(uavs);
        }

        [Authorize]
        [HttpGet]
        public ActionResult MyUsers()
        {
            mu = Membership.GetUser();

            ViewBag.CurrentUserName = mu.UserName;

            if (mu != null)
            {
                uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

                ViewBag.EnableProfileLogging = uad.EnableProfileLogging;

            }

            UserConnections unconfirmedUsers = new UserConnections();
            UserConnections allusrcons = new UserConnections();

            allusrcons.GetUserConnections(Convert.ToInt32(mu.ProviderUserKey));

            foreach (UserConnection uc1 in allusrcons)
            {
                if (!uc1.IsConfirmed && Convert.ToInt32(mu.ProviderUserKey) != uc1.FromUserAccountID)
                {
                    unconfirmedUsers.Add(uc1);
                }
            }

            ViewBag.ApprovalList = unconfirmedUsers.ToUnorderdList;

            ViewBag.BlockedUsers = BootBaronLib.AppSpec.DasKlub.BOL.BlockedUsers.HasBlockedUsers(Convert.ToInt32(mu.ProviderUserKey));


            return View();
        }

        public void GetContactsForUser(string userName, char contactType)
        {
            ua = new UserAccount(userName);

            //  if (Membership.GetUser().UserName != ua.UserName) return View();

            if (ua.UserAccountID == 0) return;

            contacts = new UserAccounts();

            UserAccount ua1 = null;

            ucons = new UserConnections();
            ucons.GetUserConnections(ua.UserAccountID);

            foreach (UserConnection uc1 in ucons)
            {
                if (!uc1.IsConfirmed || uc1.StatusType != contactType) continue;

                if (uc1.FromUserAccountID == ua.UserAccountID)
                {
                    ua1 = new UserAccount(uc1.ToUserAccountID);
                }
                else
                {
                    ua1 = new UserAccount(uc1.FromUserAccountID);
                }

                if (ua1.IsApproved && !ua1.IsLockedOut)
                {
                    contacts.Add(ua1);
                }

            }

            ViewBag.UserName = ua.UserName;
            ViewBag.ContactsCount = Convert.ToString(contacts.Count);
            ViewBag.Contacts = contacts.ToUnorderdList;
        }

        [Authorize]
        [HttpGet]
        public ActionResult CyberAssociates(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) userName = User.Identity.Name;

            GetContactsForUser(userName, 'C');

            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult IRLContacts(string userName)
        {

            if (string.IsNullOrWhiteSpace(userName)) userName = User.Identity.Name;

            GetContactsForUser(userName, 'R');


            return View();

        }


        [Authorize]
        [HttpGet]
        public ActionResult Visitors()
        {
            mu = Membership.GetUser();
            uad = new UserAccountDetail();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

            if (ua.IsAdmin)
            {
                ViewBag.AdminLink = "/m/auth/default.aspx";
            }


            uad.GetUserAccountDeailForUser(ua.UserAccountID);
            UserAccountDetail uadLooker = new UserAccountDetail();
            uadLooker.GetUserAccountDeailForUser(ua.UserAccountID);

            UserConnections unconfirmedUsers = new UserConnections();
            UserConnections allusrcons = new UserConnections();

            allusrcons.GetUserConnections(Convert.ToInt32(mu.ProviderUserKey));

            foreach (UserConnection uc1 in allusrcons)
            {
                if (!uc1.IsConfirmed && Convert.ToInt32(mu.ProviderUserKey) != uc1.FromUserAccountID)
                {
                    unconfirmedUsers.Add(uc1);
                }
            }

            ViewBag.ApprovalList = unconfirmedUsers.ToUnorderdList;

            ViewBag.BlockedUsers = BootBaronLib.AppSpec.DasKlub.BOL.BlockedUsers.HasBlockedUsers(Convert.ToInt32(mu.ProviderUserKey));

            ViewBag.EnableProfileLogging = uad.EnableProfileLogging;

            LoadVisitorsView(ua.UserName);


            return View();
        }

        private void LoadVisitorsView(string userName)
        {
            int maxcountusers = 100;
            mu = Membership.GetUser();
            uad = new UserAccountDetail();
            ua = new UserAccount(userName);
            uad.GetUserAccountDeailForUser(ua.UserAccountID);
            UserAccountDetail uadLooker = new UserAccountDetail();
            uadLooker.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

            if (mu != null && ua.UserAccountID > 0 &&
                uad.EnableProfileLogging && uad.EnableProfileLogging)
            {
                ArrayList al = ProfileLog.GetRecentProfileViews(ua.UserAccountID);

                if (al != null && al.Count > 0)
                {
                    UserAccounts uas = new UserAccounts();

                    UserAccount viewwer = null;

                    foreach (int ID in al)
                    {
                        viewwer = new UserAccount(ID);
                        if (!viewwer.IsLockedOut && viewwer.IsApproved)
                        {
                            uad = new UserAccountDetail();
                            uad.GetUserAccountDeailForUser(ID);
                            if (uad.EnableProfileLogging == false) continue;

                            if (uas.Count >= maxcountusers) break;

                            uas.Add(viewwer);
                        }
                    }

                    ViewBag.TheViewers = uas.ToUnorderdList;
                }
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult ProfileVisitors(string userName)
        {

            LoadVisitorsView(userName);

            return View();
        }

        #endregion

        #region user address

        [Authorize]
        [HttpPost]
        public ActionResult UserAddress(UserAddressModel model)
        {
            LoadCountries();

            mu = Membership.GetUser();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

            uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(ua.UserAccountID);

            BootBaronLib.AppSpec.DasKlub.BOL.UserAddress uadress = new UserAddress();

            uadress.GetUserAddress(ua.UserAccountID);

            TryUpdateModel(model);

            if (ModelState.IsValid)
            {
                uadress.AddressLine1 = model.AddressLine1;
                uadress.AddressLine2 = model.AddressLine2;
                uadress.AddressLine3 = model.AddressLine3;
                uadress.City = model.City;
                uadress.CountryISO = model.Country;
                uadress.FirstName = model.FirstName;
                uadress.LastName = model.LastName;
                uadress.PostalCode = model.PostalCode;
                uadress.Region = model.RegionState;
                uadress.UserAccountID = ua.UserAccountID;

                if (uadress.UserAddressID == 0) uadress.AddressStatus = 'U';

                ViewBag.ProfileUpdated = uadress.Set();
            }
            //if (BootBaronLib.AppSpec.DasKlub.BOL.UserAddress.IsBlank(ua.UserAccountID))
            //{
            //    newUAdd.UserAccountID = ua.UserAccountID;
            //    newUAdd.CreatedByUserID = ua.UserAccountID;

            //    if (Request.Form["no_button"] != null &&
            //        Request.Form["no_button"] == "no")
            //    {
            //        newUAdd.AddressStatus = 'N';
            //    }
            //    else
            //    {
            //        newUAdd.AddressStatus = 'U';
            //    }

            //    if (!string.IsNullOrEmpty(newUAdd.PostalCode) && (string.IsNullOrEmpty(newUAdd.City) || string.IsNullOrEmpty(newUAdd.Region)))
            //    {
            //        // for those those who think the system can figure them out
            //        SiteEnums.CountryCodeISO coiso = GeoData.GetCountryISOForCountryCode(newUAdd.CountryISO);

            //        SiteStructs.CityRegion cr = GeoData.GetCityRegionForPostalCodeCountry(newUAdd.PostalCode, coiso);

            //        newUAdd.Region = cr.Region;
            //        newUAdd.City = cr.CityName;
            //    }

            //    newUAdd.Create();
            //}
            ////Response.Redirect("/" + ua.UserName);
            //Response.Redirect("~/thanks.htm");

            //uad = new UserAccountDetail();
            //uad.GetUserAccountDeailForUser(ua.UserAccountID);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult UserAddress()
        {


            LoadCountries();

            mu = Membership.GetUser();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));



            uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(ua.UserAccountID);

            UserAddressModel model = new UserAddressModel();

            BootBaronLib.AppSpec.DasKlub.BOL.UserAddress uadress = new UserAddress();

            uadress.GetUserAddress(ua.UserAccountID);

            if (BootBaronLib.Configs.GeneralConfigs.IsGiveAway && uadress.UserAddressID > 0) return View("NotAllowed");

            if (uadress.UserAddressID == 0)
            {
                model.PostalCode = uad.PostalCode;
                model.Country = uad.Country;
            }
            else
            {
                model.AddressLine1 = uadress.AddressLine1;
                model.AddressLine2 = uadress.AddressLine2;
                model.AddressLine3 = uadress.AddressLine3;
                model.City = uadress.City;
                model.Country = uadress.CountryISO;
                model.FirstName = uadress.FirstName;
                model.LastName = uadress.LastName;
                model.PostalCode = uadress.PostalCode;
                model.RegionState = uadress.Region;
            }

            return View(model);
        }

        #endregion


        #region blocking


        [Authorize]
        [HttpGet]
        public ActionResult BlockedUsers()
        {
            mu = Membership.GetUser();

            ua = new UserAccount(mu.UserName);

            if (ua.UserAccountID == 0) return View();

            contacts = new UserAccounts();

            UserAccount ua1 = null;

            BlockedUsers bus = new BootBaronLib.AppSpec.DasKlub.BOL.BlockedUsers();

            bus.GetBlockedUsers(Convert.ToInt32(mu.ProviderUserKey));

            foreach (BlockedUser uc1 in bus)
            {
                ua1 = new UserAccount(uc1.UserAccountIDBlocked);

                contacts.Add(ua1);
            }

            ViewBag.BlockedUsers = contacts.ToUnorderdList;

            return View();
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult BlockedUser(NameValueCollection nvc)
        //{
        //    nvc = Request.Form;

        //    string userToBlock = nvc["user_to_block"];
        //    string userToUnBlock = nvc["user_to_unblock"];

        //    mu = Membership.GetUser();

        //    if (!string.IsNullOrEmpty(userToBlock))
        //    {
        //        BootBaronLib.AppSpec.DasKlub.BOL.BlockedUser bu = new BlockedUser();

        //        ucon = new UserConnection();
        //        ucon.GetUserToUserConnection(
        //            Convert.ToInt32(userToBlock), Convert.ToInt32(mu.ProviderUserKey));
        //        ucon.Delete();

        //        bu.UserAccountIDBlocked = Convert.ToInt32(userToBlock);
        //        bu.UserAccountIDBlocking = Convert.ToInt32(mu.ProviderUserKey);
        //        bu.Create();
        //    }
        //    else if (!string.IsNullOrEmpty(userToUnBlock))
        //    {
        //        BootBaronLib.AppSpec.DasKlub.BOL.BlockedUser.Delete(
        //            Convert.ToInt32(mu.ProviderUserKey), Convert.ToInt32(userToUnBlock));
        //    }

        //    return RedirectToAction("MyUsers");
        //}





        [Authorize]
        [HttpGet]
        public ActionResult ReportUser(int userAccountID)
        {

            mu = Membership.GetUser();

            ua = new UserAccount(userAccountID);

            Utilities.SendMail(BootBaronLib.Configs.GeneralConfigs.SendToErrorEmail, Messages.Report + ": " + Messages.UserAccount, Messages.Report + Environment.NewLine
                + Environment.NewLine + Messages.UserName + ": " + ua.UserName + Environment.NewLine + Messages.UserAccount + ": " + ua.UserAccountID.ToString() +
                Environment.NewLine + Environment.NewLine + "----------" + Environment.NewLine + Messages.From + ": " + mu.UserName);

            return RedirectToAction("Visitors");
        }




        [Authorize]
        [HttpGet]
        public ActionResult BlockedUser(int userAccountID)
        {
            mu = Membership.GetUser();

            BootBaronLib.AppSpec.DasKlub.BOL.BlockedUser bu = new BlockedUser();

            ucon = new UserConnection();
            ucon.GetUserToUserConnection(userAccountID, Convert.ToInt32(mu.ProviderUserKey));
            ucon.Delete();

            bu.UserAccountIDBlocked = userAccountID;
            bu.UserAccountIDBlocking = Convert.ToInt32(mu.ProviderUserKey);
            bu.Create();

            return RedirectToAction("Visitors");
        }

        #endregion

        #region chatroom

        [Authorize]
        [HttpGet]
        public ActionResult Chatroom()
        {
            return View();
        }

        public ActionResult Handler()
        {

            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult Room()
        {


            return View();
        }

        #endregion


        #region edit profile

        [Authorize]
        [HttpGet]
        public ActionResult EditProfile()
        {
            ViewBag.IsValid = true;

            mu = Membership.GetUser();

            uad = new UserAccountDetail();

            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

            LoadCountries();

            InterestIdentityViewBags();

            return View(uad);
        }

        private void InterestIdentityViewBags()
        {

            ///
            var interins = new InterestedIns();
            interins.GetAll();
            interins.Sort(delegate(InterestedIn p1, InterestedIn p2)
            {
                return p1.LocalizedName.CompareTo(p2.LocalizedName);
            });
            ViewBag.InterestedIns = interins.Select(x => new { InterestedInID = x.InterestedInID, LocalizedName = x.LocalizedName });


            ///
            var relationshipStatuses = new RelationshipStatuses();
            relationshipStatuses.GetAll();
            relationshipStatuses.Sort(delegate(RelationshipStatus p1, RelationshipStatus p2)
            {
                return p1.LocalizedName.CompareTo(p2.LocalizedName);
            });
            ViewBag.RelationshipStatuses = relationshipStatuses.Select(x => new { RelationshipStatusID = x.RelationshipStatusID, LocalizedName = x.LocalizedName });

            ///
            var youAres = new YouAres();
            youAres.GetAll();
            youAres.Sort(delegate(YouAre p1, YouAre p2)
            {
                return p1.LocalizedName.CompareTo(p2.LocalizedName);
            });
            ViewBag.YouAres = youAres.Select(x => new { YouAreID = x.YouAreID, LocalizedName = x.LocalizedName });
        }


        private void LoadCountries()
        {

            System.Collections.Generic.Dictionary<string, string> countryOptions = new Dictionary<string, string>();

            foreach (int value in System.Enum.GetValues(typeof(SiteEnums.CountryCodeISO)))
            {
                SiteEnums.CountryCodeISO countryCode =
                    (SiteEnums.CountryCodeISO)Enum.Parse(typeof(SiteEnums.CountryCodeISO),
                    Enum.GetName(typeof(SiteEnums.CountryCodeISO), value));

                if (countryCode != SiteEnums.CountryCodeISO.U0 &&
                     countryCode != SiteEnums.CountryCodeISO.RD)
                {
                    countryOptions.Add(countryCode.ToString(), Utilities.ResourceValue(
                        Utilities.GetEnumDescription(countryCode)));
                }
            }

            var items = from k in countryOptions.Keys
                        orderby countryOptions[k] ascending
                        select k;


            ViewBag.CountryOptions = items;
        }


        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(UserAccountDetail uad)
        {
            // must change culture because decimal will not be correct for long/ lat
            string currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

            LoadCountries();
            InterestIdentityViewBags();

            mu = Membership.GetUser();

            UserAccountDetail uadCurrent = new UserAccountDetail();
            uadCurrent.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);
            uadCurrent.GetUserAccountDeailForUser(uadCurrent.UserAccountID);

            ViewBag.IsValid = true;
            ViewBag.ProfileUpdated = false;

            DateTime dt = new DateTime();

            if (DateTime.TryParse(Request.Form["birthyear"]
                + "-" + Request.Form["birthmonth"] + "-" + Request.Form["birthday"], out dt))
            {
                uad.BirthDate = dt;
            }
            else
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.BirthDate);
                return View(uad);
            }

            if (string.IsNullOrEmpty(uad.Country) || uad.Country == Messages.DashSelect)
            {
                uad.Country = string.Empty;
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.Country);
                return View(uad);
            }

            if (string.IsNullOrEmpty(uad.PostalCode))
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.PostalCode);
                return View(uad);
            }

            if (uad.YouAreID == null)
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.YouAre);
                return View(uad);
            }

            if (uad.InterestedInID == null)
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.InterestedIn);
                return View(uad);
            }

            if (!string.IsNullOrEmpty(uad.ExternalURL.Trim()) &&
                !Uri.IsWellFormedUriString(uad.ExternalURL, UriKind.Absolute))
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.Website);
                return View(uad);
            }

            bool isNewProfile = false;

            if (string.IsNullOrEmpty(uad.Country.Trim()))
            {
                isNewProfile = true;
            }

            uadCurrent.AboutDesc = uad.AboutDesc;
            uadCurrent.HardwareSoftware = uad.HardwareSoftware;
            uadCurrent.BirthDate = uad.BirthDate;
            uadCurrent.YouAreID = uad.YouAreID;
            uadCurrent.ExternalURL = uad.ExternalURL;
            uadCurrent.Country = uad.Country;
            uadCurrent.PostalCode = uad.PostalCode;
            uadCurrent.BandsSeen = uad.BandsSeen;
            uadCurrent.BandsToSee = uad.BandsToSee;
            uadCurrent.RelationshipStatusID = uad.RelationshipStatusID;
            uadCurrent.InterestedInID = uad.InterestedInID;
            uadCurrent.FirstName = uad.FirstName;
            uadCurrent.LastName = uad.LastName;

            if (!string.IsNullOrWhiteSpace(uad.Country) &&
                !string.IsNullOrWhiteSpace(uad.PostalCode))
            {
                SiteStructs.LatLong latlong =
                GeoData.GetLatLongForCountryPostal(uad.Country, uad.PostalCode);

                if (latlong.latitude != 0 && latlong.longitude != 0)
                {
                    uad.Latitude = Convert.ToDecimal(latlong.latitude);
                    uad.Longitude = Convert.ToDecimal(latlong.longitude);

                    uadCurrent.Latitude = uad.Latitude;
                    uadCurrent.Longitude = uad.Longitude;
                }
            }

            if (uadCurrent.Set() > 0)
            {
                ViewBag.ProfileUpdated = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.Error);
            }


            if (isNewProfile)
            {
                return RedirectToAction("EditPhoto");
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

            return View(uad);
        }

        #endregion

        #region photos



        #endregion

        #region edit photo


        [Authorize]
        [HttpPost]
        public ActionResult EditPhotoDelete()
        {
            string str = Request.Form["delete_photo"];

            mu = Membership.GetUser();
            uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));



            S3Service s3 = new S3Service();

            s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
            s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;


            if (str == "1")
            {
                try
                {


                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, uad.ProfilePicURL))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, uad.ProfilePicURL);
                    }

                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, uad.ProfileThumbPicURL))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, uad.ProfileThumbPicURL);
                    }

                }
                catch
                {
                    // whatever
                }

                uad.ProfileThumbPicURL = string.Empty;
                uad.ProfilePicURL = string.Empty;
                uad.Update();
            }
            else if (str == "2" || str == "3")
            {
                ups = new UserPhotos();
                ups.GetUserPhotos(uad.UserAccountID);

                foreach (UserPhoto up1 in ups)
                {
                    try
                    {
                        if ((up1.RankOrder == 1 && str == "2") || (up1.RankOrder == 2 && str == "3"))
                        {
                            if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, up1.PicURL))
                            {
                                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, up1.PicURL);
                            }

                            if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, up1.ThumbPicURL))
                            {
                                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, up1.ThumbPicURL);
                            }

                            up1.Delete();
                        }
                    }
                    catch
                    {
                        // whatever
                    }
                }

                // update ranking
                ups = new UserPhotos();
                ups.GetUserPhotos(uad.UserAccountID);

                if (ups.Count == 1 && ups[0].RankOrder == 2)
                {
                    ups[0].RankOrder = 1;
                    ups[0].Update();
                }
            }



            Response.Redirect("~/account/editphoto");

            return new EmptyResult();
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditPhoto(HttpPostedFileBase file)
        {
            mu = Membership.GetUser();
            UserPhoto up1 = null;
            int swapID = 0;

            var acl = CannedAcl.PublicRead;

            S3Service s3 = new S3Service();

            s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
            s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;


            if (Request.Form["new_default"] != null &&
                int.TryParse(Request.Form["new_default"], out swapID))
            {
                // swap the default with the new default
                uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

                string currentDefaultMain = uad.ProfilePicURL;
                string currentDefaultMainThumb = uad.ProfileThumbPicURL;

                up1 = new UserPhoto(swapID);

                uad.ProfilePicURL = up1.PicURL;
                uad.ProfileThumbPicURL = up1.ThumbPicURL;
                uad.LastPhotoUpdate = DateTime.UtcNow;
                uad.Update();

                up1.PicURL = currentDefaultMain;
                up1.ThumbPicURL = currentDefaultMainThumb;
                up1.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                up1.Update();

                LoadCurrentImagesViewBag(Convert.ToInt32(mu.ProviderUserKey));

                return View(uad);
            }

            string photoOne = "photo_edit_1";
            string photoTwo = "photo_edit_2";
            string photoThree = "photo_edit_3";

            LoadCurrentImagesViewBag(Convert.ToInt32(mu.ProviderUserKey));

            uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

            if (file == null)
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, BootBaronLib.Resources.Messages.NoFile);
                return View(uad);
            }

            string photoEdited = Request.Form["photo_edit"];
            string mainPhotoToDelete = string.Empty;
            string thumbPhotoToDelete = string.Empty;

            ups = new UserPhotos();
            ups.GetUserPhotos(uad.UserAccountID);

            if (string.IsNullOrEmpty(uad.ProfilePicURL) ||
                ups.Count == 2 && photoEdited == photoOne)
            {
                mainPhotoToDelete = uad.ProfilePicURL;
                thumbPhotoToDelete = uad.ProfileThumbPicURL;
            }
            else
            {
                if (ups.Count > 1 && photoEdited == photoTwo)
                {
                    up1 = new UserPhoto(ups[0].UserPhotoID);
                    up1.RankOrder = 1;
                    mainPhotoToDelete = up1.PicURL;
                    thumbPhotoToDelete = up1.ThumbPicURL;
                }
                else if (ups.Count > 1 && photoEdited == photoThree)
                {
                    up1 = new UserPhoto(ups[1].UserPhotoID);
                    up1.RankOrder = 2;
                    mainPhotoToDelete = ups[1].FullProfilePicURL;
                    thumbPhotoToDelete = up1.ThumbPicURL;
                }

            }


            if (!string.IsNullOrEmpty(mainPhotoToDelete))
            {
                // delete the existing photos
                try
                {

                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, mainPhotoToDelete))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, mainPhotoToDelete);
                    }

                    if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, thumbPhotoToDelete))
                    {
                        s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, thumbPhotoToDelete);
                    }
                }
                catch
                {
                    // whatever
                }
            }


            Bitmap b = new Bitmap(file.InputStream);

            // full
            System.Drawing.Image fullPhoto = (System.Drawing.Image)b;

            fullPhoto = ImageResize.FixedSize(fullPhoto, 300, 300, System.Drawing.Color.Black);

            string fileNameFull = Utilities.CreateUniqueContentFilename(file);

            Stream maker = fullPhoto.ToAStream(ImageFormat.Jpeg);

            s3.AddObject(
                maker,
                maker.Length,
                AmazonCloudConfigs.AmazonBucketName,
                fileNameFull,
                file.ContentType,
                acl);

            if (string.IsNullOrEmpty(uad.ProfileThumbPicURL) ||
                ups.Count == 2 && photoEdited == photoOne)
            {
                uad.ProfilePicURL = fileNameFull;
            }
            else
            {
                if (up1 == null)
                {
                    up1 = new UserPhoto();
                }

                up1.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                up1.PicURL = fileNameFull;

                if ((ups.Count > 0 && photoEdited == photoTwo) || (ups.Count == 0))
                {
                    up1.RankOrder = 1;
                }
                else if ((ups.Count > 1 && photoEdited == photoThree) || ups.Count == 1)
                {
                    up1.RankOrder = 2;
                }

                if (ups.Count == 1 && ups[0].RankOrder == 2)
                {
                    ups[0].RankOrder = 1;
                    ups[0].Update();
                }
            }


            fullPhoto = (System.Drawing.Image)b;

            fullPhoto = ImageResize.FixedSize(fullPhoto, 75, 75, System.Drawing.Color.Black);

            fileNameFull = Utilities.CreateUniqueContentFilename(file);

            maker = fullPhoto.ToAStream(ImageFormat.Jpeg);

            s3.AddObject(
                maker,
                maker.Length,
                AmazonCloudConfigs.AmazonBucketName,
                fileNameFull,
                file.ContentType,
                acl);



            //// thumb

            if (string.IsNullOrEmpty(uad.ProfileThumbPicURL) ||
                ups.Count == 2 && photoEdited == photoOne)
            {
                uad.ProfileThumbPicURL = fileNameFull;
                uad.LastPhotoUpdate = DateTime.UtcNow;
                uad.Set();
            }
            else
            {
                up1.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                up1.ThumbPicURL = fileNameFull;

                if (
                    (ups.Count == 0 && photoEdited == photoTwo) ||
                    (ups.Count > 0 && photoEdited == photoTwo)
                    )
                {
                    up1.RankOrder = 1;
                }
                else if
                    (
                    (ups.Count == 0 && photoEdited == photoThree) ||
                    (ups.Count > 1 && photoEdited == photoThree)
                    )
                {
                    up1.RankOrder = 2;
                }
            }

            b.Dispose();

            if (up1 != null && up1.UserPhotoID == 0)
            {
                up1.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                up1.Create();
            }
            else if (up1 != null && up1.UserPhotoID > 0)
            {
                up1.UpdatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                up1.Update();
            }


            LoadCurrentImagesViewBag(Convert.ToInt32(mu.ProviderUserKey));

            return View(uad);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditPhoto()
        {
            mu = Membership.GetUser();
            uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

            LoadCurrentImagesViewBag(Convert.ToInt32(mu.ProviderUserKey));

            return View(uad);
        }

        private void LoadCurrentImagesViewBag(int userAccountID)
        {

            UserPhotos ups = new UserPhotos();
            ups.GetUserPhotos(userAccountID);

            if (ups.Count == 0)
            {
                UserPhoto up = new UserPhoto();
                ups.Add(up);
                up = new UserPhoto();
                ups.Add(up);
            }
            else if (ups.Count == 1)
            {
                UserPhoto up = new UserPhoto();
                up.RankOrder = 2;
                ups.Add(up);
            }

            ViewBag.SecondUserPhotoFull = ups[0].FullProfilePicURL;
            ViewBag.SecondUserPhotoThumb = ups[0].FullProfilePicThumbURL;
            ViewBag.SecondUserPhotoID = ups[0].UserPhotoID;

            ViewBag.ThirdUserPhotoFull = ups[1].FullProfilePicURL;
            ViewBag.ThirdUserPhotoThumb = ups[1].FullProfilePicThumbURL;
            ViewBag.ThirdUserPhotoID = ups[1].UserPhotoID;

        }



        #endregion

        #region mail

        [Authorize]
        public JsonResult OutboxMailItems(int pageNumber)
        {
            mu = Membership.GetUser();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.DirectMessages();

            model.GetMailPageWiseFromUser(pageNumber, pageSize, Convert.ToInt32(mu.ProviderUserKey));

            StringBuilder sb = new StringBuilder();

            model.AllInInbox = false;

            return Json(new
            {
                ListItems = model.ToUnorderdList
            });
        }


        [Authorize]
        public JsonResult ReplyMailItems(int pageNumber)
        {
            // basing the user on the referrer, this will be blank if it uses SSL, change then

            string referrring = Request.UrlReferrer.ToString();
            string[] partsOfreferring = referrring.Split('/');
            UserAccount ua = new UserAccount(partsOfreferring[partsOfreferring.Length - 1]);

            mu = Membership.GetUser();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.DirectMessages();

            model.GetMailPageWiseToUser(pageNumber, pageSize,
                Convert.ToInt32(mu.ProviderUserKey), ua.UserAccountID);

            StringBuilder sb = new StringBuilder();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.DirectMessage cnt in model)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
            {
                ListItems = sb.ToString()
            });
        }

        [Authorize]
        public JsonResult MailItems(int pageNumber)
        {
            mu = Membership.GetUser();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.DirectMessages();

            model.GetMailPageWise(pageNumber, pageSize, Convert.ToInt32(mu.ProviderUserKey));

            StringBuilder sb = new StringBuilder();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.DirectMessage cnt in model)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }


            foreach (DirectMessage dm in model)
            {
                if (!dm.IsRead)
                {
                    dm.IsRead = true;
                    dm.Update();
                }
            }

            return Json(new
            {
                ListItems = sb.ToString()
            });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Inbox()
        {
            mu = Membership.GetUser();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.DirectMessages();

            ViewBag.RecordCount = model.GetMailPageWise(1, pageSize, Convert.ToInt32(mu.ProviderUserKey));

            StringBuilder sb = new StringBuilder();

            ViewBag.DirectMessages = model.ToUnorderdList;

            foreach (DirectMessage dm in model)
            {
                if (!dm.IsRead)
                {
                    dm.IsRead = true;
                    dm.Update();
                }
            }



            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult Send()
        {
            string displayname = Request.Form["displayname"];
            string msg = Request.Form["message"];

            ua = new UserAccount(displayname);
            mu = Membership.GetUser();

            if (string.IsNullOrEmpty(msg) ||
                BootBaronLib.AppSpec.DasKlub.BOL.
                BlockedUser.IsBlockedUser(ua.UserAccountID, Convert.ToInt32(mu.ProviderUserKey))
                )
            {
                return RedirectToAction("Reply", new { @userName = displayname });
            }

            DirectMessage dm = new DirectMessage();

            //dm.GetMostRecentSentMessage(Convert.ToInt32(mu.ProviderUserKey));

            //DateTime dbtime = Utilities.GetDataBaseTime();

            //TimeSpan tsLastPst = dbtime.Subtract(dm.CreateDate);

            //if (tsLastPst.TotalSeconds < 5 && tsLastPst.TotalSeconds > 0)
            //{
            //    return RedirectToAction("Reply", new { @userName = displayname });
            //}

            dm = new DirectMessage();
            dm.IsRead = false;
            dm.FromUserAccountID = Convert.ToInt32(mu.ProviderUserKey);
            dm.ToUserAccountID = ua.UserAccountID;
            dm.Message = msg;
            dm.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);

            dm.Create();



            string language = Utilities.GetCurrentLanguageCode();
            // change language for message to

            uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(ua.UserAccountID);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);

            StringBuilder sb = new StringBuilder();

            sb.Append(BootBaronLib.Resources.Messages.Hello);
            sb.Append(",");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("{0}: ", BootBaronLib.Resources.Messages.From);
            sb.Append(BootBaronLib.Configs.GeneralConfigs.SiteDomain);
            sb.Append("/");
            sb.Append(mu.UserName);
            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("{0}: ", BootBaronLib.Resources.Messages.Message);
            sb.AppendLine(msg);
            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("{0}: {1}", BootBaronLib.Resources.Messages.SignIn, BootBaronLib.Configs.GeneralConfigs.SiteDomain);
            sb.AppendLine(Environment.NewLine);


            if (uad.EmailMessages)
            {
                Utilities.SendMail(ua.EMail,
                    BootBaronLib.Resources.Messages.From + ": " + mu.UserName, sb.ToString());
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

            return RedirectToAction("Reply", new { @userName = displayname });
        }



        [Authorize]
        [HttpGet]
        public ActionResult Outbox()
        {
            mu = Membership.GetUser();

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.DirectMessages();

            ViewBag.RecordCount = model.GetMailPageWiseFromUser(1, pageSize, Convert.ToInt32(mu.ProviderUserKey));

            StringBuilder sb = new StringBuilder();

            model.AllInInbox = false;

            ViewBag.DirectMessages = model.ToUnorderdList;

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Reply(string userName)
        {
            ViewBag.DisplayName = userName;

            mu = Membership.GetUser();
            ua = new UserAccount(userName);

            var model = new BootBaronLib.AppSpec.DasKlub.BOL.DirectMessages();

            ViewBag.RecordCount = model.GetMailPageWiseToUser(1, pageSize, Convert.ToInt32(mu.ProviderUserKey), ua.UserAccountID);

            ViewBag.DirectMessages = model.ToUnorderdList;

            //foreach (DirectMessage dm in model)
            //{
            //    if (!dm.IsRead)
            //    {
            //        dm.IsRead = true;
            //        dm.Update();
            //    }
            //}

            return View();
        }

        #endregion

        #region playlist

        [HttpPost]
        [Authorize]
        public ActionResult Playlist(NameValueCollection nvc)
        {
            mu = Membership.GetUser();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));
            ViewBag.UserName = ua.UserName;

            BootBaronLib.AppSpec.DasKlub.BOL.Playlist plyst = new Playlist();

            plyst.GetUserPlaylist(ua.UserAccountID);

            ViewBag.UserPlaylistID = plyst.PlaylistID;

            PlaylistVideos plyvids = new PlaylistVideos();
            plyvids.GetPlaylistVideosForPlaylist(plyst.PlaylistID);

            nvc = Request.Form;

            //video_delete_id
            //video_down_id
            //video_up_id

            PlaylistVideo plv = null;

            if (nvc["video_delete_id"] != null)
            {
                foreach (PlaylistVideo plv1 in plyvids)
                {
                    if (plv != null && plv1.RankOrder > plv.RankOrder)
                    {
                        plv1.RankOrder--;
                        plv1.UpdatedByUserID = ua.UserAccountID;
                        plv1.Update();
                    }

                    if (plv1.PlaylistID == plyst.PlaylistID &&
                        Convert.ToInt32(nvc["video_delete_id"]) == plv1.VideoID)
                    {
                        plv = new PlaylistVideo(plv1.PlaylistVideoID);

                        if (PlaylistVideo.Delete(plyst.PlaylistID, Convert.ToInt32(nvc["video_delete_id"])))
                        {
                            // deleted
                        }
                    }
                }
            }
            else if (nvc["video_down_id"] != null)
            {
                plv = new PlaylistVideo();
                plv.Get(plyst.PlaylistID, Convert.ToInt32(nvc["video_down_id"]));

                foreach (PlaylistVideo plv1 in plyvids)
                {
                    if (plv1.RankOrder == (plv.RankOrder + 1))
                    {
                        plv1.RankOrder--;
                        plv1.UpdatedByUserID = ua.UserAccountID;
                        plv1.Update();
                    }
                }

                plv.RankOrder++;
                plv.UpdatedByUserID = ua.UserAccountID;
                plv.Update();
            }
            else if (nvc["video_up_id"] != null)
            {
                plv = new PlaylistVideo();
                plv.Get(plyst.PlaylistID, Convert.ToInt32(nvc["video_up_id"]));

                foreach (PlaylistVideo plv1 in plyvids)
                {
                    if (plv1.RankOrder == (plv.RankOrder - 1))
                    {
                        plv1.RankOrder++;
                        plv1.UpdatedByUserID = ua.UserAccountID;
                        plv1.Update();
                    }
                }

                plv.RankOrder--;
                plv.UpdatedByUserID = ua.UserAccountID;
                plv.Update();
            }
            else //if (nvc["selected_autoplay"] != null)
            {
                if (!string.IsNullOrEmpty(nvc["selected_autoplay"]) && nvc["selected_autoplay"] == "on")
                {
                    plyst.AutoPlay = true;
                }
                else plyst.AutoPlay = false;

                plyst.Update();
            }

            Response.Redirect("~/account/playlist");
            return new EmptyResult();
        }


        [Authorize]
        [HttpGet]
        public ActionResult Playlist()
        {
            ViewBag.VideoHeight = (Request.Browser.IsMobileDevice) ? 100 : 277;
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ? 225 : 400;

            mu = Membership.GetUser();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

            ViewBag.UserName = ua.UserName;

            BootBaronLib.AppSpec.DasKlub.BOL.Playlist plyst = new Playlist();

            plyst.GetUserPlaylist(ua.UserAccountID);

            ViewBag.AutoPlay = plyst.AutoPlay;

            ViewBag.AutoPlayNumber = (plyst.AutoPlay) ? 1 : 0;

            ViewBag.UserPlaylistID = plyst.PlaylistID;

            PlaylistVideos plyvids = new PlaylistVideos();

            plyvids.GetPlaylistVideosForPlaylist(plyst.PlaylistID);

            BootBaronLib.AppSpec.DasKlub.BOL.Videos vids = new BootBaronLib.AppSpec.DasKlub.BOL.Videos();
            Video vid = null;

            foreach (PlaylistVideo plv in plyvids)
            {
                vid = new Video(plv.VideoID);
                vids.Add(vid);
            }

            SongRecords sngrcs = new SongRecords();
            SongRecord sngrcd = null;

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video vi in vids)
            {
                sngrcd = new SongRecord(vi);

                sngrcs.Add(sngrcd);
            }

            ViewBag.PlaylistVideos = sngrcs.VideoPlaylist();

            return View();
        }

        #endregion

        #region register
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (Utilities.IsSpamIP(Request.UserHostAddress))
            {
                // they are a duplicate IP and are no being referred by an existing user
                ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + Messages.Account);
                return View(model);
            }

            // ignore old browsers and duplicate IPs
            if
                (
                Request.Browser.Type == "IE3" ||
                Request.Browser.Type == "IE4" ||
                Request.Browser.Type == "IE5" ||
                Request.Browser.Type == "IE6" ||
                Request.Browser.Type == "IE7" ||
                BlackIPs.IsIPBlocked(Request.UserHostAddress) 
                
                )
            {
                Response.Redirect("http://browsehappy.com/");
                return View();
            }
            else if (!BootBaronLib.Configs.GeneralConfigs.EnableSameIP &&
                UserAccount.IsAccountIPTaken(Request.UserHostAddress) &&
                string.IsNullOrEmpty(model.RefUser))
            {
                // they are a duplicate IP and are no being referred by an existing user
                ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + Messages.Account);
                return View(model);
            }


            TryUpdateModel(model);

            if (ModelState.IsValid)
            {
                if (!Utilities.IsEmail(model.Email))
                {
                    ModelState.AddModelError("", BootBaronLib.Resources.Messages.IncorrectFormat + ": " + BootBaronLib.Resources.Messages.EMail);
                    return View();
                }
                else if (
                    model.UserName.Trim().Contains(" ") ||
                    model.UserName.Trim().Contains("?") ||
                    model.UserName.Trim().Contains("*") ||
                    model.UserName.Trim().Contains(":") ||
                    model.UserName.Trim().Contains("/") ||
                    model.UserName.Trim().Contains(@"\"))
                {
                    ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.UserName);
                    return View();
                }
                else if (model.YouAreID == null)
                {
                    ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.YouAre);
                    return View();
                }

                DateTime dt = new DateTime();

                if (!DateTime.TryParse(model.Year
                                + "-" + model.Month + "-" + model.Day, out dt))
                {
                    ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.BirthDate);
                    return View();
                }
                else if (DateTime.TryParse(model.Year
                    + "-" + model.Month + "-" + model.Day, out dt))
                {
                    if (Utilities.CalculateAge(dt) < BootBaronLib.Configs.GeneralConfigs.MinimumAge)
                    {
                        ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.BirthDate);
                        return View();
                    }
                }


                model.UserName = model.UserName.Replace(" ", string.Empty).Replace(":", string.Empty) /* still annoying errors */;

                // Attempt to register the user
                MembershipCreateStatus createStatus;

                Membership.CreateUser(model.UserName, model.NewPassword, model.Email, "Q", "A", true, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.RedirectFromLoginPage(model.UserName, true);

                    UserAccount ua = new UserAccount(model.UserName);
                    uad = new UserAccountDetail();
                    uad.UserAccountID = ua.UserAccountID;

                    uad.BirthDate = dt;
                    uad.YouAreID = model.YouAreID;
                    uad.DisplayAge = true;
                    uad.DefaultLanguage = Utilities.GetCurrentLanguageCode();

                    if (!string.IsNullOrEmpty(model.RefUser))
                    {
                        UserAccount refUser = new UserAccount(model.RefUser);
                        uad.ReferringUserID = refUser.UserAccountID;
                    }

                    uad.Set();

                    StringBuilder sb = new StringBuilder(100);

                    sb.Append(Messages.Hello);
                    sb.Append(Environment.NewLine);
                    sb.Append(Messages.YourNewAccountIsReadyForUse);
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append(Messages.UserName + ": ");
                    sb.Append(ua.UserName);
                    sb.Append(Environment.NewLine);
                    sb.Append(Messages.Password + ": ");
                    sb.Append(model.NewPassword);
                    sb.Append(Environment.NewLine);
                    sb.Append(BootBaronLib.Configs.GeneralConfigs.SiteDomain);

                    Utilities.SendMail(ua.EMail, Messages.YourNewAccountIsReadyForUse, sb.ToString());

                    // see if this is the 1st user
                    UserAccounts recentUsers = new UserAccounts();
                    recentUsers.GetNewestUsers();

                    if (recentUsers.Count == 1)
                    {
                        Role adminRole = new Role(SiteEnums.RoleTypes.admin.ToString());

                        UserAccountRole.AddUserToRole(ua.UserAccountID, adminRole.RoleID);
                    }


                    return RedirectToAction("editprofile", "Account");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            return View(model);
        }


        public ActionResult Register()
        {
            return View();
        }


        #endregion

        #region change password

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }



        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser mu = Membership.GetUser();

                if (mu.ChangePassword(model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", Messages.ThePasswordsEnteredDoNotMatch);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region settings


        [Authorize]
        [HttpGet]
        public ActionResult Settings()
        {
            ViewBag.IsValid = true;

            mu = Membership.GetUser();

            uad = new UserAccountDetail();

            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

            ViewBag.UserAccountDetail = uad;
            ViewBag.Membership = mu;

            return View(uad);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Settings(NameValueCollection nvc)
        {
            ViewBag.IsValid = true;

            mu = Membership.GetUser();
            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

            uad = new UserAccountDetail();

            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

            string enableProfileLogging = Request.Form["enableprofilelogging"];
            string emailmessages = Request.Form["emailmessages"];
            string showonmap = Request.Form["showonmap"];
            string displayAge = Request.Form["displayage"];
            string membersOnlyProfile = Request.Form["membersonlyprofile"];


            if (!string.IsNullOrEmpty(membersOnlyProfile))
                uad.MembersOnlyProfile = true;
            else uad.MembersOnlyProfile = false;

            if (!string.IsNullOrEmpty(enableProfileLogging))
                uad.EnableProfileLogging = true;
            else uad.EnableProfileLogging = false;


            if (!string.IsNullOrEmpty(displayAge))
                uad.DisplayAge = true;
            else uad.DisplayAge = false;

            if (!string.IsNullOrEmpty(emailmessages))
                uad.EmailMessages = true;
            else uad.EmailMessages = false;

            if (!string.IsNullOrEmpty(showonmap))
                uad.ShowOnMap = true;
            else uad.ShowOnMap = false;

            uad.Set();

            string username = Request.Form["username"].Trim();
            bool isNewUserName = false;
            bool isValidName = false;

            try
            {
                isValidName = !System.Text.RegularExpressions.Regex.IsMatch(@"[A-Za-z][A-Za-z0-9_]{3,14}", username);
            }
            catch
            {
                // bad name
                isValidName = false;
            }

            if (mu.UserName != username && isValidName)
            {
                // TODO: PUT IN ALL THE SAME VALIDATION AS REGISTRATION
                isNewUserName = true;
                UserAccount newUsername = new UserAccount(username.Replace(":", string.Empty) /* still annoying errors */);

                if (newUsername.UserAccountID != 0)
                {
                    ViewBag.IsValid = false;
                    ModelState.AddModelError("", BootBaronLib.Resources.Messages.AlreadyInUse + ": " + BootBaronLib.Resources.Messages.UserName);
                    uad = new UserAccountDetail();

                    uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));
                    mu = Membership.GetUser();

                    ViewBag.UserAccountDetail = uad;
                    ViewBag.Membership = mu;
                    return View();
                }
                else
                {
                    if (!Utilities.IsEmail(Request.Form["email"]))
                    {
                        ViewBag.IsValid = false;
                        ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.EMail);
                        return View();
                    }
                    else if (Request.Form["email"].Trim() != ua.EMail)
                    {
                        ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));
                        ua.EMail = Request.Form["email"];
                        ua.Update();
                    }

                    ua.UserName = username;
                    ua.Update();
                    FormsAuthentication.SetAuthCookie(username, false);
                    ViewBag.IsValid = true;
                }
            }
            else if (!Utilities.IsEmail(Request.Form["email"]))
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError("", BootBaronLib.Resources.Messages.Invalid + ": " + BootBaronLib.Resources.Messages.EMail);
                return View();
            }
            else if (Request.Form["email"].Trim() != ua.EMail)
            {
                ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));
                ua.EMail = Request.Form["email"];
                ua.Update();
            }

            ViewBag.ProfileUpdated = true;

            uad = new UserAccountDetail();

            uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));
            mu = Membership.GetUser();

            ViewBag.UserAccountDetail = uad;
            ViewBag.Membership = mu;

            if (isNewUserName)
            {
                // new username
                Response.Redirect("~/account/settings/?updated=1");
            }

            return View();
        }

        #endregion



        [Authorize]
        [HttpGet]
        public ActionResult DeleteVideo(int? id)
        {
            var model = new BootBaronLib.AppSpec.DasKlub.BOL.UserContent.Content(Convert.ToInt32(id));

            S3Service s3 = new S3Service();

            s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
            s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;

            if (!string.IsNullOrWhiteSpace(model.ContentVideoURL) && s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL))
            {
                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL);

                model.ContentVideoURL = string.Empty;
                model.Set();
            }

            if (!string.IsNullOrWhiteSpace(model.ContentVideoURL2) && s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL2))
            {
                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL2);

                model.ContentVideoURL2 = string.Empty;
                model.Set();
            }



            return RedirectToAction("EditArticle", new { @id = model.ContentID });
        }

        #region status updates


        //public static Image RotateImage(Image img, float rotationAngle)
        //{
        //    //create an empty Bitmap image
        //    Bitmap bmp = new Bitmap(img.Width, img.Height);

        //    //turn the Bitmap into a Graphics object
        //    Graphics gfx = Graphics.FromImage(bmp);

        //    //now we set the rotation point to the center of our image
        //    gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

        //    //now rotate the image
        //    gfx.RotateTransform(rotationAngle);

        //    gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

        //    //set the InterpolationMode to HighQualityBicubic so to ensure a high
        //    //quality image once it is transformed to the specified size
        //    gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //    //now draw our new image onto the graphics object
        //    gfx.DrawImage(img, new Point(0, 0));

        //    //dispose of our Graphics object
        //    gfx.Dispose();

        //    //return the image
        //    return bmp;
        //}

        //public static Bitmap RotateImage(Image image, PointF offset, float angle)
        //{
        //    int R1, R2;
        //    R1 = R2 = 0;
        //    if (image.Width > image.Height)
        //        R2 = image.Width - image.Height;
        //    else
        //        R1 = image.Height - image.Width;

        //    if (image == null)
        //        throw new ArgumentNullException("image");

        //    //create a new empty bitmap to hold rotated image
        //    Bitmap rotatedBmp = new Bitmap(image.Width + R1 + 40, image.Height + R2 + 40);
        //    rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    //make a graphics object from the empty bitmap
        //    Graphics g = Graphics.FromImage(rotatedBmp);

        //    //Put the rotation point in the center of the image
        //    g.TranslateTransform(offset.X + R1 / 2 + 20, offset.Y + R2 / 2 + 20);

        //    //rotate the image
        //    g.RotateTransform(angle);

        //    //move the image back
        //    g.TranslateTransform(-offset.X - R1 / 2 - 20, -offset.Y - R2 / 2 - 20);

        //    //draw passed in image onto graphics object 
        //    g.DrawImage(image, new PointF(R1 / 2 + 20, R2 / 2 + 20));

        //    return rotatedBmp;
        //}

        /// <summary>
        /// Function to download Image from website
        /// </summary>
        /// <param name="_URL">URL address to download image</param>
        /// <returns>Image</returns>
        public Image DownloadImage(string _URL)
        {
            Image _tmpImage = null;

            try
            {
                // Open a connection
                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL);

                _HttpWebRequest.AllowWriteStreamBuffering = true;

                // You can also specify additional header values like the user agent or the referer: (Optional)
                _HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                _HttpWebRequest.Referer = "http://www.google.com/";

                // set timeout for 20 seconds (Optional)
                _HttpWebRequest.Timeout = 20000;

                // Request response:
                System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();

                // Open data stream:
                System.IO.Stream _WebStream = _WebResponse.GetResponseStream();

                // convert webstream to image
                _tmpImage = Image.FromStream(_WebStream);

                // Cleanup
                _WebResponse.Close();
                _WebResponse.Close();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
                return null;
            }

            return _tmpImage;
        }

        [HttpPost]
        [Authorize]
        public ActionResult StatusDelete(NameValueCollection nvc)
        {
            if (Request.Form["delete_status_id"] != null)
            {
                StatusUpdate su = new StatusUpdate(
                    Convert.ToInt32(Request.Form["delete_status_id"])
                    );

                // delete all acknowledgements for status

                Acknowledgements.DeleteStatusAcknowledgements(su.StatusUpdateID);

                su.Delete();
            }
            else if (Request.Form["status_update_id_beat"] != null ||
                     Request.Form["status_update_id_applaud"] != null)
            {
                mu = Membership.GetUser();

                Acknowledgement ack = new Acknowledgement();

                ack.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                ack.UserAccountID = Convert.ToInt32(mu.ProviderUserKey);

                if (Request.Form["status_update_id_beat"] != null)
                {
                    ack.AcknowledgementType = 'B';
                    ack.StatusUpdateID = Convert.ToInt32(Request.Form["status_update_id_beat"]);
                }
                else if (Request.Form["status_update_id_applaud"] != null)
                {
                    ack.AcknowledgementType = 'A';
                    ack.StatusUpdateID = Convert.ToInt32(Request.Form["status_update_id_applaud"]);
                }

                if (!Acknowledgement.IsUserAcknowledgement(ack.StatusUpdateID, ack.UserAccountID))
                {
                    ack.Create();
                }
            }

            return RedirectToAction("Home");
        }

        //        /// <summary>
        ///// Creates a new Image containing the same image only rotated
        ///// </summary>
        ///// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        ///// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        ///// <returns>A new <see cref="System.Drawing.Bitmap"/> of the same size rotated.</returns>
        ///// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        //public static Bitmap RotateImage(Image image, float angle)
        //{
        //    return RotateImage(image, new PointF((float)image.Width / 2, (float)image.Height / 2), angle);
        //}

        ///// <summary>
        ///// Creates a new Image containing the same image only rotated
        ///// </summary>
        ///// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        ///// <param name="offset">The position to rotate from.</param>
        ///// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        ///// <returns>A new <see cref="System.Drawing.Bitmap"/> of the same size rotated.</returns>
        ///// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        //public static Bitmap RotateImage(Image image, PointF offset, float angle)
        //{
        //    if (image == null)
        //        throw new ArgumentNullException("image");

        //    //create a new empty bitmap to hold rotated image
        //    Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
        //    rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    //make a graphics object from the empty bitmap
        //    Graphics g = Graphics.FromImage(rotatedBmp);

        //    //Put the rotation point in the center of the image
        //    g.TranslateTransform(offset.X, offset.Y);

        //    //rotate the image
        //    g.RotateTransform(angle);

        //    //move the image back
        //    g.TranslateTransform(-offset.X, -offset.Y);

        //    //draw passed in image onto graphics object
        //    g.DrawImage(image, new PointF(0, 0));

        //    return rotatedBmp;
        //}



        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        /// <returns>A new <see cref="System.Drawing.Bitmap"/> that is just large enough
        /// to contain the rotated image without cutting any corners off.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            const double pi2 = Math.PI / 2.0;

            // Why can't C# allow these to be const, or at least readonly
            // *sigh*  I'm starting to talk like Christian Graus :omg:
            double oldWidth = (double)image.Width;
            double oldHeight = (double)image.Height;

            // Convert degrees to radians
            double theta = ((double)angle) * Math.PI / 180.0;
            double locked_theta = theta;

            // Ensure theta is now [0, 2pi)
            while (locked_theta < 0.0)
                locked_theta += 2 * Math.PI;

            double newWidth, newHeight;
            int nWidth, nHeight; // The newWidth/newHeight expressed as ints

            #region Explaination of the calculations
            /*
			 * The trig involved in calculating the new width and height
			 * is fairly simple; the hard part was remembering that when 
			 * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
			 * height are switched.
			 * 
			 * When you rotate a rectangle, r, the bounding box surrounding r
			 * contains for right-triangles of empty space.  Each of the 
			 * triangles hypotenuse's are a known length, either the width or
			 * the height of r.  Because we know the length of the hypotenuse
			 * and we have a known angle of rotation, we can use the trig
			 * function identities to find the length of the other two sides.
			 * 
			 * sine = opposite/hypotenuse
			 * cosine = adjacent/hypotenuse
			 * 
			 * solving for the unknown we get
			 * 
			 * opposite = sine * hypotenuse
			 * adjacent = cosine * hypotenuse
			 * 
			 * Another interesting point about these triangles is that there
			 * are only two different triangles. The proof for which is easy
			 * to see, but its been too long since I've written a proof that
			 * I can't explain it well enough to want to publish it.  
			 * 
			 * Just trust me when I say the triangles formed by the lengths 
			 * width are always the same (for a given theta) and the same 
			 * goes for the height of r.
			 * 
			 * Rather than associate the opposite/adjacent sides with the
			 * width and height of the original bitmap, I'll associate them
			 * based on their position.
			 * 
			 * adjacent/oppositeTop will refer to the triangles making up the 
			 * upper right and lower left corners
			 * 
			 * adjacent/oppositeBottom will refer to the triangles making up 
			 * the upper left and lower right corners
			 * 
			 * The names are based on the right side corners, because thats 
			 * where I did my work on paper (the right side).
			 * 
			 * Now if you draw this out, you will see that the width of the 
			 * bounding box is calculated by adding together adjacentTop and 
			 * oppositeBottom while the height is calculate by adding 
			 * together adjacentBottom and oppositeTop.
			 */
            #endregion

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            // We need to calculate the sides of the triangles based
            // on how much rotation is being done to the bitmap.
            //   Refer to the first paragraph in the explaination above for 
            //   reasons why.
            if ((locked_theta >= 0.0 && locked_theta < pi2) ||
                (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;

                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;

                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            }

            newWidth = adjacentTop + oppositeBottom;
            newHeight = adjacentBottom + oppositeTop;

            nWidth = (int)Math.Ceiling(newWidth);
            nHeight = (int)Math.Ceiling(newHeight);

            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                // This array will be used to pass in the three points that 
                // make up the rotated image
                Point[] points;

                /*
                 * The values of opposite/adjacentTop/Bottom are referring to 
                 * fixed locations instead of in relation to the
                 * rotating image so I need to change which values are used
                 * based on the how much the image is rotating.
                 * 
                 * For each point, one of the coordinates will always be 0, 
                 * nWidth, or nHeight.  This because the Bitmap we are drawing on
                 * is the bounding box for the rotated bitmap.  If both of the 
                 * corrdinates for any of the given points wasn't in the set above
                 * then the bitmap we are drawing on WOULDN'T be the bounding box
                 * as required.
                 */
                if (locked_theta >= 0.0 && locked_theta < pi2)
                {
                    points = new Point[] { 
											 new Point( (int) oppositeBottom, 0 ), 
											 new Point( nWidth, (int) oppositeTop ),
											 new Point( 0, (int) adjacentBottom )
										 };

                }
                else if (locked_theta >= pi2 && locked_theta < Math.PI)
                {
                    points = new Point[] { 
											 new Point( nWidth, (int) oppositeTop ),
											 new Point( (int) adjacentTop, nHeight ),
											 new Point( (int) oppositeBottom, 0 )						 
										 };
                }
                else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
                {
                    points = new Point[] { 
											 new Point( (int) adjacentTop, nHeight ), 
											 new Point( 0, (int) adjacentBottom ),
											 new Point( nWidth, (int) oppositeTop )
										 };
                }
                else
                {
                    points = new Point[] { 
											 new Point( 0, (int) adjacentBottom ), 
											 new Point( (int) oppositeBottom, 0 ),
											 new Point( (int) adjacentTop, nHeight )		
										 };
                }

                g.DrawImage(image, points);
            }

            return rotatedBmp;
        }

        [HttpGet]
        [Authorize]
        public ActionResult RotateStatusImage(int statusUpdateID)
        {
            mu = Membership.GetUser();

            StatusUpdate su = new StatusUpdate(statusUpdateID);

            if (su.PhotoItemID != null && su.PhotoItemID > 0)
            {
                var acl = CannedAcl.PublicRead;

                S3Service s3 = new S3Service();

                s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
                s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;

                PhotoItem pitm = new PhotoItem(Convert.ToInt32(su.PhotoItemID));

                // full
                Image imgFull = DownloadImage(Utilities.S3ContentPath(pitm.FilePathRaw));

                float angle = 90;

                Bitmap b = RotateImage(imgFull, angle);

                System.Drawing.Image fullPhoto = (System.Drawing.Image)b;

                string fileNameFull = Guid.NewGuid() + ".jpg";

                Stream maker = fullPhoto.ToAStream(ImageFormat.Jpeg);

                s3.AddObject(
                    maker,
                    maker.Length,
                    AmazonCloudConfigs.AmazonBucketName,
                    fileNameFull,
                    "image/jpg",
                    acl);

                if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathRaw))
                {
                    s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathRaw);
                }

                pitm.FilePathRaw = fileNameFull;


                // resized
                System.Drawing.Image photoResized = (System.Drawing.Image)b;

                string fileNameResize = Guid.NewGuid() + ".jpg";

                photoResized = ImageResize.FixedSize(photoResized, 500, 375, System.Drawing.Color.Black);

                maker = photoResized.ToAStream(ImageFormat.Jpeg);

                s3.AddObject(
                    maker,
                    maker.Length,
                    AmazonCloudConfigs.AmazonBucketName,
                    fileNameResize,
                    "image/jpg",
                    acl);

                if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathStandard))
                {
                    s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathStandard);
                }

                pitm.FilePathStandard = fileNameResize;

                // thumb
                System.Drawing.Image thumbPhoto = (System.Drawing.Image)b;

                thumbPhoto = ImageResize.Crop(thumbPhoto, 150, 150, ImageResize.AnchorPosition.Center);

                string fileNameThumb = Guid.NewGuid() + ".jpg";

                maker = thumbPhoto.ToAStream(ImageFormat.Jpeg);

                s3.AddObject(
                    maker,
                    maker.Length,
                    AmazonCloudConfigs.AmazonBucketName,
                    fileNameThumb,
                   "image/jpg",
                    acl);

                if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathThumb))
                {
                    s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, pitm.FilePathThumb);
                }

                pitm.FilePathThumb = fileNameThumb;

                pitm.Update();



            }

            return RedirectToAction("statusupdate", new { @statusUpdateID = statusUpdateID });
        }

        [Authorize]
        public JsonResult StatusUpdates(int pageNumber)
        {
            mu = Membership.GetUser();

            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

            ViewBag.CurrentUser = ua.ToUnorderdListItem;

            StatusUpdates preFilter = new StatusUpdates();

            preFilter.GetStatusUpdatesPageWise(pageNumber, 5);

            StringBuilder sb = new StringBuilder();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.StatusUpdate cnt in preFilter)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
            {
                ListItems = sb.ToString()
            });
        }

        [HttpGet]
        [Authorize]
        public ActionResult StatusUpdate(int statusUpdateID)
        {
            StatusUpdate su = new StatusUpdate(statusUpdateID);
            su.PhotoDisplay = false;

            StatusUpdates sus = new StatusUpdates();
            sus.Add(su);

            sus.IncludeStartAndEndTags = false;
            ViewBag.StatusUpdateList = @"<ul id=""status_update_list_items"">" + sus.ToUnorderdList + @"</ul>";

            return View(su);
        }

        #endregion

        #region home


        [HttpPost]
        [Authorize]
        public ActionResult Home(string message, HttpPostedFileBase file)
        {
            if (string.IsNullOrWhiteSpace(message) && file == null)
            {
                return RedirectToAction("Home");
            }

            message = message.Trim();

            mu = Membership.GetUser();

            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));
            //StatusUpdates sus = null;

            ViewBag.CurrentUser = ua.ToUnorderdListItem;

            StatusUpdate su = new StatusUpdate();

            su.GetMostRecentUserStatus(ua.UserAccountID);

            DateTime startTime = Utilities.GetDataBaseTime();

            TimeSpan span = startTime.Subtract(su.CreateDate);

            //su = new StatusUpdate();
            // TODO: this is not working properly, preventing posts
            if (su.Message == message && file == null)
            {
                // double post
                return RedirectToAction("Home");
            }
            else
            {
                su = new StatusUpdate();
            }

            if (file != null && Utilities.IsImageFile(file.FileName))
            {
                Bitmap b = new Bitmap(file.InputStream);

                var acl = CannedAcl.PublicRead;

                S3Service s3 = new S3Service();

                s3.AccessKeyID = AmazonCloudConfigs.AmazonAccessKey;
                s3.SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey;

                PhotoItem pitem = new PhotoItem();

                pitem.CreatedByUserID = ua.UserAccountID;
                pitem.Title = message;

                // full
                System.Drawing.Image fullPhoto = (System.Drawing.Image)b;

                string fileNameFull = Utilities.CreateUniqueContentFilename(file);

                Stream maker = fullPhoto.ToAStream(ImageFormat.Jpeg);

                s3.AddObject(
                    maker,
                    maker.Length,
                    AmazonCloudConfigs.AmazonBucketName,
                    fileNameFull,
                    file.ContentType,
                    acl);

                pitem.FilePathRaw = fileNameFull;

                // resized
                System.Drawing.Image photoResized = (System.Drawing.Image)b;

                string fileNameResize = Utilities.CreateUniqueContentFilename(file);

                photoResized = ImageResize.FixedSize(photoResized, 500, 375, System.Drawing.Color.Black);

                maker = photoResized.ToAStream(ImageFormat.Jpeg);

                s3.AddObject(
                    maker,
                    maker.Length,
                    AmazonCloudConfigs.AmazonBucketName,
                    fileNameResize,
                    file.ContentType,
                    acl);

                pitem.FilePathStandard = fileNameResize;

                // thumb
                System.Drawing.Image thumbPhoto = (System.Drawing.Image)b;

                thumbPhoto = ImageResize.Crop(thumbPhoto, 150, 150, ImageResize.AnchorPosition.Center);

                string fileNameThumb = Utilities.CreateUniqueContentFilename(file);

                maker = thumbPhoto.ToAStream(ImageFormat.Jpeg);

                s3.AddObject(
                    maker,
                    maker.Length,
                    AmazonCloudConfigs.AmazonBucketName,
                    fileNameThumb,
                    file.ContentType,
                    acl);

                pitem.FilePathThumb = fileNameThumb;

                pitem.Create();

                su.PhotoItemID = pitem.PhotoItemID;
            }

            su.UserAccountID = ua.UserAccountID;
            su.Message = message;
            su.CreatedByUserID = ua.UserAccountID;
            su.IsMobile = Request.Browser.IsMobileDevice;
            su.Create();

            if (Request.Browser.IsMobileDevice)
            {
                // this will bring them to the post
                return new RedirectResult(Url.Action("Home") + "#most_recent");
            }
            else
            {
                // the menu prevents brining to post correctly
                return RedirectToAction("Home");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Home()
        {
            mu = Membership.GetUser();

            if (mu == null)
            {
                return RedirectToAction("LogOff");
            }

            ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));

            ViewBag.CurrentUser = ua.ToUnorderdListItem;

            StatusUpdates preFilter = new StatusUpdates();

            preFilter.GetStatusUpdatesPageWise(1, pageSize);

            StatusUpdates sus = new StatusUpdates();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.StatusUpdate su1
                in preFilter)
            {
                if (!BootBaronLib.AppSpec.DasKlub.BOL.BlockedUser.IsBlockingUser(ua.UserAccountID, su1.UserAccountID))
                {
                    sus.Add(su1);
                }
            }

            sus.IncludeStartAndEndTags = false;
            ViewBag.StatusUpdateList = string.Format(@"<ul id=""status_update_list_items"">{0}</ul>", sus.ToUnorderdList);

            StatusUpdateNotifications suns = new StatusUpdateNotifications();
            suns.GetStatusUpdateNotificationsForUser(ua.UserAccountID);

            if (suns.Count > 0)
            {
                suns.Sort(delegate(StatusUpdateNotification p1, StatusUpdateNotification p2)
                {
                    return p1.CreateDate.CompareTo(p2.CreateDate);
                });

                ViewBag.Notifications = suns;

                foreach (StatusUpdateNotification sun1 in suns)
                {
                    sun1.IsRead = true;
                    sun1.Update();
                }
            }





            StatusUpdates applauseResult = new StatusUpdates();
            applauseResult.GetMostAcknowledgedStatus(7, 'A');
            if (applauseResult.Count > 0)
            {
                ViewBag.MostApplauded = applauseResult;
            }

            StatusUpdate beatDownResult = new BootBaronLib.AppSpec.DasKlub.BOL.StatusUpdate();
            beatDownResult = new StatusUpdate();
            beatDownResult.GetMostAcknowledgedStatus(7, 'B');

            bool isAlreadyApplauded = false;

            if (beatDownResult.StatusUpdateID > 0)
            {
                foreach (StatusUpdate ssr1 in applauseResult)
                {
                    if (beatDownResult.StatusUpdateID == ssr1.StatusUpdateID)
                    {
                        isAlreadyApplauded = true;
                        break;
                    }
                }
            }

            if (!isAlreadyApplauded && beatDownResult.StatusUpdateID > 0)
            {
                ViewBag.MostBeatDown = beatDownResult;
            }

            //
            StatusUpdate commentResponse
            = new BootBaronLib.AppSpec.DasKlub.BOL.StatusUpdate(
            StatusComments.GetMostCommentedOnStatus(DateTime.UtcNow.AddDays(-7)));

            bool isAlreadyCommented = false;

            foreach (StatusUpdate ssr1 in applauseResult)
            {
                if (commentResponse.StatusUpdateID == ssr1.StatusUpdateID)
                {
                    isAlreadyCommented = true;
                }
            }

            if (!isAlreadyCommented && beatDownResult.StatusUpdateID != commentResponse.StatusUpdateID && commentResponse.StatusUpdateID > 0)
            {
                // only show if the most commented is different from most beat down or applauded
                ViewBag.MostCommented = commentResponse;
            }

            return View();

        }





        #endregion

        #region forgot password

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            UserAccount ua = new UserAccount();
            ua.GetUserAccountByEmail(email);

            if (ua.UserAccountID == 0)
            {
                ViewBag.Result = Messages.NotFound;
            }
            else
            {
                ua.FailedPasswordAnswerAttemptCount = 0;
                ua.FailedPasswordAttemptCount = 0;
                ua.IsLockedOut = false;
                ua.Update();

                mu = Membership.GetUser(ua.UserName);
                string newPassword = mu.ResetPassword();

                BootBaronLib.Operational.Utilities.SendMail(email, BootBaronLib.Configs.AmazonCloudConfigs.SendFromEmail, Messages.PasswordReset,
                   Messages.UserName + ": " + ua.UserName + Environment.NewLine +
                    Environment.NewLine + Messages.NewPassword + ": " + newPassword +
                    Environment.NewLine +
                Environment.NewLine + Messages.SignIn + ": " + BootBaronLib.Configs.GeneralConfigs.SiteDomain);

                ViewBag.Result = Messages.CheckYourEmailAndSpamFolder;
            }

            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        #endregion

    }
}

