namespace WsSystemInfoWidget.Core.Enums;

/// <summary>
/// Whether the reported GPU is an integrated (shared-memory) adapter or a
/// discrete/dedicated adapter with its own VRAM. Drives whether a VRAM metric
/// is reported at all.
/// </summary>
public enum GpuType
{
    Integrated,
    Dedicated,
    Unknown,
}
