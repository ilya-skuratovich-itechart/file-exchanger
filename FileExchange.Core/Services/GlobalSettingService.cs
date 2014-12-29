using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.DTO;
using FileExchange.Core.Repositories;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class GlobalSettingService : IGlobalSettingService
    {
        private IGenericRepository<GlobalSetting> _globalSettingsRepository;

        public GlobalSettingService(IUnitOfWork unitOfWork)
        {
            _globalSettingsRepository = BootStrap.Container.Resolve<IGenericRepository<GlobalSetting>>();
            _globalSettingsRepository.InitializeDbContext(unitOfWork.DbContext);
        }
        public IEnumerable<GlobalSetting> GetAll()
        {
            return _globalSettingsRepository.GetAll();
        }

        public void Update(int settingId,string settingValue)
        {
            var globalSetting = GetById(settingId);
            if (globalSetting==null)
                throw  new ArgumentException(string.Format("setting not exists. SettingId:{0}",settingId));
            globalSetting.SettingValue = settingValue;
            _globalSettingsRepository.Update(globalSetting);
        }

        public List<GlobalSetting> GetPaged(int pageNumber, int pageLength, out int totalRecords)
        {
            return _globalSettingsRepository.GetPaged(s => s.SettingId, pageNumber, pageLength, out totalRecords)
                .ToList();
        }

        public GlobalSetting GetById(int settingId)
        {
            return _globalSettingsRepository.FindBy(s => s.SettingId == settingId)
                .SingleOrDefault();
        }

        public void Update(IEnumerable<GlobalSetting> globalSettings)
        {
            if (globalSettings != null)
                foreach (var globalSetting in globalSettings)
                {
                    _globalSettingsRepository.Update(globalSetting);
                }
        }
    }
}