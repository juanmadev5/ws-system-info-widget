namespace WsSystemInfoWidget.Core.Models;

/// <summary>Current read/write throughput for the system drive, its hardware name, and free/total capacity.</summary>
public sealed record DiskUsageSnapshot(
    double ReadBytesPerSecond,
    double WriteBytesPerSecond,
    string DriveName,
    string CapacityText);
