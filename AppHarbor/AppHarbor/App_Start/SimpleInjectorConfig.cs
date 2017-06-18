using System.Reflection;
using SimpleInjector;
using TestSystem.Service;
using SimpleInjector.Integration.Web;
using System.Web.Mvc;
using SimpleInjector.Integration.Web.Mvc;
using AppHarbor.DbAccess;
using TestSystem.DbAccess.Context;
using Microsoft.AspNet.Identity;
using AppHarbor.DbAccess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AppHarbor.App_Start
{
    public static class SimpleInjectorConfig
    {
        private static void RegisterDependencies(this Container container)
        {
            // configure context and asp identity
            container.Register<IAppHarborDbContext>(() => new AppHarborDbContext("AppHarborConnection"), Lifestyle.Scoped);
            container.Register<AppHarborDbContext>(() => new AppHarborDbContext("AppHarborConnection"), Lifestyle.Scoped);
            container.Register<ITestSystemDbContext>(() => new TestSystemDbContext("AppHarborConnection"), Lifestyle.Scoped);
            container.Register<IUserStore<User>>(() => new UserStore<User>(container.GetInstance<AppHarborDbContext>()), Lifestyle.Scoped);
            container.Register<UserManager<User>>(Lifestyle.Scoped);

            // configure TestSystemService
            container.Register<ITestSystemService, TestSystemServiceProxy>(Lifestyle.Scoped);
            container.Register<TestSystemService>(Lifestyle.Scoped);
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