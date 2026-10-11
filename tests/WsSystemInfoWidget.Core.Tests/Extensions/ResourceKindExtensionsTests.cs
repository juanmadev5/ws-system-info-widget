using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Extensions;
using WsSystemInfoWidget.Core.Localization;

namespace WsSystemInfoWidget.Core.Tests.Extensions;

public sealed class ResourceKindExtensionsTests
{
    [Theory]
    [InlineData(ResourceKind.Cpu, ResourceKeys.CpuCardTitle)]
    [InlineData(ResourceKind.Ram, ResourceKeys.RamCardTitle)]
    [InlineData(ResourceKind.Gpu, ResourceKeys.GpuCardTitle)]
    [InlineData(ResourceKind.Network, ResourceKeys.NetworkCardTitle)]
    [InlineData(ResourceKind.Disk, ResourceKeys.DiskCardTitle)]
    [InlineData(ResourceKind.Vram, ResourceKeys.VramCardTitle)]
    public void GetTitleResourceKey_MapsEachKindToItsOwnKey(ResourceKind kind, string expectedKey)
    {
        var actual = kind.GetTitleResourceKey();

        Assert.Equal(expectedKey, actual);
    }
}
