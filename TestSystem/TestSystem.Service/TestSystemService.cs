using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.Common.Helpers;
using TestSystem.Common.Helpers.Extensions;
using TestSystem.DbAccess.Context;
using TestSystem.DbAccess.Entities;
using TestSystem.Service.Dtos;

namespace TestSystem.Service
{
    public class TestSystemService : ITestSystemService
    {
        private readonly ITestSystemDbContext context;

        public TestSystemService(ITestSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<string> CreateUserAsync()
        {
            TestSystemUser user = new TestSystemUser()
            {
                Id = Guid.NewGuid().ToString()
            };

            context.TestSystemUsers.Add(user);
            await context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<int> CreateTestAsync(TestDto test)
        {
            ThrowIfTestIsInvalid(test);

            Test newTest = new Test();
            test.UpdateEntity(newTest);

            List<Question> testQuestions = new List<Question>(test.Questions.Count);
            foreach (QuestionDto questionDto in test.Questions)
            {
                ThrowIfQuestionIsInvalid(questionDto);

                // if questions for passing is zero, than it is like quiz. Whatewer you will pass is.
                // if questons count is zero,  than current test should contains them all

                // questions count in test.
                if (test.QuestionsCount == 0)
                {
                    newTest.QuestionsCount++;
                }

                Question question = new Question();
                questionDto.UpdateEntity(question);
                if (questionDto.QuestionTypeId == QuestionTypeEnum.Closed)
                {
                    List<QuestionAnswerOption> options = new List<QuestionAnswerOption>(questionDto.Options.Count);
                    foreach (QuestionAnswerOptionDto answerOption in questionDto.Options)
                    {
                        QuestionAnswerOption option = new QuestionAnswerOption();
                        answerOption.UpdateEntity(option);
                        options.Add(option);
                    }

                    question.QuestionAnswerOptions = options;
                }

                testQuestions.Add(question);
            }

            testQuestions.ForEach(q => newTest.Questions.Add(q));

            context.Tests.Add(newTest);
            await context.SaveChangesAsync();

            return newTest.Id;
        }

        public async Task<IEnumerable<TestDto>> GetTestsAsync()
        {
            var tests = await context.Tests.ToListAsync();

            return tests.Select(t => new TestDto(t));
        }

        public async Task<IEnumerable<TestDto>> GetTestsAsync(string userId)
        {
            var tests = await context.UserTests
                .Include(ut => ut.Test)
                .Where(ut => ut.UserId == userId)
                .Select(ut => ut.Test)
                .ToArrayAsync();

            return tests.Select(t => new TestDto(t));
        }

        public async Task<int> AddUserTestAsync(UserTestDto userTest)
        {
            ThrowIfUserTestEntityIsInvalid(userTest);

            // add user test
            UserTest newUserTest = new UserTest();
            userTest.UpdateEntity(newUserTest);

            context.UserTests.Add(newUserTest);

            // add blank answers
            Test test = await context.Tests
                .Include(t => t.Questions)
                .SingleAsync(t => t.Id == userTest.TestId);

            Question[] questions = test.Questions.ToArray()
                .Shuffle()
                .Take(test.QuestionsCount)
                .ToArray();

            List<Answer> answers = new List<Answer>(questions.Count()); // for sure one answer per questions
            foreach (Question question in questions)
            {
                Answer answer = new Answer()
                {
                    QuestionId = question.Id,
                    UserTestId = test.Id,
                    Content = String.Empty,
                    IsAccepted = false, // indicates that we have not already answered on question
                };

                answers.Add(answer);
            }

            // because each answer contains QuestuionId we can get later which questions are assigned for this test
            newUserTest.Answers = answers;

            await context.SaveChangesAsync();

            return newUserTest.Id;
        }

        public async Task<UserTestDto> GetUserTestAsync(int userTestId)
        {
            UserTest userTest = await context.UserTests
                .Include(ut => ut.Answers.Select(a => a.Question.Test))
                .Include(ut => ut.Answers.Select(a => a.Question.QuestionAnswerOptions))
                .Where(ut => ut.Id == userTestId)
                .SingleOrDefaultAsync();

            UserTestDto userTestDto = new UserTestDto();
            userTestDto.CopyEntityData(userTest);
            userTestDto.Test = new TestDto(userTest.Test);
            userTestDto.Test.Questions = userTest.Test.Questions.Select(q => new QuestionDto(q)).ToArray();

            return userTestDto;
        }

        public async Task<IEnumerable<UserTestDto>> GetUserTestsAsync(string userId)
        {
            UserTest[] userTests = await context.UserTests
                .Include(ut => ut.Test)
                .Where(ut => ut.UserId == userId)
                .ToArrayAsync();

            return userTests.Select(userTest =>
            {
                UserTestDto userTestDto = new UserTestDto();
                userTestDto.CopyEntityData(userTest);
                userTestDto.Test = new TestDto(userTest.Test);
                userTestDto.Test.Questions = userTest.Test.Questions.Select(q => new QuestionDto(q)).ToArray();

                return userTestDto;
            });
        }

        public async Task<int> AddQuestionAsync(int testId, QuestionDto question)
        {
            Test test = await context.Tests.FindAsync(testId);

            test.ThrowIfNull(new InvalidOperationException($"Test with id {testId} does not exists"));
            question.ThrowIfNull(new ArgumentNullException(nameof(question)));
            await ThrowIfTestAssignedToUserAsync(test);

            Question newQuestion = new Question();
            question.UpdateEntity(newQuestion);

            if (question.QuestionTypeId == QuestionTypeEnum.Closed)
            {
                List<QuestionAnswerOption> options = new List<QuestionAnswerOption>(question.Options.Count);
                foreach (QuestionAnswerOptionDto answerOption in question.Options)
                {
                    QuestionAnswerOption option = new QuestionAnswerOption();
                    answerOption.UpdateEntity(option);
                    options.Add(option);
                }

                newQuestion.QuestionAnswerOptions = options;
            }

            test.Questions.Add(newQuestion);
            await context.SaveChangesAsync();

            return newQuestion.Id;
        }

        public async Task<int> AddAnswerAsync(AnswerDto answer)
        {
            /**
             * Internally answer exists but with are not accepted
             * It's made to manage which questions have UserTest object
             * 
             */
            Answer existingAnswer = await context.Answers
                .Include(a => a.UserTest)
                .Include(a => a.Question.QuestionAnswerOptions)
                .Where(a => a.UserTestId == answer.UserTestId)
                .Where(a => a.QuestionId == answer.QuestionId)
                .FirstOrDefaultAsync();

            existingAnswer.ThrowIfNull(new InvalidOperationException(
                $"Answer with {nameof(answer.UserTestId)} {answer.UserTestId} and {nameof(answer.QuestionId)} {answer.QuestionId} does not exists")
            );

            ThrowIfTestAlreadyEnded(existingAnswer.UserTest);

            // only Closed questions can have more than one right answer
            if (existingAnswer.Question.RightAnswers != 1 &&
                existingAnswer.Question.QuestionTypeId == QuestionTypeEnum.Closed)
            {
                QuestionAnswerOption option = existingAnswer.Question
                    .QuestionAnswerOptions
                    .Where(q => q.Content == answer.Content) // it's closed question. content should be the same
                    .FirstOrDefault();

                if (existingAnswer.IsAccepted) // add new
                {
                    Answer newAnswer = new Answer()
                    {
                        IsAccepted = true,
                        IsCorrect = option == null ? false : true,
                        QuestionId = answer.QuestionId,
                        UserTestId = answer.UserTestId,
                        Content = answer.Content
                    };

                    context.Answers.Add(newAnswer);
                }
                else
                {
                    existingAnswer.IsAccepted = true;
                }
            }
            else // is open question
            {
                existingAnswer.IsCorrect = null;
                existingAnswer.IsAccepted = true;
            }

            await context.SaveChangesAsync();

            return existingAnswer.Id;
        }

        public async Task StartUserTestAsync(int userTestId)
        {
            UserTest userTest = await context.UserTests
                .Include(ut => ut.Test)
                .Where(ut => ut.Id == userTestId)
                .SingleOrDefaultAsync();

            ThrowIfUserTestAlreadyStarted(userTest);

            userTest.Start = DateTime.UtcNow + userTest.Test.Duration;

            await context.SaveChangesAsync();
        }

        public async Task EndUserTestAsync(int userTestId)
        {
            UserTest userTest = await context.UserTests
                .Include(ut => ut.Test)
                .Where(ut => ut.Id == userTestId)
                .SingleOrDefaultAsync();
            
            DateTime currentDateTime = DateTime.UtcNow;
            
            if ((userTest.End == null || userTest.End > currentDateTime) && userTest.Start != null)
            {
                userTest.End = currentDateTime;
                await context.SaveChangesAsync();
            }

        }

        #region check_functions

        public void ThrowIfTestIsInvalid(TestDto test)
        {
            if(String.IsNullOrEmpty(test.Name))
            {
                throw new ArgumentException("Test Name should not be the null or empty value");
            }

            if(test.QuestionsForPassing > test.QuestionsCount)
            {
                throw new ArgumentException($"Test {nameof(test.QuestionsForPassing)} should be less than {nameof(test.QuestionsCount)}");
            }
        }

        public void ThrowIfQuestionIsInvalid(QuestionDto question)
        {
            if(question.QuestionTypeId == QuestionTypeEnum.Closed)
            {
                if(question.Options.Count == 0)
                {
                    throw new InvalidOperationException("Can't add closed question with none question options");
                }

                if(question.Options.Count < question.RightAnswers)
                {
                    throw new InvalidOperationException("Can't add closed question with answers for passing less than questions count");
                }

            }
        }

        public void ThrowIfUserTestEntityIsInvalid(UserTestDto userTest)
        {
            TimeSpan accuracy = new TimeSpan(0, 1, 0);// accuracy one minute
            
            if (userTest.Start < DateTime.UtcNow - accuracy)
            {
                throw new InvalidOperationException("User test start date should be biger than current date");
            }

            if(userTest.End < userTest.Start)
            {
                throw new InvalidOperationException("Start date should be bigger than end date");
            }
        }

        public async Task ThrowIfTestAssignedToUserAsync(Test test)
        {
            int countAssigned = await context.UserTests.CountAsync(ut => ut.TestId == test.Id);
            if(countAssigned != 0)
            {
                throw new InvalidOperationException("Can't add question to test which is already assigned to user");
            }
        }

        public void ThrowIfUserTestAlreadyStarted(UserTest userTest)
        {
            if (userTest.Start != null)
            {
                throw new InvalidOperationException("Can't start test which is already started");
            }
        }
        
        private void ThrowIfTestAlreadyEnded(UserTest userTest)
        {
            if (userTest.End < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Can't add answer to test which is already ended");
            }
        }

        #endregion check_functions
        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
