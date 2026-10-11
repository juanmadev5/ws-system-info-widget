namespace WsSystemInfoWidget.Core.Models;

/// <summary>Current upload/download throughput and the interface carrying the Internet route.</summary>
public sealed record NetworkUsageSnapshot(
    double UploadBytesPerSecond,
    double DownloadBytesPerSecond,
    string AdapterName);