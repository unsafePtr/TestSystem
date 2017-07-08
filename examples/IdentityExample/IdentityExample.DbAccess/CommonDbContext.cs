using System.Data.Entity;
using TestSystem.DbAccess.Context;
using TestSystem.DbAccess.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;
using IdnetityExample.DbAccess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdnetityExample.DbAccess
{
    /// <summary>
    /// will be used just fro db creation
    /// </summary>
    public class CommonDbContext : IdentityDbContext<User>, IIdentityExampleDbContext, ITestSystemDbContext
    {
        public CommonDbContext() : this("ConnectionString") { }

        public CommonDbContext(string connectionString) : base(connectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<TestSystemUser> TestSystemUsers { get; set; }
        public DbSet<TestStatus> TestStatuses { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionAnswerOption> QuestionAnswerOptions { get; set; }
    }
}
