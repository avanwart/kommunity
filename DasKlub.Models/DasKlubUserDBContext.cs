using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using DasKlub.Models.Domain;
using DasKlub.Models.Models;
using DasKlubModel.Models;

namespace DasKlub.Models
{
    public class DasKlubUserDBContext : BaseContext<DasKlubUserDBContext>
    {
        public DasKlubUserDBContext()
            : base((ConfigurationManager.AppSettings["DatabaseName"] ?? "DasKlubDBContext"))

        {
        }

        public IDbSet<UserAccountEntity> UserAccountEntity { get; set; }
        public IDbSet<UserAccountDetailEntity> UserAccountDetailEntity { get; set; }

        public override int SaveChanges()
        {
            foreach (StateInfo entry in ChangeTracker.Entries()
                .Where(x => x.Entity is StateInfo && x.State == EntityState.Added)
                .Select(x => x.Entity as StateInfo))
            {
                entry.CreateDate = DateTime.UtcNow;
            }

            foreach (StateInfo entry in ChangeTracker.Entries()
                .Where(x => x.Entity is StateInfo && x.State == EntityState.Modified)
                .Select(x => x.Entity as StateInfo))
            {
                entry.UpdateDate = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
    }
}