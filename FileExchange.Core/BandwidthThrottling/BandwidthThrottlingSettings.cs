using System;
using FileExchange.Core.BusinessObjects;
using FileExchange.Core.Services;

namespace FileExchange.Core.BandwidthThrottling
{
    public class BandwidthThrottlingSettings : IBandwidthThrottlingSettings
    {
        private IGlobalSettingService _globalSettingService { get; set; }
        private IUserProfileService _userProfileService { get; set; }

        public BandwidthThrottlingSettings(IGlobalSettingService globalSettingService,
            IUserProfileService userProfileService)
        {
            _globalSettingService = globalSettingService;
            _userProfileService = userProfileService;
        }

        public int GetMaxDownloadSpeedKbps(int? userId)
        {
            int maxAllowSpeedKbps = 0;
            GlobalSetting globalMaxDownloadSpeedKbps =
                _globalSettingService.GetById((int) GlobalSettingTypes.MaxDownloadSpeedKbps);
            int userMaxDonwloadSpeedKbps = 0;
            if (userId.HasValue)
            {
                UserProfile userProfile = _userProfileService.GetUserById(userId.Value);
                if (userProfile == null)
                    throw new Exception(string.Format("User not exists. UserId={0}", userId.Value));
                userMaxDonwloadSpeedKbps = userProfile.MaxDonwloadSpeedKbps;
            }
            if (globalMaxDownloadSpeedKbps == null)
                throw new Exception(string.Format("Global setting by id={0} not exists.",
                    (int) GlobalSettingTypes.MaxDownloadSpeedKbps));
            if (userId.HasValue)
            {
                if (int.Parse(globalMaxDownloadSpeedKbps.SettingValue) == 0)
                    maxAllowSpeedKbps = userMaxDonwloadSpeedKbps;
                else
                    maxAllowSpeedKbps = int.Parse(globalMaxDownloadSpeedKbps.SettingValue);
            }
            else
            {
                maxAllowSpeedKbps = int.Parse(globalMaxDownloadSpeedKbps.SettingValue);
            }
            return maxAllowSpeedKbps;
        }
    }
}