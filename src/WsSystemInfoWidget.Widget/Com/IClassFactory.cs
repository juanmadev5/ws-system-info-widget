using System.Runtime.InteropServices;

namespace WsSystemInfoWidget.Widget.Com;

[ComImport]
[ComVisible(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid(NativeGuids.IClassFactory)]
internal interface IClassFactory
{
    [PreserveSig]
    int CreateInstance(IntPtr unknownOuter, ref Guid interfaceId, out IntPtr instance);

    [PreserveSig]
    int LockServer(bool shouldLock);
}
