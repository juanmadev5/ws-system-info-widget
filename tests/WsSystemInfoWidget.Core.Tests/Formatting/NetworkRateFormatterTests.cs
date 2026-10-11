using WsSystemInfoWidget.Core.Formatting;

namespace WsSystemInfoWidget.Core.Tests.Formatting;

public sealed class NetworkRateFormatterTests
{
    [Fact]
    public void FormatBytesPerSecond_UsesKilobytesBelowOneMegabyte()
    {
        var result = NetworkRateFormatter.FormatBytesPerSecond(1536d);

        Assert.EndsWith("KB/s", result);
        Assert.Contains("1.5", result.Replace(',', '.'));
    }

    [Fact]
    public void FormatBytesPerSecond_UsesMegabytesAtOneMegabyte()
    {
        var result = NetworkRateFormatter.FormatBytesPerSecond(1_572_864d);

        Assert.EndsWith("MB/s", result);
        Assert.Contains("1.5", result.Replace(',', '.'));
    }
}