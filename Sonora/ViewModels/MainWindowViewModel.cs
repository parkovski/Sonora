namespace Sonora.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public FilesViewModel Files { get; }

    public MainWindowViewModel()
    {
        Files = new FilesViewModel();
    }
}
