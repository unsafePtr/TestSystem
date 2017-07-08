using IdnetityExample.DbAccess.Entities;
using System.Data.Entity;

namespace IdnetityExample.DbAccess
{
    public interface IIdentityExampleDbContext
    {
        IDbSet<User> Users { get; set; }
    }
}
