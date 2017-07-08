using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.DbAccess.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool? IsCorrect { get; set; }
        public bool IsAccepted { get; set; }
        public int QuestionId { get; set; }
        public int UserTestId { get; set; }

        public Question Question { get; set; }
        public UserTest UserTest { get; set; }

        public Answer()
        {
            IsCorrect = null;
            IsAccepted = false;
        }
    }
}
