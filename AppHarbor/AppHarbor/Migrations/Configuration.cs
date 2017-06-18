namespace AppHarbor.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TestSystem.DbAccess.Context;
    using Microsoft.AspNet.Identity.EntityFramework;
    using AppHarbor.DbAccess.Entities;
    using TestSystem.Service.Dtos;
    using System.Collections.Generic;
    using AppHarbor.DbAccess;
    using TestSystem.Service;
    using TestSystem.DbAccess.Entities;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<AppHarbor.DbAccess.CommonDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CommonDbContext context)
        {
            context.IntializerConfig();

            foreach (var item in Enum.GetValues(typeof(AppHarborRoles)).Cast<AppHarborRoles>())
            {
                context.Roles.Add(new IdentityRole(item.ToString()));
            }
        }
    }
}
