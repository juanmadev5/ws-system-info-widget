namespace WsSystemInfoWidget.Core.Models;

/// <summary>
/// The full set of readings available at one polling tick. This always carries
/// every resource the system exposes; deciding which of these to render for a
/// given <see cref="Enums.WidgetSize"/> is a presentation concern, not a data concern.
/// </summary>
/// <param name="Cpu">CPU usage.</param>
/// <param name="Ram">RAM usage.</param>
/// <param name="Gpu">GPU usage, including adapter type.</param>
/// <param name="Network">Upload/download throughput and active interface.</param>
/// <param name="Disk">System drive read/write throughput, hardware name, and capacity.</param>
/// <param name="Vram">
/// VRAM usage, present only when <see cref="Gpu"/>.<see cref="GpuUsageSnapshot.GpuType"/> is
/// <see cref="Enums.GpuType.Dedicated"/>; <see langword="null"/> for integrated graphics.
/// </param>
public sealed record SystemInfoSnapshot(
    ResourceUsageSnapshot Cpu,
    ResourceUsageSnapshot Ram,
    GpuUsageSnapshot Gpu,
    NetworkUsageSnapshot Network,
    DiskUsageSnapshot Disk,
    ResourceUsageSnapshot? Vram);
