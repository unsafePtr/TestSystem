using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.DbAccess.Entities
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionsCount { get; set; }
        public int QuestionsForPassing { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Created { get; set; }
        public TestStatusEnum TestStatusId { get; set; }

        public TestStatus Status { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<UserTest> UserTests { get; set; }

        public Test()
        {
            this.Questions = new List<Question>();
            this.UserTests = new List<UserTest>();
        }
    }
}
