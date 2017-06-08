using IdentityExample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityExample.Context
{
    public interface IAppDbContext
    {
        IDbSet<User> Users { get; }
    }
}
