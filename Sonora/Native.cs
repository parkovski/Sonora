using System.Runtime.InteropServices;

namespace Sonora;

public enum MidiDir {
    In,
    Out,
}

[StructLayout(LayoutKind.Sequential)]
public class MidiPortInfo {
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public required string Name;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public required string Id;
    public ulong Number;
    public nint Port;
    public MidiDir Dir;
}

public static partial class Native {
#region Startup / Shutdown
    [LibraryImport("Sonora.Native")]
    public static partial void SnrStartup();

    [LibraryImport("Sonora.Native")]
    public static partial void SnrShutdown();
#endregion

#region Logging
    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial void SnrLogTrace(string s);

    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial void SnrLogDebug(string s);

    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial void SnrLogInfo(string s);

    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial void SnrLogWarn(string s);

    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial void SnrLogError(string s);

    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial void SnrLogCritical(string s);
#endregion

#region Wave files
    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint SnrWaveNewFromFile(string filename);

    [LibraryImport("Sonora.Native")]
    public static partial void SnrWaveFree(nint wave);

    [LibraryImport("Sonora.Native")]
    public static partial ulong SnrWaveGetFrames(nint wave);

    [LibraryImport("Sonora.Native")]
    public static partial uint SnrWaveGetChannels(nint wave);

    [LibraryImport("Sonora.Native")]
    public static partial ulong SnrWaveReadFrames(
        nint wave,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] float[] data,
        ulong size
    );
#endregion

#region MIDI
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MidiPortAdded(nint receiver, [In] MidiPortInfo info);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MidiPortRemoved(nint receiver, [In] MidiPortInfo info);

    [LibraryImport("Sonora.Native")]
    public static partial nint SnrMidiStateNew(
        nint receiver,
        MidiPortAdded added,
        MidiPortRemoved removed
    );

    [LibraryImport("Sonora.Native")]
    public static partial nint SnrMidiStateFree(nint state);
#endregion
}
