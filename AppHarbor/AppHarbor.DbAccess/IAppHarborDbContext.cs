using AppHarbor.DbAccess.Entities;
using System.Data.Entity;

namespace AppHarbor.DbAccess
{
    public interface IAppHarborDbContext
    {
        IDbSet<User> Users { get; set; }
    }
}
