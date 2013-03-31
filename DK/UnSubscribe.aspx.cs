using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Values;

namespace DasKlub
{
    public partial class UnSubscribe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string email = Server.UrlDecode( Request.QueryString[SiteEnums.QueryStringNames.email.ToString()]);

          
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            UserAccount ua = new UserAccount();
            ua.GetUserAccountByEmail(txtEmail.Text.Trim());
            if (ua.UserAccountID == 0)
            {
                litResult.Text = @"<span style=""color:red"">Email not found!</span>";
                return;
            }

            UserAccountDetail uad = new UserAccountDetail();
            uad.GetUserAccountDeailForUser(ua.UserAccountID);
            uad.EmailMessages = false;

            if (uad.Update())
            {
                litResult.Text = @"<span style=""color:green"">Unsubscribed!</span>";
            }
            else
            {
                litResult.Text = @"<span style=""color:red"">error!</span>";
            }
        }
    }
}