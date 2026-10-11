namespace WsSystemInfoWidget.Core.Enums;

/// <summary>
/// Qualitative severity of a resource's current usage level, used to drive
/// presentation (e.g. Adaptive Card color tokens) without leaking raw thresholds
/// into the presentation layer.
/// </summary>
public enum UsageSeverity
{
    Normal,
    Elevated,
    Critical,
}
