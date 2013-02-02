//  Copyright 2012 
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
using System.Web.Mvc;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Enums;
using System.Web.Security;
using BootBaronLib.Values;
using System.Collections;
using DasKlub.Models;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BootBaronLib.Operational.Converters;
using System.Threading;
using System.Globalization;
using BootBaronLib.Operational;


namespace DasKlub.Controllers
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
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in text)
            {
                if (c > 127) // special chars
                    sb.Append(String.Format("&#{0};", (int)c));
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

 

        public ActionResult Frame()
        {
            CultureInfo usa = new CultureInfo("en-US");

            MembershipUser mu = Membership.GetUser();

                    SiteStructs.LatLong userLatLong = new SiteStructs.LatLong();
            userLatLong.latitude = 0;
            userLatLong.longitude = 0;
            UserAccountDetail uad = null;

            if (mu != null)
            {

                UserAccount ua = new UserAccount(mu.UserName);
                uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(ua.UserAccountID);
                //if (!string.IsNullOrEmpty(uad.PostalCode))
                //{
                //    userLatLong = GeoData.GetLatLongForCountryPostal(uad.Country, uad.PostalCode);
                //}

                userLatLong.longitude = Convert.ToDouble(uad.Longitude);
                userLatLong.latitude = Convert.ToDouble(uad.Latitude);
            }

            Random rnd = new Random();



            // map

            MapModel mapPoints = new MapModel();

            mapPoints.MapPoints = new List<MapPoint>();
            MapPoint mPoint = null;

            // users
            UserAccounts uas = new UserAccounts();
            uas.GetMappableUsers();
            int offset = 0;

            // because of the foreign cultures, numbers need to stay in the English version unless a javascript encoding could be added
            string currentLang = Utilities.GetCurrentLanguageCode();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(SiteEnums.SiteLanguages.EN.ToString());

            foreach (UserAccount u1 in uas)
            {
                uad = new UserAccountDetail();
                uad.GetUserAccountDeailForUser(u1.UserAccountID);

                if (uad.UserAccountDetailID == 0 || !uad.ShowOnMapLegal) continue;

                if (uad.Longitude == null || uad.Latitude == null || uad.Longitude == 0 || uad.Latitude == 0) continue;

                mPoint = new MapPoint();
                offset = rnd.Next(10, 100);

                // BUG: language adding incorrect
                mPoint.Latitude = Convert.ToDouble(Convert.ToDecimal(uad.Latitude) + Convert.ToDecimal("0.00" + offset));
                mPoint.Longitude = Convert.ToDouble(Convert.ToDecimal(uad.Longitude) + Convert.ToDecimal("0.00" + offset));

                u1.IsProfileLinkNewWindow = true;
                mPoint.Message = string.Format(@"<ul class=""user_list"">{0}</ul>",
               u1.ToUnorderdListItem);// in javascript, escape 
                mPoint.Icon = uad.GenderIconLinkDark;
                mapPoints.MapPoints.Add(mPoint);
            }


            // venues

            Venues vnus = new Venues();
            vnus.GetAll();

            foreach (Venue v1 in vnus)
            {
                if (v1.Latitude == 0 || v1.Longitude == 0) continue;

                mPoint = new MapPoint();
                mPoint.Icon = v1.VenueTypeIcon;
                mPoint.Latitude = Convert.ToDouble(v1.Latitude);
                mPoint.Longitude = Convert.ToDouble(v1.Longitude);
                mPoint.Message = v1.MapText ;
                mapPoints.MapPoints.Add(mPoint);
            }

            string longI = userLatLong.longitude.ToString(usa);

            string latI = userLatLong.latitude.ToString(usa);
 

            StringBuilder sb = new StringBuilder();

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

            byte[] myarray2 = Encoding.Unicode.GetBytes(string.Empty);

            
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;



            foreach (DasKlub.Models.MapPoint mp1 in mapPoints.MapPoints)
            {

                if (mp1.Latitude == 0 || mp1.Longitude == 0) continue;

                byte[] utfBytes = utf8.GetBytes(mp1.Message.Replace(@"'", @"\'"));
                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                string msg = iso.GetString(isoBytes);

                  longI = mp1.Latitude.ToString(usa);

                  latI = mp1.Longitude.ToString(usa);

//                sb.AppendFormat(@" 
                    //ltlng.push(new google.maps.LatLng({0}, {1}));
                sb.Append(@" ltlng.push(new google.maps.LatLng(");
                sb.Append(longI);
                sb.Append(" , ");
                sb.Append(latI);
                sb.Append(@" )); ");
                
              //  {0}, {1}));
                    
sb.AppendFormat(@" details.push('{0}');
                    iconType.push('{1}');
                    " ,
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

            ViewBag.MapScript = sb.ToString( );
 

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);


            return View();
        }

    }
}
