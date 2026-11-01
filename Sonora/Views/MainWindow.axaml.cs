using System.Collections.ObjectModel;
using System.Threading;
using SukiUI.Controls;

namespace Sonora.Views;

public partial class MainWindow : SukiWindow {
    MidiListener _listener;
    Lock _lock = new();

    public MainWindow() {
        InitializeComponent();

        var inputs = new ObservableCollection<string>();
        midiInListBox.ItemsSource = inputs;
        var outputs = new ObservableCollection<string>();
        midiOutListBox.ItemsSource = outputs;

        _listener = new MidiListener();
        _listener.PortAdded += (s, e) => {
            lock(_lock) {
                if (e.Direction == MidiDir.In) {
                    inputs.Add(e.Name);
                } else {
                    outputs.Add(e.Name);
                }
            }
        };
        Log.Debug("Starting listener.");
        _listener.Listen();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
        Log.Debug("OnDetachedFromVisualTree");
        _listener.Dispose();
    }
}
