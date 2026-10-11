using System.Globalization;

namespace WsSystemInfoWidget.Core.Formatting;

/// <summary>Formats network throughput using compact KB/s or MB/s units.</summary>
public static class NetworkRateFormatter
{
    private const double BytesPerKilobyte = 1024d;
    private const double BytesPerMegabyte = BytesPerKilobyte * BytesPerKilobyte;

    public static string FormatBytesPerSecond(double bytesPerSecond)
    {
        var safeBytesPerSecond = Math.Max(0d, bytesPerSecond);
        var useMegabytes = safeBytesPerSecond >= BytesPerMegabyte;
        var value = safeBytesPerSecond / (useMegabytes ? BytesPerMegabyte : BytesPerKilobyte);
        var valueText = value.ToString("0.#", CultureInfo.CurrentCulture);
        return $"{valueText} {(useMegabytes ? "MB/s" : "KB/s")}";
    }
}