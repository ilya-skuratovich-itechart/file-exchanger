using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Data;
using FileExchange.Core.EntityConfiguration;

namespace FileExchange.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<FileExchange.Core.FileExchangeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

       

        protected override void Seed(FileExchange.Core.FileExchangeDbContext context)
        {

            context.UserRoles.Add(
                new UserRoles()
                {
                    RoleId = (int)UserRoleTypes.Admin,
                    RoleName =UserRoleNames.Admin
                });

            context.UserRoles.Add(
                new UserRoles()
                {
                    RoleId = (int)UserRoleTypes.Moderator,
                    RoleName = UserRoleNames.Moderator
                });

            context.FielCategories.Add(new FileCategories()
            {
                CategoryName = "бухгалтерия"
            });


            context.FielCategories.Add(new FileCategories()
            {
                CategoryName = "менеджмент"
            });

            context.FielCategories.Add(new FileCategories()
            {
                CategoryName = "техподдержка"
            });
            context.SaveChanges();
        }
    }
}
