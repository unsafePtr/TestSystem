using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TestSystem.DbAccess.Context;
using TestSystem.DbAccess.Entities;
using TestSystem.Service.Dtos;

namespace TestSystem.Service.Tests
{
    [TestClass]
    public class TestSystem
    {
        protected const string databaseName = "TestDb";

        private class IntergrationTestingUnit
        {
            public TestSystemDbContext Context { get; set; }
            public TestSystemService Service { get; set; }

            public IntergrationTestingUnit()
            {
                this.Context = new TestSystemDbContext(databaseName);
                this.Service = new TestSystemService(this.Context);
            }
        }

        //[TestCleanup]
        //public void TestCleanup()
        //{
        //    if (Database.Exists(databaseName))
        //    {
        //        Database.Delete(databaseName);
        //    }
        //}

        [TestMethod]
        public async Task CreateUser_ReterunsNewUserIdAsync()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();
            
            var userId = await unit.Service.CreateUserAsync();

            int userCount = unit.Context.TestSystemUsers.Count(t => t.Id == userId);
            
            Assert.AreEqual(userCount, 1);
            Assert.IsFalse(String.IsNullOrEmpty(userId));
        }

        [TestMethod]
        public async Task CreateTest_ReturnsNewTestIdAsync()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();

            TestDto test = new TestDto()
            {
                Name = "some test"
            };

            int testId = await unit.Service.CreateTestAsync(test);

            int testCount = unit.Context.Tests.Count(t => t.Id == testId);

