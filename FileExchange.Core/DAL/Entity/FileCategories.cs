using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.DAL.Entity
{
    public class FileCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [MaxLength(255)]
        [Required]
        public string CategoryName { get; set; }
    }
}