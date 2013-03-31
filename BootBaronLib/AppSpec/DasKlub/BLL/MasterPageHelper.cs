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

using System.Web.UI;
using System.Web.UI.WebControls;

namespace BootBaronLib.AppSpec.DasKlub.BLL
{
    public static class MasterPageHelper
    {
        #region Master page

        /// <summary>
        ///     Set the master page literal text
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="message"></param>
        public static void SetMainMasterPageMessageText(Page currentPage, string message)
        {
            //Control crtt = this.Page.Master.FindControl("litMessage");

            var myTextBox = (Literal) currentPage.Master.FindControl("litMessage");
            myTextBox.Text = message;
        }


        public static void SetMainMasterPageMessageText(Page currentPage, string message, bool isGood)
        {
            var myTextBox = (Literal) currentPage.Master.FindControl("litMessage");

            if (isGood)
            {
                myTextBox.Text = @"<span style=""color: Green; font-weight: bold;"">" + message + "</span>";
            }
            else
            {
                myTextBox.Text = @"<span style=""color: Red; font-weight: bold;"">" + message + "</span>";
            }
        }

        #endregion
    }
}