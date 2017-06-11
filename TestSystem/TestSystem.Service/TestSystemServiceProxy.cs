using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSystem.Service.Attributes;
using TestSystem.Service.Claims;
using TestSystem.Service.Dtos;
using TestSystem.Service.Exceptions;

namespace TestSystem.Service
{
    public class TestSystemServiceProxy : ITestSystemService
    {
        private readonly TestSystemService service;

        public TestSystemServiceProxy(TestSystemService service)
        {
            this.service = service;
        }

        /// <summary>
        /// check claim authorization rights
        /// </summary>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        public void ThrowIfNotAllowedAccess(string claimType, string claimValue)
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
        
        public async Task<int> AddAnswerAsync(AnswerDto answer)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.AddAnswer);
            return await service.AddAnswerAsync(answer);
        }
        
        public async Task<int> AddQuestionAsync(int testId, QuestionDto question)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.AddQuestion);
            return await service.AddQuestionAsync(testId, question);
        }
        
        public async Task<int> AddUserTestAsync(UserTestDto userTest)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.AddUserTest);
            return await service.AddUserTestAsync(userTest);
        }
        
        public async Task<int> CreateTestAsync(TestDto test)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.CreateTest);
            return await service.CreateTestAsync(test);
        }

        public async Task<string> CreateUserAsync()
        {
            return await service.CreateUserAsync();
        }
        
        public async Task EndUserTestAsync(int userTestId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.EndUserTest);
            await service.EndUserTestAsync(userTestId);
        }
        
        public async Task<IEnumerable<TestDto>> GetTestsAsync()
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.GetTests);
            return await service.GetTestsAsync();
        }
        
        public async Task<IEnumerable<TestDto>> GetTestsAsync(string userId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.GetTests);
            return await service.GetTestsAsync(userId);
        }
        
        public async Task<UserTestDto> GetUserTestAsync(int userTestId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.GetUserTest);
            return await service.GetUserTestAsync(userTestId);
        }
        
        public async Task StartUserTestAsync(int userTestId)
        {
            ThrowIfNotAllowedAccess(ActionClaimType.ActionPermission, ActionPermissionValues.StartUserTest);
            await service.StartUserTestAsync(userTestId);
        }
    }
}
