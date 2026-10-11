using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Formatting;

namespace WsSystemInfoWidget.Core.Tests.Formatting;

public sealed class ByteSizeFormatterTests
{
    [Theory]
    [InlineData(0d, ByteUnit.Bytes)]
    [InlineData(1023d, ByteUnit.Bytes)]
    [InlineData(1024d, ByteUnit.Kilobytes)]
    [InlineData(1024d * 1024, ByteUnit.Megabytes)]
    [InlineData(1024d * 1024 * 1024, ByteUnit.Gigabytes)]
    [InlineData(1024d * 1024 * 1024 * 1024, ByteUnit.Terabytes)]
    public void SelectUnit_PicksLargestUnitAtOrAboveOne(double bytes, ByteUnit expected)
    {
        var actual = ByteSizeFormatter.SelectUnit(bytes);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(512d * 1024 * 1024 * 1024, "512 GB")]
    [InlineData(1024d * 1024 * 1024 * 1024, "1 TB")]
    [InlineData(1536d * 1024 * 1024, "1.5 GB")]
    public void Format_UsesBestFitUnit(double bytes, string expected)
    {
        var actual = ByteSizeFormatter.Format(bytes);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatUsedOfTotal_FormatsEachSideWithItsOwnUnit()
    {
        var used = 512d * 1024 * 1024 * 1024;
        var total = 1024d * 1024 * 1024 * 1024;

        var actual = ByteSizeFormatter.FormatUsedOfTotal(used, total);

        Assert.Equal("512 GB / 1 TB", actual);
    }
}
