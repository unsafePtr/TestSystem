namespace IdnetityExample.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TestSystem.DbAccess.Context;
    using Microsoft.AspNet.Identity.EntityFramework;
    using IdnetityExample.DbAccess.Entities;
    using TestSystem.Service.Dtos;
    using System.Collections.Generic;
    using IdnetityExample.DbAccess;
    using TestSystem.Service;
    using TestSystem.DbAccess.Entities;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<IdnetityExample.DbAccess.CommonDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CommonDbContext context)
        {
            context.IntializerConfig();

            foreach (var item in Enum.GetValues(typeof(AppRoles)).Cast<AppRoles>())
            {
                context.Roles.Add(new IdentityRole(item.ToString()));
            }
        }
    }
}
