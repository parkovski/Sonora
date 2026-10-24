using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Sonora.ViewModels;

public partial class FilesViewModel : ViewModelBase {
    [ObservableProperty] string _directory;
    [ObservableProperty] IList<string> _files;
    [ObservableProperty] string? _selectedFile;

    public FilesViewModel() {
        _directory = Environment.CurrentDirectory;
        _files = new List<string>();
    }

    [RelayCommand]
    async Task ChooseDirectory() {
        if (TopLevel?.StorageProvider is { CanPickFolder: true } provider) {
            var folders = await provider.OpenFolderPickerAsync(new());
            if (folders.Count > 0) {
                Directory = folders[0].Path.LocalPath;
                Files = await folders[0].GetItemsAsync()
                    .Select(i => i.Path.LocalPath)
                    .Where(path => path.EndsWith(".wav"))
                    .ToListAsync();
            }
        }
    }
}
