using IdnetityExample.DbAccess;
using IdnetityExample.DbAccess.Entities;
using IdnetityExample.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestSystem.DbAccess.Entities;
using TestSystem.Service;
using TestSystem.Service.Dtos;

namespace IdnetityExample.Controllers
{
    public class TestController : BaseController
    {
        private readonly ITestSystemService service;
        private readonly IIdentityExampleDbContext context;

        public TestController(ITestSystemService service, IIdentityExampleDbContext context)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> CreateSomeTest()
        {
            TestDto test = new TestDto()
            {
                Name = "another test",
                Questions = new List<QuestionDto>()
                {
                    new QuestionDto()
                    {
                        Content = "Which city is capital of United Kingdom",
                        RightAnswers = 1,
                        QuestionTypeId = QuestionTypeEnum.Closed,
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
                        QuestionTypeId = QuestionTypeEnum.Open
                    },
                    new QuestionDto()
                    {
                        Content = "which disciplines you have this semester",
                        QuestionTypeId = QuestionTypeEnum.Closed,
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

            try
            {
                await service.CreateTestAsync(test);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
            }

            return View();
        }

        
        public async Task<ActionResult> GetTests()
        {
            var tests = await service.GetTestsAsync();

            return View(tests);
        }

        public async Task<ActionResult> MyTests()
        {
            string currentUserId = User.Identity.GetUserId();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);
            var tests = await service.GetUserTestsAsync(user.TestSystemUserId);
            
            return View(tests);
        }

        public async Task<ActionResult> AssignToTest(int testId)
        {
            string currentUserId = User.Identity.GetUserId();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);

            int userTestId = await service.AddUserTestAsync(new UserTestDto()
            {
                TestId = testId,
                UserId = user.TestSystemUserId
            });

            return RedirectToAction(nameof(UserTest), new { userTestId = userTestId });
        }

        public async Task<ActionResult> UserTest(int userTestId)
        {
            var userTest = await service.GetUserTestAsync(userTestId);

            return View(userTest);
        }

        public async Task<ActionResult> StartTest(int userTestId)
        {
            await service.StartUserTestAsync(userTestId);

            return RedirectToAction(nameof(UserTest), new { userTestId = userTestId });
        }

        //[HttpPost]
        //public async Task<ActionResult> SubmitQuestions(AnswerViewModel answers)
        //{
        //    return null;
        //}
    }
}