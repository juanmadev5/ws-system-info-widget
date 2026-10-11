using WsSystemInfoWidget.Core.Enums;

namespace WsSystemInfoWidget.Core.Extensions;

public static class ByteUnitExtensions
{
    private const double BytesPerKilobyte = 1024d;
    private const double BytesPerMegabyte = BytesPerKilobyte * 1024d;
    private const double BytesPerGigabyte = BytesPerMegabyte * 1024d;
    private const double BytesPerTerabyte = BytesPerGigabyte * 1024d;

    /// <summary>Number of bytes in one unit of <paramref name="unit"/>.</summary>
    public static double GetByteCount(this ByteUnit unit) => unit switch
    {
        ByteUnit.Bytes => 1d,
        ByteUnit.Kilobytes => BytesPerKilobyte,
        ByteUnit.Megabytes => BytesPerMegabyte,
        ByteUnit.Gigabytes => BytesPerGigabyte,
        ByteUnit.Terabytes => BytesPerTerabyte,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, message: null),
    };

    /// <summary>Short display symbol for <paramref name="unit"/> (e.g. "GB").</summary>
    public static string GetSymbol(this ByteUnit unit) => unit switch
    {
        ByteUnit.Bytes => "B",
        ByteUnit.Kilobytes => "KB",
        ByteUnit.Megabytes => "MB",
        ByteUnit.Gigabytes => "GB",
        ByteUnit.Terabytes => "TB",
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, message: null),
    };
}
