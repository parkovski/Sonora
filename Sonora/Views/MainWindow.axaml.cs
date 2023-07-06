using Avalonia.ReactiveUI;
using Sonora.ViewModels;
using System.Reactive.Disposables;

namespace Sonora.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
