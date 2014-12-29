using FileExchange.Core.Services;

namespace FileExchange.Core.BandwidthThrottling
{
    public interface IBandwidthThrottlingSettings
    {
        int GetMaxDownloadSpeedKbps(int? userId);
    }
}