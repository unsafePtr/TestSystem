using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DbAccess.Entities;

namespace TestSystem.Service.Dtos
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool? IsCorrect { get; set; }
        public bool IsAccepted { get; set; }
        public int QuestionId { get; set; }
        public int UserTestId { get; set; }

        public QuestionDto Question { get; set; }

        public AnswerDto()
        {

        }

        internal AnswerDto(Answer entity)
        {
            CopyEntityData(entity);
        }

        internal void UpdateEntity(Answer entity)
        {
            entity.Content = this.Content == null ? String.Empty : this.Content;
            entity.IsAccepted = this.IsAccepted;
            entity.IsCorrect = this.IsCorrect;
            entity.QuestionId = this.QuestionId;
            entity.UserTestId = this.UserTestId;
        }

        internal void CopyEntityData(Answer entity)
        {
            this.Id = entity.Id;
            this.Content = entity.Content;
            this.IsCorrect = entity.IsCorrect;
            this.IsAccepted = entity.IsAccepted;
            this.QuestionId = entity.QuestionId;
            this.UserTestId = entity.UserTestId;
        }
    }
}
