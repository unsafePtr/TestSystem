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
        /// <summary>
        /// Creates new user. Uses GUID for creating userId. 
        /// </summary>
        /// <returns>Returns UserId</returns>
        Task<string> CreateUserAsync();
        /// <summary>
        /// Creates new test
        /// </summary>
        /// <param name="test">Returns TestId of supplied test</param>
        /// <returns></returns>
        Task<int> CreateTestAsync(TestDto test);
        /// <summary>
        /// Get all tests from db (without nested collections)
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TestDto>> GetTestsAsync();
        /// <summary>
        /// Returns all tests assigned to user (without nested collections)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TestDto>> GetTestsAsync(string userId);
        /// <summary>
        /// Assign existed testId to existed userId
        /// </summary>
        /// <param name="userTest">Returns userTestId</param>
        /// <returns></returns>
        Task<int> AddUserTestAsync(UserTestDto userTest);
        /// <summary>
        /// Get user test with nested collections e.g questions, question options, answers
        /// </summary>
        /// <param name="userTestId"></param>
        /// <returns></returns>
        Task<UserTestDto> GetUserTestAsync(int userTestId);
        /// <summary>
        /// Add question to existing which is not assigned to test.
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="question"></param>
        /// <returns>Returns id of new question</returns>
        Task<int> AddQuestionAsync(int testId, QuestionDto question);
        /// <summary>
        /// Add new answer
        /// </summary>
        /// <param name="answer"></param>
        /// <returns>Returns id of answer</returns>
        Task<int> AddAnswerAsync(AnswerDto answer);
        /// <summary>
        /// Starts tests. If the supplied userTestId is incorrect or test already started throws exception
        /// </summary>
        /// <param name="userTestId"></param>
        /// <returns></returns>
        Task StartUserTestAsync(int userTestId);
        Task EndUserTestAsync(int userTestId);
    }
}