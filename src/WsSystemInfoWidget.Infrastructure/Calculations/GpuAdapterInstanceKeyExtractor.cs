namespace WsSystemInfoWidget.Infrastructure.Calculations;

/// <summary>
/// "GPU Engine" and "GPU Adapter Memory" performance-counter instance names
/// are per-process, per-adapter, per-engine (e.g.
/// <c>pid_1234_luid_0x00000000_0x0000ABCD_phys_0_engtype_3D</c>). To aggregate
/// per adapter across processes, the per-process <c>pid_####_</c> prefix must
/// be stripped, leaving the adapter+engine identity as the grouping key. Pure
/// string matching, kept free of any OS dependency so it's directly unit-testable.
/// </summary>
public static class GpuAdapterInstanceKeyExtractor
{
    private const string AdapterIdentityMarker = "luid_";

    public static string ExtractAdapterKey(string instanceName)
    {
        var markerIndex = instanceName.IndexOf(AdapterIdentityMarker, StringComparison.Ordinal);
        return markerIndex >= 0 ? instanceName[markerIndex..] : instanceName;
    }
}
