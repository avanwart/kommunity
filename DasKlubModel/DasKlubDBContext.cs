using System;
using System.Data.Entity;
using System.Linq;
using DasKlub.Web.Models.Domain;
using DasKlub.Web.Models.Forum;


namespace DasKlub.Web.Models 
{
    public partial class DasKlubDBContext : BaseContext<DasKlubDBContext>
    {
        public IDbSet<ForumCategory>            ForumCategory           { get; set; }
        public IDbSet<ForumSubCategory>         ForumSubCategory        { get; set; }
        public IDbSet<ForumPost>                ForumPost               { get; set; }
        public IDbSet<ForumPostNotification>    ForumPostNotification   { get; set; }

        public override int SaveChanges()
        {

            foreach (var entry in ChangeTracker.Entries()
                 .Where(x => x.Entity is StateInfo && x.State == EntityState.Added)
                 .Select(x => x.Entity as StateInfo))
            {
                entry.CreateDate = DateTime.UtcNow;
            }

            foreach (var entry in ChangeTracker.Entries()
                .Where(x => x.Entity is StateInfo && x.State == EntityState.Modified)
                .Select(x => x.Entity as StateInfo))
            {
                entry.UpdateDate = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
    }
}
