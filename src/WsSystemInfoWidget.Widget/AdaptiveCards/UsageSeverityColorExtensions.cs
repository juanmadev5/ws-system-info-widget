using WsSystemInfoWidget.Core.Enums;

namespace WsSystemInfoWidget.Widget.AdaptiveCards;

/// <summary>
/// Maps a domain <see cref="UsageSeverity"/> to the Adaptive Card color token
/// used to tint that metric's ProgressBar.
/// </summary>
public static class UsageSeverityColorExtensions
{
    private const string NormalColorToken = "good";
    private const string ElevatedColorToken = "warning";
    private const string CriticalColorToken = "attention";

    public static string ToAdaptiveCardColorToken(this UsageSeverity severity) => severity switch
    {
        UsageSeverity.Normal => NormalColorToken,
        UsageSeverity.Elevated => ElevatedColorToken,
        UsageSeverity.Critical => CriticalColorToken,
        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, message: null),
    };
}
