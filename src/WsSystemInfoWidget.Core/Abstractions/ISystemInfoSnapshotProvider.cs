using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Composes the individual per-resource providers into one <see cref="SystemInfoSnapshot"/>.
/// </summary>
public interface ISystemInfoSnapshotProvider
{
    Task<SystemInfoSnapshot> GetSnapshotAsync(CancellationToken cancellationToken);
}
