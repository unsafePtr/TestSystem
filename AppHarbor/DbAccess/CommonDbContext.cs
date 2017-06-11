using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbAccess.Entities;
using TestSystem.DbAccess.Context;
using TestSystem.DbAccess.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DbAccess
{
    /// <summary>
    /// will be used just fro db creation
    /// </summary>
    public class CommonDbContext : DbContext, IAppHarborDbContext, ITestSystemDbContext
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
            modelBuilder.ModelCreatingConfig();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m => m
                    .ToTable("UserRoles")
                    .MapLeftKey("UserId")
                    .MapRightKey("RoleId")
                );
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }
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
