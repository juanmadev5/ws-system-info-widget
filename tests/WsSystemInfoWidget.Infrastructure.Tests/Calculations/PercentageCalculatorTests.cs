using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.Tests.Calculations;

public sealed class PercentageCalculatorTests
{
    [Theory]
    [InlineData(0d, 100d, 0d)]
    [InlineData(50d, 100d, 50d)]
    [InlineData(100d, 100d, 100d)]
    [InlineData(150d, 100d, 100d)]
    [InlineData(25d, 0d, 0d)]
    public void CalculateUsedPercentage_ReturnsClampedPercentage(double used, double total, double expected)
    {
        var actual = PercentageCalculator.CalculateUsedPercentage(used, total);

        Assert.Equal(expected, actual);
    }
}
