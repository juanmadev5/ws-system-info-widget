using System.Diagnostics;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Formatting;
using WsSystemInfoWidget.Core.Localization;
using WsSystemInfoWidget.Core.Models;
using WsSystemInfoWidget.Infrastructure.SystemData;

namespace WsSystemInfoWidget.Infrastructure.Providers;

/// <summary>
/// Reads the system drive's read/write throughput from the "PhysicalDisk"
/// performance counters, its hardware model from WMI (queried once, since it
/// cannot change at runtime), and its free/total capacity from <see cref="DriveInfo"/>.
/// </summary>
public sealed class DiskUsageProvider : IDiskUsageProvider, IDisposable
{
    private const string PerformanceCounterCategory = "PhysicalDisk";
    private const string ReadBytesCounterName = "Disk Read Bytes/sec";
    private const string WriteBytesCounterName = "Disk Write Bytes/sec";
    private const string TotalInstanceName = "_Total";
    private const string DiskModelQuery = "SELECT Model FROM Win32_DiskDrive";
    private const string ModelPropertyName = "Model";

    private readonly object _counterLock = new();
    private readonly ILocalizationService _localization;
    private readonly Lazy<string> _driveName = new(ReadDriveModelName);
    private PerformanceCounter? _readBytesCounter;
    private PerformanceCounter? _writeBytesCounter;

    public DiskUsageProvider(ILocalizationService localization) => _localization = localization;

    public Task<DiskUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        double readBytesPerSecond;
        double writeBytesPerSecond;
        lock (_counterLock)
        {
            try
            {
                // Construct lazily so an unavailable counter cannot prevent DI/provider startup.
                _readBytesCounter ??= new PerformanceCounter(PerformanceCounterCategory, ReadBytesCounterName, TotalInstanceName, readOnly: true);
                _writeBytesCounter ??= new PerformanceCounter(PerformanceCounterCategory, WriteBytesCounterName, TotalInstanceName, readOnly: true);
                readBytesPerSecond = Math.Max(0f, _readBytesCounter.NextValue());
                writeBytesPerSecond = Math.Max(0f, _writeBytesCounter.NextValue());
            }
            catch (InvalidOperationException exception)
            {
                Debug.WriteLine($"Disk performance counters unavailable: {exception}");
                _readBytesCounter?.Dispose();
                _writeBytesCounter?.Dispose();
                _readBytesCounter = null;
                _writeBytesCounter = null;
                readBytesPerSecond = 0d;
                writeBytesPerSecond = 0d;
            }
        }

        var snapshot = new DiskUsageSnapshot(readBytesPerSecond, writeBytesPerSecond, _driveName.Value, ReadSystemDriveCapacityText());
        return Task.FromResult(snapshot);
    }

    private string ReadSystemDriveCapacityText()
    {
        var systemDriveRoot = Path.GetPathRoot(Environment.SystemDirectory) ?? Path.GetPathRoot(Environment.CurrentDirectory)!;
        var drive = new DriveInfo(systemDriveRoot);

        return string.Format(
            _localization.CurrentCulture,
            _localization.GetString(ResourceKeys.DiskFreeOfTotalFormat),
            ByteSizeFormatter.Format(drive.AvailableFreeSpace),
            ByteSizeFormatter.Format(drive.TotalSize));
    }

    private static string ReadDriveModelName()
    {
        foreach (var disk in WmiQuery.Select(DiskModelQuery))
        {
            var model = disk.GetPropertyOrDefault<string>(ModelPropertyName);
            if (!string.IsNullOrWhiteSpace(model))
            {
                return model.Trim();
            }
        }

        return Path.GetPathRoot(Environment.SystemDirectory) ?? string.Empty;
    }

    public void Dispose()
    {
        lock (_counterLock)
        {
            _readBytesCounter?.Dispose();
            _writeBytesCounter?.Dispose();
            _readBytesCounter = null;
            _writeBytesCounter = null;
        }
    }
}
