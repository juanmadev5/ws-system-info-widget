using System.Globalization;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Extensions;

namespace WsSystemInfoWidget.Core.Formatting;

/// <summary>
/// Formats raw byte counts into short, human-readable capacity text
/// (e.g. disk/VRAM subtitles such as "512 GB / 1 TB").
/// </summary>
public static class ByteSizeFormatter
{
    private const int DecimalPlaces = 1;
    private const string DecimalFormat = "0.#";

    /// <summary>Picks the largest <see cref="ByteUnit"/> that keeps the value at or above 1.</summary>
    public static ByteUnit SelectUnit(double bytes)
    {
        var absoluteBytes = Math.Abs(bytes);

        if (absoluteBytes >= ByteUnit.Terabytes.GetByteCount())
        {
            return ByteUnit.Terabytes;
        }

        if (absoluteBytes >= ByteUnit.Gigabytes.GetByteCount())
        {
            return ByteUnit.Gigabytes;
        }

        if (absoluteBytes >= ByteUnit.Megabytes.GetByteCount())
        {
            return ByteUnit.Megabytes;
        }

        if (absoluteBytes >= ByteUnit.Kilobytes.GetByteCount())
        {
            return ByteUnit.Kilobytes;
        }

        return ByteUnit.Bytes;
    }

    /// <summary>Formats <paramref name="bytes"/> using its own best-fit unit (e.g. "512 GB").</summary>
    public static string Format(double bytes) => Format(bytes, SelectUnit(bytes));

    /// <summary>Formats <paramref name="bytes"/> using a specific <paramref name="unit"/>.</summary>
    public static string Format(double bytes, ByteUnit unit)
    {
        var value = bytes / unit.GetByteCount();
        var rounded = Math.Round(value, DecimalPlaces, MidpointRounding.AwayFromZero);
        var valueText = rounded.ToString(DecimalFormat, CultureInfo.InvariantCulture);
        return string.Create(CultureInfo.InvariantCulture, $"{valueText} {unit.GetSymbol()}");
    }

    /// <summary>
    /// Formats a used/total capacity pair, each in its own best-fit unit
    /// (e.g. "512 GB / 1 TB").
    /// </summary>
    public static string FormatUsedOfTotal(double usedBytes, double totalBytes) =>
        string.Create(CultureInfo.InvariantCulture, $"{Format(usedBytes)} / {Format(totalBytes)}");
}
