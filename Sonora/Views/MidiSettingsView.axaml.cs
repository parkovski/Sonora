using System.Collections.ObjectModel;
using System.Threading;
using Avalonia.Controls;

namespace Sonora.Views;

public partial class MidiSettingsView : UserControl {
    MidiListener _listener;

    public MidiSettingsView() {
        InitializeComponent();

        var inputs = new ObservableCollection<string>();
        midiInListBox.ItemsSource = inputs;
        var outputs = new ObservableCollection<string>();
        midiOutListBox.ItemsSource = outputs;

        _listener = new MidiListener();
        _listener.PortAdded += (s, e) => {
            // Don't need to lock because the C++ side has locked a mutex
            // already. This is only ok as long as we only touch inputs/outputs
            // when C++ is holding the lock.
            if (e.Direction == MidiDir.In) {
                inputs.Add(e.Name);
            } else {
                outputs.Add(e.Name);
            }
        };

        _listener.PortRemoved += (s, e) => {
            if (e.Direction == MidiDir.In) {
                inputs.Remove(e.Name);
            } else {
                outputs.Remove(e.Name);
            }
        };

        Log.Debug("Starting listener.");
        _listener.Listen();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
        Log.Debug("MidiSettingsView.OnDetachedFromVisualTree");
        _listener.Dispose();
    }
}
