using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Reads current RAM utilization and installed capacity/specification (e.g. "16 GB DDR4 3200 MT/s").
/// </summary>
public interface IMemoryUsageProvider
{
    Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken);
}
