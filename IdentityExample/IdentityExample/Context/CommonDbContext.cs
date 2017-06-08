using IdentityExample.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using TestSystem.DbAccess.Context;
using TestSystem.DbAccess.Entities;

namespace IdentityExample.Context
{
    internal class CommonDbContextIntializer : DropCreateDatabaseAlways<CommonDbContext>
    {
        protected override void Seed(CommonDbContext context)
        {
            Config.IntializerConfig(context);
        }
    }

    /// <summary>
    /// This context will be used for creating database
    /// </summary>
    class CommonDbContext : IdentityDbContext<User>, ITestSystemDbContext, IAppDbContext
    {
        public CommonDbContext() : this("ConnectionString") { }

        public CommonDbContext(string connectionString) : base(connectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new CommonDbContextIntializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
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