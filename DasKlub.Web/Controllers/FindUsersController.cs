using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using DasKlub.Web.Models;

namespace DasKlub.Web.Controllers
{
    public class FindUsersController : Controller
    {
        #region variables

        private const int PageSize = 25;
        private MembershipUser _mu;
        private UserAccounts _uas;
        private int _userPageNumber = 1;

        #endregion

        private void InterestIdentityViewBags()
        {
            InterestedIns interins = UserAccountDetail.GetDistinctInterests();
            if (interins != null)
            {
                interins.Sort(
                    (p1, p2) => String.Compare(p1.LocalizedName, p2.LocalizedName, StringComparison.Ordinal));
                ViewBag.InterestedIns = interins.Select(x => new {x.InterestedInID, x.LocalizedName});
            }

            RelationshipStatuses relationshipStatuses = UserAccountDetail.GetDistinctRelationshipStatus();
            if (relationshipStatuses != null)
            {
                relationshipStatuses.Sort(
                    (p1, p2) => String.Compare(p1.LocalizedName, p2.LocalizedName, StringComparison.Ordinal));
                ViewBag.RelationshipStatuses =
                    relationshipStatuses.Select(x => new {x.RelationshipStatusID, x.LocalizedName});
            }

            YouAres youAres = UserAccountDetail.GetDistinctYouAres();
            if (youAres != null)
            {
                youAres.Sort((p1, p2) => String.Compare(p1.LocalizedName, p2.LocalizedName, StringComparison.Ordinal));
                ViewBag.YouAres = youAres.Select(x => new {x.YouAreID, x.LocalizedName});
            }

            List<SiteEnums.CountryCodeISO> countries = UserAccountDetail.GetDistinctUserCountries();
            if (countries != null)
            {
                Dictionary<string, string> countryOptions = countries.Where(
                    value => value != SiteEnums.CountryCodeISO.U0 && value != SiteEnums.CountryCodeISO.RD)
                    .ToDictionary(value => value.ToString(),
                        value =>
                            Utilities.ResourceValue(
                                Utilities.GetEnumDescription(
                                    value)));

                IOrderedEnumerable<string> items = from k in countryOptions.Keys
                    orderby countryOptions[k] ascending
                    select k;


                ViewBag.CountryOptions = items;
            }

            List<SiteEnums.SiteLanguages> languages = UserAccountDetail.GetDistinctUserLanguages();

            if (languages == null) return;

            Dictionary<string, string> languageOptions = languages.ToDictionary(value => value.ToString(),
                value =>
                    Utilities.ResourceValue(
                        Utilities.GetEnumDescription(value)));

            IOrderedEnumerable<string> languagesitems = from k in languageOptions.Keys
                orderby languageOptions[k] ascending
                select k;


            ViewBag.Languages = languagesitems;
        }

        //
        // GET: /FindUsers/
        [HttpGet]
        public ActionResult Index()
        {
            InterestIdentityViewBags();

            var model = new FindUsersModel();

            LoadFilteredUsers(false, model);

            ViewBag.FilteredUsers = _uas.ToUnorderdList;

            var rle = new Role(SiteEnums.RoleTypes.cyber_girl.ToString());

            IList<UserAccount> girlModels = UserAccountRole.GetUsersInRole(rle.RoleID);

            if (girlModels == null || girlModels.Count <= 0) return View(model);

            girlModels.Shuffle();

            UserAccount featuredModel = girlModels[0];

            var featuredPhoto = new UserAccountDetail();
            featuredPhoto.GetUserAccountDeailForUser(featuredModel.UserAccountID);

            int photoNumber = Utilities.RandomNumber(1, 4);

            string photoPath = featuredPhoto.FullProfilePicURL;

            if (photoNumber > 1)
            {
                var ups = new UserPhotos();

                ups.GetUserPhotos(featuredModel.UserAccountID);

                if (ups.Count > 0)
                {
                    foreach (UserPhoto up1 in ups.Where(up1 => (up1.RankOrder + 1) == photoNumber))
                    {
                        photoPath = up1.FullProfilePicURL;
                        break;
                    }
                }
            }

            string[] colorBorder = GeneralConfigs.RandomColors.Split(',');

            var rnd = new Random();
            string[] myRandomArray = colorBorder.OrderBy(x => rnd.Next()).ToArray();

            ViewBag.FeaturedModel = string.Format(@"
<a class=""m_over"" href=""{1}"">
<img src=""{0}"" class=""featured_user"" style="" border: 2px dashed {2}; "" /></a>", photoPath,
                featuredModel.UrlTo, myRandomArray[0]);

            return View(model);
        }


