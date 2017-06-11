using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Entities
{
    // add suffics "Entity" to avoid the naming coincidence with Claim class
    [Table("Claims")]
    public class ClaimEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public User User { get; set; }

        public ClaimEntity()
        {

        }
    }
}
