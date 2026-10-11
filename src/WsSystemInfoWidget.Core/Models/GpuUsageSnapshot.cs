using WsSystemInfoWidget.Core.Enums;

namespace WsSystemInfoWidget.Core.Models;

/// <summary>
/// A GPU reading, extended with the adapter's <see cref="GpuType"/> since that
/// determines whether a companion VRAM metric is meaningful.
/// </summary>
/// <param name="Usage">The GPU's own usage reading (<see cref="ResourceKind.Gpu"/>).</param>
/// <param name="GpuType">Whether the adapter is integrated or dedicated.</param>
public sealed record GpuUsageSnapshot(ResourceUsageSnapshot Usage, GpuType GpuType);
