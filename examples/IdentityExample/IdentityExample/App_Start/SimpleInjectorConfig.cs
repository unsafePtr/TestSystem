using System.Reflection;
using SimpleInjector;
using TestSystem.Service;
using SimpleInjector.Integration.Web;
using System.Web.Mvc;
using SimpleInjector.Integration.Web.Mvc;
using IdnetityExample.DbAccess;
using TestSystem.DbAccess.Context;
using Microsoft.AspNet.Identity;
using IdnetityExample.DbAccess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdnetityExample.App_Start
{
    public static class SimpleInjectorConfig
    {
        private static void RegisterDependencies(this Container container)
        {
            // configure context and asp identity
            container.Register<IIdentityExampleDbContext>(() => new IdentityExampleDbContext("ConnectionString"), Lifestyle.Scoped);
            container.Register<IdentityExampleDbContext>(() => new IdentityExampleDbContext("ConnectionString"), Lifestyle.Scoped);
            container.Register<ITestSystemDbContext>(() => new TestSystemDbContext("ConnectionString"), Lifestyle.Scoped);
            container.Register<IUserStore<User>>(() => new UserStore<User>(container.GetInstance<IdentityExampleDbContext>()), Lifestyle.Scoped);
            container.Register<UserManager<User>>(Lifestyle.Scoped);

            // using proxy instead of TestSystemService
            container.Register<ITestSystemService, TestSystemServiceProxy>(Lifestyle.Scoped);
        }

        public static void Configure()
        {
            Container container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterDependencies();
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}