using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.DAL.Entity
{
    [Table("webpages_UsersInRoles")]
    public class UserInRoles
    {
        [Key, ForeignKey("Role"), Column(Order=0)]
        public int RoleId { get; set; }
        [Key,  Column(Order = 1)]
        public int UserId { get; set; }
        public virtual UserRoles Role { get; set; }
    }
}