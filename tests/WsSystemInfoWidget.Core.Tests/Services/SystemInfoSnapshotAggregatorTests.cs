using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Models;
using WsSystemInfoWidget.Core.Services;
using WsSystemInfoWidget.Core.Tests.Fakes;

namespace WsSystemInfoWidget.Core.Tests.Services;

public sealed class SystemInfoSnapshotAggregatorTests
{
    private static readonly ResourceUsageSnapshot CpuSnapshot =
        new(ResourceKind.Cpu, 42d, UsageSeverity.Normal, "Intel Core i5");

    private static readonly ResourceUsageSnapshot RamSnapshot =
        new(ResourceKind.Ram, 55d, UsageSeverity.Normal, "DDR4 3200 MT/s");

    private static readonly NetworkUsageSnapshot NetworkSnapshot =
        new(12_000d, 24_000d, "Qualcomm Atheros QCA9377");

    private static readonly DiskUsageSnapshot DiskSnapshot =
        new(1_000d, 2_000d, "Kingston SNV3S500G", "512 GB free of 1 TB");

    private static readonly ResourceUsageSnapshot VramSnapshot =
        new(ResourceKind.Vram, 61d, UsageSeverity.Normal, "24 GB");

    [Fact]
    public async Task GetSnapshotAsync_WithDedicatedGpu_IncludesVramFromProvider()
    {
        var gpuSnapshot = new GpuUsageSnapshot(
            new ResourceUsageSnapshot(ResourceKind.Gpu, 77d, UsageSeverity.Elevated, "Nvidia RTX 3090"),
            GpuType.Dedicated);
        var vramProvider = new FakeVramUsageProvider(VramSnapshot);

        var aggregator = new SystemInfoSnapshotAggregator(
            new FakeCpuUsageProvider(CpuSnapshot),
            new FakeMemoryUsageProvider(RamSnapshot),
            new FakeGpuUsageProvider(gpuSnapshot),
            new FakeNetworkUsageProvider(NetworkSnapshot),
            new FakeDiskUsageProvider(DiskSnapshot),
            vramProvider);

        var snapshot = await aggregator.GetSnapshotAsync(CancellationToken.None);

        Assert.Equal(CpuSnapshot, snapshot.Cpu);
        Assert.Equal(RamSnapshot, snapshot.Ram);
        Assert.Equal(gpuSnapshot, snapshot.Gpu);
        Assert.Equal(NetworkSnapshot, snapshot.Network);
        Assert.Equal(DiskSnapshot, snapshot.Disk);
        Assert.Equal(VramSnapshot, snapshot.Vram);
        Assert.Equal(1, vramProvider.CallCount);
    }

    [Fact]
    public async Task GetSnapshotAsync_WithIntegratedGpu_LeavesVramNullAndNeverCallsProvider()
    {
        var gpuSnapshot = new GpuUsageSnapshot(
            new ResourceUsageSnapshot(ResourceKind.Gpu, 20d, UsageSeverity.Normal, "Intel UHD Graphics"),
            GpuType.Integrated);
        var vramProvider = new FakeVramUsageProvider(VramSnapshot);

        var aggregator = new SystemInfoSnapshotAggregator(
            new FakeCpuUsageProvider(CpuSnapshot),
            new FakeMemoryUsageProvider(RamSnapshot),
            new FakeGpuUsageProvider(gpuSnapshot),
            new FakeNetworkUsageProvider(NetworkSnapshot),
            new FakeDiskUsageProvider(DiskSnapshot),
            vramProvider);

        var snapshot = await aggregator.GetSnapshotAsync(CancellationToken.None);

        Assert.Null(snapshot.Vram);
        Assert.Equal(0, vramProvider.CallCount);
    }
}
