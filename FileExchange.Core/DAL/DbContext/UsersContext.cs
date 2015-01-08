using System.Data.Entity;
using FileExchange.Core.DAL.Entity;

namespace FileExchange.Core.DAL.DbContext
{
    public class UsersContext : System.Data.Entity.DbContext
    {
        public UsersContext()
            : base("FileExchangeDbConnectionString")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}