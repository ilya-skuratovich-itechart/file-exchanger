using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExchange.Core.DAL.Entity
{
    public class GlobalSetting
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        [Index("uniqconstr_settingname", 1, IsUnique = true)]
        [Required()]
        [MaxLength(50)]
        public string SettingName { get; set; }

        [MaxLength(4000)]
        public string SettingValue { get; set; }

        public string Description { get; set; }

         [MaxLength(256)]
        public string VaidationRegexMask { get; set; }
    }

    public enum GlobalSettingTypes
    {
        MaxDownloadSpeedKbps=1
    }
}