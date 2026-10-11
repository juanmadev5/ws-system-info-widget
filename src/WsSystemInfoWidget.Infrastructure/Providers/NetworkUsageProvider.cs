using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Localization;
using WsSystemInfoWidget.Core.Models;
using WsSystemInfoWidget.Infrastructure.Calculations;

namespace WsSystemInfoWidget.Infrastructure.Providers;

/// <summary>
/// Reads upload and download throughput from the interface Windows routes Internet traffic through.
/// </summary>
public sealed class NetworkUsageProvider : INetworkUsageProvider
{
    private string? _lastInterfaceId;
    private long _lastBytesSent;
    private long _lastBytesReceived;
    private DateTime _lastSampleTimeUtc;
    private readonly ILocalizationService _localization;

    public NetworkUsageProvider(ILocalizationService localization) => _localization = localization;

    public Task<NetworkUsageSnapshot> GetUsageAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var activeInterface = SelectActiveInterface();
        if (activeInterface is null)
        {
            var emptySnapshot = new NetworkUsageSnapshot(0d, 0d, _localization.GetString(ResourceKeys.NetworkUnavailable));
            return Task.FromResult(emptySnapshot);
        }

        var stats = activeInterface.GetIPv4Statistics();
        var nowUtc = DateTime.UtcNow;

        var uploadBytesPerSecond = 0d;
        var downloadBytesPerSecond = 0d;
        if (_lastInterfaceId == activeInterface.Id)
        {
            var elapsed = nowUtc - _lastSampleTimeUtc;
            uploadBytesPerSecond = NetworkUtilizationCalculator.CalculateBytesPerSecond(
                stats.BytesSent - _lastBytesSent, elapsed);
            downloadBytesPerSecond = NetworkUtilizationCalculator.CalculateBytesPerSecond(
                stats.BytesReceived - _lastBytesReceived, elapsed);
        }

        _lastInterfaceId = activeInterface.Id;
        _lastBytesSent = stats.BytesSent;
        _lastBytesReceived = stats.BytesReceived;
        _lastSampleTimeUtc = nowUtc;

        var snapshot = new NetworkUsageSnapshot(uploadBytesPerSecond, downloadBytesPerSecond, activeInterface.Description);
        return Task.FromResult(snapshot);
    }

    private static NetworkInterface? SelectActiveInterface()
    {
        var candidates = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(networkInterface =>
                networkInterface.OperationalStatus == OperationalStatus.Up &&
                networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                networkInterface.Speed > 0)
            .ToList();

        // Use Windows' routing table for a public IPv4 destination instead of choosing
        // a faster-but-unused virtual adapter (for example, Hyper-V) by link speed.
        if (GetBestInterface(0x01010101, out var bestInterfaceIndex) == 0)
        {
            var routedInterface = candidates.FirstOrDefault(networkInterface =>
                networkInterface.GetIPProperties().GetIPv4Properties()?.Index == bestInterfaceIndex);
            if (routedInterface is not null)
            {
                return routedInterface;
            }
        }

        // Fallback for systems where IPv4 route lookup is unavailable.
        return candidates
            .Where(networkInterface => networkInterface.GetIPProperties().GatewayAddresses
                .Any(gateway => !gateway.Address.Equals(System.Net.IPAddress.Any) &&
                                !gateway.Address.Equals(System.Net.IPAddress.IPv6Any)))
            .OrderByDescending(networkInterface => networkInterface.Speed)
            .FirstOrDefault();
    }

    [DllImport("iphlpapi.dll", ExactSpelling = true)]
    private static extern uint GetBestInterface(uint destinationAddress, out uint bestInterfaceIndex);
}
