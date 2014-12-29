using System.ComponentModel.DataAnnotations;

namespace FileExchange.Areas.Admin.Models
{
    public class GlobalSettingViewModel
    {
        public int SettingId { get; set; }

        [Required()]
        [MaxLength(50)]
        [Display(Name = "Setting name:")]
        public string SettingName { get; set; }

        [MaxLength(4000)]
        [Display(Name = "Setting value:")]
        public string SettingValue { get; set; }

        [Display(Name = "Description:")]
        public string Description { get; set; }

        [MaxLength(256)]
        public string VaidationRegexMask { get; set; }
    }
}