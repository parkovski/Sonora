using System.Runtime.InteropServices;

namespace Sonora;

[StructLayout(LayoutKind.Sequential)]
public struct HostInterface
{
    internal static HostInterface _instance;
    public static ref readonly HostInterface Instance { get => ref _instance; }

    public readonly bool IsValid => IsHosted != null;

    #region Is hosted

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool FnIsHosted();

    #endregion

    #region Wave

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint FnWaveNewFromFile(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filename);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FnWaveFree(nint wave);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate ulong FnWaveGetFrames(nint wave);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint FnWaveGetChannels(nint wave);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate ulong FnWaveReadFrames(
        nint wave,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] float[] data,
        ulong size);

    #endregion

    #region Audio context

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint FnAudioContextNew();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FnAudioContextFree(nint ctx);

    #endregion

    #region Device enumeration

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint FnDeviceEnumeratorNew(nint ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FnDeviceEnumeratorFree(nint devEnum);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint FnDeviceEnumeratorCount(nint devEnum, bool playback);

    // Returns a string but marshalling tries to free it, so it must be converted manually
    // via Marshal.PtrToStringUTF8.
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint FnDeviceEnumeratorGetDeviceName(
        nint devEnum, bool playback, uint index);

    #endregion

    #region Device

    // Must match ma_format in miniaudio.h
    public enum AudioFormat
    {
        Unknown = 0,
        U8      = 1,
        S16     = 2,
        S24     = 3,
        S32     = 4,
        F32     = 5,
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint FnDeviceNew(
        nint ctx, nint devEnum, bool playback, uint index,
        [MarshalAs(UnmanagedType.I4)] AudioFormat format,
        uint channels, uint sampleRate);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FnDeviceFree(nint device);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FnDeviceStart(nint device);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FnDeviceStop(nint device);

    #endregion

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
    public FnDeviceEnumeratorGetDeviceName DeviceEnumeratorGetDeviceName;

    public FnDeviceNew DeviceNew;
    public FnDeviceFree DeviceFree;
    public FnDeviceStart DeviceStart;
    public FnDeviceStop DeviceStop;
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
