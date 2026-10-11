using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Reads upload/download throughput for the interface selected by the system's Internet route.
/// </summary>
public interface INetworkUsageProvider
{
    Task<NetworkUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken);
}
