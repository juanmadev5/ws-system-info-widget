using System.Diagnostics;
using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.SystemData;

/// <summary>
/// Reads the "GPU Engine" / "Utilization Percentage" performance counters for
/// 3D-engine instances and sums them into one overall GPU busy percentage.
/// These are timer-percentage counters that need two time-separated samples
/// to be meaningful, so one <see cref="PerformanceCounter"/> per instance is
/// kept alive across polls; the per-process instance list is refreshed every
/// read since engine instances churn as processes start/stop.
/// </summary>
internal sealed class GpuEngineUtilizationReader : IDisposable
{
    private const string CategoryName = "GPU Engine";
    private const string UtilizationCounterName = "Utilization Percentage";

    private readonly Dictionary<string, PerformanceCounter> _counters = new(StringComparer.Ordinal);

    internal double ReadUtilizationPercentage()
    {
        try
        {
            if (!PerformanceCounterCategory.Exists(CategoryName))
            {
                return 0d;
            }

            var currentInstances = new PerformanceCounterCategory(CategoryName)
                .GetInstanceNames()
                .Where(GpuEngineInstanceFilter.IsGraphicsEngineInstance)
                .ToHashSet(StringComparer.Ordinal);

            RemoveStaleCounters(currentInstances);
            AddNewCounters(currentInstances);

            var total = _counters.Values.Sum(counter => counter.NextValue());
            return Math.Clamp(total, 0d, 100d);
        }
        catch (InvalidOperationException exception)
        {
            Debug.WriteLine($"GPU performance counters unavailable: {exception}");
            ClearCounters();
            return 0d;
        }
    }

    private void RemoveStaleCounters(HashSet<string> currentInstances)
    {
        foreach (var staleInstance in _counters.Keys.Where(instance => !currentInstances.Contains(instance)).ToList())
        {
            _counters[staleInstance].Dispose();
            _counters.Remove(staleInstance);
        }
    }

    private void AddNewCounters(HashSet<string> currentInstances)
    {
        foreach (var instanceName in currentInstances)
        {
            if (!_counters.ContainsKey(instanceName))
            {
                _counters[instanceName] = new PerformanceCounter(CategoryName, UtilizationCounterName, instanceName, readOnly: true);
            }
        }
    }

    public void Dispose()
    {
        ClearCounters();
    }

    private void ClearCounters()
    {
        foreach (var counter in _counters.Values)
        {
            counter.Dispose();
        }

        _counters.Clear();
    }
}
