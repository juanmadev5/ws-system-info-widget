using System.Runtime.InteropServices;

namespace WsSystemInfoWidget.Infrastructure.Interop;

internal static partial class NativeMethods
{
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GlobalMemoryStatusEx(ref MemoryStatusEx buffer);
}

[StructLayout(LayoutKind.Sequential)]
internal struct MemoryStatusEx
{
    public uint Length;
    public uint MemoryLoad;
    public ulong TotalPhysicalBytes;
    public ulong AvailablePhysicalBytes;
    public ulong TotalPageFileBytes;
    public ulong AvailablePageFileBytes;
    public ulong TotalVirtualBytes;
    public ulong AvailableVirtualBytes;
    public ulong AvailableExtendedVirtualBytes;

    public static MemoryStatusEx Create() => new()
    {
        Length = (uint)Marshal.SizeOf<MemoryStatusEx>(),
    };
}
