using System.Runtime.InteropServices;

namespace Sonora;

[StructLayout(LayoutKind.Sequential)]
public struct HostInterface
{
    internal static HostInterface _instance;
    public static ref readonly HostInterface Instance { get => ref _instance; }

    public readonly bool IsValid => IsHosted != null;

    public delegate bool FnIsHosted();

    public delegate nint FnWaveNewFromFile([MarshalAs(UnmanagedType.LPUTF8Str)] string filename);
    public delegate void FnWaveFree(nint wave);
    public delegate ulong FnWaveGetFrames(nint wave);
    public delegate uint FnWaveGetChannels(nint wave);
    public delegate ulong FnWaveReadFrames(nint wave, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] float[] data, ulong size);

    public delegate nint FnAudioContextNew();
    public delegate void FnAudioContextFree(nint ctx);

    public delegate nint FnDeviceEnumeratorNew(nint ctx);
    public delegate void FnDeviceEnumeratorFree(nint devEnum);
    public delegate uint FnDeviceEnumeratorCount(nint devEnum, bool playback);
    // Returns a string but marshalling tries to free it, so it must be converted manually
    // via Marshal.PtrToStringUTF8.
    public delegate nint FnDeviceGetName(nint devEnum, bool playback, uint index);

    public FnIsHosted IsHosted;

    public FnWaveNewFromFile WaveNewFromFile;
    public FnWaveFree WaveFree;
    public FnWaveGetFrames WaveGetFrames;
    public FnWaveGetChannels WaveGetChannels;
    public FnWaveReadFrames WaveReadFrames;

    public FnAudioContextNew AudioContextNew;
    public FnAudioContextFree AudioContextFree;

    public FnDeviceEnumeratorNew DeviceEnumeratorNew;
    public FnDeviceEnumeratorFree DeviceEnumeratorFree;
    public FnDeviceEnumeratorCount DeviceEnumeratorCount;
    public FnDeviceGetName DeviceGetName;
}

public static class HostInterfaceLoader
{
#if DEBUG
    [DllImport("../../../../native/build/bin/sonoradbg")]
    private static extern void GetHostInterface(ref HostInterface host);
#endif

    public static void Load()
    {
#if DEBUG
        GetHostInterface(ref HostInterface._instance);
#else
        throw new NotSupportedException("Release builds must be run from the native host");
#endif
    }
}
