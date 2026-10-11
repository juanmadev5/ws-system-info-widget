using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Reads current dedicated video memory utilization. Only invoked when the
/// active GPU is <see cref="Enums.GpuType.Dedicated"/>.
/// </summary>
public interface IVramUsageProvider
{
    Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken);
}
