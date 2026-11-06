using System.Collections.ObjectModel;
using Sonora.Services;

namespace Sonora.ViewModels;

public class MidiSettingsViewModel : ViewModelBase {
    private MidiSettings _settings;

    public ObservableCollection<MidiPortInfo> Inputs { get; init; }
    public ObservableCollection<MidiPortInfo> Outputs { get; init; }

    public MidiSettingsViewModel(MidiSettings settings) {
        _settings = settings;
        Inputs = new();
        Outputs = new();

        _settings.PortAdded += (s, e) => {
            // Don't need to lock because the C++ side has locked a mutex
            // already. This is only ok as long as we only touch inputs/outputs
            // when C++ is holding the lock.
            if (e.Info.Direction == MidiDirection.In) {
                Inputs.Add(e.Info);
            } else {
                Outputs.Add(e.Info);
            }
        };

        _settings.PortRemoved += (s, e) => {
            if (e.Info.Direction == MidiDirection.In) {
                for (int i = 0; i < Inputs.Count; ++i) {
                    if (Inputs[i].Id == e.Info.Id) {
                        Inputs.RemoveAt(i);
                        break;
                    }
                }
            } else {
                for (int i = 0; i < Outputs.Count; ++i) {
                    if (Outputs[i].Id == e.Info.Id) {
                        Outputs.RemoveAt(i);
                        break;
                    }
                }
            }
        };

        _settings.Listen();
    }
}
