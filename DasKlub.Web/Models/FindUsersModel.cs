namespace DasKlub.Web.Models
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