            Assert.AreEqual(testCount, 1);
        }

        [TestMethod]
        public async Task CreateTestWithQuestions_ReturnNewTestIdAsync()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();

            TestDto test = new TestDto()
            {
                Name = "another test",
                Questions = new List<QuestionDto>()
                {
                    new QuestionDto()
                    {
                        Content = "Which city is capital of United Kingdom",
                        RightAnswers = 1,
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Closed,
                        Options = new List<QuestionAnswerOptionDto>()
                        {
                            new QuestionAnswerOptionDto()
                            {
                                Content = "London",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Cardiff",
                                IsCorrect = false
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Manchester",
                                IsCorrect = false
                            }
                        }
                    },
                    new QuestionDto()
                    {
                        Content = "Why you read books",
                        Options = null,
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Open
                    },
                    new QuestionDto()
                    {
                        Content = "which disciplines you have this semester",
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Closed,
                        Options = new List<QuestionAnswerOptionDto>()
                        {
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Entity framework",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Business modeling",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Some kind of math",
                                IsCorrect = false
                            }
                        }
                    }
                }
            };

            int testId = await unit.Service.CreateTestAsync(test);
            int testCount = await unit.Context.Tests.CountAsync(t => t.Id == testId);
            int questionsCount = await unit.Context.Questions.CountAsync(q => q.TestId == testId);
            int questionsOptionsCount = await unit.Context.Questions
                .Include(q => q.QuestionAnswerOptions)
                .Where(q => q.TestId == testId)
                .SelectMany(q => q.QuestionAnswerOptions)
                .CountAsync();


            Assert.AreEqual(1, testCount);
            Assert.AreEqual(3, questionsCount);
            Assert.AreEqual(6, questionsOptionsCount);
        }

        [TestMethod]
        public async Task GetTests_ReturnsAllTests()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();

            var tests = await unit.Service.GetTestsAsync();
            var contextTestCount = await unit.Context.Tests.CountAsync();

            Assert.AreEqual(contextTestCount, tests.Count());
        }

        [TestMethod]
        public async Task AddUserTest_ReturnsNewUserTestId()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();

            string userId = await unit.Service.CreateUserAsync();
            int testId = await unit.Service.CreateTestAsync(new TestDto()
            {
                Name = "another test",
                Questions = new List<QuestionDto>()
                {
                    new QuestionDto()
                    {
                        Content = "Which city is capital of United Kingdom",
                        RightAnswers = 1,
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Closed,
                        Options = new List<QuestionAnswerOptionDto>()
                        {
                            new QuestionAnswerOptionDto()
                            {
                                Content = "London",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Cardiff",
                                IsCorrect = false
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Manchester",
                                IsCorrect = false
                            }
                        }
                    },
                    new QuestionDto()
                    {
                        Content = "Why you read books",
                        Options = null,
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Open
                    },
                    new QuestionDto()
                    {
                        Content = "which disciplines you have this semester",
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Closed,
                        RightAnswers = 2,
                        Options = new List<QuestionAnswerOptionDto>()
                        {
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Entity framework",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Business modeling",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Some kind of math",
                                IsCorrect = false
                            }
                        }
                    }
                }
            });

            int userTestId = await unit.Service.AddUserTestAsync(new UserTestDto()
            {
                TestId = testId,
                UserId = userId
            });

            int userTestsCount = await unit.Context.UserTests.CountAsync(ut => ut.Id == userTestId);
            int questionsCount = await unit.Context.Questions.CountAsync(q => q.TestId == testId);
            int answersCount = await unit.Context.Answers.CountAsync(a => a.UserTestId == userTestId);

            Assert.AreEqual(1, userTestsCount);
            Assert.AreEqual(3, questionsCount);
            Assert.AreEqual(3, answersCount);
        }

        [TestMethod]
        public async Task GetUserTest_ReturnsUserTestWithNestedCollections()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();

            string userId = await unit.Service.CreateUserAsync();
            int testId = await unit.Service.CreateTestAsync(new TestDto()
            {
                Name = "another test",
                Questions = new List<QuestionDto>()
                {
                    new QuestionDto()
                    {
                        Content = "Which city is capital of United Kingdom",
                        RightAnswers = 1,
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Closed,
                        Options = new List<QuestionAnswerOptionDto>()
                        {
                            new QuestionAnswerOptionDto()
                            {
                                Content = "London",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Cardiff",
                                IsCorrect = false
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Manchester",
                                IsCorrect = false
                            }
                        }
                    },
                    new QuestionDto()
                    {
                        Content = "Why you read books",
                        Options = null,
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Open
                    },
                    new QuestionDto()
                    {
                        Content = "which disciplines you have this semester",
                        QuestionTypeId = DbAccess.Entities.QuestionTypeEnum.Closed,
                        RightAnswers = 2,
                        Options = new List<QuestionAnswerOptionDto>()
                        {
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Entity framework",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Business modeling",
                                IsCorrect = true
                            },
                            new QuestionAnswerOptionDto()
                            {
                                Content = "Some kind of math",
                                IsCorrect = false
                            }
                        }
                    }
                }
            });

            int userTestId = await unit.Service.AddUserTestAsync(new UserTestDto()
            {
                TestId = testId,
                UserId = userId
            });

            UserTestDto userTest = await unit.Service.GetUserTestAsync(userTestId);

            Assert.AreEqual(testId, userTest.Test.Id);
            Assert.AreEqual(3, userTest.Test.Questions.Count);
            Assert.AreEqual(3, userTest.Test.Questions.SelectMany(q => q.Answers).Count());
            Assert.AreEqual(6, userTest.Test.Questions.SelectMany(q => q.Options).Count());
        }

        [TestMethod]
        public async Task GetTestsByUserId_ReturnsAllUserTests()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();

            string userId = await unit.Service.CreateUserAsync();
            int test1 = await unit.Service.CreateTestAsync(new TestDto()
            {
                Name = "test No 1"
            });

            int test2 = await unit.Service.CreateTestAsync(new TestDto()
            {
                Name = "test no 2"
            });

            await unit.Service.AddUserTestAsync(new UserTestDto()
            {
                UserId = userId,
                TestId = test1
            });

            await unit.Service.AddUserTestAsync(new UserTestDto()
            {
                UserId = userId,
                TestId = test2
            });

            IEnumerable<TestDto> tests = await unit.Service.GetTestsAsync(userId);

            Assert.AreEqual(2, tests.Count());
        }

        [TestMethod]
        public async Task AddQuestion_AddsNewQuestionToTest()
        {
            IntergrationTestingUnit unit = new IntergrationTestingUnit();

            var testId = await unit.Service.CreateTestAsync(new TestDto()
            {
                Name = "Let's try this",
                Questions = new List<QuestionDto>()
                {
                    new QuestionDto()
                    {
                        Content = "What is your name?",
                        QuestionTypeId = QuestionTypeEnum.Open
                    }
                }
            });
            

            await unit.Service.AddQuestionAsync(testId, new QuestionDto()
            {
                Content = "Does Bulgaria have border with Romania?",
                Options = new List<QuestionAnswerOptionDto>()
                {
                    new QuestionAnswerOptionDto()
                    {
                        Content = "Yes"
                    },
                    new QuestionAnswerOptionDto()
                    {
                        Content = "No"
                    }
                }
            });

            int questionsCount = await unit.Context.Questions
                .CountAsync(q => q.TestId == testId);


            Assert.AreEqual(questionsCount, 2);
        }
    }
}
