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
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.ArtistContent;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Lib.BOL.VideoContest;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Services;
using DasKlub.Lib.Values;
using DasKlub.Web.Models;
using LitS3;
using Microsoft.Ajax.Utilities;

namespace DasKlub.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(IMailService mail)
        {
            _mail = mail;
            _mu = Membership.GetUser();
            CanBeStealth();
        }

        #region variables

        private const int PageSize = 5;
        private UserAccounts _contacts;
        private MembershipUser _mu;
        private UserAccount _ua;
        private UserAccountDetail _uad;
        private UserAccounts _uas;
        private UserConnection _ucon;
        private UserConnections _ucons;
        private UserPhotos _ups;
        private readonly IMailService _mail;

        #endregion

        #region Articles

        [HttpGet]
        [Authorize]
        public ActionResult DeleteArticle(int? id)
        {
            if (id != null && id > 0)
            {
                var model = new Content(Convert.ToInt32(id));

                var concoms = new ContentComments();
                concoms.GetCommentsForContent(model.ContentID, SiteEnums.CommentStatus.U);
                concoms.GetCommentsForContent(model.ContentID, SiteEnums.CommentStatus.C);

                if (_mu != null && model.CreatedByUserID == Convert.ToInt32(_mu.ProviderUserKey))
                {
                    // security check
                    foreach (var c1 in concoms)
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
            var model = new Content();

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
            HttpPostedFileBase videoFile)
        {
            var model = new Content();

            if (contentID != null && contentID > 0)
            {
                model = new Content(
                    Convert.ToInt32(contentID));
            }

            TryUpdateModel(model);
            
            const CannedAcl acl = CannedAcl.PublicRead;

            var s3 = new S3Service
                {
                    AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                    SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                };

            if (string.IsNullOrWhiteSpace(model.Detail))
            {
                ModelState.AddModelError(Messages.Required, Messages.Required + @": " + Messages.Details);
            }

            if (imageFile == null && string.IsNullOrWhiteSpace(model.ContentPhotoURL))
            {
                ModelState.AddModelError(Messages.Required, Messages.Required + @": " + Messages.Photo);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ContentKey = FromString.URLKey(model.Title);

            if (model.ContentID == 0)
            {
                if (_mu != null)
                {
                    if(model.CreatedByUserID == 0)
                        model.CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                }
            

            if (model.ReleaseDate == DateTime.MinValue)
                {
                    model.ReleaseDate = DateTime.UtcNow;
                }
            }

            if (model.Set() <= 0) return View(model);

            if (imageFile != null)
            {
                var b = new Bitmap(imageFile.InputStream);
                var fileNameFull = Utilities.CreateUniqueContentFilename(imageFile);
                var imgPhoto = ImageResize.FixedSize(b, 600, 400, Color.Black);
                var maker = imgPhoto.ToAStream(ImageFormat.Jpeg);

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

                var fileNameThumb = Utilities.CreateUniqueContentFilename(imageFile);
                var imgPhotoThumb = ImageResize.FixedSize(b, 350, 250, Color.Black);

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
                    // full
                    var fileName3 = Utilities.CreateUniqueContentFilename(videoFile);

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
                catch (Exception)
                {
                }
            }

             
            return RedirectToAction("Articles", "SiteAdmin"); // TODO: ENABLE SELF ARTICLE PAGE
        }


        [Authorize]
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


        [Authorize]
        public ActionResult Articles()
        {
            var totalRecords = 0;
            const int pageSize = 10;
            var model = new Contents();

            if (string.IsNullOrEmpty(Request.QueryString[SiteEnums.QueryStringNames.pg.ToString()]))
            {
                if (_mu != null) model.GetContentForUser(Convert.ToInt32(_mu.ProviderUserKey));

                model.Sort((p1, p2) => p2.ReleaseDate.CompareTo(p1.ReleaseDate));
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

        #region sign in/ out

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var remember = Convert.ToBoolean(model.RememberMe);

                if (string.IsNullOrWhiteSpace(model.UserName) ||
                    string.IsNullOrWhiteSpace(model.Password))
                {
                    return View(model);
                }

                _ua = new UserAccount(model.UserName);

                if (_ua.UserAccountID == 0)
                {
                    _ua = new UserAccount();
                    _ua.GetUserAccountByEmail(model.UserName);

                    if (_ua.UserAccountID > 0)
                    {
                        // they were stupid and put their email not username
                        model.UserName = _ua.UserName;
                    }
                }

                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.RedirectFromLoginPage(model.UserName, remember);

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    _ua = new UserAccount(model.UserName);

                    if (_ua.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, Messages.IsLockedOut);
                        return View(model);
                    }

                    _ua.LastLoginDate = DateTime.UtcNow;
                    _ua.FailedPasswordAttemptCount = 0;
                    _ua.IsOnLine = true;
                    _ua.SigningOut = false;
                    _ua.Update();

                    var uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(_ua.UserAccountID);

                    if (string.IsNullOrWhiteSpace(uad.DefaultLanguage)) 
                        return RedirectToAction("Home", "Account");

                    var nvc = new NameValueCollection
                    {
                        {SiteEnums.CookieValue.language.ToString(), uad.DefaultLanguage}
                    };

                    Utilities.CookieMaker(SiteEnums.CookieName.Usersetting, nvc);

                    Thread.CurrentThread.CurrentUICulture =
                        CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(uad.DefaultLanguage);

                    return RedirectToAction("Home", "Account");
                }
              
                _ua = new UserAccount(model.UserName);

                if (_ua.UserAccountID > 0)
                {
                    _ua.IsOnLine = false;
                    _ua.SigningOut = true;
                    _ua.Update();
                }
            }

            ModelState.AddModelError(string.Empty, Messages.LoginUnsuccessfulPleaseCorrect);

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

            var membershipUser = Membership.GetUser();
            if (membershipUser != null)
                _ua = new UserAccount(membershipUser.UserName) {IsOnLine = false, SigningOut = true};
            _ua.Update();
            _ua.RemoveCache();

            var cru = new ChatRoomUser();

            cru.GetChatRoomUserByUserAccountID(_ua.UserAccountID);
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
            var contest = Contest.GetLastContest();
            var cResults = new ContestResults();
            cResults.GetContestVideosForContest(contest.ContestID);
            ViewBag.ContestResults = cResults;
        }

        [Authorize]
        [HttpPost]
        public ActionResult VideoVote(int videoVote)
        {
            var contest = Contest.GetLastContest();

            ViewBag.Contest = contest;

            if (_mu != null && ContestVideo.IsUserContestVoted(Convert.ToInt32(_mu.ProviderUserKey), contest.ContestID))
            {
                LoadContestResults();
                return View();
            }

            var convid = new ContestVideo();

            convid.GetContestVideoForContestAndVideo(videoVote, contest.ContestID);

            if (_mu != null)
            {
                var cvv = new ContestVideoVote
                    {
                        UserAccountID = Convert.ToInt32(_mu.ProviderUserKey),
                        CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey),
                        ContestVideoID = convid.ContestVideoID
                    };

                cvv.Create();
            }

            LoadContestResults();

            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult VideoVote()
        {
            var contest = Contest.GetLastContest();

            ViewBag.Contest = contest;

            if (_mu != null &&
                (ContestVideo.IsUserContestVoted(Convert.ToInt32(_mu.ProviderUserKey), contest.ContestID) &&
                 contest.DeadLine.AddHours(72) > DateTime.UtcNow))
            {
                LoadContestResults();

                return View();
            }

            var cvids = new ContestVideos();

            cvids.GetContestVideosForContest(contest.ContestID);

            var vidsInContest = new Videos();
            vidsInContest.AddRange(cvids.Select(cv1 => new Video(cv1.VideoID)));

            vidsInContest.Sort((p1, p2) => p1.PublishDate.CompareTo(p2.PublishDate));

            ViewBag.ContestVideos = vidsInContest;

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
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            if (_ua.IsAdmin)
            {
                // admin should not die
                return View();
            }

            if (_ua.Delete(true))
            {
                FormsAuthentication.SignOut();

                return RedirectToAction("Register", "Account");
            }
            return View();
        }

        #endregion

        #region contact request

        [Authorize]
        [HttpPost]
        public ActionResult ContactRequest(NameValueCollection postedData)
        {
            var qury = Request.QueryString;
            var userToBe = new UserAccount(qury["username"]);
            var ucToSet = new UserConnection();

            if (_mu != null)
                ucToSet.GetUserToUserConnection(Convert.ToInt32(_mu.ProviderUserKey), userToBe.UserAccountID);

            if (ucToSet.UserConnectionID == 0 && qury["rslt"] == "0")
            {
                // they never set anything up and are ending the request
                Response.Redirect("~/" + userToBe.UserName);
            }
            else if (ucToSet.UserConnectionID != 0 && qury["rslt"] == "0")
            {
                if (_mu != null && Convert.ToInt32(_mu.ProviderUserKey) != ucToSet.CreatedByUserID)
                {
                    // one user sent a request and now it's rejected
                    ucToSet.StatusType = 'L';
                    ucToSet.UpdatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                    ucToSet.IsConfirmed = true;
                    ucToSet.Update();

                    if (_mu != null) Response.Redirect("/" + _mu.UserName);
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
                if (_mu != null)
                {
                    ucToSet.CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                    ucToSet.FromUserAccountID = Convert.ToInt32(_mu.ProviderUserKey);
                }
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

                    if (_mu != null && Convert.ToInt32(_mu.ProviderUserKey) == ucToSet.ToUserAccountID)
                    {
                        // the opposite this time
                        ucToSet.FromUserAccountID = Convert.ToInt32(_mu.ProviderUserKey);
                        ucToSet.ToUserAccountID = userToBe.UserAccountID;
                    }
                }
                if (_mu != null) ucToSet.UpdatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                ucToSet.StatusType = Convert.ToChar(qury["contacttype"]);
                ucToSet.Update();

                switch (Convert.ToChar(qury["contacttype"]))
                {
                    case 'R':
                        if (ucToSet.IsConfirmed)
                        {
                            if (_mu != null)
                            {
                                Response.Redirect("~/account/irlcontacts/" + _mu.UserName);
                            }
                        }
                        else
                        {
                            var requestedUserDetails = new UserAccountDetail();

                            requestedUserDetails.GetUserAccountDeailForUser(userToBe.UserAccountID);

                            NotifyUserOfContactRequest(userToBe);

                            Response.Redirect("/" + userToBe.UserName);
                        }
                        break;
                    case 'C':
                        if (ucToSet.IsConfirmed)
                        {
                            if (_mu != null)
                            {
                                
                                Response.Redirect("~/account/CyberAssociates/" + _mu.UserName);
                            }
                        }
                        else
                        {
                            NotifyUserOfContactRequest(userToBe);

                            Response.Redirect("~/" + userToBe.UserName);
                        }
                        break;
                    case 'L':
                        if (_mu != null) Response.Redirect("~/" + _mu.UserName);
                        break;
                }
            }


            return View();
        }

        /// <summary>
        /// Notify the user that there is a contact request
        /// </summary>
        /// <param name="userToBe"></param>
        private void NotifyUserOfContactRequest(UserAccount userToBe)
        {
            if (_mu == null) return;

            var currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

            var requestedUserDetails = new UserAccountDetail();

            requestedUserDetails.GetUserAccountDeailForUser(userToBe.UserAccountID);

            if (requestedUserDetails.EmailMessages)
            {

                _mail.SendMail(AmazonCloudConfigs.SendFromEmail,
                               userToBe.EMail,
                               string.Format("{0}: {1}", Messages.ContactRequest, _mu.UserName),
                               string.Format("{0}{1}{2}", Messages.ContactRequest, Environment.NewLine,
                                             GeneralConfigs.SiteDomain));
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ContactRequest()
        {
            var nvc = Request.QueryString;

            _ua = new UserAccount(nvc["username"]);
            _uas = new UserAccounts {_ua};

            ViewBag.UserTo = _uas.ToUnorderdList;
            ViewBag.ContactType = nvc["contacttype"];
            ViewBag.UserNameTo = _ua.UserName;

            var contype = Convert.ToChar(nvc["contacttype"]);

            switch (contype)
            {
                case 'R':
                    ViewBag.HeaderRequest = string.Format("{0} : {1}", _ua.UserName, Messages.HaveYouMetInRealLife);
                    ViewBag.HeaderRequest += string.Format(
                        @"<img src=""{0}"" />",
                        VirtualPathUtility.ToAbsolute("~/content/images/userstatus/handprint_small.png"));
                    break;
                case 'C':
                    ViewBag.HeaderRequest = string.Format("{0} : {1}", _ua.UserName,
                                                          Messages.WouldYouLikeToBeCyberAssociates);
                    ViewBag.HeaderRequest +=
                        string.Format(
                            @"<img src=""{0}"" />",
                            VirtualPathUtility.ToAbsolute("~/content/images/userstatus/keyboard_small.png"));
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
            _ucon = new UserConnection(userConnectionID);

            var conType = _ucon.StatusType;

            if (_ucon.IsConfirmed)
            {
                _ucon.Delete();
            }

            if (conType == 'R')
            {
                return RedirectToAction("irlcontacts");
            }
            if (conType == 'C')
            {
                return RedirectToAction("CyberAssociates");
            }
            Utilities.LogError("deleted no existing contact type");
            // something went wrong 
            return new EmptyResult();
        }


        [Authorize]
        [HttpPost]
        public ActionResult ManageVideos(NameValueCollection nvc)
        {
            if (nvc == null) throw new ArgumentNullException("nvc");
            
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));
            ViewBag.UserName = _ua.UserName;


            var plyvids = new UserAccountVideos();
            plyvids.GetVideosForUserAccount(_ua.UserAccountID, 'U');

            nvc = Request.Form;

            if (nvc["video_delete_id"] != null)
            {
                UserAccountVideo.DeleteVideoForUser(_ua.UserAccountID, Convert.ToInt32(nvc["video_delete_id"]));
            }

            return View("ManageVideos");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ManageVideos()
        {
            
            var uavs = new UserAccountVideos();
            if (_mu != null) uavs.GetVideosForUserAccount(Convert.ToInt32(_mu.ProviderUserKey), 'U');

            if (uavs.Count > 0)
            {
                var favvids = new Videos();
                favvids.AddRange(uavs.Select(uav1 => new Video(uav1.VideoID)).Where(f1 => f1.IsEnabled));

                var sngrcds2 = new SongRecords();
                sngrcds2.AddRange(favvids.Select(v1 => new SongRecord(v1)));

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
            if (_mu != null)
            {
                ViewBag.CurrentUserName = _mu.UserName;

                if (_mu != null)
                {
                    _uad = new UserAccountDetail();
                    _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

                    ViewBag.EnableProfileLogging = _uad.EnableProfileLogging;
                }
            }

            var unconfirmedUsers = new UserConnections();
            var allusrcons = new UserConnections();

            if (_mu != null)
            {
                allusrcons.GetUserConnections(Convert.ToInt32(_mu.ProviderUserKey));

                unconfirmedUsers.AddRange(
                    allusrcons.Where(
                        uc1 => !uc1.IsConfirmed && Convert.ToInt32(_mu.ProviderUserKey) != uc1.FromUserAccountID));
            }

            ViewBag.ApprovalList = unconfirmedUsers.ToUnorderdList;

            ViewBag.BlockedUsers =
                _mu != null &&
                Lib.BOL.BlockedUsers.HasBlockedUsers(Convert.ToInt32(_mu.ProviderUserKey));


            return View();
        }

        public void GetContactsForUser(string userName, char contactType)
        {
            _ua = new UserAccount(userName);

            if (_ua.UserAccountID == 0) return;

            _contacts = new UserAccounts();

            _ucons = new UserConnections();
            _ucons.GetUserConnections(_ua.UserAccountID);

            foreach (var ua1 in
                _ucons.Where(uc1 => uc1.IsConfirmed && uc1.StatusType == contactType)
                .Select(uc1 => uc1.FromUserAccountID == _ua.UserAccountID
                    ? new UserAccount(uc1.ToUserAccountID)
                    : new UserAccount(uc1.FromUserAccountID)).Where(ua1 => ua1.IsApproved && !ua1.IsLockedOut))
            {
                _contacts.Add(ua1);
            }
 
            ViewBag.UserName = _ua.UserName;
            ViewBag.ContactsCount = Convert.ToString(_contacts.Count);
            ViewBag.Contacts = _contacts.ToUnorderdList;
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
            _uad = new UserAccountDetail();
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            if (_ua.IsAdmin)
            {
                ViewBag.AdminLink = "/m/auth/default.aspx";
            }


            _uad.GetUserAccountDeailForUser(_ua.UserAccountID);
            var uadLooker = new UserAccountDetail();
            uadLooker.GetUserAccountDeailForUser(_ua.UserAccountID);

            var unconfirmedUsers = new UserConnections();
            var allusrcons = new UserConnections();

            if (_mu != null)
            {
                allusrcons.GetUserConnections(Convert.ToInt32(_mu.ProviderUserKey));

                unconfirmedUsers.AddRange(
                    allusrcons.Where(
                        uc1 => !uc1.IsConfirmed && Convert.ToInt32(_mu.ProviderUserKey) != uc1.FromUserAccountID));
            }

            ViewBag.ApprovalList = unconfirmedUsers.ToUnorderdList;

            ViewBag.BlockedUsers =
                _mu != null &&
                Lib.BOL.BlockedUsers.HasBlockedUsers(Convert.ToInt32(_mu.ProviderUserKey));

            ViewBag.EnableProfileLogging = _uad.EnableProfileLogging;

            if (_mu != null && 
                (Roles.IsUserInRole(_mu.UserName, SiteEnums.RoleTypes.supporter.ToString()) ||
                 Roles.IsUserInRole(_mu.UserName, SiteEnums.RoleTypes.admin.ToString())
                ))
            {
                LoadVisitorsView(_ua.UserName);
            }

            return View();
        }

        private void LoadVisitorsView(string userName)
        {
            const int maxcountusers = 100;
            
            _uad = new UserAccountDetail();
            if (userName != null) _ua = new UserAccount(userName);
            _uad.GetUserAccountDeailForUser(_ua.UserAccountID);

            var uadLooker = new UserAccountDetail();
            if (_mu == null) return;
            uadLooker.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

            if (_mu == null || _ua.UserAccountID <= 0 || !_uad.EnableProfileLogging || !_uad.EnableProfileLogging)
                return;
            var al = ProfileLog.GetRecentProfileViews(_ua.UserAccountID);

            if (al == null || al.Count <= 0) return;

            var uas = new UserAccounts();

            foreach (int id in al)
            {
                var viewwer = new UserAccount(id);
                if (viewwer.IsLockedOut || !viewwer.IsApproved) continue;
                _uad = new UserAccountDetail();
                _uad.GetUserAccountDeailForUser(id);
                if (_uad.EnableProfileLogging == false) continue;

                if (uas.Count >= maxcountusers) break;

                uas.Add(viewwer);
            }

            ViewBag.TheViewers = uas.ToUnorderdList;
        }

        [Authorize]
        [HttpGet]
        public ActionResult ProfileVisitors(string userName)
        {
            if (_mu != null && 
                (
                Roles.IsUserInRole(_mu.UserName, SiteEnums.RoleTypes.supporter.ToString() )
                ||
                Roles.IsUserInRole(_mu.UserName, SiteEnums.RoleTypes.admin.ToString())
                ))
            {
                LoadVisitorsView(userName);
            }
            return View();
        }

        #endregion

        #region user address

        [Authorize]
        [HttpPost]
        public ActionResult UserAddress(UserAddressModel model)
        {
            LoadCountries();

            
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            _uad = new UserAccountDetail();
            _uad.GetUserAccountDeailForUser(_ua.UserAccountID);

            var uadress = new UserAddress();

            uadress.GetUserAddress(_ua.UserAccountID);

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
                uadress.UserAccountID = _ua.UserAccountID;
              //  uadress.Choice1 = "||" + Request.Form["sex"] + "|" + Request.Form["size"];

                if (uadress.UserAddressID == 0) uadress.AddressStatus = 'U';

                ViewBag.ProfileUpdated = uadress.Set();

                Response.Redirect("/account/home");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult UserAddress()
        {
            LoadCountries();
            
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            _uad = new UserAccountDetail();
            _uad.GetUserAccountDeailForUser(_ua.UserAccountID);

            var model = new UserAddressModel();

            var uadress = new UserAddress();

            uadress.GetUserAddress(_ua.UserAccountID);

            if (GeneralConfigs.IsGiveAway && uadress.UserAddressID > 0) return View("NotAllowed");

            if (uadress.UserAddressID == 0)
            {
                model.PostalCode = _uad.PostalCode;
                model.Country = _uad.Country;
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
            if (_mu != null) _ua = new UserAccount(_mu.UserName);

            if (_ua.UserAccountID == 0) return View();

            _contacts = new UserAccounts();

            var bus = new BlockedUsers();

            if (_mu != null) bus.GetBlockedUsers(Convert.ToInt32(_mu.ProviderUserKey));

            foreach (var ua1 in bus.Select(uc1 => new UserAccount(uc1.UserAccountIDBlocked)))
            {
                _contacts.Add(ua1);
            }

            ViewBag.BlockedUsers = _contacts.ToUnorderdList;

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult ReportUser(int userAccountID)
        {
            _ua = new UserAccount(userAccountID);

            if (_mu != null)
                _mail.SendMail(
                    AmazonCloudConfigs.SendFromEmail,
                    GeneralConfigs.SendToErrorEmail, string.Format("{0}: {1}", Messages.Report, Messages.UserAccount),
                                   string.Format("{0}{1}{1}{2}: {3}{1}{4}: {5}{1}{1}----------{1}{6}: {7}",
                                                 Messages.Report, Environment.NewLine,
                                                 Messages.UserName, _ua.UserName, Messages.UserAccount,
                                                 _ua.UserAccountID, Messages.From, _mu.UserName));

            return RedirectToAction("Visitors");
        }


        [Authorize]
        [HttpGet]
        public ActionResult BlockedUser(int userAccountID)
        {
            var bu = new BlockedUser();

            _ucon = new UserConnection();
            if (_mu != null) _ucon.GetUserToUserConnection(userAccountID, Convert.ToInt32(_mu.ProviderUserKey));
            _ucon.Delete();

            bu.UserAccountIDBlocked = userAccountID;
            if (_mu != null) bu.UserAccountIDBlocking = Convert.ToInt32(_mu.ProviderUserKey);
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

            _uad = new UserAccountDetail();

            if (_mu != null) _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

            LoadCountries();

            InterestIdentityViewBags();

            return View(_uad);
        }

        private void InterestIdentityViewBags()
        {
            var interins = new InterestedIns();
            interins.GetAll();
            interins.Sort(
                (p1, p2) => String.Compare(p1.LocalizedName, p2.LocalizedName, StringComparison.Ordinal));
            ViewBag.InterestedIns = interins.Select(x => new {x.InterestedInID, x.LocalizedName});

            var relationshipStatuses = new RelationshipStatuses();
            relationshipStatuses.GetAll();
            relationshipStatuses.Sort(
                (p1, p2) => String.Compare(p1.LocalizedName, p2.LocalizedName, StringComparison.Ordinal));
            ViewBag.RelationshipStatuses =
                relationshipStatuses.Select(x => new {x.RelationshipStatusID, x.LocalizedName});

            var youAres = new YouAres();
            youAres.GetAll();
            youAres.Sort((p1, p2) => String.Compare(p1.LocalizedName, p2.LocalizedName, StringComparison.Ordinal));
            ViewBag.YouAres = youAres.Select(x => new {x.YouAreID, x.LocalizedName});
        }


        private void LoadCountries()
        {
            Dictionary<string, string> countryOptions = Enum.GetValues(typeof (SiteEnums.CountryCodeISO))
                                                            .Cast<int>()
                                                            .Select(value => (SiteEnums.CountryCodeISO)
// ReSharper disable AssignNullToNotNullAttribute
                                                                             Enum.Parse(
                                                                                 typeof (SiteEnums.CountryCodeISO),
                                                                                 Enum.GetName(typeof (
                                                                                                  SiteEnums.
                                                                                                  CountryCodeISO), value)))
// ReSharper restore AssignNullToNotNullAttribute
                                                            .Where(
                                                                countryCode =>
                                                                countryCode != SiteEnums.CountryCodeISO.U0 &&
                                                                countryCode != SiteEnums.CountryCodeISO.RD)
                                                            .ToDictionary(countryCode => countryCode.ToString(),
                                                                          countryCode =>
                                                                          Utilities.ResourceValue(
                                                                              Utilities.GetEnumDescription(countryCode)));

            IOrderedEnumerable<string> items = from k in countryOptions.Keys
                                               orderby countryOptions[k] ascending
                                               select k;


            ViewBag.CountryOptions = items;
        }


        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(UserAccountDetail uad)
        {
            // must change culture because decimal will not be correct for long/ lat
            var currentLang = Utilities.GetCurrentLanguageCode();
            if (currentLang == null) throw new ArgumentNullException("uad");

            Thread.CurrentThread.CurrentUICulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

            LoadCountries();
            InterestIdentityViewBags();

            if (_mu != null)
            {
                var uadCurrent = new UserAccountDetail {UserAccountID = Convert.ToInt32(_mu.ProviderUserKey)};
                uadCurrent.GetUserAccountDeailForUser(uadCurrent.UserAccountID);

                ViewBag.IsValid = true;
                ViewBag.ProfileUpdated = false;

                DateTime dt;

                if (DateTime.TryParse(Request.Form["birthyear"]
                                      + "-" + Request.Form["birthmonth"] + "-" + Request.Form["birthday"], out dt))
                {
                    uad.BirthDate = dt;
                }
                else
                {
                    ViewBag.IsValid = false;
                    ModelState.AddModelError(string.Empty, Messages.Invalid + @": " + Messages.BirthDate);
                    return View(uad);
                }

                if (string.IsNullOrEmpty(uad.Country) || uad.Country == Messages.DashSelect)
                {
                    uad.Country = string.Empty;
                    ViewBag.IsValid = false;
                    ModelState.AddModelError(string.Empty, Messages.Invalid + @": " + Messages.Country);
                    return View(uad);
                }

                if (uad.YouAreID == null)
                {
                    ViewBag.IsValid = false;
                    ModelState.AddModelError(string.Empty, Messages.Invalid + @": " + Messages.YouAre);
                    return View(uad);
                }

                if (uad.InterestedInID == null)
                {
                    ViewBag.IsValid = false;
                    ModelState.AddModelError(string.Empty, Messages.Invalid + @": " + Messages.InterestedIn);
                    return View(uad);
                }

                if (!string.IsNullOrEmpty(uad.ExternalURL.Trim()) &&
                    !Uri.IsWellFormedUriString(uad.ExternalURL, UriKind.Absolute))
                {
                    ViewBag.IsValid = false;
                    ModelState.AddModelError(string.Empty, Messages.Invalid + @": " + Messages.Website);
                    return View(uad);
                }

                bool isNewProfile = string.IsNullOrEmpty(uad.Country.Trim());

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
                    var latlong = GeoData.GetLatLongForCountryPostal(uad.Country, uad.PostalCode);

// ReSharper disable CompareOfFloatsByEqualityOperator
                    if (latlong.latitude != 0 && latlong.longitude != 0)
// ReSharper restore CompareOfFloatsByEqualityOperator
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
                    ModelState.AddModelError(string.Empty, Messages.Error);
                }


                if (isNewProfile)
                {
                    return RedirectToAction("EditPhoto");
                }
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

            return View(uad);
        }

        #endregion
 
        #region edit photo

        [Authorize]
        [HttpPost]
        public ActionResult EditPhotoDelete()
        {
            var str = Request.Form["delete_photo"];
            
            _uad = new UserAccountDetail();
            
            if (_mu != null) _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

            var s3 = new S3Service
                {
                    AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                    SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                };

            switch (str)
            {
                case "1":
                    try
                    {
                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, _uad.ProfilePicURL))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, _uad.ProfilePicURL);
                        }

                        if (s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, _uad.ProfileThumbPicURL))
                        {
                            s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, _uad.ProfileThumbPicURL);
                        }
                    }
                    catch
                    {
                        // whatever
                    }
                    _uad.ProfileThumbPicURL = string.Empty;
                    _uad.ProfilePicURL = string.Empty;
                    _uad.Update();
                    break;
                case "3":
                case "2":
                    _ups = new UserPhotos();
                    _ups.GetUserPhotos(_uad.UserAccountID);
                    foreach (var up1 in _ups)
                    {
                        try
                        {
                            if ((up1.RankOrder != 1 || str != "2") && (up1.RankOrder != 2 || str != "3")) continue;

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
                        catch
                        {
                            // whatever
                        }
                    }
                    _ups = new UserPhotos();
                    _ups.GetUserPhotos(_uad.UserAccountID);
                    if (_ups.Count == 1 && _ups[0].RankOrder == 2)
                    {
                        _ups[0].RankOrder = 1;
                        _ups[0].Update();
                    }
                    break;
            }

            return RedirectToAction("EditPhoto");
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditPhoto(HttpPostedFileBase file)
        {
            UserPhoto up1 = null;
            int swapID;
            const CannedAcl acl = CannedAcl.PublicRead;

            var s3 = new S3Service
                {
                    AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                    SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                };

            if (Request.Form["new_default"] != null &&
                int.TryParse(Request.Form["new_default"], out swapID))
            {
                // swap the default with the new default
                _uad = new UserAccountDetail();
                if (_mu != null) _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

                var currentDefaultMain = _uad.ProfilePicURL;
                var currentDefaultMainThumb = _uad.ProfileThumbPicURL;

                up1 = new UserPhoto(swapID);

                _uad.ProfilePicURL = up1.PicURL;
                _uad.ProfileThumbPicURL = up1.ThumbPicURL;
                _uad.LastPhotoUpdate = DateTime.UtcNow;
                _uad.Update();

                up1.PicURL = currentDefaultMain;
                up1.ThumbPicURL = currentDefaultMainThumb;
                if (_mu != null) up1.UpdatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                up1.Update();

                if (_mu != null) LoadCurrentImagesViewBag(Convert.ToInt32(_mu.ProviderUserKey));

                return View(_uad);
            }

            const string photoOne = "photo_edit_1";
            const string photoTwo = "photo_edit_2";
            const string photoThree = "photo_edit_3";

            if (_mu != null) LoadCurrentImagesViewBag(Convert.ToInt32(_mu.ProviderUserKey));

            _uad = new UserAccountDetail();

            if (_mu != null) _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

            if (file == null)
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError(string.Empty, Messages.NoFile);
                return View(_uad);
            }

            var photoEdited = Request.Form["photo_edit"];
            var mainPhotoToDelete = string.Empty;
            var thumbPhotoToDelete = string.Empty;

            _ups = new UserPhotos();
            _ups.GetUserPhotos(_uad.UserAccountID);

            if (string.IsNullOrEmpty(_uad.ProfilePicURL) ||
                _ups.Count == 2 && photoEdited == photoOne)
            {
                mainPhotoToDelete = _uad.ProfilePicURL;
                thumbPhotoToDelete = _uad.ProfileThumbPicURL;
            }
            else
            {
                if (_ups.Count > 1 && photoEdited == photoTwo)
                {
                    up1 = new UserPhoto(_ups[0].UserPhotoID) {RankOrder = 1};
                    mainPhotoToDelete = up1.PicURL;
                    thumbPhotoToDelete = up1.ThumbPicURL;
                }
                else if (_ups.Count > 1 && photoEdited == photoThree)
                {
                    up1 = new UserPhoto(_ups[1].UserPhotoID) {RankOrder = 2};
                    mainPhotoToDelete = _ups[1].FullProfilePicURL;
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
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {
                    // whatever
                }
            }


            var b = new Bitmap(file.InputStream);

            // full
            Image fullPhoto = b;

            fullPhoto = ImageResize.FixedSize(fullPhoto, 300, 300, Color.Black);

            var fileNameFull = Utilities.CreateUniqueContentFilename(file);
            var maker = fullPhoto.ToAStream(ImageFormat.Jpeg);

            s3.AddObject(
                maker,
                maker.Length,
                AmazonCloudConfigs.AmazonBucketName,
                fileNameFull,
                file.ContentType,
                acl);

            if (string.IsNullOrEmpty(_uad.ProfileThumbPicURL) ||
                _ups.Count == 2 && photoEdited == photoOne)
            {
                _uad.ProfilePicURL = fileNameFull;
            }
            else
            {
                if (up1 == null)
                {
                    up1 = new UserPhoto();
                }

                if (_mu != null) up1.UserAccountID = Convert.ToInt32(_mu.ProviderUserKey);
                up1.PicURL = fileNameFull;

                if ((_ups.Count > 0 && photoEdited == photoTwo) || (_ups.Count == 0))
                {
                    up1.RankOrder = 1;
                }
                else if ((_ups.Count > 1 && photoEdited == photoThree) || _ups.Count == 1)
                {
                    up1.RankOrder = 2;
                }

                if (_ups.Count == 1 && _ups[0].RankOrder == 2)
                {
                    _ups[0].RankOrder = 1;
                    _ups[0].Update();
                }
            }


            fullPhoto = b;
            fullPhoto = ImageResize.FixedSize(fullPhoto, 75, 75, Color.Black);
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

            if (string.IsNullOrEmpty(_uad.ProfileThumbPicURL) ||
                _ups.Count == 2 && photoEdited == photoOne)
            {
                _uad.ProfileThumbPicURL = fileNameFull;
                _uad.LastPhotoUpdate = DateTime.UtcNow;
                _uad.Set();
            }
            else
            {
                if (up1 != null)
                {
                    if (_mu != null) up1.UserAccountID = Convert.ToInt32(_mu.ProviderUserKey);
                    up1.ThumbPicURL = fileNameFull;

                    if (
                        (_ups.Count == 0 && photoEdited == photoTwo) ||
                        (_ups.Count > 0 && photoEdited == photoTwo)
                        )
                    {
                        up1.RankOrder = 1;
                    }
                    else if
                        (
                        (_ups.Count == 0 && photoEdited == photoThree) ||
                        (_ups.Count > 1 && photoEdited == photoThree)
                        )
                    {
                        up1.RankOrder = 2;
                    }
                }
            }

            b.Dispose();

            if (up1 != null && up1.UserPhotoID == 0)
            {
                if (_mu != null) up1.CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                up1.Create();
            }
            else if (up1 != null && up1.UserPhotoID > 0)
            {
                up1.UpdatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);
                up1.Update();
            }


            LoadCurrentImagesViewBag(Convert.ToInt32(_mu.ProviderUserKey));

            return View(_uad);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditPhoto()
        {
            _uad = new UserAccountDetail();
            if (_mu != null)
            {
                _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

                LoadCurrentImagesViewBag(Convert.ToInt32(_mu.ProviderUserKey));
            }

            return View(_uad);
        }

        private void LoadCurrentImagesViewBag(int userAccountID)
        {
            var ups = new UserPhotos();
            ups.GetUserPhotos(userAccountID);

            switch (ups.Count)
            {
                case 0:
                    {
                        var up = new UserPhoto();
                        ups.Add(up);
                        up = new UserPhoto();
                        ups.Add(up);
                    }
                    break;
                case 1:
                    {
                        var up = new UserPhoto {RankOrder = 2};
                        ups.Add(up);
                    }
                    break;
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
            var model = new DirectMessages();

            if (_mu != null) model.GetMailPageWiseFromUser(pageNumber, PageSize, Convert.ToInt32(_mu.ProviderUserKey));

            model.AllInInbox = false;

            return Json(new
                {
                    ListItems = model.ToUnorderdList
                });
        }


        [Authorize]
        public JsonResult ReplyMailItems(int pageNumber)
        {
            //TODO: DON'T DO THIS basing the user on the referrer, this will be blank if it uses SSL, change then

            if (Request.UrlReferrer != null)
            {
                var referrring = Request.UrlReferrer.ToString();
                var partsOfreferring = referrring.Split('/');
                var ua = new UserAccount(partsOfreferring[partsOfreferring.Length - 1]);
                var model = new DirectMessages();

                if (_mu != null)
                    model.GetMailPageWiseToUser(pageNumber, PageSize,
                                                Convert.ToInt32(_mu.ProviderUserKey), ua.UserAccountID);

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
            return null;
        }

        [Authorize]
        public JsonResult MailItems(int pageNumber)
        {
            var model = new DirectMessages();

            if (_mu != null) model.GetMailPageWise(pageNumber, PageSize, Convert.ToInt32(_mu.ProviderUserKey));

            var sb = new StringBuilder();

            foreach (var cnt in model)
            {
                sb.Append(cnt.ToUnorderdListItem);
            }

            foreach (var dm in model.Where(dm => !dm.IsRead))
            {
                dm.IsRead = true;
                dm.Update();
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
            var model = new DirectMessages();

            if (_mu != null)
                ViewBag.RecordCount = model.GetMailPageWise(1, PageSize, Convert.ToInt32(_mu.ProviderUserKey));

            ViewBag.DirectMessages = model.ToUnorderdList;

            foreach (var dm in model.Where(dm => !dm.IsRead))
            {
                dm.IsRead = true;
                dm.Update();
            }

            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult Send()
        {
            var displayname = Request.Form["displayname"];
            var msg = Request.Form["message"];

            _ua = new UserAccount(displayname);

            if (_mu != null && (string.IsNullOrEmpty(msg) ||
                                Lib.BOL.BlockedUser.IsBlockedUser(_ua.UserAccountID,
                                                                                           Convert.ToInt32(
                                                                                               _mu.ProviderUserKey)))
                )
            {
                return RedirectToAction("Reply", new {@userName = displayname});
            }

            var dm = new DirectMessage {IsRead = false};
            if (_mu != null) dm.FromUserAccountID = Convert.ToInt32(_mu.ProviderUserKey);
            dm.ToUserAccountID = _ua.UserAccountID;
            dm.Message = msg;
            if (_mu != null) dm.CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey);

            dm.Create();

            var language = Utilities.GetCurrentLanguageCode();
            // change language for message to

            _uad = new UserAccountDetail();
            _uad.GetUserAccountDeailForUser(_ua.UserAccountID);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(_uad.DefaultLanguage);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_uad.DefaultLanguage);

            var sb = new StringBuilder();

            sb.Append(Messages.Hello);
            sb.Append(",");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("{0}: ", Messages.From);
            sb.Append(GeneralConfigs.SiteDomain);
            sb.Append("/");
            if (_mu != null) sb.Append(_mu.UserName);
            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("{0}: ", Messages.Message);
            sb.AppendLine(msg);
            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("{0}: {1}", Messages.SignIn, GeneralConfigs.SiteDomain);
            sb.AppendLine(Environment.NewLine);


            if (_uad.EmailMessages)
            {
                if (_mu != null)
                    _mail.SendMail(
                        AmazonCloudConfigs.SendFromEmail,
                        _ua.EMail, string.Format("{0}: {1}", Messages.From, _mu.UserName), sb.ToString());
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

            return RedirectToAction("Reply", new {@userName = displayname});
        }


        [Authorize]
        [HttpGet]
        public ActionResult Outbox()
        {
            var model = new DirectMessages();

            if (_mu != null)
                ViewBag.RecordCount = model.GetMailPageWiseFromUser(1, PageSize, Convert.ToInt32(_mu.ProviderUserKey));

            model.AllInInbox = false;

            ViewBag.DirectMessages = model.ToUnorderdList;

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Reply(string userName)
        {
            _ua = new UserAccount(userName);

            ViewBag.DisplayName = _ua.UserName;

            var model = new DirectMessages();

            if (_mu != null)
                ViewBag.RecordCount = model.GetMailPageWiseToUser(1, 
                    PageSize, 
                    Convert.ToInt32(_mu.ProviderUserKey),
                    _ua.UserAccountID);

            ViewBag.DirectMessages = model.ToUnorderdList;

            return View();
        }

        #endregion

        #region playlist

        [HttpPost]
        [Authorize]
        public ActionResult Playlist(NameValueCollection nvc)
        {
            if (nvc == null) throw new ArgumentNullException("nvc");
            
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));
            ViewBag.UserName = _ua.UserName;

            var plyst = new Playlist();

            plyst.GetUserPlaylist(_ua.UserAccountID);

            ViewBag.UserPlaylistID = plyst.PlaylistID;

            var plyvids = new PlaylistVideos();
            plyvids.GetPlaylistVideosForPlaylist(plyst.PlaylistID);

            nvc = Request.Form;

            PlaylistVideo plv = null;

            if (nvc["video_delete_id"] != null)
            {
                foreach (var plv1 in plyvids)
                {
                    if (plv != null && plv1.RankOrder > plv.RankOrder)
                    {
                        plv1.RankOrder--;
                        plv1.UpdatedByUserID = _ua.UserAccountID;
                        plv1.Update();
                    }

                    if (plv1.PlaylistID != plyst.PlaylistID || Convert.ToInt32(nvc["video_delete_id"]) != plv1.VideoID)
                        continue;
                    plv = new PlaylistVideo(plv1.PlaylistVideoID);

                    if (PlaylistVideo.Delete(plyst.PlaylistID, Convert.ToInt32(nvc["video_delete_id"])))
                    {
                        // deleted
                    }
                }
            }
            else if (nvc["video_down_id"] != null)
            {
                plv = new PlaylistVideo();
                plv.Get(plyst.PlaylistID, Convert.ToInt32(nvc["video_down_id"]));

                foreach (var plv1 in plyvids.Where(plv1 => plv1.RankOrder == (plv.RankOrder + 1)))
                {
                    plv1.RankOrder--;
                    plv1.UpdatedByUserID = _ua.UserAccountID;
                    plv1.Update();
                }

                plv.RankOrder++;
                plv.UpdatedByUserID = _ua.UserAccountID;
                plv.Update();
            }
            else if (nvc["video_up_id"] != null)
            {
                plv = new PlaylistVideo();
                plv.Get(plyst.PlaylistID, Convert.ToInt32(nvc["video_up_id"]));

                foreach (PlaylistVideo plv1 in plyvids.Where(plv1 => plv1.RankOrder == (plv.RankOrder - 1)))
                {
                    plv1.RankOrder++;
                    plv1.UpdatedByUserID = _ua.UserAccountID;
                    plv1.Update();
                }

                plv.RankOrder--;
                plv.UpdatedByUserID = _ua.UserAccountID;
                plv.Update();
            }
            else 
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
            
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            ViewBag.UserName = _ua.UserName;

            var plyst = new Playlist();

            plyst.GetUserPlaylist(_ua.UserAccountID);

            ViewBag.AutoPlay = plyst.AutoPlay;

            ViewBag.AutoPlayNumber = (plyst.AutoPlay) ? 1 : 0;

            ViewBag.UserPlaylistID = plyst.PlaylistID;

            var plyvids = new PlaylistVideos();

            plyvids.GetPlaylistVideosForPlaylist(plyst.PlaylistID);

            var vids = new Videos();
            vids.AddRange(plyvids.Select(plv => new Video(plv.VideoID)));

            var sngrcs = new SongRecords();
            sngrcs.AddRange(vids.Select(vi => new SongRecord(vi)));

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
                ModelState.AddModelError("", Messages.Invalid + @": " + Messages.Account);
                return View(model);
            }

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

            if (!GeneralConfigs.EnableSameIP && UserAccount.IsAccountIPTaken(Request.UserHostAddress) &&
                string.IsNullOrEmpty(model.RefUser))
            {
                ModelState.AddModelError("", Messages.Invalid + @": " + Messages.Account);
                return View(model);
            }

            TryUpdateModel(model);

            if (!ModelState.IsValid) return View(model);

            if (!Utilities.IsEmail(model.Email))
            {
                ModelState.AddModelError("", Messages.IncorrectFormat + @": " + Messages.EMail);
                return View();
            }
            if (
                model.UserName.Trim().Contains(" ") ||
                model.UserName.Trim().Contains("?") ||
                model.UserName.Trim().Contains("*") ||
                model.UserName.Trim().Contains(":") ||
                model.UserName.Trim().Contains("/") ||
                model.UserName.Trim().Contains(@"\"))
            {
                ModelState.AddModelError("", Messages.Invalid + @": " + Messages.UserName);
                return View();
            }
            if (model.YouAreID == null)
            {
                ModelState.AddModelError("", Messages.Invalid + @": " + Messages.YouAre);
                return View();
            }

            DateTime dt;

            if (!DateTime.TryParse(model.Year
                                   + "-" + model.Month + "-" + model.Day, out dt))
            {
                ModelState.AddModelError("", Messages.Invalid + @": " + Messages.BirthDate);
                return View();
            }
            if (DateTime.TryParse(model.Year
                                  + "-" + model.Month + "-" + model.Day, out dt))
            {
                if (Utilities.CalculateAge(dt) < GeneralConfigs.MinimumAge)
                {
                    ModelState.AddModelError("", Messages.Invalid + @": " + Messages.BirthDate);
                    return View();
                }
            }

            model.UserName = model.UserName.Replace(":", string.Empty);
            model.UserName = model.UserName.Replace(" ", string.Empty);
            model.UserName = model.UserName.Replace(".", string.Empty);

            MembershipCreateStatus createStatus;

            Membership.CreateUser(model.UserName, model.NewPassword, model.Email, "Q", "A", true, out createStatus);

            if (createStatus == MembershipCreateStatus.Success)
            {
                FormsAuthentication.RedirectFromLoginPage(model.UserName, true);

                var ua = new UserAccount(model.UserName);
                _uad = new UserAccountDetail
                {
                    UserAccountID = ua.UserAccountID,
                    BirthDate = dt,
                    YouAreID = model.YouAreID,
                    DisplayAge = true,
                    DefaultLanguage = Utilities.GetCurrentLanguageCode()
                };

                if (!string.IsNullOrEmpty(model.RefUser))
                {
                    var refUser = new UserAccount(model.RefUser);
                    _uad.ReferringUserID = refUser.UserAccountID;
                }

                _uad.Set();

                var sb = new StringBuilder(100);

                sb.Append(Messages.Hello);
                sb.Append(Environment.NewLine);
                sb.Append(Messages.YourNewAccountIsReadyForUse);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.AppendFormat("{0}: ", Messages.UserName);
                sb.Append(ua.UserName);
                sb.Append(Environment.NewLine);
                sb.AppendFormat("{0}: ", Messages.Password);
                sb.Append(model.NewPassword);
                sb.Append(Environment.NewLine);
                sb.Append(GeneralConfigs.SiteDomain);

                _mail.SendMail(AmazonCloudConfigs.SendFromEmail, ua.EMail, Messages.YourNewAccountIsReadyForUse, sb.ToString());

                // see if this is the 1st user
                var recentUsers = new UserAccounts();
                recentUsers.GetNewestUsers();

                if (recentUsers.Count == 1)
                {
                    var adminRole = new Role(SiteEnums.RoleTypes.admin.ToString());

                    UserAccountRole.AddUserToRole(ua.UserAccountID, adminRole.RoleID);
                }

                var dm = new DirectMessage {IsRead = false};
                var admin = new UserAccount(GeneralConfigs.AdminUserName);

                dm.FromUserAccountID = admin.UserAccountID;
                dm.ToUserAccountID = ua.UserAccountID;

                sb = new StringBuilder(100);

                sb.AppendLine("Welcome to the klub!");
                sb.AppendLine();
                sb.AppendLine("We are SO happy to have you here as a member of our elite group! Das Klubbers are very nice people and of course great dancers.");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Make SURE to introduce yourself here: http://dasklub.com/forum/introduce-yourself/start-your-self-introduction-here ");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("If you have any questions about your account, make sure to read this article: http://dasklub.com/news/how-to-use-das-klub");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("You are one of the predestined ones.");
                sb.AppendLine();
                sb.AppendLine("- The Admin");
                sb.AppendLine();
                dm.Message = sb.ToString();

                if (admin.UserAccountID != 0)
                {
                    dm.CreatedByUserID = admin.UserAccountID;
                }

                dm.Create();

                return RedirectToAction("Home", "Account");
            }
            ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));

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
            if (!ModelState.IsValid) return View(model);

            var mu = Membership.GetUser();

            if (mu != null && mu.ChangePassword(model.OldPassword, model.NewPassword))
            {
                return RedirectToAction("ChangePasswordSuccess");
            }
            ModelState.AddModelError("", Messages.ThePasswordsEnteredDoNotMatch);

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

            _uad = new UserAccountDetail();

            if (_mu != null) _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

            ViewBag.UserAccountDetail = _uad;
            ViewBag.Membership = _mu;

            return View(_uad);
        }

        private void CanBeStealth()
        {
            ViewBag.CanBeStealth = (_mu != null &&
                                    (Roles.IsUserInRole(_mu.UserName, SiteEnums.RoleTypes.supporter.ToString()) ||
                                     Roles.IsUserInRole(_mu.UserName, SiteEnums.RoleTypes.admin.ToString())
                                    ));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Settings(NameValueCollection nvc)
        {
            ViewBag.IsValid = true;

            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            _uad = new UserAccountDetail();

            if (_mu != null) _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

            var enableProfileLogging = Request.Form["enableprofilelogging"];
            var emailmessages = Request.Form["emailmessages"];
            var showonmap = Request.Form["showonmap"];
            var displayAge = Request.Form["displayage"];
            var membersOnlyProfile = Request.Form["membersonlyprofile"];

            _uad.MembersOnlyProfile = !string.IsNullOrEmpty(membersOnlyProfile);
            _uad.EnableProfileLogging = !string.IsNullOrEmpty(enableProfileLogging);
            _uad.DisplayAge = !string.IsNullOrEmpty(displayAge);
            _uad.EmailMessages = !string.IsNullOrEmpty(emailmessages);
            _uad.ShowOnMap = !string.IsNullOrEmpty(showonmap);

            _uad.Set();

            var username = Request.Form["username"].Trim();
            var isNewUserName = false;
            bool isValidName;

            try
            {
                isValidName = !Regex.IsMatch(@"[A-Za-z][A-Za-z0-9_]{3,14}", username);
            }
            catch
            {
                // bad name
                isValidName = false;
            }

            if (_mu.UserName != username && isValidName)
            {
                // TODO: PUT IN ALL THE SAME VALIDATION AS REGISTRATION
                isNewUserName = true;
                var newUsername = new UserAccount(username.Replace(":", string.Empty) /* still annoying errors */);

                if (newUsername.UserAccountID != 0)
                {
                    ViewBag.IsValid = false;
                    ModelState.AddModelError("", Messages.AlreadyInUse + @": " + Messages.UserName);
                    _uad = new UserAccountDetail();
                    _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));
                    ViewBag.UserAccountDetail = _uad;
                    ViewBag.Membership = _mu;
                    return View();
                }
                if (!Utilities.IsEmail(Request.Form["email"]))
                {
                    ViewBag.IsValid = false;
                    ModelState.AddModelError("", Messages.Invalid + @": " + Messages.EMail);
                    return View();
                }
                if (Request.Form["email"].Trim() != _ua.EMail)
                {
                    _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey)) {EMail = Request.Form["email"]};
                    _ua.Update();
                }

                username = username.Replace(":", string.Empty);
                username = username.Replace(" ", string.Empty);
                username = username.Replace(".", string.Empty);
                _ua.UserName = username;
                _ua.Update();
                FormsAuthentication.SetAuthCookie(username, false);
                ViewBag.IsValid = true;
            }
            else if (!Utilities.IsEmail(Request.Form["email"]))
            {
                ViewBag.IsValid = false;
                ModelState.AddModelError("", Messages.Invalid + @": " + Messages.EMail);
                return View();
            }
            else if (Request.Form["email"].Trim() != _ua.EMail)
            {
                _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey)) {EMail = Request.Form["email"]};
                _ua.Update();
            }

            ViewBag.ProfileUpdated = true;

            _uad = new UserAccountDetail();

            _uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));
            

            ViewBag.UserAccountDetail = _uad;
            ViewBag.Membership = _mu;

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
            var model = new Content(Convert.ToInt32(id));

            var s3 = new S3Service
                {
                    AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                    SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                };

            if (!string.IsNullOrWhiteSpace(model.ContentVideoURL) &&
                s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL))
            {
                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL);

                model.ContentVideoURL = string.Empty;
                model.Set();
            }

            if (!string.IsNullOrWhiteSpace(model.ContentVideoURL2) &&
                s3.ObjectExists(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL2))
            {
                s3.DeleteObject(AmazonCloudConfigs.AmazonBucketName, model.ContentVideoURL2);

                model.ContentVideoURL2 = string.Empty;
                model.Set();
            }


            return RedirectToAction("EditArticle", new {@id = model.ContentID});
        }

        #region status updates

        public Image DownloadImage(string url)
        {
            Image tmpImage = null;

            try
            {
                // Open a connection
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);

                httpWebRequest.AllowWriteStreamBuffering = true;

                // You can also specify additional header values like the user agent or the referer: (Optional)
                httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                httpWebRequest.Referer = "http://www.google.com/";

                // set timeout for 20 seconds (Optional)
                httpWebRequest.Timeout = 20000;

                // Request response:
                var webResponse = httpWebRequest.GetResponse();

                // Open data stream:
                var webStream = webResponse.GetResponseStream();

                // convert webstream to image
                if (webStream != null) tmpImage = Image.FromStream(webStream);

                // Cleanup
                webResponse.Close();
                webResponse.Close();
            }
            catch (Exception exception)
            {
                // Error
                Console.WriteLine(@"Exception caught in process: {0}", exception);
                return null;
            }

            return tmpImage;
        }

        [HttpPost]
        [Authorize]
        public ActionResult StatusDelete(NameValueCollection nvc)
        {
            if (Request.Form["delete_status_id"] != null)
            {
                var su = new StatusUpdate(
                    Convert.ToInt32(Request.Form["delete_status_id"])
                    );

                // delete all acknowledgements for status

                Acknowledgements.DeleteStatusAcknowledgements(su.StatusUpdateID);

                su.Delete();
            }
            else if (Request.Form["status_update_id_beat"] != null ||
                     Request.Form["status_update_id_applaud"] != null)
            {
                

                if (_mu != null)
                {
                    var ack = new Acknowledgement
                        {
                            CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey),
                            UserAccountID = Convert.ToInt32(_mu.ProviderUserKey)
                        };

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
            }

            return RedirectToAction("Home");
        }


        /// <summary>
        ///     Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name="image">
        ///     The <see cref="System.Drawing.Image" /> to rotate
        /// </param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        /// <returns>
        ///     A new <see cref="System.Drawing.Bitmap" /> that is just large enough
        ///     to contain the rotated image without cutting any corners off.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <see cref="image" /> is null.
        /// </exception>
        private static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            const double pi2 = Math.PI/2.0;

            // Why can't C# allow these to be const, or at least readonly
            // *sigh*  I'm starting to talk like Christian Graus :omg:
            double oldWidth = image.Width;
            double oldHeight = image.Height;

            // Convert degrees to radians
            double theta = (angle)*Math.PI/180.0;
            double lockedTheta = theta;

            // Ensure theta is now [0, 2pi)
            while (lockedTheta < 0.0)
                lockedTheta += 2*Math.PI;

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
            if ((lockedTheta >= 0.0 && lockedTheta < pi2) ||
                (lockedTheta >= Math.PI && lockedTheta < (Math.PI + pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(lockedTheta))*oldWidth;
                oppositeTop = Math.Abs(Math.Sin(lockedTheta))*oldWidth;

                adjacentBottom = Math.Abs(Math.Cos(lockedTheta))*oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(lockedTheta))*oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(lockedTheta))*oldHeight;
                oppositeTop = Math.Abs(Math.Cos(lockedTheta))*oldHeight;

                adjacentBottom = Math.Abs(Math.Sin(lockedTheta))*oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(lockedTheta))*oldWidth;
            }

            double newWidth = adjacentTop + oppositeBottom;
            double newHeight = adjacentBottom + oppositeTop;

            var nWidth = (int) Math.Ceiling(newWidth);
            var nHeight = (int) Math.Ceiling(newHeight);

            var rotatedBmp = new Bitmap(nWidth, nHeight);

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
                if (lockedTheta >= 0.0 && lockedTheta < pi2)
                {
                    points = new[]
                        {
                            new Point((int) oppositeBottom, 0),
                            new Point(nWidth, (int) oppositeTop),
                            new Point(0, (int) adjacentBottom)
                        };
                }
                else if (lockedTheta >= pi2 && lockedTheta < Math.PI)
                {
                    points = new[]
                        {
                            new Point(nWidth, (int) oppositeTop),
                            new Point((int) adjacentTop, nHeight),
                            new Point((int) oppositeBottom, 0)
                        };
                }
                else if (lockedTheta >= Math.PI && lockedTheta < (Math.PI + pi2))
                {
                    points = new[]
                        {
                            new Point((int) adjacentTop, nHeight),
                            new Point(0, (int) adjacentBottom),
                            new Point(nWidth, (int) oppositeTop)
                        };
                }
                else
                {
                    points = new[]
                        {
                            new Point(0, (int) adjacentBottom),
                            new Point((int) oppositeBottom, 0),
                            new Point((int) adjacentTop, nHeight)
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
            

            var su = new StatusUpdate(statusUpdateID);

            if (su.PhotoItemID != null && su.PhotoItemID > 0)
            {
                const CannedAcl acl = CannedAcl.PublicRead;

                var s3 = new S3Service
                    {
                        AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                        SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                    };

                var pitm = new PhotoItem(Convert.ToInt32(su.PhotoItemID));

                // full
                Image imgFull = DownloadImage(Utilities.S3ContentPath(pitm.FilePathRaw));

                const float angle = 90;

                Bitmap b = RotateImage(imgFull, angle);

                Image fullPhoto = b;

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
                Image photoResized = b;

                string fileNameResize = Guid.NewGuid() + ".jpg";

                photoResized = ImageResize.FixedSize(photoResized, 500, 375, Color.Black);

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
                Image thumbPhoto = b;

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

            return RedirectToAction("statusupdate", new {statusUpdateID});
        }

        [Authorize]
        public JsonResult StatusUpdates(int pageNumber)
        {
            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            ViewBag.CurrentUser = _ua.ToUnorderdListItem;

            var preFilter = new StatusUpdates();

            preFilter.GetStatusUpdatesPageWise(pageNumber, 5);

            var sb = new StringBuilder();

            foreach (var cnt in preFilter)
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
            var su = new StatusUpdate(statusUpdateID) {PhotoDisplay = false};
            var sus = new StatusUpdates {su};
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

            if (message != null) message = message.Trim();

            if (_mu != null) _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            ViewBag.CurrentUser = _ua.ToUnorderdListItem;

            var su = new StatusUpdate();

            su.GetMostRecentUserStatus(_ua.UserAccountID);

            var startTime = DateTime.UtcNow;

            var span = startTime.Subtract(su.CreateDate);

            // TODO: this is not working properly, preventing posts
            if (su.Message == message && file == null)
            {
                // double post
                return RedirectToAction("Home");
            }
            su = new StatusUpdate();

            if (file != null && Utilities.IsImageFile(file.FileName))
            {
                var b = new Bitmap(file.InputStream);

                const CannedAcl acl = CannedAcl.PublicRead;

                var s3 = new S3Service
                    {
                        AccessKeyID = AmazonCloudConfigs.AmazonAccessKey,
                        SecretAccessKey = AmazonCloudConfigs.AmazonSecretKey
                    };

                var pitem = new PhotoItem {CreatedByUserID = _ua.UserAccountID, Title = message};

                Image fullPhoto = b;

                var fileNameFull = Utilities.CreateUniqueContentFilename(file);

                var maker = fullPhoto.ToAStream(ImageFormat.Jpeg);

                s3.AddObject(
                    maker,
                    maker.Length,
                    AmazonCloudConfigs.AmazonBucketName,
                    fileNameFull,
                    file.ContentType,
                    acl);

                pitem.FilePathRaw = fileNameFull;

                // resized
                Image photoResized = b;

                var fileNameResize = Utilities.CreateUniqueContentFilename(file);

                photoResized = ImageResize.FixedSize(photoResized, 500, 375, Color.Black);

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
                Image thumbPhoto = b;

                thumbPhoto = ImageResize.Crop(thumbPhoto, 150, 150, ImageResize.AnchorPosition.Center);

                var fileNameThumb = Utilities.CreateUniqueContentFilename(file);

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

            su.UserAccountID = _ua.UserAccountID;
            su.Message = message;
            su.CreatedByUserID = _ua.UserAccountID;
            su.IsMobile = Request.Browser.IsMobileDevice;
            su.Create();

            if (Request.Browser.IsMobileDevice)
            {
                return new RedirectResult(Url.Action("Home") + "#most_recent");
            }
            return RedirectToAction("Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Home()
        {
            if (_mu == null)
            {
                return RedirectToAction("LogOff");
            }

            var uas = new UserAccounts();
            uas.GetNewestUsers();
            ViewBag.NewestUsers = uas.ToUnorderdList;

            _ua = new UserAccount(Convert.ToInt32(_mu.ProviderUserKey));

            ViewBag.CurrentUser = _ua.ToUnorderdListItem;

            var preFilter = new StatusUpdates();

            preFilter.GetStatusUpdatesPageWise(1, PageSize);

            var sus = new StatusUpdates();
            sus.AddRange(
                preFilter.Where(
                    su1 =>
                    !Lib.BOL.BlockedUser.IsBlockingUser(_ua.UserAccountID, su1.UserAccountID)));

            sus.IncludeStartAndEndTags = false;
            ViewBag.StatusUpdateList = string.Format(@"<ul id=""status_update_list_items"">{0}</ul>", sus.ToUnorderdList);

            var suns = new StatusUpdateNotifications();
            suns.GetStatusUpdateNotificationsForUser(_ua.UserAccountID);

            if (suns.Count > 0)
            {
                suns.Sort((p1, p2) => p1.CreateDate.CompareTo(p2.CreateDate));

                ViewBag.Notifications = suns;

                foreach (var sun1 in suns)
                {
                    sun1.IsRead = true;
                    sun1.Update();
                }
            }

            var applauseResult = new StatusUpdates();
            applauseResult.GetMostAcknowledgedStatus(7, SiteEnums.AcknowledgementType.A);
            if (applauseResult.Count > 0)
            {
                ViewBag.MostApplauded = applauseResult;
            }

            var beatDownResult = new StatusUpdate();
            beatDownResult.GetMostAcknowledgedStatus(7, SiteEnums.AcknowledgementType.B);

            var isAlreadyApplauded = false;

            if (beatDownResult.StatusUpdateID > 0)
            {
                if (applauseResult.Any(ssr1 => beatDownResult.StatusUpdateID == ssr1.StatusUpdateID))
                {
                    isAlreadyApplauded = true;
                }
            }

            if (!isAlreadyApplauded && beatDownResult.StatusUpdateID > 0)
            {
                ViewBag.MostBeatDown = beatDownResult;
            }
            
            var commentResponse = new StatusUpdate(StatusComments.GetMostCommentedOnStatus(DateTime.UtcNow.AddDays(-7)));

            var isAlreadyCommented = false;

            foreach (var ssr1 in applauseResult.Where(ssr1 => commentResponse.StatusUpdateID == ssr1.StatusUpdateID))
            {
                isAlreadyCommented = true;
            }

            if (!isAlreadyCommented && beatDownResult.StatusUpdateID != commentResponse.StatusUpdateID &&
                commentResponse.StatusUpdateID > 0)
            {
                ViewBag.MostCommented = commentResponse;
            }

            return View();
        }

        #endregion

        #region forgot password

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var ua = new UserAccount();
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

                _mu = Membership.GetUser(ua.UserName);
                if (_mu != null)
                {
                    var newPassword = _mu.ResetPassword();

                    _mail.SendMail(AmazonCloudConfigs.SendFromEmail, email, Messages.PasswordReset,
                                       string.Format("{0}: {1}{2}{2}{3}: {4}{2}{2}{5}: {6}",
                                       Messages.UserName, ua.UserName, Environment.NewLine, 
                                       Messages.NewPassword, newPassword, Messages.SignIn, GeneralConfigs.SiteDomain));
                }

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