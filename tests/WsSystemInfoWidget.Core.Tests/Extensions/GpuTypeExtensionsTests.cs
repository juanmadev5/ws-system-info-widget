using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Extensions;

namespace WsSystemInfoWidget.Core.Tests.Extensions;

public sealed class GpuTypeExtensionsTests
{
    [Theory]
    [InlineData(GpuType.Dedicated, true)]
    [InlineData(GpuType.Integrated, false)]
    [InlineData(GpuType.Unknown, false)]
    public void HasDedicatedVram_ReflectsAdapterType(GpuType gpuType, bool expected)
    {
        var actual = gpuType.HasDedicatedVram();

        Assert.Equal(expected, actual);
    }
}
