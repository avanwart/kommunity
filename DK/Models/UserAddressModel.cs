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

using System.ComponentModel.DataAnnotations;
using BootBaronLib.Resources;

namespace DasKlub.Models
{
    public class UserAddressModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "StreetAddress")]
        public string AddressLine1 { get; set; }

        [Display(ResourceType = typeof (Messages), Name = "StreetAddress")]
        public string AddressLine2 { get; set; }

        [Display(ResourceType = typeof (Messages), Name = "StreetAddress")]
        public string AddressLine3 { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "RegionState")]
        public string RegionState { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Country")]
        public string Country { get; set; }


        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "PostalCode")]
        public string PostalCode { get; set; }
    }
}