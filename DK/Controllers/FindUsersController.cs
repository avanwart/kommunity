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
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Enums;
using BootBaronLib.Operational;
using DasKlub.Models;

namespace DasKlub.Controllers
{
    public class FindUsersController : Controller
    {
        #region variables

        MembershipUser mu = null;
      
        UserAccounts uas = null;
 
        
        
        
        
        int pageSize = 25;
        int userPageNumber = 1;

        #endregion

 

        private void InterestIdentityViewBags()
        {

            ///
            var interins = UserAccountDetail.GetDistinctInterests();
            if (interins != null)
            {
                interins.Sort(delegate(InterestedIn p1, InterestedIn p2)
                {
                    return p1.LocalizedName.CompareTo(p2.LocalizedName);
                });
                ViewBag.InterestedIns = interins.Select(x => new { InterestedInID = x.InterestedInID, LocalizedName = x.LocalizedName });
            }

            ///
            var relationshipStatuses = UserAccountDetail.GetDistinctRelationshipStatus();
            if (relationshipStatuses != null)
            {
                relationshipStatuses.Sort(delegate(RelationshipStatus p1, RelationshipStatus p2)
                {
                    return p1.LocalizedName.CompareTo(p2.LocalizedName);
                });
                ViewBag.RelationshipStatuses = relationshipStatuses.Select(x => new { RelationshipStatusID = x.RelationshipStatusID, LocalizedName = x.LocalizedName });
            }

            ///
            var youAres = UserAccountDetail.GetDistinctYouAres();
            if (youAres != null)
            {
                youAres.Sort(delegate(YouAre p1, YouAre p2)
                {
                    return p1.LocalizedName.CompareTo(p2.LocalizedName);
                });
                ViewBag.YouAres = youAres.Select(x => new { YouAreID = x.YouAreID, LocalizedName = x.LocalizedName });
            }

            ///
            var countries = UserAccountDetail.GetDistinctUserCountries();
            if (countries != null)
            {
                System.Collections.Generic.Dictionary<string, string> countryOptions = new Dictionary<string, string>();
                foreach (SiteEnums.CountryCodeISO value in countries)
                {
                    if (value != SiteEnums.CountryCodeISO.U0 &&
                         value != SiteEnums.CountryCodeISO.RD)
                    {
                        countryOptions.Add(value.ToString(), Utilities.ResourceValue(
                            Utilities.GetEnumDescription(value)));
                    }
                }

                var items = from k in countryOptions.Keys
                            orderby countryOptions[k] ascending
                            select k;


                ViewBag.CountryOptions = items;

            }



            ///
            var languages = UserAccountDetail.GetDistinctUserLanguages();
            if (languages != null)
            {
                System.Collections.Generic.Dictionary<string, string> languageOptions = new Dictionary<string, string>();

                foreach (SiteEnums.SiteLanguages value in languages)
                {
                    languageOptions.Add(value.ToString(), Utilities.ResourceValue(
                            Utilities.GetEnumDescription(value)));

                }

                var languagesitems = from k in languageOptions.Keys
                                     orderby languageOptions[k] ascending
                                     select k;


                ViewBag.Languages = languagesitems;
            }
        }

        private int ReverseYouAreByInterestName(int? interesetedinID)
        {
            if (interesetedinID == null) return 0;

            InterestedIn iiinets = new InterestedIn();

            iiinets.Get(Convert.ToInt32(interesetedinID));

            YouAres iins = new YouAres();

            iins.GetAll();

            foreach (YouAre inn1 in iins)
            {
                if (inn1.Name == iiinets.Name)
                {
                    return inn1.YouAreID; ;
                }
            }

            return 0;
        }

        private int ReverseInterestYouAreByName(int? youAreID )
        {
            if (youAreID == null) return 0;

            YouAre youAre = new YouAre();

            youAre.Get(Convert.ToInt32(youAreID));

            InterestedIns iins = new InterestedIns();

            iins.GetAll();

            foreach (InterestedIn inn1 in iins)
            {
                if (inn1.Name == youAre.Name)
                {
                    return inn1.InterestedInID;
                }
            }

            return 0;
        }

        //
        // GET: /FindUsers/
        [HttpGet]
        public ActionResult Index()
        {
            InterestIdentityViewBags();

            FindUsersModel model = new FindUsersModel();

            LoadFilteredUsers(false, model);

            ViewBag.FilteredUsers = uas.ToUnorderdList;


            // random
            Role rle = new Role(SiteEnums.RoleTypes.cyber_girl.ToString());

            UserAccounts girlModels = UserAccountRole.GetUsersInRole(rle.RoleID);

            if (girlModels!= null && girlModels.Count > 0)
            {
                BootBaronLib.Operational.StaticHelper.Shuffle(girlModels);

                UserAccount featuredModel = girlModels[0];

                UserAccountDetail featuredPhoto = new UserAccountDetail();
                featuredPhoto.GetUserAccountDeailForUser(featuredModel.UserAccountID);

                int photoNumber = Utilities.RandomNumber(1, 4);

                string photoPath = featuredPhoto.FullProfilePicURL;

                if (photoNumber > 1)
                {
                    UserPhotos ups = new UserPhotos();

                    ups.GetUserPhotos(featuredModel.UserAccountID);

                    if (ups.Count > 0)
                    {
                        foreach (UserPhoto up1 in ups)
                        {
                            if ((up1.RankOrder + 1) == photoNumber)
                            {
                                photoPath = up1.FullProfilePicURL;
                                break;
                            }
                        }
                    }
                }

                // random border color with random user pic from their 3 photos
                string[] colorBorder = BootBaronLib.Configs.GeneralConfigs.RandomColors.Split(',');

                Random rnd=new Random();
                string[] myRandomArray = colorBorder.OrderBy(x => rnd.Next()).ToArray();

                ViewBag.FeaturedModel = string.Format(@"
<a class=""m_over"" href=""{1}"">
<img src=""{0}"" class=""featured_user"" style="" border: 2px dashed {2}; "" /></a>", photoPath,
        featuredModel.UrlTo.ToString(), myRandomArray[0]);
            }


            return View(model);
        }



