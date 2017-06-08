using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.DbAccess.Entities
{
    public class TestSystemUser 
    {
        public string Id { get; set; }

        public ICollection<UserTest> UserTests { get; set; }

        public TestSystemUser()
        {
            this.UserTests = new List<UserTest>();
        }
    }
}
