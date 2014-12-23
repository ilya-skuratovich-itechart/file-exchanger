﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.BusinessObjects
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        public string UserName { get; set; }
        
        public string UserEmail { get; set; }

        public int MaxDonwloadSpeedKbps { get;set; }

        public int FileMaxSizeKbps { get; set; }
      
        public virtual ICollection<UserInRoles> Roles { get; set; }
    }
}