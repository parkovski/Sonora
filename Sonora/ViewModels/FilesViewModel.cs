using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sonora.ViewModels;

public class FilesViewModel : ViewModelBase
{
    private string _directory;

    public string Directory
    {
        get => _directory;
        set
        {
            this.RaiseAndSetIfChanged(ref _directory, value);
            RefreshFiles();
        }
    }

    private List<string> _files;

    public List<string> Files
    {
        get => _files;
        set => this.RaiseAndSetIfChanged(ref _files, value);
    }

    public string? _selectedFile;

    public string? SelectedFile
    {
        get => _selectedFile;
        set => this.RaiseAndSetIfChanged(ref _selectedFile, value);
    }

    public ICommand ChooseDirectoryCommand { get; }

    public Interaction<Unit, string?> ShowOpenFolderDialog { get; }

    // Complains about not setting _files, but RefreshFiles sets it.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public FilesViewModel()
#pragma warning restore CS8618
    {
        _directory = Environment.CurrentDirectory;
        RefreshFiles();
        ChooseDirectoryCommand = ReactiveCommand.CreateFromTask(ChooseDirectoryAsync);
        ShowOpenFolderDialog = new();
    }

    private async Task ChooseDirectoryAsync()
    {
        var directory = await ShowOpenFolderDialog.Handle(Unit.Default);
        if (directory != null)
        {
            Directory = directory;
        }
    }

    private void RefreshFiles()
    {
        var collection = new List<string>();
        var dirInfo = new System.IO.DirectoryInfo(Directory);
        foreach (var file in dirInfo.EnumerateFiles())
        {
            if (file.Name.EndsWith(".wav"))
            {
                collection.Add(file.FullName);
            }
        }
        Files = collection;
    }
}
