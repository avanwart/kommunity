using DasKlub.Models.Domain;

namespace DasKlub.Models.Forum
{
    public class ForumPostNotification : StateInfo
    {
        public int ForumPostNotificationID { get; set; }

        public int UserAccountID { get; set; }

        public bool IsRead { get; set; }

        public int ForumSubCategoryID { get; set; }

        public ForumSubCategory ForumSubCategory { get; set; }
    }
}