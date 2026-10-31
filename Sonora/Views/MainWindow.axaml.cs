using System.Collections.ObjectModel;
using System.Threading;
using SukiUI.Controls;

namespace Sonora.Views;

public partial class MainWindow : SukiWindow {
    public MainWindow() {
        InitializeComponent();

        var inputs = new ObservableCollection<string>();
        midiInListBox.ItemsSource = inputs;
        var outputs = new ObservableCollection<string>();
        midiOutListBox.ItemsSource = outputs;

        new Thread(new ThreadStart(() => {
            var listener = new MidiListener();
            listener.PortAdded += (s, e) => {
                if (e.Direction == MidiDir.In) {
                    inputs.Add(e.Name);
                } else {
                    outputs.Add(e.Name);
                }
            };
        })).Start();
    }
}
