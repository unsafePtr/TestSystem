using IdentityExample.Context;
using IdentityExample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestSystem.DbAccess.Context;
using TestSystem.Service;

namespace IdentityExample.App_Start
{
    public class SimpleInjectorConfig
    {
        public static void Configure(Container container)
        {
            container.Register<IAppDbContext>(() => new AppDbContext(),Lifestyle.Scoped);
            container.Register<AppDbContext>(() => new AppDbContext(), Lifestyle.Scoped);
            container.Register<ITestSystemDbContext>(() => new TestSystemDbContext("ConnectionString"), Lifestyle.Scoped);
            container.Register<IUserStore<User>>(() => new UserStore<User>(container.GetInstance<AppDbContext>()), Lifestyle.Scoped);
            container.Register<UserManager<User>>(Lifestyle.Scoped);
            container.Register<ITestSystemService, TestSystemServiceProxy>(Lifestyle.Scoped);
            container.Register<TestSystemService>(Lifestyle.Scoped);
        }
    }
}