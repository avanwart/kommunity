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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL;
using DasKlub.Lib.BOL.ArtistContent;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Services;
using DasKlub.Lib.Values;
using DasKlub.Web.Models;

namespace DasKlub.Web.Controllers
{
    public class ProfileController : Controller
    {
        private const int Maxcountusers = 5000;
        public static readonly int PageSize = 25;
        private PhotoItem _pitm;
        private UserAccount _ua;
        private readonly MembershipUser _mu;
        private readonly IMailService _mail;

        public ProfileController(IMailService mail)
        {
            _mail = mail;
            _mu = Membership.GetUser();
            
            if (_mu != null) _ua = new UserAccount(_mu.UserName);
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

            if (!string.IsNullOrWhiteSpace(ups[0].PicURL))
            {
                ViewBag.SecondUserPhotoFull = ups[0].FullProfilePicURL;
                ViewBag.SecondUserPhotoThumb = ups[0].FullProfilePicThumbURL;
            }

            if (string.IsNullOrWhiteSpace(ups[1].PicURL)) return;
            ViewBag.ThirdUserPhotoFull = ups[1].FullProfilePicURL;
            ViewBag.ThirdUserPhotoThumb = ups[1].FullProfilePicThumbURL;
        }

        [HttpGet]
        public ActionResult ProfileDetail(string userName)
        {
            ViewBag.VideoHeight = (Request.Browser.IsMobileDevice) ? 100 : 277;
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ? 225 : 400;

            _ua = new UserAccount(userName);

            var uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(_ua.UserAccountID);

            uad.BandsSeen = ContentLinker.InsertBandLinks(uad.BandsSeen, false);
            uad.BandsToSee = ContentLinker.InsertBandLinks(uad.BandsToSee, false);

            var model = new ProfileModel();

            if (_ua.UserAccountID > 0)
            {
                model.UserAccountID = _ua.UserAccountID;
                model.PhotoCount = PhotoItems.GetPhotoItemCountForUser(_ua.UserAccountID);
                model.CreateDate = _ua.CreateDate;
            }

            if (_mu != null)
            {
                ViewBag.IsBlocked = BlockedUser.IsBlockedUser(_ua.UserAccountID, Convert.ToInt32(_mu.ProviderUserKey));
                ViewBag.IsBlocking = BlockedUser.IsBlockedUser(Convert.ToInt32(_mu.ProviderUserKey), _ua.UserAccountID);

                if (_ua.UserAccountID == Convert.ToInt32(_mu.ProviderUserKey))
                {
                    model.IsViewingSelf = true;
                }
                else
                {
                    var ucon = new UserConnection();

                    ucon.GetUserToUserConnection(Convert.ToInt32(_mu.ProviderUserKey), _ua.UserAccountID);

                    model.UserConnectionID = ucon.UserConnectionID;


                    if (BlockedUser.IsBlockedUser(Convert.ToInt32(_mu.ProviderUserKey), _ua.UserAccountID))
                    {
                        return RedirectToAction("index", "home");
                    }
                }
            }
            else
            {
                if (uad.MembersOnlyProfile)
                {
                    return RedirectToAction("LogOn", "Account");
                }
            }

            //
            model.UserName = _ua.UserName;
            model.CreateDate = _ua.CreateDate;
            model.LastActivityDate = _ua.LastActivityDate;
            //
            model.DisplayAge = uad.DisplayAge;
            model.Age = uad.YearsOld;
            model.BandsSeen = uad.BandsSeen;
            model.BandsToSee = uad.BandsToSee;
            model.HardwareAndSoftwareSkills = uad.HardwareSoftware;
            model.MessageToTheWorld = uad.AboutDescription;

            model.YouAreFull = uad.Sex;
            model.InterestedInFull = uad.InterestedFull;
            model.RelationshipStatusFull = uad.RelationshipStatusFull;
            model.RelationshipStatus = uad.RelationshipStatus;
            model.InterestedIn = uad.InterestedIn;
            model.YouAre = uad.YouAre;

            model.Website = uad.ExternalURL;
            model.CountryCode = uad.Country;
            model.CountryName = uad.CountryName;
            model.IsBirthday = uad.IsBirthdayToday;
            model.ProfilePhotoMain = uad.FullProfilePicURL;
            ViewBag.ThumbIcon = uad.FullProfilePicURL;
            model.ProfilePhotoMainThumb = uad.FullProfilePicThumbURL;
            model.DefaultLanguage = uad.DefaultLanguage;

            model.EnableProfileLogging = uad.EnableProfileLogging;
            model.Handed = uad.HandedFull;
            model.RoleIcon = uad.SiteBages;


            //
            var su = new StatusUpdate();
            su.GetMostRecentUserStatus(_ua.UserAccountID);

            if (su.StatusUpdateID > 0)
            {
                model.LastStatusUpdate = su.CreateDate;
                model.MostRecentStatusUpdate = su.Message;
            }

            model.ProfileVisitorCount = ProfileLog.GetUniqueProfileVisitorCount(_ua.UserAccountID);

            var ptiems = new PhotoItems();
            ptiems.GetUserPhotos(_ua.UserAccountID);

            if (ptiems.Count > 0)
            {
                ptiems.Sort((x, y) => (y.CreateDate.CompareTo(x.CreateDate)));

                var ptiemsDisplay = new PhotoItems();

                const int maxPhotos = 8;

                foreach (PhotoItem pitm1 in ptiems)
                {
                    pitm1.UseThumb = true;
                    if (ptiemsDisplay.Count < maxPhotos)
                    {
                        ptiemsDisplay.Add(pitm1);
                    }
                    else break;
                }

                ptiemsDisplay.UseThumb = true;
                ptiemsDisplay.ShowTitle = false;

                model.HasMoreThanMaxPhotos = (ptiems.Count > maxPhotos);
                ptiemsDisplay.IsUserPhoto = true;
                model.PhotoItems = ptiemsDisplay.ToUnorderdList;
            }

            var conts = new Contents();

            conts.GetContentForUser(_ua.UserAccountID);

            model.NewsCount = conts.Count;

            if (conts.Count > 0)
            {
                conts.Sort((x, y) => (y.ReleaseDate.CompareTo(x.ReleaseDate)));

                var displayContents = new Contents();
                const int maxCont = 1;
                var currentCount = 0;
                foreach (var ccn1 in conts)
                {
                    currentCount++;
                    if (maxCont >= currentCount)
                    {
                        displayContents.Add(ccn1);
                    }
                    else break;
                }

                displayContents.IncludeStartAndEndTags = false;

                model.NewsArticles = displayContents.ToUnorderdList;
            }

            model.MetaDescription = _ua.UserName + " " + Messages.Profile + " " +
                                    FromDate.DateToYYYY_MM_DD(_ua.LastActivityDate);

            // playlist
            var plyst = new Playlist();

            plyst.GetUserPlaylist(_ua.UserAccountID);

            if (plyst.PlaylistID > 0 && PlaylistVideos.GetCountOfVideosInPlaylist(plyst.PlaylistID) > 0)
            {
                ViewBag.AutoPlay = plyst.AutoPlay;
                ViewBag.AutoPlayNumber = (plyst.AutoPlay) ? 1 : 0;
                ViewBag.UserPlaylistID = plyst.PlaylistID;
            }


            if (uad.UserAccountID > 0)
            {
                model.Birthday = uad.BirthDate;

                if (uad.ShowOnMapLegal)
                {
                    // because of the foreign cultures, numbers need to stay in the English version unless a javascript encoding could be added
                    var currentLang = Utilities.GetCurrentLanguageCode();

                    Thread.CurrentThread.CurrentUICulture =
                        CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
                    Thread.CurrentThread.CurrentCulture =
                        CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());


                    model.DisplayOnMap = uad.ShowOnMapLegal;

                    var rnd = new Random();
                    int offset = rnd.Next(10, 100);

                    var latlong = GeoData.GetLatLongForCountryPostal(uad.Country, uad.PostalCode);

                    if (latlong.latitude != 0 && latlong.longitude != 0)
                    {
                        model.Latitude =
                            Convert.ToDecimal(latlong.latitude + Convert.ToDouble("0.00" + offset))
                                   .ToString(CultureInfo.InvariantCulture);
                        model.Longitude =
                            Convert.ToDecimal(latlong.longitude + Convert.ToDouble("0.00" + offset))
                                   .ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        model.DisplayOnMap = false;
                    }

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);
                }


