using System;
using System.Web.UI;
using DasKlub.Lib.BOL;

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