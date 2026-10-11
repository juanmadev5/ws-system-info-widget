using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Localization;

namespace WsSystemInfoWidget.Core.Extensions;

public static class ResourceKindExtensions
{
    /// <summary>
    /// The <see cref="ResourceKeys"/> entry for this resource's card title.
    /// </summary>
    public static string GetTitleResourceKey(this ResourceKind kind) => kind switch
    {
        ResourceKind.Cpu => ResourceKeys.CpuCardTitle,
        ResourceKind.Ram => ResourceKeys.RamCardTitle,
        ResourceKind.Gpu => ResourceKeys.GpuCardTitle,
        ResourceKind.Network => ResourceKeys.NetworkCardTitle,
        ResourceKind.Disk => ResourceKeys.DiskCardTitle,
        ResourceKind.Vram => ResourceKeys.VramCardTitle,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, message: null),
    };
}
