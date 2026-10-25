using CommunityToolkit.Mvvm.ComponentModel;

namespace Sonora.ViewModels;

public partial class MainViewModel : ViewModelBase {
    public FilesViewModel Files { get; }

    public MainViewModel() {
        Files = new FilesViewModel();
    }
}
