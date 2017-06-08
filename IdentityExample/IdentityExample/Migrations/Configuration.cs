namespace IdentityExample.Migrations
{
    using IdentityExample.Context;
    using IdentityExample.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;
    using TestSystem.DbAccess.Context;
    using TestSystem.DbAccess.Entities;
    using TestSystem.DbAccess.Helpers;
    using TestSystem.Service;

    internal sealed class Configuration : DbMigrationsConfiguration<IdentityExample.Context.CommonDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IdentityExample.Context.CommonDbContext context)
        {
            var manager = new UserManager<User>(new UserStore<User>(context));
            var service = new TestSystemService(context);

            var user = new User()
            {
                UserName = "user",
                Email = "some@example.com",
                EmailConfirmed = false,
                TestSystemUserId = service.CreateUserAsync().Result,
            };

            manager.Create(user, "password");

            context.TestStatuses.SeedEnumValues<TestStatus, TestStatusEnum>(e => new TestStatus(e));
            context.QuestionTypes.SeedEnumValues<QuestionType, QuestionTypeEnum>(e => new QuestionType(e));
        }
    }
}
