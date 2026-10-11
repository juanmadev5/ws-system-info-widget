using WsSystemInfoWidget.Core.Formatting;

namespace WsSystemInfoWidget.Core.Tests.Formatting;

public sealed class UsageDisplayFormatterTests
{
    [Theory]
    [InlineData(0d, "0%")]
    [InlineData(42d, "42%")]
    [InlineData(42.4d, "42%")]
    [InlineData(42.5d, "43%")]
    [InlineData(100d, "100%")]
    public void FormatPercentage_RoundsToWholeNumberWithSuffix(double usagePercentage, string expected)
    {
        var actual = UsageDisplayFormatter.FormatPercentage(usagePercentage);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-10d, "0%")]
    [InlineData(150d, "100%")]
    public void FormatPercentage_ClampsOutOfRangeValues(double usagePercentage, string expected)
    {
        var actual = UsageDisplayFormatter.FormatPercentage(usagePercentage);

        Assert.Equal(expected, actual);
    }
}
