using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.BusinessObjects
{
    public class FileComments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [ForeignKey("File")]
        public int FileId { get; set; }

        [MaxLength(1024)]
        public string Comment { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ExchangeFile File { get; set; }
    }
}