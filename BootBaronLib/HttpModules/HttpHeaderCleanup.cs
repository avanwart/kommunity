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
using System.Web;

namespace BootBaronLib.HttpModules
{
    /// <summary>
    ///     For this to work,
    ///     make sure the application pool is configured to Integreated mode (which is the default)
    /// </summary>
    public class HttpHeaderCleanup : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        private static void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Headers.Remove("Server");
            }
            catch //(PlatformNotSupportedException ex)
            {
                // cannot do anything
            }
        }

        #endregion
    }
}