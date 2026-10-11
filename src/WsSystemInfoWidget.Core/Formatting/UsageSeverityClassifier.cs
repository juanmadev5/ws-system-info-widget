using WsSystemInfoWidget.Core.Enums;

namespace WsSystemInfoWidget.Core.Formatting;

/// <summary>
/// Maps a raw usage percentage to a qualitative <see cref="UsageSeverity"/>.
/// Thresholds are named constants so no magic numbers leak into call sites.
/// </summary>
public static class UsageSeverityClassifier
{
    public const double ElevatedThresholdPercentage = 70d;
    public const double CriticalThresholdPercentage = 90d;

    /// <summary>
    /// Classifies <paramref name="usagePercentage"/> into a <see cref="UsageSeverity"/>.
    /// </summary>
    public static UsageSeverity Classify(double usagePercentage)
    {
        if (usagePercentage >= CriticalThresholdPercentage)
        {
            return UsageSeverity.Critical;
        }

        if (usagePercentage >= ElevatedThresholdPercentage)
        {
            return UsageSeverity.Elevated;
        }

        return UsageSeverity.Normal;
    }
}
