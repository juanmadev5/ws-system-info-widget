using CommunityToolkit.Mvvm.ComponentModel;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Core.Enums;
using WsSystemInfoWidget.Core.Models;

namespace WsSystemInfoWidget.Widget.ViewModels;

/// <summary>
/// Holds the latest polled system data for one pinned widget instance.
/// Deliberately free of any Adaptive-Card-specific serialization; mapping to
/// the JSON sent to the widget host is <see cref="AdaptiveCards.WidgetDataMapper"/>'s job.
/// </summary>
public sealed partial class SystemInfoWidgetViewModel : ObservableObject
{
    private readonly ISystemInfoSnapshotProvider _snapshotProvider;

    [ObservableProperty]
    public partial SystemInfoSnapshot? Snapshot { get; set; }

    [ObservableProperty]
    public partial WidgetSize Size { get; set; } = WidgetSize.Small;

    public SystemInfoWidgetViewModel(ISystemInfoSnapshotProvider snapshotProvider)
    {
        _snapshotProvider = snapshotProvider;
    }

    public async Task RefreshAsync(CancellationToken cancellationToken)
    {
        Snapshot = await _snapshotProvider.GetSnapshotAsync(cancellationToken).ConfigureAwait(false);
    }
}
