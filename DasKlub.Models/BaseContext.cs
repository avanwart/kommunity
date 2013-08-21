using System.Configuration;
using System.Data.Entity;

namespace DasKlub.Models
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext(string con)
            : base(con)
        {

        }
       
    }
}
