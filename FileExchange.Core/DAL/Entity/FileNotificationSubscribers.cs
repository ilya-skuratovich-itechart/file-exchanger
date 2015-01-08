using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.DAL.Entity
{
    public class FileNotificationSubscribers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Subscriberid { get; set; }

        [ForeignKey("File")]
        [Required]
        public int FileId { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        public UserProfile User { get; set; }

        public ExchangeFile File { get; set; }


    }
}