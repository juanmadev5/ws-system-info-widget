using System.Diagnostics;
using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.SystemData;

/// <summary>
/// Reads the "GPU Adapter Memory" performance-counter category (the same
/// source Task Manager's Performance tab uses) and aggregates it to the
/// system's primary adapter: the one with the largest dedicated-VRAM limit.
/// The underlying counters are instantaneous raw values, so no warm-up read
/// is required. One <see cref="PerformanceCounter"/> per instance is kept
/// alive and reused across polls instead of being recreated every read;
/// shared as a singleton between <see cref="Providers.GpuUsageProvider"/> and
/// <see cref="Providers.VramUsageProvider"/> so both read from the same
/// counter set instead of opening it twice.
/// </summary>
public sealed class GpuAdapterMemoryReader : IDisposable
{
    private const string CategoryName = "GPU Adapter Memory";
    private const string DedicatedUsageCounterName = "Dedicated Usage";
    private const string DedicatedLimitCounterName = "Dedicated Limit";

    private readonly Dictionary<string, PerformanceCounter> _usageCounters = new(StringComparer.Ordinal);
    private readonly Dictionary<string, PerformanceCounter> _limitCounters = new(StringComparer.Ordinal);

    internal readonly record struct AdapterMemorySnapshot(double DedicatedUsageBytes, double DedicatedLimitBytes);

    internal AdapterMemorySnapshot? ReadPrimaryAdapter()
    {
        try
        {
            if (!PerformanceCounterCategory.Exists(CategoryName))
            {
                ClearCounters();
                return null;
            }

            var currentInstances = new PerformanceCounterCategory(CategoryName)
                .GetInstanceNames()
                .ToHashSet(StringComparer.Ordinal);

            RemoveStaleCounters(currentInstances);
            AddNewCounters(currentInstances);

            var usageByAdapter = new Dictionary<string, double>(StringComparer.Ordinal);
            var limitByAdapter = new Dictionary<string, double>(StringComparer.Ordinal);

            foreach (var instanceName in currentInstances)
            {
                var adapterKey = GpuAdapterInstanceKeyExtractor.ExtractAdapterKey(instanceName);
                usageByAdapter[adapterKey] = usageByAdapter.GetValueOrDefault(adapterKey) + _usageCounters[instanceName].NextValue();

                if (!limitByAdapter.ContainsKey(adapterKey))
                {
                    limitByAdapter[adapterKey] = _limitCounters[instanceName].NextValue();
                }
            }

            if (limitByAdapter.Count == 0)
            {
                return null;
            }

            var primaryAdapterKey = limitByAdapter
                .OrderByDescending(entry => entry.Value)
                .First()
                .Key;

            return new AdapterMemorySnapshot(
                usageByAdapter.GetValueOrDefault(primaryAdapterKey),
                limitByAdapter[primaryAdapterKey]);
        }
        catch (InvalidOperationException exception)
        {
            // Windows may expose the category but omit one or more counters.
            Debug.WriteLine($"GPU adapter memory counters unavailable: {exception}");
            ClearCounters();
            return null;
        }
    }

    private void RemoveStaleCounters(HashSet<string> currentInstances)
    {
        foreach (var staleInstance in _usageCounters.Keys.Where(instance => !currentInstances.Contains(instance)).ToList())
        {
            _usageCounters[staleInstance].Dispose();
            _usageCounters.Remove(staleInstance);
            _limitCounters[staleInstance].Dispose();
            _limitCounters.Remove(staleInstance);
        }
    }

    private void AddNewCounters(HashSet<string> currentInstances)
    {
        foreach (var instanceName in currentInstances)
        {
            if (!_usageCounters.ContainsKey(instanceName))
            {
                _usageCounters[instanceName] = new PerformanceCounter(CategoryName, DedicatedUsageCounterName, instanceName, readOnly: true);
                _limitCounters[instanceName] = new PerformanceCounter(CategoryName, DedicatedLimitCounterName, instanceName, readOnly: true);
            }
        }
    }

    public void Dispose() => ClearCounters();

    private void ClearCounters()
    {
        foreach (var counter in _usageCounters.Values)
        {
            counter.Dispose();
        }

        foreach (var counter in _limitCounters.Values)
        {
            counter.Dispose();
        }

        _usageCounters.Clear();
        _limitCounters.Clear();
    }
}
