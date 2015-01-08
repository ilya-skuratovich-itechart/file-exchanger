using System.Collections.Generic;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.DTO;

namespace FileExchange.Core.Services
{
    public interface IGlobalSettingService
    {
        IEnumerable<GlobalSetting> GetAll();

        List<GlobalSetting> GetPaged(int pageNumber, int pageLength, out int totalRecords);

        void Update(int settingId, string settingValue);

        GlobalSetting GetById(int settingId);
    }
}