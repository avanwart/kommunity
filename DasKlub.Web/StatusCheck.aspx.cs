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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Services;

namespace DasKlub.Web
{
    public partial class StatusCheck : Page
    {
        private readonly IMailService _mail;

        public StatusCheck(IMailService mail)
        {
            _mail = mail;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblPing.Text = Utilities.GetIPForDomain("google.com");

            lblIP.Text = Request.UserHostAddress;

            lblServerIP.Text = Request.ServerVariables["LOCAL_ADDR"];

            //////////// DB


            using (var connection = new SqlConnection(DataBaseConfigs.DbConnectionString))
            {
                try
                {
                    //connection.ConnectionTimeout = ;
                    connection.Open();
                    connection.Close();

                    lblDB.ForeColor = Color.Green;
                    lblDB.Text = "OK";
                }
                catch (Exception ex)
                {
                    lblDB.ForeColor = Color.Red;
                    lblDB.Text = ex.ToString();
                }
            }


            //////////// IP
            //SiteStructs.RegionCountry rc = IPLookUp.GetRegionCountryForIPValue(Request.UserHostAddress);

            //lblCountry.ForeColor = System.Drawing.Color.Green;
            //lblCountry.Text = rc.CountryCode;

            //lblRegion.ForeColor = System.Drawing.Color.Green;
            //lblRegion.Text = rc.Region;


            //////////// Error

            Utilities.LogError("error test");

            lblError.Text = "?";
            //lblError.ForeColor = System.Drawing.Color.Green;

            lblBrowser.Text = Request.Browser.Type;
            lblBrowser.ForeColor = Color.Green;

            lblIsMobile.Text = Request.Browser.IsMobileDevice.ToString();
            lblIsMobile.ForeColor = Color.Green;

            var sb = new StringBuilder();
            Cache cache = HttpRuntime.Cache;
            var keys = new List<string>();

            foreach (string entry in Application.AllKeys)
            {
                sb.AppendLine(entry);
                sb.AppendLine(" : ");
                sb.AppendLine(Convert.ToString(Application[entry]));
                sb.AppendLine("<br />");
            }

            foreach (DictionaryEntry entry in cache)
            {
                sb.AppendLine((string) entry.Key);
                sb.AppendLine("<br />");
            }

            litCache.Text = sb.ToString();
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            string resp = string.Empty;

            //////////// Email
            if (_mail.SendMail(GeneralConfigs.SendToErrorEmail, AmazonCloudConfigs.SendFromEmail, "subject", "body"))
            {
                lblEmail.ForeColor = Color.Green;
                lblEmail.Text = "OK";
            }
            else
            {
                lblEmail.ForeColor = Color.Red;
                lblEmail.Text = resp;
            }
        }
    }
}