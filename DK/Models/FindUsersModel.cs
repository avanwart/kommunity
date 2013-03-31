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

namespace DasKlub.Models
{
    public class FindUsersModel
    {
        private int _ageFrom = 18;
        private int _ageTo = 35;
        public string PostalCode { get; set; }

        public int? YouAreID { get; set; }

        public int? RelationshipStatusID { get; set; }

        public int? InterestedInID { get; set; }

        public string Lang { get; set; }

        public int AgeFrom
        {
            get { return _ageFrom; }
            set { _ageFrom = value; }
        }

        public int AgeTo
        {
            get { return _ageTo; }
            set { _ageTo = value; }
        }

        public string Country { get; set; }
    }
}