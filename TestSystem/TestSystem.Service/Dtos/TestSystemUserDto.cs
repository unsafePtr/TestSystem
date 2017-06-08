using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.DbAccess.Entities;

namespace TestSystem.Service.Dtos
{
    public class TestSystemUserDto
    {
        public string Id { get; set; }

        public TestSystemUserDto(string Id)
        {
            this.Id = Id;
        }

        internal TestSystemUserDto(TestSystemUser entity)
        {
            CopyEntityData(entity);
        }

        internal void UpdateEntity(TestSystemUser entity)
        {
            entity.Id = this.Id;
        }

        internal void CopyEntityData(TestSystemUser entity)
        {
            this.Id = entity.Id;
        }
    }
}
