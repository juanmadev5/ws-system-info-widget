using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Core.Tests.Fakes;

internal sealed class FakeCpuUsageProvider(ResourceUsageSnapshot snapshot) : ICpuUsageProvider
{
    public Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken) =>
        Task.FromResult(snapshot);
}

internal sealed class FakeMemoryUsageProvider(ResourceUsageSnapshot snapshot) : IMemoryUsageProvider
{
    public Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken) =>
        Task.FromResult(snapshot);
}

internal sealed class FakeGpuUsageProvider(GpuUsageSnapshot snapshot) : IGpuUsageProvider
{
    public Task<GpuUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken) =>
        Task.FromResult(snapshot);
}

internal sealed class FakeNetworkUsageProvider(NetworkUsageSnapshot snapshot) : INetworkUsageProvider
{
    public Task<NetworkUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken) =>
        Task.FromResult(snapshot);
}

internal sealed class FakeDiskUsageProvider(DiskUsageSnapshot snapshot) : IDiskUsageProvider
{
    public Task<DiskUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken) =>
        Task.FromResult(snapshot);
}

internal sealed class FakeVramUsageProvider(ResourceUsageSnapshot snapshot) : IVramUsageProvider
{
    public int CallCount { get; private set; }

    public Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken)
    {
        CallCount++;
        return Task.FromResult(snapshot);
    }
}
