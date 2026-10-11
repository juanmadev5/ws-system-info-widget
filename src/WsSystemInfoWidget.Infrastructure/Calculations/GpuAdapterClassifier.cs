using WsSystemInfoWidget.Core.Enums;

namespace WsSystemInfoWidget.Infrastructure.Calculations;

/// <summary>
/// Classifies a GPU adapter as integrated or dedicated from its reported
/// dedicated-VRAM capacity (the same "Dedicated Limit" figure the
/// "GPU Adapter Memory" performance counter category and Task Manager use).
/// Integrated/UMA adapters report a near-zero dedicated limit; dedicated
/// adapters report several hundred MB or more.
/// </summary>
public static class GpuAdapterClassifier
{
    public const double DedicatedVramThresholdBytes = 512d * 1024 * 1024;

    public static GpuType Classify(double dedicatedVramLimitBytes) =>
        dedicatedVramLimitBytes >= DedicatedVramThresholdBytes
            ? GpuType.Dedicated
            : GpuType.Integrated;
}
