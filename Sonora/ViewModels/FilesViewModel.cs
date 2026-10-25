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
        Files = new List<string>(); // Shut up the compiler
        this.PropertyChanged += (sender, e) => {
            if (e.PropertyName == nameof(Directory)) {
                Files = System.IO.Directory.EnumerateFiles(Directory)
                    .Where(name => name.EndsWith(".wav"))
                    .ToList();
            }
        };
        Directory = Environment.CurrentDirectory;
    }

    [RelayCommand]
    async Task ChooseDirectory() {
        if (TopLevel?.StorageProvider is { CanPickFolder: true } provider) {
            var folders = await provider.OpenFolderPickerAsync(new());
            if (folders.Count > 0) {
                Directory = folders[0].Path.LocalPath;
            }
        }
    }
}
