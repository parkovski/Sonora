using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Sonora.ViewModels;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Sonora.Views;

public partial class FilesView : ReactiveUserControl<FilesViewModel>
{
    public FilesView()
    {
        InitializeComponent();

        this.WhenActivated(d => d(ViewModel!.ShowOpenFolderDialog.RegisterHandler(ShowOpenFolderDialog)));
    }

    private async Task ShowOpenFolderDialog(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFolderDialog();
        var mainWindow = (App.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow;
        var folder = await dialog.ShowAsync(mainWindow);
        interaction.SetOutput(folder);
    }
}