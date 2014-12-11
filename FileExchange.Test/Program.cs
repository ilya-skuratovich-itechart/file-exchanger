using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExchange.Core;
using FileExchange.Core.BusinessObjects;

namespace FileExchange.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

          
            using (var db = new FileExchangeDbContext())
            {
                db.UserRoles.Add(new UserRoles() {RoleId = 1,RoleName = "test"});
                db.SaveChanges();
            }
            }
            catch (ModelValidationException exc)
            {
                foreach (var excMes in exc.Data)
                {
                    Console.Write(excMes);
                }
            }
            Console.ReadKey();
        }
    }
}
