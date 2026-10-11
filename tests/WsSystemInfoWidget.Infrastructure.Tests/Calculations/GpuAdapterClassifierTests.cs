using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.Tests.Calculations;

public sealed class GpuAdapterClassifierTests
{
    [Theory]
    [InlineData(0d, GpuType.Integrated)]
    [InlineData(128d * 1024 * 1024, GpuType.Integrated)]
    [InlineData(GpuAdapterClassifier.DedicatedVramThresholdBytes - 1, GpuType.Integrated)]
    [InlineData(GpuAdapterClassifier.DedicatedVramThresholdBytes, GpuType.Dedicated)]
    [InlineData(24d * 1024 * 1024 * 1024, GpuType.Dedicated)]
    public void Classify_UsesDedicatedVramThreshold(double dedicatedLimitBytes, GpuType expected)
    {
        var actual = GpuAdapterClassifier.Classify(dedicatedLimitBytes);

        Assert.Equal(expected, actual);
    }
}
