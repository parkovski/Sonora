using System.Runtime.InteropServices;

namespace Sonora;

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
}
