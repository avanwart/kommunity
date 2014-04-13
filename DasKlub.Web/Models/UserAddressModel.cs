using System.ComponentModel.DataAnnotations;
using DasKlub.Lib.Resources;

namespace DasKlub.Web.Models
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