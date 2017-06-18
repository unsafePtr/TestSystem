using AppHarbor.DbAccess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AppHarbor.DbAccess
{
    public class AppHarborDbContext : IdentityDbContext<User>, IAppHarborDbContext
    {
        public AppHarborDbContext(string connectionString) : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }

    }
}
