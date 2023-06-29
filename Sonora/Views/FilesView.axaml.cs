using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Sonora.ViewModels;
using System.Linq;
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
        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider == null || !storageProvider.CanPickFolder)
        {
            interaction.SetOutput(null);
            return;
        }
        var options = new FolderPickerOpenOptions { Title = "Choose a WAV folder" };
        var folder = (await storageProvider.OpenFolderPickerAsync(options)).FirstOrDefault();
        interaction.SetOutput(folder?.Path.LocalPath);
    }
}
