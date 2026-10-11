using System.Globalization;

namespace WsSystemInfoWidget.Infrastructure.Calculations;

/// <summary>
/// Builds the installed-memory subtitle text (e.g. "DDR4 3200 MT/s") from raw
/// WMI figures. Kept free of any OS dependency so it's directly unit-testable.
/// </summary>
public static class MemorySpecFormatter
{
    private const string UnknownMemoryTypeLabel = "RAM";
    private const string SpeedUnitSuffix = "MT/s";

    public static string MapMemoryTypeLabel(SmbiosMemoryType memoryType) => memoryType switch
    {
        SmbiosMemoryType.Ddr => "DDR",
        SmbiosMemoryType.Ddr2 => "DDR2",
        SmbiosMemoryType.Ddr2FbDimm => "DDR2 FB-DIMM",
        SmbiosMemoryType.Ddr3 => "DDR3",
        SmbiosMemoryType.Ddr4 => "DDR4",
        SmbiosMemoryType.Ddr5 => "DDR5",
        _ => UnknownMemoryTypeLabel,
    };

    /// <summary>
    /// Formats a spec string like "DDR4\n3200 MT/s". The '\n' is a plain line
    /// separator for the caller to split on (not markup for direct rendering)
    /// so the UI layer can place the type and speed on two separate lines.
    /// When <paramref name="speedMegatransfersPerSecond"/> is not positive, only
    /// the type label is returned.
    /// </summary>
    public static string FormatSpec(SmbiosMemoryType memoryType, int speedMegatransfersPerSecond)
    {
        var typeLabel = MapMemoryTypeLabel(memoryType);

        return speedMegatransfersPerSecond > 0
            ? string.Create(CultureInfo.InvariantCulture, $"{typeLabel}\n{speedMegatransfersPerSecond} {SpeedUnitSuffix}")
            : typeLabel;
    }
}
