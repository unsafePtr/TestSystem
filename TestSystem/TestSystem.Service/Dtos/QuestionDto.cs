using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.Common.Helpers.Extensions;
using TestSystem.DbAccess.Entities;

namespace TestSystem.Service.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int RightAnswers { get; set; }
        public DateTime Created { get; set; }
        public QuestionTypeEnum QuestionTypeId { get; set; }

        public ICollection<QuestionAnswerOptionDto> Options { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }

        public QuestionDto()
        {
            this.Options = new List<QuestionAnswerOptionDto>();
            this.RightAnswers = 1;
        }

        internal QuestionDto(Question entity) : this()
        {
            CopyEntityData(entity);
        }

        internal void UpdateEntity(Question entity)
        {
            entity.Content = this.Content;
            entity.RightAnswers = this.RightAnswers < 1 ? 1 : this.RightAnswers;
            entity.Created = this.Created == DateTime.MinValue ? DateTime.UtcNow : this.Created;
            entity.QuestionTypeId = this.QuestionTypeId;
        }

        internal void CopyEntityData(Question entity)
        {
            this.Id = entity.Id;
            this.Content = entity.Content;
            this.RightAnswers = entity.RightAnswers;
            this.Created = entity.Created;
            this.QuestionTypeId = entity.QuestionTypeId;
            this.Options = entity.QuestionAnswerOptions.Select(q => new QuestionAnswerOptionDto(q)).ToArray();
            this.Answers = entity.Answers.Select(a => new AnswerDto(a)).ToArray();
        }
    }
}
