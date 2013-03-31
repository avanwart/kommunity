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

namespace BootBaronLib.Configs
{
    public class AmazonCloudConfigs
    {
        #region variables

        private static readonly string _amazonAccessKey = string.Empty;
        private static readonly string _amazonSecretKey = string.Empty;
        private static readonly string _amazonCloudDomain = string.Empty;
        private static readonly string _amazonBucketName = string.Empty;
        private static readonly string _sendFromEmail = string.Empty;

        #endregion

        #region static constructor

        static AmazonCloudConfigs()
        {
            _sendFromEmail = ConfigurationManager.AppSettings["SendFromEmail"];
            _amazonAccessKey = ConfigurationManager.AppSettings["AmazonAccessKey"];
            _amazonSecretKey = ConfigurationManager.AppSettings["AmazonSecretKey"];
            _amazonCloudDomain = ConfigurationManager.AppSettings["AmazonCloudDomain"];
            _amazonBucketName = ConfigurationManager.AppSettings["AmazonBucketName"];
        }

        #endregion

        #region properties

        public static string SendFromEmail
        {
            get { return _sendFromEmail; }
        }


        public static string AmazonBucketName
        {
            get { return _amazonBucketName; }
        }

        public static string AmazonCloudDomain
        {
            get { return _amazonCloudDomain; }
        }

        public static string AmazonSecretKey
        {
            get { return _amazonSecretKey; }
        }


        public static string AmazonAccessKey
        {
            get { return _amazonAccessKey; }
        }

        #endregion
    }
}