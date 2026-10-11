using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Reads current GPU utilization, the adapter's display name, and whether it is
/// integrated or dedicated.
/// </summary>
public interface IGpuUsageProvider
{
    Task<GpuUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken);
}
