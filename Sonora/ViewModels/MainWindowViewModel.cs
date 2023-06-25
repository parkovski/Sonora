using ReactiveUI;

namespace Sonora.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; }

    public MainWindowViewModel()
    {
        Greeting = "Hello world!";
    }
}
