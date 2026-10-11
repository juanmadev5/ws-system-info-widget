using Microsoft.Extensions.DependencyInjection;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Services;
using WsSystemInfoWidget.Infrastructure.Providers;
using WsSystemInfoWidget.Infrastructure.SystemData;

namespace WsSystemInfoWidget.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registers the real system-data provider implementations and the
    /// snapshot aggregator that composes them.
    /// </summary>
    public static IServiceCollection AddSystemInfoInfrastructure(this IServiceCollection services) => services
        .AddSingleton<ICpuUsageProvider, CpuUsageProvider>()
        .AddSingleton<IMemoryUsageProvider, MemoryUsageProvider>()
        // Shared so GpuUsageProvider and VramUsageProvider read the same
        // "GPU Adapter Memory" performance counters instead of each opening
        // and polling their own copy.
        .AddSingleton<GpuAdapterMemoryReader>()
        .AddSingleton<IGpuUsageProvider, GpuUsageProvider>()
        .AddSingleton<INetworkUsageProvider, NetworkUsageProvider>()
        .AddSingleton<IDiskUsageProvider, DiskUsageProvider>()
        .AddSingleton<IVramUsageProvider, VramUsageProvider>()
        .AddSingleton<ISystemInfoSnapshotProvider, SystemInfoSnapshotAggregator>();
}
