using System.Runtime.InteropServices;

namespace WsSystemInfoWidget.Widget.Com;

/// <summary>
/// COM registration P/Invokes. Uses classic <see cref="DllImportAttribute"/>
/// rather than source-generated <c>LibraryImport</c> because IUnknown object
/// marshaling (<see cref="UnmanagedType.IUnknown"/>) isn't supported by the
/// source-generated marshaller.
/// </summary>
internal static class OleNativeMethods
{
    /// <summary>CLSCTX_LOCAL_SERVER: the class object is served out of this process.</summary>
    internal const uint ClsctxLocalServer = 0x4;

    /// <summary>REGCLS_MULTIPLEUSE: one registered instance serves multiple activation requests.</summary>
    internal const uint RegclsMultipleUse = 0x1;

    [DllImport("ole32.dll")]
    internal static extern int CoRegisterClassObject(
        [MarshalAs(UnmanagedType.LPStruct)] Guid classId,
        [MarshalAs(UnmanagedType.IUnknown)] object classFactory,
        uint classContext,
        uint flags,
        out uint registrationCookie);

    [DllImport("ole32.dll")]
    internal static extern int CoRevokeClassObject(uint registrationCookie);
}
