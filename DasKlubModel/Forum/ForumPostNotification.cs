﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DasKlub.Web.Models.Domain;

namespace DasKlub.Web.Models.Forum
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
