using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExchange.Core.BusinessObjects
{
    [Table("webpages_Roles")]
    public class UserRoles
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        [NotMapped]
        public UserRoleTypes RoleType
        {
            get { return (UserRoleTypes) RoleId; }
        }

        public virtual ICollection<UserProfile> Users { get; set; }
    }

    public enum UserRoleTypes
    {
        Admin = 1,
        Moderator = 2
    }
}
