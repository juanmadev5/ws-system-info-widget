namespace WsSystemInfoWidget.Infrastructure.Calculations;

/// <summary>
/// DMTF SMBIOS Type 17 memory-type codes, as reported by
/// <c>Win32_PhysicalMemory.SMBIOSMemoryType</c>. Modeled as an enum instead of
/// comparing/casting the raw WMI integer at call sites.
/// </summary>
public enum SmbiosMemoryType : ushort
{
    Unknown = 0,
    Ddr = 20,
    Ddr2 = 21,
    Ddr2FbDimm = 22,
    Ddr3 = 24,
    Ddr4 = 26,
    Ddr5 = 34,
}
