using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.EntityConfiguration;

namespace FileExchange.Core.DAL.DbContext
{
    public class FileExchangeDbContext : System.Data.Entity.DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserInRoles> UserInRoles { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<ExchangeFile> ExchangeFiles { get; set; }

        public DbSet<FileCategories> FielCategories { get; set; }

        public DbSet<FileComments> FileComments { get; set; }

        public DbSet<FileNotificationSubscribers> FileNotificationSubscribers { get; set; }

        public DbSet<GlobalSetting> GlobalSettings { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<MemberShip> Membership { get; set; }

        public FileExchangeDbContext()
            : base("FileExchangeDbConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MembershipMapping());
            modelBuilder.Configurations.Add(new RolesMapping());
            modelBuilder.Configurations.Add(new UsersInRolesMapping());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}