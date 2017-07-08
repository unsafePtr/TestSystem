using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdnetityExample.Models
{
    public class AnswerViewModel
    {
        public int UserTestId { get; set; }
        public ICollection<AnswerContent> Answers { get; set; }
        
        public AnswerViewModel()
        {
            this.Answers = new List<AnswerContent>();
        }
    }

    public class AnswerContent
    {
        public int QuestionId { get; set; }
        public ICollection<string> Contents { get; set; }

        public AnswerContent()
        {
            this.Contents = new List<string>();
        }
    }
}