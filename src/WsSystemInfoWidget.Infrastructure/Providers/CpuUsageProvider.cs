using System.Runtime.InteropServices;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Formatting;
using WsSystemInfoWidget.Core.Models;
using WsSystemInfoWidget.Infrastructure.SystemData;

namespace WsSystemInfoWidget.Infrastructure.Providers;

/// <summary>
/// Reads CPU utilization from the Windows kernel counters and the processor name from WMI.
/// GetSystemTimes avoids localized or unavailable Performance Counter categories.
/// </summary>
public sealed class CpuUsageProvider : ICpuUsageProvider
{
    private const string ProcessorNameQuery = "SELECT Name FROM Win32_Processor";
    private const string NamePropertyName = "Name";
    private const string FallbackProcessorName = "CPU";

    private readonly object _sampleLock = new();
    private readonly Lazy<string> _processorName = new(ReadProcessorName);
    private ulong? _previousIdleTime;
    private ulong? _previousTotalTime;

    public Task<ResourceUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        double usagePercentage;
        lock (_sampleLock)
        {
            if (!GetSystemTimes(out var idleFileTime, out var kernelFileTime, out var userFileTime))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            var idleTime = idleFileTime.ToUInt64();
            var totalTime = kernelFileTime.ToUInt64() + userFileTime.ToUInt64();

            if (_previousIdleTime is { } previousIdle && _previousTotalTime is { } previousTotal && totalTime > previousTotal)
            {
                var totalDelta = totalTime - previousTotal;
                var idleDelta = idleTime >= previousIdle ? idleTime - previousIdle : 0;
                usagePercentage = Math.Clamp((double)(totalDelta - Math.Min(idleDelta, totalDelta)) / totalDelta * 100d, 0d, 100d);
            }
            else
            {
                // The first sample establishes a baseline; the next polling tick has a real percentage.
                usagePercentage = 0d;
            }

            _previousIdleTime = idleTime;
            _previousTotalTime = totalTime;
        }

        var severity = UsageSeverityClassifier.Classify(usagePercentage);
        var snapshot = new ResourceUsageSnapshot(ResourceKind.Cpu, usagePercentage, severity, _processorName.Value);
        return Task.FromResult(snapshot);
    }

    private static string ReadProcessorName()
    {
        foreach (var processor in WmiQuery.Select(ProcessorNameQuery))
        {
            var name = processor.GetPropertyOrDefault<string>(NamePropertyName);
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name.Trim();
            }
        }

        return FallbackProcessorName;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetSystemTimes(out NativeFileTime idleTime, out NativeFileTime kernelTime, out NativeFileTime userTime);

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeFileTime
    {
        public uint LowDateTime;
        public uint HighDateTime;

        public readonly ulong ToUInt64() => ((ulong)HighDateTime << 32) | LowDateTime;
    }
}
