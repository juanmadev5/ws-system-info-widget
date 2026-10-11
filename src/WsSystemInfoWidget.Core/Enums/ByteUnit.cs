namespace WsSystemInfoWidget.Core.Enums;

/// <summary>
/// A binary (1024-based) byte unit, used instead of a raw exponent/int to
/// select display scale for capacity readings (Disk, VRAM).
/// </summary>
public enum ByteUnit
{
    Bytes,
    Kilobytes,
    Megabytes,
    Gigabytes,
    Terabytes,
}
