using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.Service.Claims
{
    public class ActionPermissionValues
    {
        public const string AddAnswer = nameof(AddAnswer);
        public const string CreateTest = nameof(CreateTest);
        public const string AddQuestion = nameof(AddQuestion);
        public const string AddUserTest = nameof(AddUserTest);
        public const string EndUserTest = nameof(EndUserTest);
        public const string GetTests = nameof(GetTests);
        public const string GetUserTest = nameof(GetUserTest);
        public const string StartUserTest = nameof(StartUserTest);
    }
}
