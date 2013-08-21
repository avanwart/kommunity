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

using System.Configuration;

namespace DasKlub.Lib.Configs
{
    public static class GeneralConfigs
    {
        static GeneralConfigs()
        {
            _enableErrorLogEmail = bool.Parse(ConfigurationManager.AppSettings["EnableErrorLogEmail"]);
            _useNetworkForMail = bool.Parse(ConfigurationManager.AppSettings["UseNetworkForMail"]);
            _postInterval = int.Parse(ConfigurationManager.AppSettings["PostInterval"]);
            _siteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            _payPalPDTKey = ConfigurationManager.AppSettings["PayPalPDTKey"];
            _payPalURL = ConfigurationManager.AppSettings["PayPalURL"];
            _emailSettingsURL = ConfigurationManager.AppSettings["EmailSettingsURL"];
            _sendToErrorEmail = ConfigurationManager.AppSettings["SendToErrorEmail"];
            _googleAPIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];
            _defaultLanguage = ConfigurationManager.AppSettings["DefaultLanguage"];
            _siteName = ConfigurationManager.AppSettings["SiteName"];
            _facebookLink = ConfigurationManager.AppSettings["FacebookLink"];
            _twitterLink = ConfigurationManager.AppSettings["TwitterLink"];
            _youtubeLink = ConfigurationManager.AppSettings["YoutubeLink"];
            _defaultVideo = ConfigurationManager.AppSettings["DefaultVideo"];
            _enableVideoCheck = bool.Parse(ConfigurationManager.AppSettings["EnableVideoCheck"]);
            _minimumAge = int.Parse(ConfigurationManager.AppSettings["MinimumAge"]);
            _userChatRoomSessionTimeout = int.Parse(ConfigurationManager.AppSettings["UserChatRoomSessionTimeout"]);
            _randomColors = ConfigurationManager.AppSettings["RandomColors"];
            _adminUserName = ConfigurationManager.AppSettings["AdminUserName"];
            _isGiveAway = bool.Parse(ConfigurationManager.AppSettings["IsGiveAway"]);
            _enableSameIP = bool.Parse(ConfigurationManager.AppSettings["EnableSameIP"]);
            _youTubeDevKey = ConfigurationManager.AppSettings["YouTubeDevKey"];
            _youTubeDevUser = ConfigurationManager.AppSettings["YouTubeDevUser"];
            _youTubeDevPass = ConfigurationManager.AppSettings["YouTubeDevPass"];
        }

        #region readonly variables 

        private static readonly bool _isGiveAway;
        private static readonly int _userChatRoomSessionTimeout;
        private static readonly bool _enableSameIP;
        private static readonly string _adminUserName = string.Empty;
        private static readonly int _minimumAge;
        private static readonly string _defaultVideo = string.Empty;
        private static readonly string _siteName = string.Empty;
        private static readonly string _googleAPIKey = string.Empty;
        private static readonly string _nonWebFilePath = string.Empty;
        private static readonly bool _enableErrorLogEmail;
        private static readonly string _errorLogFilename = string.Empty;
        private static readonly bool _useNetworkForMail;
        private static readonly int _postInterval;
        private static readonly string _siteDomain = string.Empty;
        private static readonly string _payPalPDTKey = string.Empty;
        private static readonly string _payPalURL = string.Empty;
        private static readonly string _emailSettingsURL = string.Empty;
        private static readonly string _sendToErrorEmail = string.Empty;
        private static readonly string _defaultLanguage = string.Empty;
        private static readonly bool _enableVideoCheck;
        private static readonly string _facebookLink = string.Empty;
        private static readonly string _twitterLink = string.Empty;
        private static readonly string _youtubeLink = string.Empty;
        private static readonly string _randomColors = string.Empty;
        private static readonly string _youTubeDevKey = string.Empty;
        private static readonly string _youTubeDevUser = string.Empty;
        private static readonly string _youTubeDevPass = string.Empty;

        #endregion

        #region properties

 

        public static bool EnableSameIP
        {
            get { return _enableSameIP; }
        }


        public static bool IsGiveAway
        {
            get { return _isGiveAway; }
        }


        public static int UserChatRoomSessionTimeout
        {
            get { return _userChatRoomSessionTimeout; }
        }

        public static string AdminUserName
        {
            get { return _adminUserName; }
        }


        public static string RandomColors
        {
            get { return _randomColors; }
        }


        public static int MinimumAge
        {
            get { return _minimumAge; }
        }


        public static string FacebookLink
        {
            get { return _facebookLink; }
        }

        public static string TwitterLink
        {
            get { return _twitterLink; }
        }

        public static string YoutubeLink
        {
            get { return _youtubeLink; }
        }
 
        public static string DefaultVideo
        {
            get { return _defaultVideo; }
        }


        public static int PostInterval
        {
            get { return _postInterval; }
        }

        public static bool EnableVideoCheck
        {
            get { return _enableVideoCheck; }
        }


        public static string SiteName
        {
            get { return _siteName; }
        }


        public static string DefaultLanguage
        {
            get { return _defaultLanguage; }
        }

      

        public static string GoogleAPIKey
        {
            get { return _googleAPIKey; }
        }


        public static string SendToErrorEmail
        {
            get { return _sendToErrorEmail; }
        }


        public static string EmailSettingsURL
        {
            get { return _emailSettingsURL; }
        }


        public static string PayPalURL
        {
            get { return _payPalURL; }
        }


        public static string PayPalPDTKey
        {
            get { return _payPalPDTKey; }
        }


        public static string SiteDomain
        {
            get { return _siteDomain; }
        }


        public static bool UseNetworkForMail
        {
            get { return _useNetworkForMail; }
        }


        public static string ErrorLogFilename
        {
            get { return _errorLogFilename; }
        }


        public static bool EnableErrorLogEmail
        {
            get { return _enableErrorLogEmail; }
        }

     
        public static string NonWebFilePath
        {
            get { return _nonWebFilePath; }
        }

        public static string YouTubeDevKey
        {
            get { return _youTubeDevKey; }
        }

        public static string YouTubeDevUser
        {
            get { return _youTubeDevUser; }
        }

        public static string YouTubeDevPass
        {
            get { return _youTubeDevPass; }
        }

        #endregion
    }
}