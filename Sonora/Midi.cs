using System.Runtime.InteropServices;

namespace Sonora;

public class MidiPortChangedEventArgs : EventArgs {
    public required string Name { get; set; }
    public required string Id { get; set; }
    public required ulong Number { get; set; }
    public required MidiDir Direction { get; set; }
}

public class MidiListener : IDisposable {
    private nint _midiState;
    private GCHandle _handle;
    bool _disposed;

    public event EventHandler<MidiPortChangedEventArgs>? PortAdded;
    public event EventHandler<MidiPortChangedEventArgs>? PortRemoved;

    private static void _PortAdded(nint receiver, [In] MidiPortInfo info) {
        GCHandle handle = GCHandle.FromIntPtr(receiver);
        MidiListener self = (MidiListener)handle.Target!;
        self.PortAdded?.Invoke(self, new MidiPortChangedEventArgs {
            Name = info.Name,
            Id = info.Id,
            Number = info.Number,
            Direction = info.Dir,
        });
    }

    private static void _PortRemoved(nint receiver, [In] MidiPortInfo info) {
        GCHandle handle = GCHandle.FromIntPtr(receiver);
        MidiListener self = (MidiListener)handle.Target!;
        self.PortRemoved?.Invoke(self, new MidiPortChangedEventArgs {
            Name = info.Name,
            Id = info.Id,
            Number = info.Number,
            Direction = info.Dir,
        });
    }

    public MidiListener() {
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

    ~MidiListener() => Dispose(false);
}
