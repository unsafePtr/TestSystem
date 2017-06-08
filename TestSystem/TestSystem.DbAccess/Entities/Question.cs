using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.DbAccess.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int RightAnswers { get; set; }
        public DateTime Created { get; set; }
        public QuestionTypeEnum QuestionTypeId { get; set; }
        public int TestId { get; set; }

        public QuestionType QuestionType { get; set; }
        public Test Test { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public ICollection<QuestionAnswerOption> QuestionAnswerOptions { get; set; }

        public Question()
        {
            this.Answers = new List<Answer>();
            this.RightAnswers = 1;
        }
    }
}
