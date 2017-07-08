using System;
using System.Data.Entity;
using System.Threading.Tasks;
using TestSystem.DbAccess.Entities;

namespace TestSystem.DbAccess.Context
{
    public interface ITestSystemDbContext : IDisposable
    {
        DbSet<TestSystemUser> TestSystemUsers { get; set; }
        DbSet<TestStatus> TestStatuses { get; set; }
        DbSet<Test> Tests { get; set; }
        DbSet<UserTest> UserTests { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<QuestionType> QuestionTypes { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<QuestionAnswerOption> QuestionAnswerOptions { get; set; }

        Task<int> SaveChangesAsync();
    }
}
