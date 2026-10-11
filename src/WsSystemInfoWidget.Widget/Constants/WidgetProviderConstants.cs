namespace WsSystemInfoWidget.Widget.Constants;

/// <summary>
/// Identifiers that must stay in sync with <c>Package.appxmanifest</c> in the
/// packaging project: the COM class CLSID (<c>com:Class/@Id</c> and
/// <c>uap3:Activation/CreateInstance/@ClassId</c>) and the widget definition
/// ID (<c>Definition/@Id</c>).
/// </summary>
public static class WidgetProviderConstants
{
    /// <summary>
    /// COM CLSID used to register this app as a widget provider. Must match
    /// Package.appxmanifest exactly.
    /// </summary>
    public const string ProviderClsid = "879FD009-13D1-40C2-BDB8-0384065C9FDC";

    /// <summary>
    /// The widget definition ID declared in Package.appxmanifest. This app
    /// defines a single widget.
    /// </summary>
    public const string SystemInfoWidgetDefinitionId = "SystemInfo_Widget";
}
