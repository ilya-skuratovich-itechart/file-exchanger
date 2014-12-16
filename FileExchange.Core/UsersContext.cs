using System.Data.Entity;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Core
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("FileExchangeDbConnectionString")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}