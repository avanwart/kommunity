﻿//  Copyright 2013 
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
using BootBaronLib.AppSpec.DasKlub.BOL;

namespace DasKlub.Web
{
    public partial class AffReport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var ua = new UserAccount(Request.QueryString["username"]);

                if (ua.UserAccountID == 0) return;

                litUserName.Text = ua.UserName;

                gvwReport.DataSource = UserAccountDetail.GetUserAffReport(ua.UserAccountID);
                gvwReport.DataBind();
            }
        }
    }
}