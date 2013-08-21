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
using DasKlub.Lib.AppSpec.DasKlub.BOL;
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