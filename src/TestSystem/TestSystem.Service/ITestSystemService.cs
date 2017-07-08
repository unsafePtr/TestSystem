using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestSystem.DbAccess.Entities;
using TestSystem.Service.Dtos;

namespace TestSystem.Service
{
    public interface ITestSystemService
    {
        Task<string> CreateUserAsync();
        Task<int> CreateTestAsync(TestDto test);
        Task<IEnumerable<TestDto>> GetTestsAsync();
        Task<IEnumerable<TestDto>> GetTestsAsync(string userId);
        Task<int> AddUserTestAsync(UserTestDto userTest);
        Task<UserTestDto> GetUserTestAsync(int userTestId);
        Task<IEnumerable<UserTestDto>> GetUserTestsAsync(string userId);
        Task<int> AddQuestionAsync(int testId, QuestionDto question);
        Task<int> AddAnswerAsync(AnswerDto answer);
        Task StartUserTestAsync(int userTestId);
        Task EndUserTestAsync(int userTestId);
    }
}