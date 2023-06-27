using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Sonora.ViewModels;
using System.Reactive;
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
        if (App.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            var folder = await dialog.ShowAsync(lifetime.MainWindow);
            interaction.SetOutput(folder);
        }
        else
        {
            throw new PlatformNotSupportedException("Open folder only implemented for desktop platforms");
        }
    }
}