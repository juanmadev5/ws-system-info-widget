using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Formatting;
using WsSystemInfoWidget.Core.Models;
using WsSystemInfoWidget.Infrastructure.Calculations;
using WsSystemInfoWidget.Infrastructure.Interop;
using WsSystemInfoWidget.Infrastructure.SystemData;

namespace WsSystemInfoWidget.Infrastructure.Providers;

/// <summary>
/// Reads RAM utilization via <c>GlobalMemoryStatusEx</c> and the installed
/// memory's type/speed spec from WMI.
/// </summary>
public sealed class MemoryUsageProvider : IMemoryUsageProvider
{
    private const string MemorySpecQuery = "SELECT Capacity, MemoryType, SMBIOSMemoryType, Speed FROM Win32_PhysicalMemory";
    private const string CapacityPropertyName = "Capacity";
    private const string MemoryTypePropertyName = "MemoryType";
    private const string SmbiosMemoryTypePropertyName = "SMBIOSMemoryType";
    private const string SpeedPropertyName = "Speed";

    private readonly Lazy<string> _memorySpec = new(ReadMemorySpec);

    public Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken)
    {
        var status = MemoryStatusEx.Create();
        NativeMethods.GlobalMemoryStatusEx(ref status);

        var usedBytes = status.TotalPhysicalBytes - status.AvailablePhysicalBytes;
        var usagePercentage = PercentageCalculator.CalculateUsedPercentage(usedBytes, status.TotalPhysicalBytes);
        var severity = UsageSeverityClassifier.Classify(usagePercentage);

        var snapshot = new ResourceUsageSnapshot(ResourceKind.Ram, usagePercentage, severity, _memorySpec.Value);
        return Task.FromResult(snapshot);
    }

    private static string ReadMemorySpec()
    {
        ulong totalCapacityBytes = 0;
        var selectedMemoryType = SmbiosMemoryType.Unknown;
        uint selectedSpeed = 0;

        foreach (var module in WmiQuery.Select(MemorySpecQuery))
        {
            totalCapacityBytes += module.GetPropertyOrDefault<ulong>(CapacityPropertyName);
            var memoryType = (SmbiosMemoryType)module.GetPropertyOrDefault<ushort>(SmbiosMemoryTypePropertyName);
            if (memoryType == SmbiosMemoryType.Unknown)
            {
                // Some firmware leaves SMBIOSMemoryType unset but still fills the legacy WMI property.
                memoryType = (SmbiosMemoryType)module.GetPropertyOrDefault<ushort>(MemoryTypePropertyName);
            }
            var speed = module.GetPropertyOrDefault<uint>(SpeedPropertyName);

            if (selectedMemoryType == SmbiosMemoryType.Unknown && memoryType != SmbiosMemoryType.Unknown)
            {
                selectedMemoryType = memoryType;
            }

            selectedSpeed = Math.Max(selectedSpeed, speed);
        }

        if (selectedMemoryType == SmbiosMemoryType.Unknown)
        {
            // If firmware omits both WMI type fields, infer the likely generation from standard speed bands.
            selectedMemoryType = selectedSpeed switch
            {
                >= 4800 => SmbiosMemoryType.Ddr5,
                >= 2400 => SmbiosMemoryType.Ddr4,
                >= 1600 => SmbiosMemoryType.Ddr3,
                _ => SmbiosMemoryType.Unknown,
            };
        }

        var memorySpec = MemorySpecFormatter.FormatSpec(selectedMemoryType, (int)selectedSpeed);
        return totalCapacityBytes > 0
            ? $"{ByteSizeFormatter.Format(totalCapacityBytes)} {memorySpec}"
            : memorySpec;
    }
}
