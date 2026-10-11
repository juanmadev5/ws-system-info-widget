namespace WsSystemInfoWidget.Core.Enums;

/// <summary>
/// Identifies which monitored system resource a metric belongs to.
/// </summary>
public enum ResourceKind
{
    Cpu,
    Ram,
    Gpu,
    Network,
    Disk,
    Vram,
}
