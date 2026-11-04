using System.Collections.ObjectModel;
using System.Threading;
using Avalonia.Controls;

namespace Sonora.Views;

public partial class MidiSettingsView : UserControl {
    MidiListener _listener;

    public MidiSettingsView() {
        InitializeComponent();

        var inputs = new ObservableCollection<MidiPortInfo>();
        midiInListBox.ItemsSource = inputs;
        var outputs = new ObservableCollection<MidiPortInfo>();
        midiOutListBox.ItemsSource = outputs;

        _listener = new MidiListener();
        _listener.PortAdded += (s, e) => {
            // Don't need to lock because the C++ side has locked a mutex
            // already. This is only ok as long as we only touch inputs/outputs
            // when C++ is holding the lock.
            if (e.Info.Direction == MidiDirection.In) {
                inputs.Add(e.Info);
            } else {
                outputs.Add(e.Info);
            }
        };

        _listener.PortRemoved += (s, e) => {
            if (e.Info.Direction == MidiDirection.In) {
                for (int i = 0; i < inputs.Count; ++i) {
                    if (inputs[i].Id == e.Info.Id) {
                        inputs.RemoveAt(i);
                        break;
                    }
                }
            } else {
                for (int i = 0; i < outputs.Count; ++i) {
                    if (outputs[i].Id == e.Info.Id) {
                        outputs.RemoveAt(i);
                        break;
                    }
                }
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
