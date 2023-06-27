using System.Runtime.InteropServices;

namespace Sonora;

[StructLayout(LayoutKind.Sequential)]
public struct HostInterface
{
    public delegate void FnHello();

    public FnHello Hello;
}