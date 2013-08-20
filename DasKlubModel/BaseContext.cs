using System.Data.Entity;

namespace DasKlub.Models
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext()
            // : base("Name=DasKlubDBContext")
            //: base("Name=DasKlubDB")
            : base("DasKlubDB")
        {
        }
    }
}
