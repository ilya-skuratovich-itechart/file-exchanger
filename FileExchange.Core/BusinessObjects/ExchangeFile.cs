using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.BusinessObjects
{
    public class ExchangeFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("FileCategory")]
        public int FileCategoryId { get; set; }

        [MaxLength(56)]
        public string UniqFileName { get; set; }

        [MaxLength(128)]
        public string OrigFileName { get; set; }

        [MaxLength(1024)]
        public string Tags { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual FileCategories FileCategory { get; set; }

        public virtual ICollection<FileComments> FileComments { get; set; }

        public virtual ICollection<FileNotificationSubscribers> NotificationSubscribers { get; set; } 


    }
}