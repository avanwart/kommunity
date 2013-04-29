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
using System.Web.UI;
using BootBaronLib.Resources;

namespace DasKlub.Web
{
    public partial class VideoSubmission : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["statustype"])) return;
            string rslt = Request.QueryString["statustype"];

            switch (rslt)
            {
                case "W":
                    litResult.Text = Messages.WaitingToBeReviewed;
                    break;
                case "R":
                    litResult.Text = Messages.VideoRejected;
                    break;
                case "I":
                    litResult.Text = Messages.InvalidLink;
                    break;
                case "P":
                    litResult.Text = Messages.Error;
                    break;
            }
        }
    }
}