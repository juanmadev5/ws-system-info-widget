using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.Tests.Calculations;

public sealed class GpuAdapterInstanceKeyExtractorTests
{
    [Fact]
    public void ExtractAdapterKey_StripsProcessIdPrefix()
    {
        const string instanceA = "pid_1234_luid_0x00000000_0x0000ABCD_phys_0_engtype_3D";
        const string instanceB = "pid_5678_luid_0x00000000_0x0000ABCD_phys_0_engtype_3D";

        var keyA = GpuAdapterInstanceKeyExtractor.ExtractAdapterKey(instanceA);
        var keyB = GpuAdapterInstanceKeyExtractor.ExtractAdapterKey(instanceB);

        Assert.Equal(keyA, keyB);
        Assert.Equal("luid_0x00000000_0x0000ABCD_phys_0_engtype_3D", keyA);
    }

    [Fact]
    public void ExtractAdapterKey_DistinctAdaptersProduceDistinctKeys()
    {
        const string gpu1 = "pid_1234_luid_0x00000000_0x0000AAAA_phys_0_engtype_3D";
        const string gpu2 = "pid_1234_luid_0x00000000_0x0000BBBB_phys_0_engtype_3D";

        var key1 = GpuAdapterInstanceKeyExtractor.ExtractAdapterKey(gpu1);
        var key2 = GpuAdapterInstanceKeyExtractor.ExtractAdapterKey(gpu2);

        Assert.NotEqual(key1, key2);
    }

    [Fact]
    public void ExtractAdapterKey_WithoutMarker_ReturnsInputUnchanged()
    {
        const string instanceName = "_Total";

        var actual = GpuAdapterInstanceKeyExtractor.ExtractAdapterKey(instanceName);

        Assert.Equal(instanceName, actual);
    }
}
