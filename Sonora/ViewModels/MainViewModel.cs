using CommunityToolkit.Mvvm.ComponentModel;

namespace Sonora.ViewModels;

public partial class MainViewModel : ViewModelBase {
    public string Greeting { get => "Hello, world!"; }

    public MainViewModel() {
    }
}
