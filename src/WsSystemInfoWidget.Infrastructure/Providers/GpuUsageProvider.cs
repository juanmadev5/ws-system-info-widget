using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Formatting;
using WsSystemInfoWidget.Core.Models;
using WsSystemInfoWidget.Infrastructure.Calculations;
using WsSystemInfoWidget.Infrastructure.SystemData;

namespace WsSystemInfoWidget.Infrastructure.Providers;

/// <summary>
/// Reads GPU busy percentage from "GPU Engine" counters, the adapter type
/// from "GPU Adapter Memory" counters, and the adapter's display name from WMI.
/// </summary>
public sealed class GpuUsageProvider : IGpuUsageProvider, IDisposable
{
    private const string VideoControllerNameQuery = "SELECT Name FROM Win32_VideoController";
    private const string NamePropertyName = "Name";
    private const string FallbackAdapterName = "GPU";

    private readonly GpuEngineUtilizationReader _engineUtilizationReader = new();
    private readonly GpuAdapterMemoryReader _adapterMemoryReader;
    private readonly Lazy<string> _adapterName = new(ReadAdapterName);

    public GpuUsageProvider(GpuAdapterMemoryReader adapterMemoryReader) => _adapterMemoryReader = adapterMemoryReader;

    public Task<GpuUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken)
    {
        var usagePercentage = _engineUtilizationReader.ReadUtilizationPercentage();
        var severity = UsageSeverityClassifier.Classify(usagePercentage);

        var adapterMemory = _adapterMemoryReader.ReadPrimaryAdapter();
        var gpuType = adapterMemory is { } memory
            ? GpuAdapterClassifier.Classify(memory.DedicatedLimitBytes)
            : GpuType.Unknown;

        var usage = new ResourceUsageSnapshot(ResourceKind.Gpu, usagePercentage, severity, _adapterName.Value);
        var snapshot = new GpuUsageSnapshot(usage, gpuType);
        return Task.FromResult(snapshot);
    }

    private static string ReadAdapterName()
    {
        foreach (var controller in WmiQuery.Select(VideoControllerNameQuery))
        {
            var name = controller.GetPropertyOrDefault<string>(NamePropertyName);
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name.Trim();
            }
        }

        return FallbackAdapterName;
    }

    public void Dispose() => _engineUtilizationReader.Dispose();
}
