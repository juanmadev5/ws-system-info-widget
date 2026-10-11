using System.Diagnostics;
using Microsoft.Windows.Widgets;
using Microsoft.Windows.Widgets.Providers;
using System.Linq;
using System.Runtime.Versioning;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Widget.AdaptiveCards;

namespace WsSystemInfoWidget.Widget.Providers;

/// <summary>
/// The widget provider's COM-facing surface. Keeps the system metrics
/// polling loop running only while the Widgets Board is actually showing the
/// widget (between <see cref="Activate"/> and <see cref="Deactivate"/>), per
/// the "resource-light" constraint.
/// </summary>
[SupportedOSPlatform("Windows10.0.17763")]
public sealed class SystemInfoWidgetProvider : IWidgetProvider
{
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(2);

    private readonly ISystemInfoSnapshotProvider _snapshotProvider;
    private readonly ILocalizationService _localizationService;
    private readonly Dictionary<string, WidgetInstance> _widgets = new(StringComparer.Ordinal);

    public SystemInfoWidgetProvider(IEnumerable<WidgetInfo> widgetInfos, ISystemInfoSnapshotProvider snapshotProvider, ILocalizationService localizationService)
    {
        _snapshotProvider = snapshotProvider;
        _localizationService = localizationService;

        foreach (var widgetInfo in widgetInfos ?? [])
        {
            _widgets[widgetInfo.WidgetContext.Id] = new WidgetInstance();
        }
    }

    /// <summary>Signaled when the last pinned widget is removed, so the host process can exit.</summary>
    public ManualResetEventSlim EmptyWidgetListEvent { get; } = new(initialState: false);

    public void CreateWidget(WidgetContext widgetContext)
    {
        _widgets[widgetContext.Id] = new WidgetInstance();
        _ = UpdateWidgetAsync(widgetContext.Id);
    }

    public void DeleteWidget(string widgetId, string customState)
    {
        if (_widgets.Remove(widgetId, out var instance))
        {
            instance.StopPolling();
        }

        if (_widgets.Count == 0)
        {
            EmptyWidgetListEvent.Set();
        }
    }

    public void OnActionInvoked(WidgetActionInvokedArgs actionInvokedArgs)
    {
        // No interactive actions are defined for this widget in v1.
    }

    public void OnWidgetContextChanged(WidgetContextChangedArgs contextChangedArgs)
    {
        var context = contextChangedArgs.WidgetContext;
        if (_widgets.ContainsKey(context.Id))
        {
            _ = UpdateWidgetAsync(context.Id);
        }
    }

    public void Activate(WidgetContext widgetContext)
    {
        if (!_widgets.TryGetValue(widgetContext.Id, out var instance))
        {
            return;
        }

        instance.StartPolling(cancellationToken => PollWhileActiveAsync(widgetContext.Id, cancellationToken));
    }

    public void Deactivate(string widgetId)
    {
        if (_widgets.TryGetValue(widgetId, out var instance))
        {
            instance.StopPolling();
        }
    }

    private async Task PollWhileActiveAsync(string widgetId, CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(PollingInterval);
        do
        {
            await UpdateWidgetAsync(widgetId).ConfigureAwait(false);
        }
        while (!cancellationToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false));
    }

    private async Task UpdateWidgetAsync(string widgetId)
    {
        try
        {
            if (!_widgets.TryGetValue(widgetId, out var instance))
            {
                return;
            }

            var snapshot = await _snapshotProvider.GetSnapshotAsync(CancellationToken.None).ConfigureAwait(false);
            var dataJson = WidgetDataMapper.BuildDataJson(snapshot, _localizationService);

            var updateOptions = new WidgetUpdateRequestOptions(widgetId)
            {
                Template = AdaptiveCardTemplateProvider.GetTemplateJson(),
                Data = dataJson,
            };

            WidgetManager.GetDefault().UpdateWidget(updateOptions);
        }
        catch (Exception exception)
        {
            // UpdateWidgetAsync is fire-and-forget from COM callbacks; log the full cause
            // instead of losing it as an unobserved Task exception.
            Debug.WriteLine($"Widget update failed for '{widgetId}': {exception}");
        }
    }

    private sealed class WidgetInstance
    {
        private CancellationTokenSource? _pollingCancellation;

        public void StartPolling(Func<CancellationToken, Task> runPollingLoop)
        {
            StopPolling();
            var cancellation = new CancellationTokenSource();
            _pollingCancellation = cancellation;
            _ = runPollingLoop(cancellation.Token);
        }

        public void StopPolling()
        {
            _pollingCancellation?.Cancel();
            _pollingCancellation?.Dispose();
            _pollingCancellation = null;
        }
    }
}
