using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Reads current CPU utilization and the processor's display name.
/// </summary>
public interface ICpuUsageProvider
{
    Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken);
}
