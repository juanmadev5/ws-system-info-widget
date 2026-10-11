using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Formatting;

namespace WsSystemInfoWidget.Core.Tests.Formatting;

public sealed class UsageSeverityClassifierTests
{
    [Theory]
    [InlineData(0d, UsageSeverity.Normal)]
    [InlineData(69.9d, UsageSeverity.Normal)]
    [InlineData(70d, UsageSeverity.Elevated)]
    [InlineData(89.9d, UsageSeverity.Elevated)]
    [InlineData(90d, UsageSeverity.Critical)]
    [InlineData(100d, UsageSeverity.Critical)]
    public void Classify_MapsPercentageToExpectedSeverity(double usagePercentage, UsageSeverity expected)
    {
        var actual = UsageSeverityClassifier.Classify(usagePercentage);

        Assert.Equal(expected, actual);
    }
}
