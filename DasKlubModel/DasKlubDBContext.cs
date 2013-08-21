using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using DasKlub.Models.Domain;
using DasKlub.Models.Forum;

namespace DasKlub.Models 
{
    public partial class DasKlubDbContext : BaseContext<DasKlubDbContext>
    {
        public IDbSet<ForumCategory>            ForumCategory           { get; set; }
        public IDbSet<ForumSubCategory>         ForumSubCategory        { get; set; }
        public IDbSet<ForumPost>                ForumPost               { get; set; }
        public IDbSet<ForumPostNotification>    ForumPostNotification   { get; set; }

        public DasKlubDbContext()
            : base((ConfigurationManager.AppSettings["DatabaseName"] ?? "DasKlubDbContext"))
           
        {
        }

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
