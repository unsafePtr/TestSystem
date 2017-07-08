using System.Data.Entity;
using TestSystem.DbAccess.Entities;
using TestSystem.DbAccess.Helpers;

namespace TestSystem.DbAccess.Context
{
    public static class Config
    {
        public static void IntializerConfig(this ITestSystemDbContext context)
        {
            context.TestStatuses.SeedEnumValues<TestStatus, TestStatusEnum>(e => new TestStatus(e));
            context.QuestionTypes.SeedEnumValues<QuestionType, QuestionTypeEnum>(e => new QuestionType(e));
        }

        public static void ModelCreatingConfig(this DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTest>().HasRequired(ut => ut.User).WithMany(u => u.UserTests);
            modelBuilder.Entity<TestStatus>().Property(t => t.Name).IsRequired();
            
            modelBuilder.Entity<Test>().Property(t => t.Name).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Test>().Property(t => t.Description).HasMaxLength(600).IsOptional();
            modelBuilder.Entity<Test>().HasRequired(t => t.Status);
            modelBuilder.Entity<Test>().HasMany(t => t.Questions).WithRequired(t => t.Test);

            modelBuilder.Entity<Question>().Property(q => q.Content).HasMaxLength(1000).IsRequired();
            modelBuilder.Entity<Question>().HasRequired(q => q.QuestionType);
            modelBuilder.Entity<Question>().Property(q => q.TestId).IsRequired();

            modelBuilder.Entity<QuestionType>().Property(t => t.Name).IsRequired();

            modelBuilder.Entity<Answer>().Property(a => a.Content).HasMaxLength(1000).IsRequired();
            modelBuilder.Entity<Answer>().Property(a => a.QuestionId).IsRequired();
            modelBuilder.Entity<Answer>().Property(a => a.UserTestId).IsRequired();
            modelBuilder.Entity<Answer>().HasRequired(a => a.UserTest).WithMany(t => t.Answers);
            modelBuilder.Entity<Answer>().HasRequired(a => a.Question).WithMany(q => q.Answers);

            modelBuilder.Entity<QuestionAnswerOption>().HasRequired(q => q.Question).WithMany(q => q.QuestionAnswerOptions);
            modelBuilder.Entity<QuestionAnswerOption>().Property(a => a.Content).HasMaxLength(500);
        }
    }
}
