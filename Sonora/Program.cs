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
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

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
        public HostInterface host;
    }

    public static int HostedMain(IntPtr arg, int size)
    {
        if (size < Marshal.SizeOf(typeof(HostedMainArgs)))
        {
            return 1;
        }

        var hostedArgs = Marshal.PtrToStructure<HostedMainArgs>(arg);
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

        hostedArgs.host.Hello();

        Main(args);
        return 0;
    }
}
