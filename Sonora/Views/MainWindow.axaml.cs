using Avalonia.ReactiveUI;
using Sonora.ViewModels;

namespace Sonora.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}