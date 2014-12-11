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
        public int FileId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public ExchangeFile File { get; set; }

        public UserProfile User { get; set; }

        
    }
}