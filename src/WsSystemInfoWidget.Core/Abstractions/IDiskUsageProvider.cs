using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Reads current system-drive read/write throughput, its hardware name, and
/// its capacity (used/total).
/// </summary>
public interface IDiskUsageProvider
{
    Task<DiskUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken);
}
