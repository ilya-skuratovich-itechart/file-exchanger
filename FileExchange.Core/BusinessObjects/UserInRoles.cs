using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.BusinessObjects
{
    [Table("webpages_UsersInRoles")]
    public class UserInRoles
    {
        [Key, ForeignKey("Role"), Column(Order = 0)]
        public int RoleId { get; set; }
        [Key, ForeignKey("User"), Column(Order = 1)]
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }

        public virtual UserRoles Role { get; set; }
    }
}