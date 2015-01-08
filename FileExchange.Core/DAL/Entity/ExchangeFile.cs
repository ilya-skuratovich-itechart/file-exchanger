using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.DAL.Entity
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
        [Required]
        public string UniqFileName { get; set; }

        [MaxLength(128)]
        [Required]
        public string OrigFileName { get; set; }

        [MaxLength(256)]
        [Required]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Tags { get; set; }

        [Required]
        public bool AllowViewAnonymousUsers { get; set; }

        [Required]
        public bool AccessDenied { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual FileCategories FileCategory { get; set; }

        public virtual ICollection<FileComments> FileComments { get; set; }

        public virtual ICollection<FileNotificationSubscribers> NotificationSubscribers { get; set; }


    }
}