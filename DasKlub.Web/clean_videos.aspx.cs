using System;
using System.Web.UI;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Operational;

namespace DasKlub.Web
{
    public partial class clean_videos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int removed = 0;
            var vids = new Videos();

            vids.GetAll();

            foreach (Video vv1 in vids)
            {
                if (vv1.IsEnabled)
                {
                    bool? sss = Utilities.GETRequest(new Uri(
                        string.Format("http://i3.ytimg.com/vi/{0}/1.jpg",
                            vv1.ProviderKey)),
                        true);

                    if (sss == null) continue;

                    if (!Convert.ToBoolean(sss))
                    {
                        vv1.IsEnabled = false;
                        removed++;
                        vv1.Update();
                    }
                }
            }

            Response.Write(removed);
        }
    }
}