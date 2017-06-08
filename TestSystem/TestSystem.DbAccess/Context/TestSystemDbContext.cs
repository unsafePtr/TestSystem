using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TestSystem.DbAccess.Entities;

namespace TestSystem.DbAccess.Context
{
    public class TestSystemDbContext : DbContext, ITestSystemDbContext
    {
        public TestSystemDbContext(string nameOrConnectionString) 
            : this(nameOrConnectionString, new Intializer())
        {

        }

        public TestSystemDbContext(string nameOrConnectionString, IDatabaseInitializer<TestSystemDbContext> strategy) 
            : base(nameOrConnectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;

            Database.SetInitializer(strategy);
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.ModelCreatingConfig();
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
