﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;
using DasKlub.Web.Models;

namespace DasKlub.Web.Controllers
{
    public class MapController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MapHeight = Request.Browser.IsMobileDevice ? "300px" : "500px";

            return View();
        }

        public string HTMLEncodeSpecialChars(string text)
        {
            var sb = new StringBuilder();

            foreach (char c in text)
            {
                if (c > 127) // special chars
                    sb.Append(String.Format("&#{0};", (int) c));
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }


        public ActionResult Frame()
        {
            var usa = new CultureInfo("en-US");
            MembershipUser mu = Membership.GetUser();
            var userLatLong = new SiteStructs.LatLong {latitude = 0, longitude = 0};

            UserAccountDetail uad;

            if (mu != null)
            {
                var ua = new UserAccount(mu.UserName);
                uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                userLatLong.longitude = Convert.ToDouble(uad.Longitude);
                userLatLong.latitude = Convert.ToDouble(uad.Latitude);
            }

            var rnd = new Random();
            var mapPoints = new MapModel {MapPoints = new List<MapPoint>()};

            MapPoint mPoint;

            var uas = new UserAccounts();
            uas.GetMappableUsers();

            // because of the foreign cultures, numbers need to stay in the English version unless a javascript encoding could be added
            string currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

            foreach (UserAccount u1 in uas)
            {
                uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(u1.UserAccountID);

                if (uad.UserAccountDetailID == 0 || !uad.ShowOnMapLegal) continue;

                if (uad.Longitude == null || uad.Latitude == null || uad.Longitude == 0 || uad.Latitude == 0) continue;

                mPoint = new MapPoint();
                int offset = rnd.Next(10, 100);

                // BUG: language adding incorrect
                mPoint.Latitude = Convert.ToDouble(Convert.ToDecimal(uad.Latitude) + Convert.ToDecimal("0.00" + offset));
                mPoint.Longitude =
                    Convert.ToDouble(Convert.ToDecimal(uad.Longitude) + Convert.ToDecimal("0.00" + offset));

                u1.IsProfileLinkNewWindow = true;
                mPoint.Message = string.Format(@"<ul class=""user_list"">{0}</ul>",
                    u1.ToUnorderdListItem); // in javascript, escape 
                mPoint.Icon = uad.GenderIconLinkDark;
                mapPoints.MapPoints.Add(mPoint);
            }

            var vnus = new Venues();
            vnus.GetAll();

            foreach (Venue v1 in vnus.Where(v1 => v1.Latitude != 0 && v1.Longitude != 0))
            {
                mPoint = new MapPoint
                {
                    Icon = v1.VenueTypeIcon,
                    Latitude = Convert.ToDouble(v1.Latitude),
                    Longitude = Convert.ToDouble(v1.Longitude),
                    Message = v1.MapText
                };
                mapPoints.MapPoints.Add(mPoint);
            }

            string longI = userLatLong.longitude.ToString(usa);
            string latI = userLatLong.latitude.ToString(usa);
            var sb = new StringBuilder();

            sb.Append(@"
    var map; 
    var infowindow;
    function InitializeMap() { ");

            sb.AppendFormat(@"
        var latlng = new google.maps.LatLng({0}, {1});",
                latI, longI);

            if (mu != null && userLatLong.longitude != 0 && userLatLong.latitude != 0)
            {
                // zoom in on user
                sb.Append(@"
            var myOptions =
        {
            zoom: 8,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };");
            }
            else
            {
                // zoom out
                sb.Append(@"
            var myOptions =
        {
            zoom: 2,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };");
            }

            sb.Append(@"
        map = new google.maps.Map(document.getElementById(""map""), myOptions);
    }
             function markicons() {

        InitializeMap();

        var ltlng = [];
        var details = [];
        var iconType = [];");

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            foreach (MapPoint mp1 in mapPoints.MapPoints)
            {
                if (mp1.Latitude == 0 || mp1.Longitude == 0) continue;

                byte[] utfBytes = utf8.GetBytes(mp1.Message.Replace(@"'", @"\'"));
                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                string msg = iso.GetString(isoBytes);

                longI = mp1.Latitude.ToString(usa);
                latI = mp1.Longitude.ToString(usa);
                sb.Append(@" ltlng.push(new google.maps.LatLng(");
                sb.Append(longI);
                sb.Append(" , ");
                sb.Append(latI);
                sb.Append(@" )); ");
                sb.AppendFormat(@" details.push('{0}');
                    iconType.push('{1}');
                    ",
                    msg, mp1.Icon);
            }


            sb.Append(@"
        for (var i = 0; i <= ltlng.length; i++) {

            marker = new google.maps.Marker({
                map: map,
                position: ltlng[i],
                icon: iconType[i]
            });

            (function (i, marker) {

                google.maps.event.addListener(marker, 'click', function () {

                    if (!infowindow) {
                        infowindow = new google.maps.InfoWindow();
                    }

                infowindow.setContent(details[i]);
                    infowindow.open(map, marker);

                });

            })(i, marker);
        }

    }

    window.onload = markicons; ");

            ViewBag.MapScript = sb.ToString();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);

            return View();
        }
    }
}