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
        [Required]
        public string Header { get; set; }

        [Required]
        public string Text { get; set; }

        [MaxLength(56)]
        [Required]
        public string UniqImageName { get; set; }

        [MaxLength(128)]
        [Required]
        public string OrigImageName { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}