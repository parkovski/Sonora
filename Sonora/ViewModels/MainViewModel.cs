using CommunityToolkit.Mvvm.ComponentModel;
using Sonora.Services;

namespace Sonora.ViewModels;

public partial class MainViewModel : ViewModelBase {
    public FilesViewModel Files { get; }
    public MidiSettingsViewModel MidiSettings { get; }

    public MainViewModel() {
        Files = new();
        MidiSettings = new(new MidiSettings());
    }
}
