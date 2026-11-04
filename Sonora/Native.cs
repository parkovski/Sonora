using System.Runtime.InteropServices;

namespace Sonora;

public enum MidiDirection {
    In,
    Out,
}

[StructLayout(LayoutKind.Sequential)]
public class MidiPortInfo {
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public required string _name;
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public required string _id;
    public ulong _number;
    public nint _port;
    public MidiDirection _direction;

    public string Name {
        get => _name;
        set => _name = value;
    }

    public string Id {
        get => _id;
        set => _id = value;
    }

    public ulong Number {
        get => _number;
        set => _number = value;
    }

    public nint Port {
        get => _port;
        set => _port = value;
    }

    public MidiDirection Direction {
        get => _direction;
        set => _direction = value;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct UMP {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public uint[] Data;
    public long Timestamp;
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

    [LibraryImport("Sonora.Native")]
    public static partial void SnrMidiStateListen(nint state);

    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool SnrMidiStateOpenPort(
        nint state, MidiDirection direction, string id
    );

    [LibraryImport("Sonora.Native",
                   StringMarshalling = StringMarshalling.Utf8)]
    public static partial void SnrMidiStateClosePort(
        nint state, MidiDirection direction, string id
    );
#endregion
}
