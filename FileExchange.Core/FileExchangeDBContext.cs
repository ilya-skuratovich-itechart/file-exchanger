using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core
{
    public class FileExchangeDbContext: DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserInRoles> UserInRoles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<ExchangeFile> ExchangeFiles { get; set; }

        public DbSet<FileCategories> FielCategories { get; set; }

        public DbSet<FileComments> FileComments { get; set; }

        public DbSet<FileNotificationSubscribers> FileNotificationSubscribers { get; set; }

        public DbSet<News> News { get; set; }

        public FileExchangeDbContext()
            : base("FileExchangeDbConnectionString") 
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
           
        }
    }
}