                ViewBag.ThumbIcon = uad.FullProfilePicThumbURL;

                LoadCurrentImagesViewBag(uad.UserAccountID);
            }


            ViewBag.UserAccountDetail = uad;
            ViewBag.UserAccount = _ua;

            var ucons = new UserConnections();
            ucons.GetUserConnections(_ua.UserAccountID);
            ucons.Shuffle();

            var irlContacts = new UserAccounts();
            var cyberAssociates = new UserAccounts();

            foreach (var uc1 in ucons.Where(uc1 => uc1.IsConfirmed))
            {
                UserAccount userCon;
                switch (uc1.StatusType)
                {
                    case 'C':
                        if (cyberAssociates.Count >= Maxcountusers) continue;

                        userCon = uc1.ToUserAccountID != _ua.UserAccountID
                                      ? new UserAccount(uc1.ToUserAccountID)
                                      : new UserAccount(uc1.FromUserAccountID);
                        cyberAssociates.Add(userCon);
                        break;
                    case 'R':
                        if (irlContacts.Count >= Maxcountusers) continue;

                        userCon = uc1.ToUserAccountID != _ua.UserAccountID
                                      ? new UserAccount(uc1.ToUserAccountID)
                                      : new UserAccount(uc1.FromUserAccountID);
                        irlContacts.Add(userCon);
                        break;
                }
            }

