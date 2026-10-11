using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.Tests.Calculations;

public sealed class NetworkUtilizationCalculatorTests
{
    [Fact]
    public void CalculateBytesPerSecond_DividesDeltaByElapsedSeconds()
    {
        var actual = NetworkUtilizationCalculator.CalculateBytesPerSecond(1_000_000, TimeSpan.FromSeconds(2));

        Assert.Equal(500_000d, actual);
    }

    [Fact]
    public void CalculateBytesPerSecond_NegativeDeltaTreatedAsZero()
    {
        var actual = NetworkUtilizationCalculator.CalculateBytesPerSecond(-10, TimeSpan.FromSeconds(1));

        Assert.Equal(0d, actual);
    }

    [Fact]
    public void CalculateBytesPerSecond_ZeroElapsedTreatedAsZero()
    {
        var actual = NetworkUtilizationCalculator.CalculateBytesPerSecond(1000, TimeSpan.Zero);

        Assert.Equal(0d, actual);
    }

    [Theory]
    [InlineData(0d, 1_000_000d, 0d)]
    [InlineData(125_000d, 1_000_000d, 100d)]
    [InlineData(62_500d, 1_000_000d, 50d)]
    [InlineData(1_000_000d, 1_000_000d, 100d)]
    [InlineData(1000d, 0d, 0d)]
    public void CalculateUtilizationPercentage_ReturnsClampedPercentageOfLinkSpeed(
        double bytesPerSecond, double linkSpeedBitsPerSecond, double expected)
    {
        var actual = NetworkUtilizationCalculator.CalculateUtilizationPercentage(bytesPerSecond, linkSpeedBitsPerSecond);

        Assert.Equal(expected, actual);
    }
}
