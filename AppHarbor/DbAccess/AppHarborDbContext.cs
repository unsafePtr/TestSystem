using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbAccess.Entities;

namespace DbAccess
{
    public class AppHarborDbContext : DbContext, IAppHarborDbContext
    {
        public AppHarborDbContext(string connectionString) : base(connectionString)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }
    }
}
