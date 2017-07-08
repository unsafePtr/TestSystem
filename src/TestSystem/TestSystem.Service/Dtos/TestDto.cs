using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DbAccess.Entities;

namespace TestSystem.Service.Dtos
{
    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionsCount { get; set; }
        public int QuestionsForPassing { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Created { get; set; }
        public TestStatusEnum TestStatusId { get; set; }

        public ICollection<QuestionDto> Questions { get; set; }

        public TestDto()
        {
            this.Questions = new List<QuestionDto>();
        }

        internal TestDto(Test entity) : this()
        {
            CopyEntityData(entity);
        }

        internal void UpdateEntity(Test entity)
        {
            entity.Name = this.Name;
            entity.Description = this.Description;
            entity.QuestionsCount = this.QuestionsCount;
            entity.QuestionsForPassing = this.QuestionsForPassing;
            entity.Duration = this.Duration == TimeSpan.MinValue ? TimeSpan.MaxValue : this.Duration;
            entity.Created = this.Created == DateTime.MinValue ? DateTime.UtcNow : this.Created;
            entity.TestStatusId = this.TestStatusId;
        }

        internal void CopyEntityData(Test entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            QuestionsForPassing = entity.QuestionsForPassing;
            Duration = entity.Duration;
            Created = entity.Created;
            TestStatusId = entity.TestStatusId;
        }
    }
}
