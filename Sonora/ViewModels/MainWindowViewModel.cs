using ReactiveUI;
using System;

namespace Sonora.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; }

    public FilesViewModel Files { get; }

    public MainWindowViewModel()
    {
        Greeting = "Hello world!";
        Files = new FilesViewModel();
    }
}
