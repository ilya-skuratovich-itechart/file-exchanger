using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileExchange.Models
{
    public class CreateCommentViewModel
    {
        [Required]
        public int FileId { get; set; }

        [Required]
        public string Comment { get; set; }
    }

}