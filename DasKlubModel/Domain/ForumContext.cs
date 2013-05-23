using System;
using System.Data.Entity;
using System.Linq;
using DasKlub.Web.Models.Forum;

namespace DasKlub.Web.Models.Domain
{
    public class ForumContext : DbContext
    {
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<ForumSubCategory> SubCategories { get; set; }
        public DbSet<ForumPost> Posts { get; set; }
 
    }
}
