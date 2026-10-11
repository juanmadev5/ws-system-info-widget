using System.Reflection;

namespace WsSystemInfoWidget.Widget.AdaptiveCards;

/// <summary>
/// Loads the widget's Adaptive Card template JSON, embedded as a resource so
/// it's not a magic inline string and is only read from disk/assembly once.
/// </summary>
public static class AdaptiveCardTemplateProvider
{
    private const string TemplateResourceName = "WsSystemInfoWidget.Widget.AdaptiveCards.Templates.widget-template.json";

    private static readonly Lazy<string> TemplateJson = new(LoadTemplate);

    public static string GetTemplateJson() => TemplateJson.Value;

    private static string LoadTemplate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(TemplateResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{TemplateResourceName}' was not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
