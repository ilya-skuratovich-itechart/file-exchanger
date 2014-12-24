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
            AutomaticMigrationDataLossAllowed = true;
        }

       

        protected override void Seed(FileExchange.Core.FileExchangeDbContext context)
        {
            

         
            context.UserRoles.AddOrUpdate(
                new UserRoles()
                {
                    RoleId = (int)UserRoleTypes.Admin,
                    RoleName =UserRoleNames.Admin
                });

            context.UserRoles.AddOrUpdate(
                new UserRoles()
                {
                    RoleId = (int)UserRoleTypes.Moderator,
                    RoleName = UserRoleNames.Moderator
                });

            context.UserRoles.AddOrUpdate(
               new UserRoles()
               {
                   RoleId = (int)UserRoleTypes.ActiveUser,
                   RoleName = UserRoleNames.ActiveUser
               });


            context.FielCategories.AddOrUpdate(new FileCategories()
            {
                CategoryId = 1,
                CategoryName = "бухгалтерия"
            });


            context.FielCategories.AddOrUpdate(new FileCategories()
            {
                CategoryId = 2,
                CategoryName = "менеджмент"
            });

            context.FielCategories.AddOrUpdate(new FileCategories()
            {
                CategoryId = 3,
                CategoryName = "техподдержка"
            });

            context.FielCategories.AddOrUpdate(new FileCategories()
            {
                CategoryId = 4,
                CategoryName = "tEST"
            });

            context.GlobalSettings.Add(new GlobalSetting()
            {
                SettingId = (int) GlobalSettingTypes.MaxDownloadSpeedKbps,
                SettingName = "MaxDownloadSpeedKbps",
                SettingValue = "0",
                VaidationRegexMask = "^[0-9]*$"
            });

            context.SaveChanges();
        }
    }
}
