using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess
{
    public class AppHarborDbContext : DbContext, IAppHarborDbContext
    {
        public AppHarborDbContext(string connectionString) : base(connectionString)
        {

        }
    }
}
