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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootBaronLib.AppSpec.DasKlub.BOL;
using System.Web.Security;
using System.Data.SqlClient;
using BootBaronLib.Configs;
using BootBaronLib.Values;

using System.Text;
using System.Web.Caching;
using BootBaronLib.Operational;
using System.Collections;

namespace DasKlub
{
    public partial class StatusCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            lblPing.Text = Utilities.GetIPForDomain("google.com");

            lblIP.Text = Request.UserHostAddress;

            lblServerIP.Text = Request.ServerVariables["LOCAL_ADDR"];

            //////////// DB


            using (SqlConnection connection = new SqlConnection(DataBaseConfigs.DbConnectionString))
            {
                try
                {
                    //connection.ConnectionTimeout = ;
                    connection.Open();
                    connection.Close();

                    lblDB.ForeColor = System.Drawing.Color.Green;
                    lblDB.Text = "OK";
                }
                catch (Exception ex)
                {
                    lblDB.ForeColor = System.Drawing.Color.Red;
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
            lblBrowser.ForeColor = System.Drawing.Color.Green;

            lblIsMobile.Text = Request.Browser.IsMobileDevice.ToString();
            lblIsMobile.ForeColor = System.Drawing.Color.Green;

            StringBuilder sb = new StringBuilder();
            Cache cache = HttpRuntime.Cache;
            List<string> keys = new List<string>();

            foreach (string entry in Application.AllKeys)
            {
                sb.AppendLine(entry);
                sb.AppendLine(" : ");
                sb.AppendLine(Convert.ToString(Application[entry]));
                sb.AppendLine("<br />");
            }

            foreach (DictionaryEntry entry in cache)
            {
                sb.AppendLine((string)entry.Key);
                sb.AppendLine("<br />");
            }

            litCache.Text = sb.ToString();

        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            string resp = string.Empty;

            //////////// Email
            if (Utilities.SendMail(GeneralConfigs.SendToErrorEmail, AmazonCloudConfigs.SendFromEmail, "subject", "body"))
            {
                lblEmail.ForeColor = System.Drawing.Color.Green;
                lblEmail.Text = "OK";
            }
            else
            {
                lblEmail.ForeColor = System.Drawing.Color.Red;
                lblEmail.Text = resp;
            }
        }
    }
}