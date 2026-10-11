using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Microsoft.Windows.Widgets;
using Microsoft.Windows.Widgets.Providers;
using Microsoft.Extensions.DependencyInjection;
using WsSystemInfoWidget.Core.Abstractions;
using WsSystemInfoWidget.Infrastructure.DependencyInjection;
using WsSystemInfoWidget.Widget.Com;
using WsSystemInfoWidget.Widget.Constants;
using WsSystemInfoWidget.Widget.Providers;
using WsSystemInfoWidget.Widget.Services;

[assembly: SupportedOSPlatform("Windows10.0.17763")]

var services = new ServiceCollection();
services.AddSystemInfoInfrastructure();
services.AddSingleton<ILocalizationService, LocalizationService>();
// Registrar la lista de WidgetInfo obtenida desde WidgetManager en DI para inyectarla
services.AddSingleton<IEnumerable<WidgetInfo>>(sp =>
{
    var infos = WidgetManager.GetDefault()?.GetWidgetInfos();
    return infos ?? Enumerable.Empty<WidgetInfo>();
});

services.AddSingleton<SystemInfoWidgetProvider>();

using var serviceProvider = services.BuildServiceProvider();
var widgetProvider = serviceProvider.GetRequiredService<SystemInfoWidgetProvider>();

var classId = Guid.Parse(WidgetProviderConstants.ProviderClsid);
var classFactory = new WidgetProviderClassFactory(() => widgetProvider);

var registrationResult = OleNativeMethods.CoRegisterClassObject(
    classId,
    classFactory,
    OleNativeMethods.ClsctxLocalServer,
    OleNativeMethods.RegclsMultipleUse,
    out var registrationCookie);

if (registrationResult != 0)
{
    Marshal.ThrowExceptionForHR(registrationResult);
}

// Keep the process alive only while the widget host has pinned widgets;
// exit once the last one is unpinned so the provider isn't idling in the background.
widgetProvider.EmptyWidgetListEvent.Wait();

OleNativeMethods.CoRevokeClassObject(registrationCookie);
