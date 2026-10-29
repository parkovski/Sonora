global using Avalonia;
global using System;
global using static Sonora.Native;

using System.Runtime.InteropServices;

namespace Sonora;

sealed partial class Program
{
    [LibraryImport("Kernel32")]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static partial bool AttachConsole(int processId);

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) {
        AttachConsole(-1);
        SnrStartup();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        SnrShutdown();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();

    [StructLayout(LayoutKind.Sequential)]
    public struct HostedMainArgs {
        public int argc;
        public nint argv;
    }

    public static int HostedMain(nint arg, int size) {
        if (size < Marshal.SizeOf(typeof(HostedMainArgs))) {
            return 1;
        }

        var hostedArgs = Marshal.PtrToStructure<HostedMainArgs>(arg);
        var argvp = new nint[hostedArgs.argc];
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

        Main(args);

        return 0;
    }
}
