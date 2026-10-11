using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.Tests.Calculations;

public sealed class MemorySpecFormatterTests
{
    [Theory]
    [InlineData(SmbiosMemoryType.Ddr3, "DDR3")]
    [InlineData(SmbiosMemoryType.Ddr4, "DDR4")]
    [InlineData(SmbiosMemoryType.Ddr5, "DDR5")]
    [InlineData(SmbiosMemoryType.Unknown, "RAM")]
    public void MapMemoryTypeLabel_ReturnsExpectedLabel(SmbiosMemoryType memoryType, string expected)
    {
        var actual = MemorySpecFormatter.MapMemoryTypeLabel(memoryType);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatSpec_IncludesSpeedWhenPositive()
    {
        var actual = MemorySpecFormatter.FormatSpec(SmbiosMemoryType.Ddr4, 3200);

        Assert.Equal("DDR4\n3200 MT/s", actual);
    }

    [Fact]
    public void FormatSpec_OmitsSpeedWhenNotPositive()
    {
        var actual = MemorySpecFormatter.FormatSpec(SmbiosMemoryType.Ddr4, 0);

        Assert.Equal("DDR4", actual);
    }
}
