using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.BusinessObjects
{
    public class FileNotificationSubscribers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Subscriberid { get; set; }

        [ForeignKey("File")]
        [Required]
        public int FileId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [Required]
        public ExchangeFile File { get; set; }

        public UserProfile User { get; set; }


    }
}