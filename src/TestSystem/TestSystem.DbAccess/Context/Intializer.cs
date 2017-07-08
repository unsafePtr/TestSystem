using System.Data.Entity;

namespace TestSystem.DbAccess.Context
{
    public class Intializer : CreateDatabaseIfNotExists<TestSystemDbContext>
    {
        protected override void Seed(TestSystemDbContext context)
        {
            context.IntializerConfig();
        }
    }
}
