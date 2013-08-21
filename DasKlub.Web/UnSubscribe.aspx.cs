using System;
using System.Web.UI;
using DasKlub.Lib.AppSpec.DasKlub.BOL;
using DasKlub.Lib.Values;

namespace DasKlub.Web
{
    public partial class UnSubscribe : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string email = Server.UrlDecode(Request.QueryString[SiteEnums.QueryStringNames.email.ToString()]);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var ua = new UserAccount();
            ua.GetUserAccountByEmail(txtEmail.Text.Trim());
            if (ua.UserAccountID == 0)
            {
                litResult.Text = @"<span style=""color:red"">Email not found!</span>";
                return;
            }

            var uad = new UserAccountDetail();
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