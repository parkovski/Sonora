using System.Runtime.InteropServices;

namespace Sonora.Services;

public class MidiPortChangedEventArgs : EventArgs {
    public required MidiPortInfo Info { get; set; }
}

public class MidiSettings : IDisposable {
    private nint _midiState;
    private GCHandle _handle;
    bool _disposed;

    public event EventHandler<MidiPortChangedEventArgs>? PortAdded;
    public event EventHandler<MidiPortChangedEventArgs>? PortRemoved;

    private static void _PortAdded(nint receiver, [In] MidiPortInfo info) {
        GCHandle handle = GCHandle.FromIntPtr(receiver);
        MidiSettings self = (MidiSettings)handle.Target!;
        self.PortAdded?.Invoke(self, new MidiPortChangedEventArgs {
            Info = info,
        });
    }

    private static void _PortRemoved(nint receiver, [In] MidiPortInfo info) {
        GCHandle handle = GCHandle.FromIntPtr(receiver);
        MidiSettings self = (MidiSettings)handle.Target!;
        self.PortRemoved?.Invoke(self, new MidiPortChangedEventArgs {
            Info = info,
        });
    }

    public MidiSettings() {
        _handle = GCHandle.Alloc(this);
        nint receiver = GCHandle.ToIntPtr(_handle);
        _midiState = SnrMidiStateNew(receiver, _PortAdded, _PortRemoved);
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
            }
            _handle.Free();
            SnrMidiStateFree(_midiState);
            _disposed = true;
        }
    }

    ~MidiSettings() => Dispose(false);

    public void Listen() {
        SnrMidiStateListen(_midiState);
    }

    public bool OpenPort(MidiPortInfo info) {
        return SnrMidiStateOpenPort(_midiState, info.Direction, info.Id);
    }

    public void ClosePort(MidiPortInfo info) {
        SnrMidiStateClosePort(_midiState, info.Direction, info.Id);
    }
}
