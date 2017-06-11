namespace AppHarbor.Migrations
{
    using DbAccess;
    using DbAccess.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TestSystem.DbAccess.Entities;
    using TestSystem.DbAccess.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<CommonDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CommonDbContext context)
        {
            context.Roles.Add(new Role() { Name = "Admin" });
            context.Roles.Add(new Role() { Name = "Lector" });
            context.Roles.Add(new Role() { Name = "Student" });
            
            context.TestStatuses.SeedEnumValues<TestStatus, TestStatusEnum>(e => new TestStatus(e));
            context.QuestionTypes.SeedEnumValues<QuestionType, QuestionTypeEnum>(e => new QuestionType(e));
        }
    }
}
