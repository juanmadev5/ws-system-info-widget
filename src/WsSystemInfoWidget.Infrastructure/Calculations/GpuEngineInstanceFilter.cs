namespace WsSystemInfoWidget.Infrastructure.Calculations;

/// <summary>
/// Identifies which "GPU Engine" performance-counter instances represent 3D
/// engine work, so overall GPU busy-ness can be summed from just those.
/// Pure string matching, kept free of any OS dependency so it's directly
/// unit-testable.
/// </summary>
public static class GpuEngineInstanceFilter
{
    private const string GraphicsEngineTypeMarker = "engtype_3D";

    public static bool IsGraphicsEngineInstance(string instanceName) =>
        instanceName.Contains(GraphicsEngineTypeMarker, StringComparison.Ordinal);
}
