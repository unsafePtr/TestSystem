using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSystem.DbAccess.Context;
using TestSystem.Service.Attributes;
using TestSystem.Service.Claims;
using TestSystem.Service.Dtos;
using TestSystem.Service.Exceptions;

namespace TestSystem.Service
{
    public class TestSystemServiceProxy : ITestSystemService, IDisposable
    {
        private readonly TestSystemService service;

        public TestSystemServiceProxy(ITestSystemDbContext context)
        {
            this.service = new TestSystemService(context);
        }

        /// <summary>
        /// check claim authorization rights
        /// </summary>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        private void ThrowIfNotAllowedAccess(string claimType, string claimValue)
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            bool isAllowedAccess = identity.Claims
                .Where(c => c.Type == claimType && c.Value == claimValue)
                .Any();

            if(!isAllowedAccess)
            {
                throw new ActionClaimTypeException($"Provided credentials are not satisfied to access the method");
            }
        }
        
        public Task<int> AddAnswerAsync(AnswerDto answer)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.AddAnswer);
            return service.AddAnswerAsync(answer);
        }
        
        public Task<int> AddQuestionAsync(int testId, QuestionDto question)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.AddQuestion);
            return service.AddQuestionAsync(testId, question);
        }
        
        public Task<int> AddUserTestAsync(UserTestDto userTest)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.AddUserTest);
            return service.AddUserTestAsync(userTest);
        }
        
        public Task<int> CreateTestAsync(TestDto test)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.CreateTest);
            return service.CreateTestAsync(test);
        }

        public Task<string> CreateUserAsync()
        {
            return service.CreateUserAsync();
        }
        
        public async Task EndUserTestAsync(int userTestId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.EndUserTest);
            await service.EndUserTestAsync(userTestId);
        }
        
        public Task<IEnumerable<TestDto>> GetTestsAsync()
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.GetTests);
            return service.GetTestsAsync();
        }
        
        public Task<IEnumerable<TestDto>> GetTestsAsync(string userId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.GetTests);
            return service.GetTestsAsync(userId);
        }
        
        public Task<UserTestDto> GetUserTestAsync(int userTestId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.GetUserTest);
            return service.GetUserTestAsync(userTestId);
        }

        public Task<IEnumerable<UserTestDto>> GetUserTestsAsync(string userId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.GetUserTest);
            return service.GetUserTestsAsync(userId);
        }

        public async Task StartUserTestAsync(int userTestId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.StartUserTest);
            await service.StartUserTestAsync(userTestId);
        }

        public void Dispose()
        {
            this.service.Dispose();
        }
    }
}
