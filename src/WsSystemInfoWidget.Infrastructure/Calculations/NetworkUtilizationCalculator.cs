namespace WsSystemInfoWidget.Infrastructure.Calculations;

/// <summary>
/// Pure math for turning raw interface byte counters into a utilization
/// percentage of the adapter's link speed. Kept free of any OS dependency so
/// it's directly unit-testable.
/// </summary>
public static class NetworkUtilizationCalculator
{
    private const double BitsPerByte = 8d;

    /// <summary>
    /// Throughput, in bytes/second, implied by a byte counter delta over an elapsed time span.
    /// A negative delta (counter reset, e.g. adapter restart) is treated as no throughput.
    /// </summary>
    public static double CalculateBytesPerSecond(long byteDelta, TimeSpan elapsed)
    {
        if (elapsed <= TimeSpan.Zero || byteDelta <= 0)
        {
            return 0d;
        }

        return byteDelta / elapsed.TotalSeconds;
    }

    /// <summary>
    /// Utilization percentage of <paramref name="linkSpeedBitsPerSecond"/> implied by
    /// <paramref name="bytesPerSecond"/> of throughput.
    /// </summary>
    public static double CalculateUtilizationPercentage(double bytesPerSecond, double linkSpeedBitsPerSecond)
    {
        if (linkSpeedBitsPerSecond <= 0d)
        {
            return 0d;
        }

        var bitsPerSecond = bytesPerSecond * BitsPerByte;
        var percentage = bitsPerSecond / linkSpeedBitsPerSecond * 100d;
        return Math.Clamp(percentage, 0d, 100d);
    }
}
