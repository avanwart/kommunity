using System;
using System.Data.Entity;
using System.Linq;
using DasKlub.Models.Domain;
using DasKlub.Models.Forum;


namespace DasKlub.Models 
{
    public partial class DasKlubDBContext : BaseContext<DasKlubDBContext>
    {
        //static DasKlubDBContext()
        //{
        //    Database.SetInitializer<DasKlubDBContext>(null);
        //}

        //public DasKlubDBContext()
        //   // : base("Name=DasKlubDBContext")
        //    : base("DasKlubDBContext")
        //{
        //}


        public DbSet<ForumCategory> ForumCategory { get; set; }
        public DbSet<ForumSubCategory> ForumSubCategory { get; set; }
        public DbSet<ForumPost> ForumPost { get; set; }
        public DbSet<ForumPostNotification> ForumPostNotification { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    modelBuilder.Configurations.Add(new ForumCategory());
        //    modelBuilder.Configurations.Add(new ForumSubCategory());
        //    modelBuilder.Configurations.Add(new ForumPost());
        //}



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
