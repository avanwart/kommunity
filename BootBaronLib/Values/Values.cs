//  Copyright 2012 
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
using System.Collections;
using System.Web.UI.WebControls;
using BootBaronLib.Enums;

namespace BootBaronLib.Values
{
	public class InputValues
	{
        public InputValues()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Load states
        /// </summary>
        /// <param name="ddl"></param>
        public static void AssignUSStates(ref DropDownList ddl)
        {
            ddl.Items.Add(new ListItem("-Select US State-", string.Empty));

            ddl.Items.Add(new ListItem("Alabama", "AL"));
            ddl.Items.Add(new ListItem("Alaska", "AK"));
            ddl.Items.Add(new ListItem("Arizona", "AZ"));
            ddl.Items.Add(new ListItem("Arkansas", "AR"));
            ddl.Items.Add(new ListItem("California", "CA"));
            ddl.Items.Add(new ListItem("Colorado", "CO"));
            ddl.Items.Add(new ListItem("Connecticut", "CT"));
            ddl.Items.Add(new ListItem("Delaware", "DE"));
            ddl.Items.Add(new ListItem("Florida", "FL"));
            ddl.Items.Add(new ListItem("Georgia", "GA"));
            ddl.Items.Add(new ListItem("Hawaii", "HI"));
            ddl.Items.Add(new ListItem("Idaho", "ID"));
            ddl.Items.Add(new ListItem("Illinois", "IL"));
            ddl.Items.Add(new ListItem("Indiana", "IN"));
            ddl.Items.Add(new ListItem("Iowa", "IA"));
            ddl.Items.Add(new ListItem("Kansas", "KS"));
            ddl.Items.Add(new ListItem("Kentucky", "KY"));
            ddl.Items.Add(new ListItem("Louisiana", "LA"));
            ddl.Items.Add(new ListItem("Maine", "ME"));
            ddl.Items.Add(new ListItem("Maryland", "MD"));
            ddl.Items.Add(new ListItem("Massachusetts", "MA"));
            ddl.Items.Add(new ListItem("Michigan", "MI"));
            ddl.Items.Add(new ListItem("Minnesota", "MN"));
            ddl.Items.Add(new ListItem("Mississippi", "MS"));
            ddl.Items.Add(new ListItem("Missouri", "MO"));
            ddl.Items.Add(new ListItem("Montana", "MT"));
            ddl.Items.Add(new ListItem("Nebraska", "NE"));
            ddl.Items.Add(new ListItem("Nevada", "NV"));
            ddl.Items.Add(new ListItem("New Hampshire", "NH"));
            ddl.Items.Add(new ListItem("New Jersey", "NJ"));
            ddl.Items.Add(new ListItem("New Mexico", "NM"));
            ddl.Items.Add(new ListItem("New York", "NY"));
            ddl.Items.Add(new ListItem("North Carolina", "NC"));
            ddl.Items.Add(new ListItem("North Dakota", "ND"));
            ddl.Items.Add(new ListItem("Ohio", "OH"));
            ddl.Items.Add(new ListItem("Oklahoma", "OK"));
            ddl.Items.Add(new ListItem("Oregon", "OR"));
            ddl.Items.Add(new ListItem("Pennsylvania", "PA"));
            ddl.Items.Add(new ListItem("Puerto Rico", "PR"));
            ddl.Items.Add(new ListItem("Rhode Island", "RI"));
            ddl.Items.Add(new ListItem("South Carolina", "SC"));
            ddl.Items.Add(new ListItem("South Dakota", "SD"));
            ddl.Items.Add(new ListItem("Tennessee", "TN"));
            ddl.Items.Add(new ListItem("Texas", "TX"));
            ddl.Items.Add(new ListItem("Utah", "UT"));
            ddl.Items.Add(new ListItem("Vermont", "VT"));
            ddl.Items.Add(new ListItem("Virginia", "VA"));
            ddl.Items.Add(new ListItem("Washington", "WA"));
            ddl.Items.Add(new ListItem("West Virginia", "WV"));
            ddl.Items.Add(new ListItem("Wisconsin", "WI"));
            ddl.Items.Add(new ListItem("Wyoming", "WY"));

        }
 
	}
}