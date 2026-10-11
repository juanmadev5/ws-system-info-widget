using System.Globalization;

namespace WsSystemInfoWidget.Core.Abstractions;

/// <summary>
/// Resolves user-facing UI strings for the currently selected display culture.
/// Implementations detect the Windows system UI language at startup and fall
/// back to a default language when no matching resource set exists.
/// No UI-facing string may be hardcoded in a ViewModel or mapper; it must be
/// resolved through this service instead.
/// </summary>
public interface ILocalizationService
{
    /// <summary>The culture this service is currently resolving strings for.</summary>
    CultureInfo CurrentCulture { get; }

    /// <summary>
    /// Resolves the localized string for <paramref name="resourceKey"/>.
    /// </summary>
    string GetString(string resourceKey);
}
