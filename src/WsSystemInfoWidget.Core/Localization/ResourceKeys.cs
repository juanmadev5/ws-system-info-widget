namespace WsSystemInfoWidget.Core.Localization;

/// <summary>
/// Named keys for every localized UI string the widget uses. Resolved through
/// <see cref="Abstractions.ILocalizationService"/>; must match the key names
/// used in each language's .resw resource file exactly.
/// </summary>
public static class ResourceKeys
{
    public const string WidgetHeaderTitle = "WidgetHeaderTitle";

    public const string CpuCardTitle = "CpuCardTitle";
    public const string RamCardTitle = "RamCardTitle";
    public const string GpuCardTitle = "GpuCardTitle";
    public const string NetworkCardTitle = "NetworkCardTitle";
    public const string NetworkUploadLabel = "NetworkUploadLabel";
    public const string NetworkDownloadLabel = "NetworkDownloadLabel";
    public const string NetworkUnavailable = "NetworkUnavailable";
    public const string DiskCardTitle = "DiskCardTitle";
    public const string DiskReadLabel = "DiskReadLabel";
    public const string DiskWriteLabel = "DiskWriteLabel";
    public const string DiskFreeOfTotalFormat = "DiskFreeOfTotalFormat";
    public const string VramCardTitle = "VramCardTitle";
}
