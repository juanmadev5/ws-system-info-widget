using WsSystemInfoWidget.Core.Enums;

namespace WsSystemInfoWidget.Core.Extensions;

public static class GpuTypeExtensions
{
    /// <summary>
    /// Whether a VRAM metric is meaningful for this adapter type.
    /// </summary>
    public static bool HasDedicatedVram(this GpuType gpuType) => gpuType switch
    {
        GpuType.Dedicated => true,
        GpuType.Integrated => false,
        GpuType.Unknown => false,
        _ => throw new ArgumentOutOfRangeException(nameof(gpuType), gpuType, message: null),
    };
}
