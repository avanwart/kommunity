using System.Data.Entity;
using DasKlub.Models.Forum;

namespace DasKlub.Models.Domain
{
    public class ForumContext : DbContext
    {
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<ForumSubCategory> SubCategories { get; set; }
        public DbSet<ForumPost> Posts { get; set; }
 
    }
}
