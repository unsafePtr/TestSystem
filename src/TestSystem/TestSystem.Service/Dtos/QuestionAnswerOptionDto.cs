using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DbAccess.Entities;

namespace TestSystem.Service.Dtos
{
    public class QuestionAnswerOptionDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }

        public QuestionAnswerOptionDto() { }

        internal QuestionAnswerOptionDto(QuestionAnswerOption entity)
        {
            CopyEntityData(entity);
        }

        internal void UpdateEntity(QuestionAnswerOption entity)
        {
            entity.Id = this.Id;
            entity.Content = this.Content;
            entity.IsCorrect = this.IsCorrect;
        }

        internal void CopyEntityData(QuestionAnswerOption entity)
        {
            this.Id = entity.Id;
            this.Content = entity.Content;
            this.IsCorrect = entity.IsCorrect;
        }
    }
}
