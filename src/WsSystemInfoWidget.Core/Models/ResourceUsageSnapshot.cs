using WsSystemInfoWidget.Core.Enums;

namespace WsSystemInfoWidget.Core.Models;

/// <summary>
/// A single point-in-time reading of one monitored resource.
/// </summary>
/// <param name="Kind">Which resource this reading belongs to.</param>
/// <param name="UsagePercentage">Usage expressed as a percentage in the [0, 100] range.</param>
/// <param name="Severity">Qualitative severity derived from <paramref name="UsagePercentage"/>.</param>
/// <param name="SubtitleText">Secondary line shown under the headline value (hardware name, capacity, throughput, etc.).</param>
public sealed record ResourceUsageSnapshot(
    ResourceKind Kind,
    double UsagePercentage,
    UsageSeverity Severity,
    string SubtitleText);
