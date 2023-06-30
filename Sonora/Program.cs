using Avalonia;
using Avalonia.ReactiveUI;
using System.Runtime.InteropServices;

namespace Sonora;

public class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        if (!HostInterface.Instance.IsValid)
        {
            HostInterfaceLoader.Load();
        }
#else
        if (!HostInterface.Instance.IsValid)
        {
            throw new NotSupportedException("HostInterface not present; must be loaded by the Sonora native host");
        }
#endif

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI()
            .LogToTrace();

    [StructLayout(LayoutKind.Sequential)]
    public struct HostedMainArgs
    {
        public int argc;
        public IntPtr argv;

        public delegate void FnGetHostInterface(ref HostInterface host);
        public FnGetHostInterface GetHostInterface;
    }

    public static int HostedMain(IntPtr arg, int size)
    {
        if (size < Marshal.SizeOf(typeof(HostedMainArgs)))
        {
            return 1;
        }

        var hostedArgs = Marshal.PtrToStructure<HostedMainArgs>(arg);

        // Copy args from C++ to C# string[].
        var argvp = new IntPtr[hostedArgs.argc];
        Marshal.Copy(hostedArgs.argv, argvp, 0, hostedArgs.argc);
        var args = new string[hostedArgs.argc];
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            for (int i = 0; i < hostedArgs.argc; i++)
            {
                args[i] = Marshal.PtrToStringUni(argvp[i])!;
            }
        }
        else
        {
            for (int i = 0; i < hostedArgs.argc; i++)
            {
                args[i] = Marshal.PtrToStringUTF8(argvp[i])!;
            }
        }

        hostedArgs.GetHostInterface(ref HostInterface._instance);

        Main(args);
        return 0;
    }
}
