using System.Collections.Generic;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.DTO;

namespace FileExchange.Core.Services
{
    public interface IGlobalSettingService
    {
        IEnumerable<GlobalSetting> GetAll();

        List<GlobalSetting> GetPaged(int pageNumber, int pageLength, out int totalRecords);

        void Update(int settingId, string settingValue, string vaidationRegexMask, string description);

        GlobalSetting GetById(int settingId);
    }
}