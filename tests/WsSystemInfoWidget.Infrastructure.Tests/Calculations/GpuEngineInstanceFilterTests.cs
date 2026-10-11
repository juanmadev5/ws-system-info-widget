using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.Tests.Calculations;

public sealed class GpuEngineInstanceFilterTests
{
    [Theory]
    [InlineData("pid_1234_luid_0x00000000_0x0000ABCD_phys_0_eng_0_engtype_3D", true)]
    [InlineData("pid_1234_luid_0x00000000_0x0000ABCD_phys_0_eng_1_engtype_Copy", false)]
    [InlineData("pid_5678_luid_0x00000000_0x0000ABCD_phys_0_eng_2_engtype_VideoDecode", false)]
    public void IsGraphicsEngineInstance_MatchesOnly3DEngines(string instanceName, bool expected)
    {
        var actual = GpuEngineInstanceFilter.IsGraphicsEngineInstance(instanceName);

        Assert.Equal(expected, actual);
    }
}
