using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Extensions;
using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Services;

/// <summary>
/// Default <see cref="ISystemInfoSnapshotProvider"/> that polls every per-resource
/// provider in parallel and only queries VRAM when the active GPU is dedicated.
/// </summary>
public sealed class SystemInfoSnapshotAggregator : ISystemInfoSnapshotProvider
{
    private readonly ICpuUsageProvider _cpuUsageProvider;
    private readonly IMemoryUsageProvider _memoryUsageProvider;
    private readonly IGpuUsageProvider _gpuUsageProvider;
    private readonly INetworkUsageProvider _networkUsageProvider;
    private readonly IDiskUsageProvider _diskUsageProvider;
    private readonly IVramUsageProvider _vramUsageProvider;

    public SystemInfoSnapshotAggregator(
        ICpuUsageProvider cpuUsageProvider,
        IMemoryUsageProvider memoryUsageProvider,
        IGpuUsageProvider gpuUsageProvider,
        INetworkUsageProvider networkUsageProvider,
        IDiskUsageProvider diskUsageProvider,
        IVramUsageProvider vramUsageProvider)
    {
        _cpuUsageProvider = cpuUsageProvider;
        _memoryUsageProvider = memoryUsageProvider;
        _gpuUsageProvider = gpuUsageProvider;
        _networkUsageProvider = networkUsageProvider;
        _diskUsageProvider = diskUsageProvider;
        _vramUsageProvider = vramUsageProvider;
    }

    public async Task<SystemInfoSnapshot> GetSnapshotAsync(CancellationToken cancellationToken)
    {
        var cpuTask = _cpuUsageProvider.GetUsageAsync(cancellationToken);
        var ramTask = _memoryUsageProvider.GetUsageAsync(cancellationToken);
        var gpuTask = _gpuUsageProvider.GetUsageAsync(cancellationToken);
        var networkTask = _networkUsageProvider.GetUsageAsync(cancellationToken);
        var diskTask = _diskUsageProvider.GetUsageAsync(cancellationToken);

        await Task.WhenAll(cpuTask, ramTask, gpuTask, networkTask, diskTask).ConfigureAwait(false);

        var gpu = gpuTask.Result;
        var vram = gpu.GpuType.HasDedicatedVram()
            ? await _vramUsageProvider.GetUsageAsync(cancellationToken).ConfigureAwait(false)
            : null;

        return new SystemInfoSnapshot(
            cpuTask.Result,
            ramTask.Result,
            gpu,
            networkTask.Result,
            diskTask.Result,
            vram);
    }
}