        [Authorize]
        public JsonResult OnlineNow()
        {
            uas = new UserAccounts();
            uas.GetOnlineUsers();

            uas.Sort(delegate(UserAccount p1, UserAccount p2)
            {
                return p2.LastActivityDate.CompareTo(p1.LastActivityDate);
            });

            return Json(new
            {
                Value = uas.ToUnorderdList
            });
        }

        public JsonResult Items(int pageNumber)
        {
            FindUsersModel model = new FindUsersModel();

            userPageNumber = pageNumber;

            string currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

            LoadFilteredUsers(true, model);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

            uas.IncludeStartAndEndTags = false;

            return Json(new
            {
                ListItems = uas.ToUnorderdList
            });
        }


        private void LoadFilteredUsers(bool isAjax, FindUsersModel model)
        {
            mu = Membership.GetUser();

            model.AgeFrom = (!string.IsNullOrWhiteSpace(Request.QueryString["AgeFrom"])) ? Convert.ToInt32(Request.QueryString["AgeFrom"]) : model.AgeFrom;
            model.AgeTo = (!string.IsNullOrWhiteSpace(Request.QueryString["AgeTo"])) ? Convert.ToInt32(Request.QueryString["AgeTo"]) : model.AgeTo;



            if (mu != null)
            {
                // there aren't enough results for this filter

                //ua = new UserAccount(Convert.ToInt32(mu.ProviderUserKey));
                //uad = new UserAccountDetail();
                //uad.GetUserAccountDeailForUser(ua.UserAccountID);

                //model.InterestedInID = ReverseInterestYouAreByName(uad.YouAreID);
                //model.RelationshipStatusID = uad.RelationshipStatusID;
                //model.YouAreID = ReverseYouAreByInterestName(uad.InterestedInID);
                //model.Lang = uad.DefaultLanguage;
                //model.PostalCode = uad.PostalCode;
                //model.Country = uad.Country;

                //if (uad.YearsOld > model.AgeTo)
                //{
                //    // they are old(er)
                //    model.AgeFrom = 30;
                //    model.AgeTo = 69;
                //}

                if (!isAjax)
                {
                    UserAccountDetail uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));


                    if (!string.IsNullOrWhiteSpace(Request.QueryString.ToString()))
                    {
                        uad.FindUserFilter = Request.QueryString.ToString();
                        uad.Update();
                    }
                    else if ( !string.IsNullOrWhiteSpace(uad.FindUserFilter))
                    {
                        Response.Redirect(string.Format("~/findusers?{0}", uad.FindUserFilter));
                    }



                }
            }


            model.InterestedInID =
                    (Request.QueryString["InterestedInID"] != null && Request.QueryString["InterestedInID"] == string.Empty) ? null :
                    (Request.QueryString["InterestedInID"] == null) ? model.InterestedInID : Convert.ToInt32(Request.QueryString["InterestedInID"]);



            model.RelationshipStatusID =
        (Request.QueryString["RelationshipStatusID"] != null && Request.QueryString["RelationshipStatusID"] == string.Empty) ? null :
        (Request.QueryString["RelationshipStatusID"] == null) ? model.RelationshipStatusID : Convert.ToInt32(Request.QueryString["RelationshipStatusID"]);




            model.YouAreID =
        (Request.QueryString["YouAreID"] != null && Request.QueryString["YouAreID"] == string.Empty) ? null :
        (Request.QueryString["YouAreID"] == null) ? model.YouAreID : Convert.ToInt32(Request.QueryString["YouAreID"]);



            model.Lang = (Request.QueryString["lang"] != null && Request.QueryString["lang"] == string.Empty) ? null :
        (Request.QueryString["lang"] == null) ? model.Lang : Request.QueryString["lang"];


            model.PostalCode
                = (Request.QueryString["postalcode"] != null && Request.QueryString["postalcode"] == string.Empty) ? null :
        (Request.QueryString["postalcode"] == null) ? model.PostalCode : Request.QueryString["postalcode"];



            model.Country
                = (Request.QueryString["country"] != null && Request.QueryString["country"] == string.Empty) ? null :
        (Request.QueryString["country"] == null) ? model.Country : Request.QueryString["country"];


            uas = new UserAccounts();

            bool sortByDistance = false;

            uas.GetListUsers(userPageNumber, pageSize, model.AgeFrom, model.AgeTo, model.InterestedInID, model.RelationshipStatusID,
                model.YouAreID, model.Country, model.PostalCode, model.Lang, out sortByDistance);

            if (!isAjax)
            {
                ViewBag.SortByDistance = sortByDistance;
            }

            if (mu != null && !isAjax)
            {

                if (!string.IsNullOrWhiteSpace(Request.QueryString.ToString()))
                {
                    UserAccountDetail uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(Convert.ToInt32(mu.ProviderUserKey));

                    uad.FindUserFilter = Request.QueryString.ToString();
                    uad.Update();
                }

            }
        }


        [HttpGet]
        [Authorize]
        public ActionResult Online()
        {
            uas = new UserAccounts();
            uas.GetOnlineUsers();

            uas.Sort(delegate(UserAccount p1, UserAccount p2)
            {
                return p2.LastActivityDate.CompareTo(p1.LastActivityDate);
            });
            
            return View(uas);
        }

    }
}
