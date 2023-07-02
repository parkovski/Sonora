using Sonora.Models;

namespace Sonora.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public FilesViewModel Files { get; }

    private Wave _wave;
    public Wave Wave
    {
        get => _wave;
        set => this.RaiseAndSetIfChanged(ref _wave, value);
    }

    public MainWindowViewModel()
    {
        Files = new FilesViewModel();
        _wave = new();

        Files.WhenAnyValue(x => x.SelectedFile)
            .Subscribe(x => Wave = x == null ? new Wave() : new Wave(x));
    }
}