            if (irlContacts.Count > 0)
            {
                model.IRLFriendCount = irlContacts.Count;
            }

            if (cyberAssociates.Count > 0)
            {
                model.CyberFriendCount = cyberAssociates.Count;
            }

 
            UserAccountDetail uadLooker = null;

            if (_mu != null)
            {
                uadLooker = new UserAccountDetail();
                uadLooker.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));
            }

            if (uadLooker != null && (_mu != null && _ua.UserAccountID > 0 && uadLooker.EnableProfileLogging && uad.EnableProfileLogging))
            {
                var pl = new ProfileLog
                    {
                        LookedAtUserAccountID = _ua.UserAccountID,
                        LookingUserAccountID = Convert.ToInt32(_mu.ProviderUserKey)
                    };

                if (pl.LookingUserAccountID != pl.LookedAtUserAccountID) pl.Create();

                var al = ProfileLog.GetRecentProfileViews(_ua.UserAccountID);

                if (al != null && al.Count > 0)
                {
                    var uas = new UserAccounts();

                    foreach (var viewwer in al.Cast<int>().Select(id =>
                                                                          new UserAccount(id))
                                                      .Where(viewwer => !viewwer.IsLockedOut && viewwer.IsApproved)
                                                      .TakeWhile(viewwer => uas.Count < Maxcountusers))
                    {
                        uas.Add(viewwer);
                    }
                }
            }

            UserAccountVideos uavs;

            if (_ua.UserAccountID > 0)
            {
                uavs = new UserAccountVideos();

                uavs.GetRecentUserAccountVideos(_ua.UserAccountID, 'F');

                if (uavs.Count > 0)
                {
                    var favvids = new Videos();
                    favvids.AddRange(uavs.Select(uav1 => new Video(uav1.VideoID)).Where(f1 => f1.IsEnabled));

                    var sngrcds2 = new SongRecords();
                    sngrcds2.AddRange(favvids.Select(v1 => new SongRecord(v1)));

                    sngrcds2.IsUserSelected = true;

                    ViewBag.UserFavorites = sngrcds2.VideosList(); 
                }
            }

            // this is either a youtube user or this is a band
            var art = new Artist();
            art.GetArtistByAltname(userName);

            if (art.ArtistID > 0)
            {
                // try this way for dashers
                model.UserName = art.Name;
            }

            var vids = new Videos();
            var sngrs = new SongRecords();

            if (art.ArtistID == 0)
            {
                vids.GetAllVideosByUser(userName);

                uavs = new UserAccountVideos();
                uavs.GetRecentUserAccountVideos(_ua.UserAccountID, 'U');

                foreach (var f2 in uavs.Select(uav1 => new Video(uav1.VideoID)).Where(f2 => !vids.Contains(f2)))
                {
                    vids.Add(f2);
                }

                vids.Sort((x, y) => (y.PublishDate.CompareTo(x.PublishDate)));

                if (_ua != null)
                {
                    model.UserName = _ua.UserName;
                }
                else
                {
                    model.UserName = userName;    
                }
            }
            else
            {
                // photo
                var aprop = new ArtistProperty();
                aprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.PH.ToString());

                if (!string.IsNullOrEmpty(aprop.PropertyContent))
                {
                    ViewBag.ArtistPhoto = VirtualPathUtility.ToAbsolute(aprop.PropertyContent);
                    ViewBag.ThumbIcon = VirtualPathUtility.ToAbsolute(aprop.PropertyContent);
                }

                // meta descriptione
                aprop = new ArtistProperty();
                aprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.MD.ToString());
                if (!string.IsNullOrEmpty(aprop.PropertyContent)) ViewBag.MetaDescription = aprop.PropertyContent;

                // description
                aprop = new ArtistProperty();
                aprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.LD.ToString());
                if (!string.IsNullOrEmpty(aprop.PropertyContent))
                    ViewBag.ArtistDescription = ContentLinker.InsertBandLinks(aprop.PropertyContent, false);

                #region rss

                ///// rss 
                //RssResources rssrs = new RssResources();

                //rssrs.GetArtistRssResource(art.ArtistID);

                //if (rssrs.Count > 0)
                //{
                //    RssItems ritems = new RssItems();
                //    RssItems ritemsOUT = new RssItems();

                //    foreach (RssResource rssre in rssrs)
                //    {
                //        ritems.GetTopRssItemsForResource(rssre.RssResourceID);
                //        //ritm = new RssItem(rssre..ArtistID);
                //    }

                //    ritems.Sort((RssItem x, RssItem y) => (y.PubDate.CompareTo(x.PubDate)));

                //    foreach (RssItem ritm in ritems)
                //    {
                //        if (ritemsOUT.Count < 10)
                //        {
                //            ritemsOUT.Add(ritm);
                //        }
                //    }

                //    ViewBag.ArtistNews = ritemsOUT.ToUnorderdList;
                //}
                //else
                //{
                //    ViewBag.ArtistNews = null;
                //}

                //ViewBag.DisplayName = art.DisplayName;

                #endregion

                var sngss = new Songs();

                sngss.GetSongsForArtist(art.ArtistID);

                foreach (var sn1 in sngss)
                {
                    vids.GetVideosForSong(sn1.SongID);
                }
            }

            vids.Sort((p1, p2) => p2.PublishDate.CompareTo(p1.PublishDate));
            sngrs.AddRange(vids.Select(v1 => new SongRecord(v1)));

            if (_mu != null && _ua.UserAccountID != Convert.ToInt32(_mu.ProviderUserKey))
            {
                var uc1 = new UserConnection();

                uc1.GetUserToUserConnection(_ua.UserAccountID, Convert.ToInt32(_mu.ProviderUserKey));

                if (uc1.UserConnectionID > 0)
                {
                    switch (uc1.StatusType)
                    {
                        case 'C':
                            if (uc1.IsConfirmed)
                            {
                                model.IsCyberFriend = true;
                            }
                            else
                            {
                                model.IsWatingToBeCyberFriend = true;
                            }
                            break;
                        case 'R':
                            if (uc1.IsConfirmed)
                            {
                                model.IsRealFriend = true;
                            }
                            else
                            {
                                model.IsWatingToBeRealFriend = true;
                            }
                            break;
                        default:
                            model.IsDeniedCyberFriend = true;
                            model.IsDeniedRealFriend = true;
                            break;
                    }
                }
            }


            if (sngrs.Count == 0 && art.ArtistID == 0 && _ua.UserAccountID == 0)
            {
                // no longer exists
                Response.RedirectPermanent("/");
                return new EmptyResult();
            }

            var sngDisplay = new SongRecords();
            sngDisplay.AddRange(from sr1 in sngrs
                                let vidToShow = new Video(sr1.VideoID)
                                where vidToShow.IsEnabled
                                select sr1);

            model.SongRecords = sngDisplay.VideosList();

            return View(model);
        }


        [Authorize]
        public ActionResult DeleteWallItem(int wallItemID)
        {
            var wallItem = new WallMessage {WallMessageID = wallItemID};

            wallItem.Delete();

            return RedirectToAction("Visitors", "Account");
        }

        public JsonResult WallMessages(int pageNumber)
        {
            var sb = new StringBuilder();

            if (Request.UrlReferrer != null)
            {
                var referrring = Request.UrlReferrer.ToString();
                var partsOfreferring = referrring.Split('/');
                var ua = new UserAccount(partsOfreferring[partsOfreferring.Length - 1]);

                var wallItems = new WallMessages();

                wallItems.GetWallMessagessPageWise(pageNumber, 5, ua.UserAccountID);
 
                if (_mu != null && Convert.ToInt32(_mu.ProviderUserKey) == ua.UserAccountID)
                {
                    wallItems.IsUsersWall = true;
                }

                foreach (var cnt in wallItems)
                {
                    cnt.IsUsersWall = wallItems.IsUsersWall;
                    sb.Append(cnt.ToUnorderdListItem);
                }
            }

            return Json(new
                {
                    ListItems = sb.ToString()
                });
        }

        [Authorize]
        public ActionResult WriteWallMessage(string message, int toUserAccountID)
        {
            _ua = new UserAccount(toUserAccountID);

            if (_ua.UserAccountID != 0 && !string.IsNullOrWhiteSpace(message))
            {
                if (_mu != null)
                {
                    var comment = new WallMessage
                        {
                            Message = Server.HtmlEncode(message),
                            ToUserAccountID = toUserAccountID,
                            FromUserAccountID = Convert.ToInt32(_mu.ProviderUserKey),
                            CreatedByUserID = Convert.ToInt32(_mu.ProviderUserKey)
                        };

                    comment.Create();

                    var uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(_ua.UserAccountID);

                    if (uad.EmailMessages)
                    {
                        _mail.SendMail(Lib.Configs.AmazonCloudConfigs.SendFromEmail, _ua.EMail, "Wall Post From: " + _mu.UserName, _ua.UrlTo.ToString());
                    }
                }
            }

            return RedirectToAction("ProfileDetail", new {@userName = _ua.UserName});
        }


        [HttpGet]
        public ActionResult UserPhotos(string userName)
        {
            _ua = new UserAccount(userName);

            ViewBag.UserName = _ua.UserName;

            var ptiems = new PhotoItems();
            ptiems.GetUserPhotos(_ua.UserAccountID);

            ptiems.Sort((x, y) => (y.CreateDate.CompareTo(x.CreateDate)));

            ptiems.ShowTitle = false;
            ptiems.UseThumb = true;

            ViewBag.PhotoCount = PhotoItems.GetPhotoItemCountForUser(_ua.UserAccountID);

            return View(ptiems);
        }

        [HttpGet]
        public ActionResult UserNews(string userName)
        {
            _ua = new UserAccount(userName);

            ViewBag.UserName = _ua.UserName;

            var conts = new Contents();

            conts.GetContentForUser(_ua.UserAccountID);

            conts.IncludeStartAndEndTags = false;

            conts.Sort((x, y) => (y.ReleaseDate.CompareTo(x.ReleaseDate)));

            ViewBag.NewsCount = conts.Count;

            return View(conts);
        }

        public ActionResult UserPhoto(string userName, int photoItemID)
        {
            _ua = new UserAccount(userName);

            ViewBag.UserName = _ua.UserName;

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

            pitm2.GetPreviousPhotoForUser(_pitm.CreateDate, _ua.UserAccountID);
            if (pitm2.PhotoItemID > 0)
            {
                pitm2.IsUserPhoto = true;
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.PreviousPhoto = pitm2;
            }

            pitm2 = new PhotoItem();
            pitm2.GetNextPhotoForUser(_pitm.CreateDate, _ua.UserAccountID);

            if (pitm2.PhotoItemID > 0)
            {
                pitm2.IsUserPhoto = true;
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.NextPhoto = pitm2;
            }

            return View(_pitm);
        }
    }
}