        [Authorize]
        public JsonResult OnlineNow()
        {
            _uas = new UserAccounts();
            _uas.GetOnlineUsers();

            _uas.Sort(
                (p1, p2) => p2.LastActivityDate.CompareTo(p1.LastActivityDate));

            return Json(new
            {
                Value = _uas.ToUnorderdList
            });
        }

        public JsonResult Items(int pageNumber)
        {
            var model = new FindUsersModel();

            _userPageNumber = pageNumber;

            string currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

            LoadFilteredUsers(true, model);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

            _uas.IncludeStartAndEndTags = false;

            return Json(new
            {
                ListItems = _uas.ToUnorderdList
            });
        }


        private void LoadFilteredUsers(bool isAjax, FindUsersModel model)
        {
            _mu = Membership.GetUser();

            model.AgeFrom = (!string.IsNullOrWhiteSpace(Request.QueryString["AgeFrom"]))
                ? Convert.ToInt32(Request.QueryString["AgeFrom"])
                : model.AgeFrom;
            model.AgeTo = (!string.IsNullOrWhiteSpace(Request.QueryString["AgeTo"]))
                ? Convert.ToInt32(Request.QueryString["AgeTo"])
                : model.AgeTo;


            UserAccountDetail uad;
            if (_mu != null)
            {
                if (!isAjax)
                {
                    uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));


                    if (!string.IsNullOrWhiteSpace(Request.QueryString.ToString()))
                    {
                        uad.FindUserFilter = Request.QueryString.ToString();
                        uad.Update();
                    }
                    else if (!string.IsNullOrWhiteSpace(uad.FindUserFilter))
                    {
                        Response.Redirect(string.Format("~/findusers?{0}", uad.FindUserFilter));
                    }
                }
            }


            model.InterestedInID =
                (Request.QueryString["InterestedInID"] != null && Request.QueryString["InterestedInID"] == string.Empty)
                    ? null
                    : (Request.QueryString["InterestedInID"] == null)
                        ? model.InterestedInID
                        : Convert.ToInt32(Request.QueryString["InterestedInID"]);


            model.RelationshipStatusID =
                (Request.QueryString["RelationshipStatusID"] != null &&
                 Request.QueryString["RelationshipStatusID"] == string.Empty)
                    ? null
                    : (Request.QueryString["RelationshipStatusID"] == null)
                        ? model.RelationshipStatusID
                        : Convert.ToInt32(Request.QueryString["RelationshipStatusID"]);


            model.YouAreID =
                (Request.QueryString["YouAreID"] != null && Request.QueryString["YouAreID"] == string.Empty)
                    ? null
                    : (Request.QueryString["YouAreID"] == null)
                        ? model.YouAreID
                        : Convert.ToInt32(Request.QueryString["YouAreID"]);


            model.Lang = (Request.QueryString["lang"] != null && Request.QueryString["lang"] == string.Empty)
                ? null
                : Request.QueryString["lang"] ?? model.Lang;


            model.PostalCode
                = (Request.QueryString["postalcode"] != null && Request.QueryString["postalcode"] == string.Empty)
                    ? null
                    : Request.QueryString["postalcode"] ?? model.PostalCode;


            model.Country
                = (Request.QueryString["country"] != null && Request.QueryString["country"] == string.Empty)
                    ? null
                    : Request.QueryString["country"] ?? model.Country;


            _uas = new UserAccounts();

            bool sortByDistance;

            _uas.GetListUsers(_userPageNumber, PageSize, model.AgeFrom, model.AgeTo, model.InterestedInID,
                model.RelationshipStatusID,
                model.YouAreID, model.Country, model.PostalCode, model.Lang, out sortByDistance);

            if (!isAjax)
            {
                ViewBag.SortByDistance = sortByDistance;
            }

            if (_mu == null || isAjax) return;
            if (string.IsNullOrWhiteSpace(Request.QueryString.ToString())) return;
            uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(Convert.ToInt32(_mu.ProviderUserKey));

            uad.FindUserFilter = Request.QueryString.ToString();
            uad.Update();
        }


        [HttpGet]
        [Authorize]
        public ActionResult Online()
        {
            _uas = new UserAccounts();
            _uas.GetOnlineUsers();

            _uas.Sort(
                (p1, p2) => p2.LastActivityDate.CompareTo(p1.LastActivityDate));

            return View(_uas);
        }
    }
}