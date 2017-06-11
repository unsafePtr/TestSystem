using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public ICollection<Role> Roles { get; set; }
        public ICollection<ClaimEntity> Claims { get; set; }

        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<Role>();
            this.Claims = new List<ClaimEntity>();
        }
    }
}
