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
        /// check claims authorization rights
        /// </summary>
        /// <param name="methodName"></param>
        public void CheckClaimsAttributes(string methodName)
        {
            MethodBase methodBase = typeof(TestSystemServiceProxy).GetMethod(methodName);

            var claimCheckAttributes = methodBase.GetCustomAttributes<ClaimCheckAttribute>();

            if(!claimCheckAttributes.All(c => c.IsAllowedAccess))
            {
                throw new ActionClaimTypeException($"Provided credentials are not satisfied to access the method {methodName}");
            }
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.AddAnswer)]
        public async Task<int> AddAnswerAsync(AnswerDto answer)
        {
            CheckClaimsAttributes(nameof(AddAnswerAsync));
            return await service.AddAnswerAsync(answer);
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.AddQuestion)]
        public async Task<int> AddQuestionAsync(int testId, QuestionDto question)
        {
            CheckClaimsAttributes(nameof(AddQuestionAsync));
            return await service.AddQuestionAsync(testId, question);
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.AddUserTest)]
        public async Task<int> AddUserTestAsync(UserTestDto userTest)
        {
            CheckClaimsAttributes(nameof(AddUserTestAsync));
            return await service.AddUserTestAsync(userTest);
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.CreateTest)]
        public async Task<int> CreateTestAsync(TestDto test)
        {
            CheckClaimsAttributes(nameof(CreateTestAsync));
            return await service.CreateTestAsync(test);
        }

        public async Task<string> CreateUserAsync()
        {
            return await service.CreateUserAsync();
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.EndUserTest)]
        public async Task EndUserTestAsync(int userTestId)
        {
            CheckClaimsAttributes(nameof(EndUserTestAsync));
            await service.EndUserTestAsync(userTestId);
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.GetTests)]
        public async Task<IEnumerable<TestDto>> GetTestsAsync()
        {
            CheckClaimsAttributes(nameof(GetTestsAsync));
            return await service.GetTestsAsync();
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.GetTests)]
        public async Task<IEnumerable<TestDto>> GetTestsAsync(string userId)
        {
            CheckClaimsAttributes(nameof(GetTestsAsync));
            return await service.GetTestsAsync(userId);
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.GetUserTest)]
        public async Task<UserTestDto> GetUserTestAsync(int userTestId)
        {
            CheckClaimsAttributes(nameof(GetUserTestAsync));
            return await service.GetUserTestAsync(userTestId);
        }

        [ClaimCheck(ActionClaimType.ActionPermission, ActionPermissionValues.StartUserTest)]
        public async Task StartUserTestAsync(int userTestId)
        {
            CheckClaimsAttributes(nameof(StartUserTestAsync));
            await service.StartUserTestAsync(userTestId);
        }
    }
}
