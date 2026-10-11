using System.Runtime.InteropServices;
using Microsoft.Windows.Widgets.Providers;
using WinRT;

namespace WsSystemInfoWidget.Widget.Com;

/// <summary>
/// <see cref="IClassFactory"/> implementation that hands the widget host a
/// <see cref="IWidgetProvider"/> instance built by <paramref name="createProvider"/>,
/// so the provider itself can be constructed through dependency injection
/// instead of requiring a parameterless constructor.
/// </summary>
[ComVisible(true)]
internal sealed class WidgetProviderClassFactory(Func<IWidgetProvider> createProvider) : IClassFactory
{
    private const int ClassNotAggregableHResult = -2147221232; // CLASS_E_NOAGGREGATION
    private const int NoInterfaceHResult = -2147467262; // E_NOINTERFACE

    public int CreateInstance(IntPtr unknownOuter, ref Guid interfaceId, out IntPtr instance)
    {
        instance = IntPtr.Zero;

        if (unknownOuter != IntPtr.Zero)
        {
            Marshal.ThrowExceptionForHR(ClassNotAggregableHResult);
        }

        if (interfaceId == typeof(IWidgetProvider).GUID || interfaceId == Guid.Parse(NativeGuids.IUnknown))
        {
            instance = MarshalInspectable<IWidgetProvider>.FromManaged(createProvider());
        }
        else
        {
            Marshal.ThrowExceptionForHR(NoInterfaceHResult);
        }

        return 0;
    }

    public int LockServer(bool shouldLock) => 0;
}
