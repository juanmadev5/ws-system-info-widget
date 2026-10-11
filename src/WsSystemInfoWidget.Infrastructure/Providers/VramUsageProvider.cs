using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Formatting;
using WsSystemInfoWidget.Core.Models;
using WsSystemInfoWidget.Infrastructure.Calculations;
using WsSystemInfoWidget.Infrastructure.SystemData;

namespace WsSystemInfoWidget.Infrastructure.Providers;

/// <summary>
/// Reads dedicated VRAM utilization from the "GPU Adapter Memory" performance
/// counter category. Only invoked by the aggregator when the GPU is dedicated.
/// </summary>
public sealed class VramUsageProvider : IVramUsageProvider
{
    private readonly GpuAdapterMemoryReader _adapterMemoryReader;

    public VramUsageProvider(GpuAdapterMemoryReader adapterMemoryReader) => _adapterMemoryReader = adapterMemoryReader;

    public Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken)
    {
        var adapterMemory = _adapterMemoryReader.ReadPrimaryAdapter();
        var dedicatedUsageBytes = adapterMemory?.DedicatedUsageBytes ?? 0d;
        var dedicatedLimitBytes = adapterMemory?.DedicatedLimitBytes ?? 0d;

        var usagePercentage = PercentageCalculator.CalculateUsedPercentage(
            dedicatedUsageBytes,
            dedicatedLimitBytes);
        var severity = UsageSeverityClassifier.Classify(usagePercentage);
        var capacityText = ByteSizeFormatter.Format(dedicatedLimitBytes);

        var snapshot = new ResourceUsageSnapshot(ResourceKind.Vram, usagePercentage, severity, capacityText);
        return Task.FromResult(snapshot);
    }
}
