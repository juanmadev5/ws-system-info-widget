using System.Linq;
using System.Text.Json;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Extensions;
using WsSystemInfoWidget.Core.Formatting;
using WsSystemInfoWidget.Core.Localization;
using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Widget.AdaptiveCards;

/// <summary>
/// Maps a Core <see cref="SystemInfoSnapshot"/> into the JSON data payload
/// the widget template binds to. This is the only place in the app that
/// knows about Adaptive Card field names, keeping the ViewModel free of any
/// Adaptive-Card-specific serialization (per the project's MVVM split).
/// </summary>
public static class WidgetDataMapper
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static string BuildDataJson(SystemInfoSnapshot snapshot, ILocalizationService localization)
    {
        var vram = snapshot.Vram;
        var ramSubtitleLines = snapshot.Ram.SubtitleText.Split('\n');

        var data = new WidgetCardData
        {
            HeaderTitle = localization.GetString(ResourceKeys.WidgetHeaderTitle),

            CpuTitle = localization.GetString(ResourceKeys.CpuCardTitle),
            CpuPercentageText = UsageDisplayFormatter.FormatPercentage(snapshot.Cpu.UsagePercentage),
            CpuPercentage = snapshot.Cpu.UsagePercentage,
            CpuColor = snapshot.Cpu.Severity.ToAdaptiveCardColorToken(),
            CpuSubtitle = snapshot.Cpu.SubtitleText,

            RamTitle = localization.GetString(ResourceKeys.RamCardTitle),
            RamPercentageText = UsageDisplayFormatter.FormatPercentage(snapshot.Ram.UsagePercentage),
            RamPercentage = snapshot.Ram.UsagePercentage,
            RamColor = snapshot.Ram.Severity.ToAdaptiveCardColorToken(),
            RamSubtitleLine1 = ramSubtitleLines.ElementAtOrDefault(0) ?? string.Empty,
            RamSubtitleLine2 = ramSubtitleLines.ElementAtOrDefault(1) ?? string.Empty,

            GpuTitle = localization.GetString(ResourceKeys.GpuCardTitle),
            GpuPercentageText = UsageDisplayFormatter.FormatPercentage(snapshot.Gpu.Usage.UsagePercentage),
            GpuPercentage = snapshot.Gpu.Usage.UsagePercentage,
            GpuColor = snapshot.Gpu.Usage.Severity.ToAdaptiveCardColorToken(),
            GpuSubtitle = snapshot.Gpu.Usage.SubtitleText,

            NetworkTitle = localization.GetString(ResourceKeys.NetworkCardTitle),
            NetworkUploadText = $"{localization.GetString(ResourceKeys.NetworkUploadLabel)} {NetworkRateFormatter.FormatBytesPerSecond(snapshot.Network.UploadBytesPerSecond)}",
            NetworkDownloadText = $"{localization.GetString(ResourceKeys.NetworkDownloadLabel)} {NetworkRateFormatter.FormatBytesPerSecond(snapshot.Network.DownloadBytesPerSecond)}",
            NetworkSubtitle = snapshot.Network.AdapterName,

            DiskTitle = localization.GetString(ResourceKeys.DiskCardTitle),
            DiskDriveName = snapshot.Disk.DriveName,
            DiskReadText = $"{localization.GetString(ResourceKeys.DiskReadLabel)} {NetworkRateFormatter.FormatBytesPerSecond(snapshot.Disk.ReadBytesPerSecond)}",
            DiskWriteText = $"{localization.GetString(ResourceKeys.DiskWriteLabel)} {NetworkRateFormatter.FormatBytesPerSecond(snapshot.Disk.WriteBytesPerSecond)}",
            DiskSubtitle = snapshot.Disk.CapacityText,

            ShowVram = vram is not null,
            VramTitle = localization.GetString(ResourceKeys.VramCardTitle),
            VramPercentageText = vram is not null ? UsageDisplayFormatter.FormatPercentage(vram.UsagePercentage) : string.Empty,
            VramPercentage = vram?.UsagePercentage ?? 0d,
            VramColor = (vram?.Severity ?? UsageSeverity.Normal).ToAdaptiveCardColorToken(),
            VramSubtitle = vram?.SubtitleText ?? string.Empty,
        };

        return JsonSerializer.Serialize(data, SerializerOptions);
    }
}
