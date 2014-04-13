using System;
using System.Web.UI;
using DasKlub.Lib.Resources;

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