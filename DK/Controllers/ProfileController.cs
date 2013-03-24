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
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.AppSpec.DasKlub.BOL.UserContent;
using BootBaronLib.Enums;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Values;
using DasKlub.Models;

namespace DasKlub.Controllers
{
    public class ProfileController : Controller
    {
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

            if (!string.IsNullOrWhiteSpace(ups[0].PicURL))
            {
                ViewBag.SecondUserPhotoFull = ups[0].FullProfilePicURL;
                ViewBag.SecondUserPhotoThumb = ups[0].FullProfilePicThumbURL;
            }

            if (!string.IsNullOrWhiteSpace(ups[1].PicURL))
            {
                ViewBag.ThirdUserPhotoFull = ups[1].FullProfilePicURL;
                ViewBag.ThirdUserPhotoThumb = ups[1].FullProfilePicThumbURL;
            }
        }



        
        public static readonly int pageSize = 25;
        PhotoItem pitm = null;
        int maxcountusers = 5000;
        UserAccount ua = null;

        [HttpGet]
        public ActionResult ProfileDetail(string userName)
        {
            ViewBag.VideoHeight = (Request.Browser.IsMobileDevice) ? 100 : 277;
            ViewBag.VideoWidth = (Request.Browser.IsMobileDevice) ? 225 : 400;

            ua = new UserAccount(userName);
            
            UserAccountDetail uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(ua.UserAccountID);

            uad.BandsSeen = ContentLinker.InsertBandLinks(uad.BandsSeen, false);
            uad.BandsToSee = ContentLinker.InsertBandLinks(uad.BandsToSee, false);

            MembershipUser mu = Membership.GetUser();

            ProfileModel model = new ProfileModel();

            if (ua.UserAccountID > 0)
            {
                model.UserAccountID = ua.UserAccountID;
                model.PhotoCount = PhotoItems.GetPhotoItemCountForUser(ua.UserAccountID);
                model.CreateDate = ua.CreateDate;
            }

            if (mu != null)
            {
                ViewBag.IsBlocked = BlockedUser.IsBlockedUser(ua.UserAccountID, Convert.ToInt32(mu.ProviderUserKey));
                ViewBag.IsBlocking = BlockedUser.IsBlockedUser(Convert.ToInt32(mu.ProviderUserKey), ua.UserAccountID);

                if (ua.UserAccountID == Convert.ToInt32(mu.ProviderUserKey))
                {
                    model.IsViewingSelf = true;
                }
                else
                {
                    UserConnection ucon = new UserConnection();

                    ucon.GetUserToUserConnection(Convert.ToInt32(mu.ProviderUserKey), ua.UserAccountID);

                    model.UserConnectionID = ucon.UserConnectionID;


                    if (BlockedUser.IsBlockedUser(Convert.ToInt32(mu.ProviderUserKey), ua.UserAccountID))
                    {
                        return RedirectToAction("index", "home");
                    }

                }
            }
            else
            {

                if (uad.MembersOnlyProfile)
                {
                    return RedirectToAction("Account", "LogOn");
                }

            }

            //
            model.UserName = ua.UserName;
            model.CreateDate = ua.CreateDate;
            model.LastActivityDate = ua.LastActivityDate;
            //
            model.DisplayAge = uad.DisplayAge;
            model.Age = uad.YearsOld;
            model.BandsSeen = uad.BandsSeen;
            model.BandsToSee = uad.BandsToSee;
            model.HardwareAndSoftwareSkills = uad.HardwareSoftware;
            model.MessageToTheWorld = uad.AboutDescription;

            model.YouAreFull = uad.Sex;
            model.InterestedInFull = uad.InterestedFull;
            model.RelationshipStatusFull = uad.RelationshipStatusFull ;
            model.RelationshipStatus = uad.RelationshipStatus;
            model.InterestedIn = uad.InterestedIn;
            model.YouAre = uad.YouAre;
            
            model.Website = uad.ExternalURL;
            model.CountryCode = uad.Country;
            model.CountryName = uad.CountryName;
            model.IsBirthday = uad.IsBirthdayToday;
            model.ProfilePhotoMain = uad.FullProfilePicURL;
            model.ProfilePhotoMainThumb = uad.FullProfilePicThumbURL;
            model.DefaultLanguage = uad.DefaultLanguage;

            model.EnableProfileLogging = uad.EnableProfileLogging;
            model.Handed = uad.HandedFull;
            model.RoleIcon = uad.SiteBages;

 
            //
            StatusUpdate su = new StatusUpdate();
            su.GetMostRecentUserStatus(ua.UserAccountID);

            if (su.StatusUpdateID > 0)
            {
                model.LastStatusUpdate = su.CreateDate;
                model.MostRecentStatusUpdate = su.Message;
            }

            model.ProfileVisitorCount = ProfileLog.GetUniqueProfileVisitorCount(ua.UserAccountID);

            PhotoItems ptiems = new PhotoItems();
            ptiems.GetUserPhotos(ua.UserAccountID);

            if (ptiems.Count > 0)
            {
                ptiems.Sort((PhotoItem x, PhotoItem y) => (y.CreateDate.CompareTo(x.CreateDate)));

                PhotoItems ptiemsDisplay = new PhotoItems();

                int maxPhotos = 8;

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

            Contents conts = new Contents();

            conts.GetContentForUser(ua.UserAccountID);

            model.NewsCount = conts.Count;

            if (conts.Count > 0)
            {
                conts.Sort((Content x, Content y) => (y.ReleaseDate.CompareTo(x.ReleaseDate)));

                Contents displayContents = new Contents();
                int maxCont = 1;
                int currentCount = 0;
                foreach (Content ccn1 in conts)
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

            model.MetaDescription = ua.UserName + " " + BootBaronLib.Resources.Messages.Profile + " " + FromDate.DateToYYYY_MM_DD(ua.LastActivityDate);

            // playlist
            BootBaronLib.AppSpec.DasKlub.BOL.Playlist plyst = new Playlist();

            plyst.GetUserPlaylist(ua.UserAccountID);

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

                    byte[] myarray2 = Encoding.Unicode.GetBytes(string.Empty);

                    // because of the foreign cultures, numbers need to stay in the English version unless a javascript encoding could be added
                    string currentLang = Utilities.GetCurrentLanguageCode();

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    Encoding utf8 = Encoding.UTF8;


                    model.DisplayOnMap = uad.ShowOnMapLegal;

                    Random rnd = new Random();
                    int offset = rnd.Next(10, 100);

                    SiteStructs.LatLong latlong = GeoData.GetLatLongForCountryPostal(uad.Country, uad.PostalCode);

                    if (latlong.latitude != 0 && latlong.longitude != 0)
                    {
                        model.Latitude = Convert.ToDecimal(latlong.latitude + Convert.ToDouble("0.00" + offset)).ToString();
                        model.Longitude = Convert.ToDecimal(latlong.longitude + Convert.ToDouble("0.00" + offset)).ToString();
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
            ViewBag.UserAccount = ua;

            UserConnections ucons = new UserConnections();
            ucons.GetUserConnections(ua.UserAccountID);
            ucons.Shuffle();

            UserAccounts irlContacts = new UserAccounts();
            UserAccounts CyberAssociates = new UserAccounts();
            UserAccount userCon = null;

            foreach (UserConnection uc1 in ucons)
            {
                if (!uc1.IsConfirmed) continue;

                switch (uc1.StatusType)
                {
                    case 'C':
                        if (CyberAssociates.Count >= maxcountusers) continue;

                        if (uc1.ToUserAccountID != ua.UserAccountID)
                        {
                            userCon = new UserAccount(uc1.ToUserAccountID);
                        }
                        else
                        {
                            userCon = new UserAccount(uc1.FromUserAccountID);
                        }
                        CyberAssociates.Add(userCon);
                        break;
                    case 'R':
                        if (irlContacts.Count >= maxcountusers) continue;

                        if (uc1.ToUserAccountID != ua.UserAccountID)
                        {
                            userCon = new UserAccount(uc1.ToUserAccountID);
                        }
                        else
                        {
                            userCon = new UserAccount(uc1.FromUserAccountID);
                        }
                        irlContacts.Add(userCon);
                        break;
                    default:
                        break;
                }
            }

            if (irlContacts.Count > 0)
            {
                model.IRLFriendCount = irlContacts.Count;
            }

            if (CyberAssociates.Count > 0)
            {
                // ViewBag.CyberAssociatesCount = Convert.ToString( CyberAssociates.Count );
                model.CyberFriendCount = CyberAssociates.Count;
            }

            mu = Membership.GetUser();
            UserAccountDetail uadLooker = null;

            if (mu != null)
            {
                uadLooker = new UserAccountDetail();
                uadLooker.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));
            }

            if (mu != null && ua.UserAccountID > 0 &&
                uadLooker.EnableProfileLogging && uad.EnableProfileLogging)
            {
                ProfileLog pl = new ProfileLog();

                pl.LookedAtUserAccountID = ua.UserAccountID;
                pl.LookingUserAccountID = Convert.ToInt32(mu.ProviderUserKey);

                if (pl.LookingUserAccountID != pl.LookedAtUserAccountID) pl.Create();


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
                            if (uas.Count >= maxcountusers) break;

                            uas.Add(viewwer);
                        }
                    }

                   // model.ViewingUsers = uas.ToUnorderdList;
                }
            }

            UserAccountVideos uavs = null;

            if (ua.UserAccountID > 0)
            {
                uavs = new UserAccountVideos();

                uavs.GetRecentUserAccountVideos(ua.UserAccountID, 'F');

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

                    ViewBag.UserFavorites = sngrcds2.VideosList();//.ListOfVideos();
                }
            }

            
            // this is either a youtube user or this is a band
            Artist art = new Artist(  );
            art.GetArtistByAltname(userName);

            if (art.ArtistID > 0)
            {
                // try this way for dashers
                model.UserName = art.Name;
            }

            Videos vids = new Videos();
            SongRecords sngrs = new SongRecords();

            if (art.ArtistID == 0)
            {
                vids.GetAllVideosByUser(userName);

                uavs = new UserAccountVideos();
                uavs.GetRecentUserAccountVideos(ua.UserAccountID, 'U');

                Video f2 = null;

                foreach (UserAccountVideo uav1 in uavs)
                {
                    f2 = new Video(uav1.VideoID);

                    if (!vids.Contains(f2)) vids.Add(f2);
                }

                vids.Sort((Video x, Video y) => (y.PublishDate.CompareTo(x.PublishDate)));

                model.UserName = userName;
                
            }
            else
            {
                // photo
                ArtistProperty aprop = new ArtistProperty();
                aprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.PH.ToString());

                if (!string.IsNullOrEmpty(aprop.PropertyContent))
                {
                    ViewBag.ArtistPhoto = System.Web.VirtualPathUtility.ToAbsolute(aprop.PropertyContent);
                    ViewBag.ThumbIcon = System.Web.VirtualPathUtility.ToAbsolute(aprop.PropertyContent);
                }

                // meta descriptione
                aprop = new ArtistProperty();
                aprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.MD.ToString());
                if (!string.IsNullOrEmpty(aprop.PropertyContent)) ViewBag.MetaDescription = aprop.PropertyContent;

                // description
                aprop = new ArtistProperty();
                aprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.LD.ToString());
                if (!string.IsNullOrEmpty(aprop.PropertyContent)) ViewBag.ArtistDescription = ContentLinker.InsertBandLinks(aprop.PropertyContent, false);

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

                Songs sngss = new Songs();

                sngss.GetSongsForArtist(art.ArtistID);

                SongRecord snrcd = new SongRecord();

                foreach (Song sn1 in sngss)
                {
                    vids.GetVideosForSong(sn1.SongID);
                }
            }


            vids.Sort(delegate(Video p1, Video p2)
            {
                return p2.PublishDate.CompareTo(p1.PublishDate);
            });


            foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video v1 in vids)
            {
                sngrs.Add(new SongRecord(v1));
            }

            if (mu != null && ua.UserAccountID != Convert.ToInt32(mu.ProviderUserKey))
            {
                UserConnection uc1 = new UserConnection();

                uc1.GetUserToUserConnection(ua.UserAccountID, Convert.ToInt32(mu.ProviderUserKey));

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


            if (sngrs == null || sngrs.Count == 0 && art.ArtistID == 0 && ua.UserAccountID == 0)
            {
                // no longer exists
                Response.RedirectPermanent("/");
                return new EmptyResult();
            }

            //sngrs.Sort((SongRecord x, SongRecord y) => (y.cre.CompareTo(x.CreateDate)));

            SongRecords sngDisplay = new SongRecords();

            Video vidToShow = null;

            foreach (SongRecord sr1 in sngrs)
            {
                vidToShow = new Video(sr1.VideoID);

                if (vidToShow.IsEnabled)
                {
                    sngDisplay.Add(sr1);
                }
            }

            model.SongRecords = sngDisplay.VideosList();

            return View(model);
        }


        [Authorize]
        public ActionResult DeleteWallItem(int wallItemID)
        {
            WallMessage wallItem = new WallMessage();

            wallItem.WallMessageID = wallItemID;
            wallItem.Delete();

            return RedirectToAction("Visitors", "Account"); 
        }

        public JsonResult WallMessages(int pageNumber)
        {
            string referrring = Request.UrlReferrer.ToString();
            string[] partsOfreferring = referrring.Split('/');
            UserAccount ua = new UserAccount(partsOfreferring[partsOfreferring.Length - 1]);

            WallMessages wallItems = new WallMessages();

            wallItems.GetWallMessagessPageWise(pageNumber, 5, ua.UserAccountID);

            MembershipUser mu = Membership.GetUser();

            if (mu != null && Convert.ToInt32(mu.ProviderUserKey) == ua.UserAccountID)
            {
                wallItems.IsUsersWall = true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.WallMessage cnt in wallItems)
            {
                cnt.IsUsersWall = wallItems.IsUsersWall;
                sb.Append(cnt.ToUnorderdListItem);
            }

            return Json(new
            {
                ListItems = sb.ToString()
            });
        }

        [Authorize]
        public ActionResult WriteWallMessage(string message, int toUserAccountID)
        {
            ua = new UserAccount(toUserAccountID);

            if (ua.UserAccountID != 0 && !string.IsNullOrWhiteSpace(message))
            {
                MembershipUser mu = Membership.GetUser();

                WallMessage comment = new WallMessage();

                comment.Message = Server.HtmlEncode(message);
                comment.ToUserAccountID = toUserAccountID;
                comment.FromUserAccountID = Convert.ToInt32(mu.ProviderUserKey);
                comment.CreatedByUserID = Convert.ToInt32(mu.ProviderUserKey);
                comment.Create();
            }

            return RedirectToAction("ProfileDetail", new { @userName = ua.UserName });
        }
       

        [HttpGet]
        public ActionResult UserPhotos(string userName)
        {
            ua = new UserAccount(userName);

            ViewBag.UserName = ua.UserName;

            PhotoItems ptiems = new PhotoItems();
            ptiems.GetUserPhotos(ua.UserAccountID);

            ptiems.Sort((PhotoItem x, PhotoItem y) => (y.CreateDate.CompareTo(x.CreateDate)));

            ptiems.ShowTitle = false;
            ptiems.UseThumb = true;

            ViewBag.PhotoCount = PhotoItems.GetPhotoItemCountForUser(ua.UserAccountID);

            return View(ptiems);
        }

        [HttpGet]
        public ActionResult UserNews(string userName)
        {
            ua = new UserAccount(userName);

            ViewBag.UserName = ua.UserName;

            Contents conts = new Contents();

            conts.GetContentForUser(ua.UserAccountID);

            conts.IncludeStartAndEndTags = false;

            conts.Sort((Content x, Content y) => (y.ReleaseDate.CompareTo(x.ReleaseDate)));

            ViewBag.NewsCount = conts.Count;

            return View(conts);
        }

        public ActionResult UserPhoto(string userName, int photoItemID)
        {
            ua = new UserAccount(userName);

            ViewBag.UserName = ua.UserName;

            pitm = new PhotoItem(photoItemID);

            StatusUpdates sus = new StatusUpdates();

            StatusUpdate su = new StatusUpdate();

            su.GetStatusUpdateByPhotoID(photoItemID);

            su.PhotoDisplay = true;
            sus.Add(su);

            if (string.IsNullOrWhiteSpace(pitm.Title))
            {
                pitm.Title = String.Format("{0:u}", pitm.CreateDate);
            }

            sus.IncludeStartAndEndTags = false;
            ViewBag.StatusUpdateList = @"<ul id=""status_update_list_items"">" + sus.ToUnorderdList + @"</ul>";

            PhotoItem pitm2 = new PhotoItem();

            pitm2.GetPreviousPhotoForUser(pitm.CreateDate, ua.UserAccountID);
            if (pitm2.PhotoItemID > 0)
            {
                pitm2.IsUserPhoto = true;
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.PreviousPhoto = pitm2;
            }

            pitm2 = new PhotoItem();
            pitm2.GetNextPhotoForUser(pitm.CreateDate, ua.UserAccountID);

            if (pitm2.PhotoItemID > 0)
            {
                pitm2.IsUserPhoto = true;
                pitm2.ShowTitle = false;
                pitm2.UseThumb = true;
                ViewBag.NextPhoto = pitm2;
            }

            return View(pitm);
        }
 
 

 
    }
 
}
