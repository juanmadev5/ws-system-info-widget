using System.Globalization;

namespace WsSystemInfoWidget.Core.Formatting;

/// <summary>
/// Formats raw numeric readings into the display strings shown on the widget.
/// Kept in Core so the mapping from data to display text is unit-testable
/// without any Adaptive Card or WinUI dependency.
/// </summary>
public static class UsageDisplayFormatter
{
    private const int PercentageDecimalPlaces = 0;
    private const string PercentageSuffix = "%";

    /// <summary>
    /// Formats a [0, 100] usage percentage as a rounded, culture-invariant
    /// headline value (e.g. <c>42%</c>).
    /// </summary>
    public static string FormatPercentage(double usagePercentage)
    {
        var clamped = Math.Clamp(usagePercentage, 0d, 100d);
        var rounded = Math.Round(clamped, PercentageDecimalPlaces, MidpointRounding.AwayFromZero);
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{rounded}{PercentageSuffix}");
    }
}
