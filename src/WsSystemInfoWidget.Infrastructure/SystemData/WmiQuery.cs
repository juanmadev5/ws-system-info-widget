using System.Management;

namespace WsSystemInfoWidget.Infrastructure.SystemData;

/// <summary>
/// Thin helper around <see cref="ManagementObjectSearcher"/> shared by every
/// provider that needs a one-shot WMI lookup (hardware names/specs rarely
/// change at runtime, so these are not polled on every tick).
/// </summary>
internal static class WmiQuery
{
    internal static IEnumerable<ManagementBaseObject> Select(string wqlQuery)
    {
        using var searcher = new ManagementObjectSearcher(wqlQuery);
        using var results = searcher.Get();
        foreach (var result in results)
        {
            // Each ManagementBaseObject wraps a native COM object; dispose it
            // as soon as the caller is done reading it for this iteration
            // instead of leaving it for finalization.
            using (result)
            {
                yield return result;
            }
        }
    }

    internal static T? GetPropertyOrDefault<T>(this ManagementBaseObject managementObject, string propertyName, T? defaultValue = default)
    {
        var value = managementObject[propertyName];
        return value is T typedValue ? typedValue : defaultValue;
    }
}
