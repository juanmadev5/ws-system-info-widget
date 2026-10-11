using System.Globalization;
using Windows.ApplicationModel.Resources;
using WsSystemInfoWidget.Core.Abstractions;

namespace WsSystemInfoWidget.Widget.Services;

/// <summary>
/// Resolves UI strings from the package's per-language .resw resources.
/// <see cref="ResourceLoader"/> automatically negotiates the best match
/// between the system's preferred UI languages and the languages declared in
/// Package.appxmanifest, falling back to the package's default language
/// (English) when the system language has no matching resource set.
/// </summary>
public sealed class LocalizationService : ILocalizationService
{
    private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForViewIndependentUse();

    public CultureInfo CurrentCulture => CultureInfo.CurrentUICulture;

    public string GetString(string resourceKey) => _resourceLoader.GetString(resourceKey);
}
