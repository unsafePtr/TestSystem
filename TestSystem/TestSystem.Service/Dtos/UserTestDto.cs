using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DbAccess.Entities;

namespace TestSystem.Service.Dtos
{
    public class UserTestDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public TestDto Test { get; set; }

        public UserTestDto()
        {

        }

        internal void UpdateEntity(UserTest entity)
        {
            entity.UserId = UserId;
            entity.TestId = TestId;
            entity.Start = Start;
            entity.End = End;
        }

        internal void CopyEntityData(UserTest entity)
        {
            this.Id = entity.Id;
            this.UserId = entity.UserId;
            this.TestId = entity.TestId;
            this.Start = entity.Start;
            this.End = entity.End;
        }
    }
}
