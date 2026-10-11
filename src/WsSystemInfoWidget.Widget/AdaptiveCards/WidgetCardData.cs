namespace WsSystemInfoWidget.Widget.AdaptiveCards;

/// <summary>
/// The data payload sent alongside the (static) Adaptive Card template.
/// Property names are serialized camelCase and must match the
/// <c>${...}</c> bindings in <c>widget-template.json</c> exactly.
/// </summary>
internal sealed record WidgetCardData
{
    public required string HeaderTitle { get; init; }

    public required string CpuTitle { get; init; }
    public required string CpuPercentageText { get; init; }
    public required double CpuPercentage { get; init; }
    public required string CpuColor { get; init; }
    public required string CpuSubtitle { get; init; }

    public required string RamTitle { get; init; }
    public required string RamPercentageText { get; init; }
    public required double RamPercentage { get; init; }
    public required string RamColor { get; init; }
    public required string RamSubtitleLine1 { get; init; }
    public required string RamSubtitleLine2 { get; init; }

    public required string GpuTitle { get; init; }
    public required string GpuPercentageText { get; init; }
    public required double GpuPercentage { get; init; }
    public required string GpuColor { get; init; }
    public required string GpuSubtitle { get; init; }

    public required string NetworkTitle { get; init; }
    public required string NetworkUploadText { get; init; }
    public required string NetworkDownloadText { get; init; }
    public required string NetworkSubtitle { get; init; }

    public required string DiskTitle { get; init; }
    public required string DiskDriveName { get; init; }
    public required string DiskReadText { get; init; }
    public required string DiskWriteText { get; init; }
    public required string DiskSubtitle { get; init; }

    public required bool ShowVram { get; init; }
    public required string VramTitle { get; init; }
    public required string VramPercentageText { get; init; }
    public required double VramPercentage { get; init; }
    public required string VramColor { get; init; }
    public required string VramSubtitle { get; init; }
}
