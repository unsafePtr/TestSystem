using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.DbAccess.Entities
{
    public class UserTest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public TestSystemUser User { get; set; }
        public Test Test { get; set; }
        public ICollection<Answer> Answers { get; set; }

        public UserTest()
        {
            this.Answers = new List<Answer>();
        }
    }
}
