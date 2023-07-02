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

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--device" && i < args.Length - 1)
            {
                if (uint.TryParse(args[i + 1], out uint value))
                {
                    ref readonly var host = ref HostInterface.Instance;
                    var auctx = host.AudioContextNew();
                    var devEnum = host.DeviceEnumeratorNew(auctx);
                    var dev = host.DeviceNew(auctx, devEnum, true, value, HostInterface.AudioFormat.F32, 2, 48000);
                    Console.WriteLine("Press enter to start device.");
                    Console.In.ReadLine();
                    host.DeviceStart(dev);
                    Console.WriteLine("Type value 0-3 to change osc, or press enter to stop device.");
                    string? line = string.Empty;
                    while (true)
                    {
                        line = Console.In.ReadLine();
                        if (string.IsNullOrEmpty(line)) break;
                        if (int.TryParse(line, out int osc))
                        {
                            host.DeviceSetOsc(dev, osc);
                        }
                    }
                    host.DeviceStop(dev);
                    host.DeviceFree(dev);
                    host.DeviceEnumeratorFree(devEnum);
                    host.AudioContextFree(auctx);
                    return 0;
                }
            }
            else if (args[i] == "--list-devices")
            {
                ref readonly var host = ref HostInterface.Instance;
                var auctx = host.AudioContextNew();
                var devEnum = host.DeviceEnumeratorNew(auctx);
                uint count = host.DeviceEnumeratorCount(devEnum, true);
                for (uint n = 0; n < count; n++)
                {
                    var name = Marshal.PtrToStringUTF8(host.DeviceEnumeratorGetDeviceName(devEnum, true, n));
                    Console.WriteLine($"{n}: {name}");
                }
                host.DeviceEnumeratorFree(devEnum);
                host.AudioContextFree(auctx);
                return 0;
            }
        }

        Main(args);
        return 0;
    }
}
