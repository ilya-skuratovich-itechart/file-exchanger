using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.BusinessObjects
{
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }

        [MaxLength(255)]
        public string Header { get; set; }

        public string Text { get; set; }

        [MaxLength(56)]
        public string UniqImageName { get; set; }

        [MaxLength(128)]
        public string OrigImageName{ get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}