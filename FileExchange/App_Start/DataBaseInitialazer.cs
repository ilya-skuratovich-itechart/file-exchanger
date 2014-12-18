using System;
using System.Data.Entity.Infrastructure;
using WebMatrix.WebData;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using FileExchange.Core;
using FileExchange.Models;

namespace FileExchange
{
    public static class SimpleMemberShipInitialazer
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public static void Initializate()
        {

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FileExchangeDbContext, FileExchange.Core.Migrations.Configuration>("FileExchangeDbConnectionString"));
          
            WebSecurity.InitializeDatabaseConnection("FileExchangeDbConnectionString", "UserProfile", "UserId",
                  "UserName", autoCreateTables: false);
        }

    }

    internal class SimpleMembershipInitializer
    {
        public SimpleMembershipInitializer()
        {

            Database.SetInitializer<UsersContext>(null);
            try
            {
                using (var context = new UsersContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Create the SimpleMembership database without Entity Framework migration schema
                        ((IObjectContextAdapter) context).ObjectContext.CreateDatabase();
                    }
                }

              
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588",
                    ex);
            }
        }
    }
}