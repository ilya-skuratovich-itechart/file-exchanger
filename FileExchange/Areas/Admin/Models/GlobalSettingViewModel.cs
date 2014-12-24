using System.ComponentModel.DataAnnotations;

namespace FileExchange.Areas.Admin.Models
{
    public class GlobalSettingViewModel
    {
        public int SettingId { get; set; }
     
        [Required()]
        [MaxLength(50)]
        public string SettingName { get; set; }

        [MaxLength(4000)]
        public string SettingValue { get; set; }

        public string Description { get; set; }

        [MaxLength(256)]
        public string VaidationRegexMask { get; set; }
    }
}