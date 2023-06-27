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

    public ICommand ChooseDirectoryCommand { get; }

    public Interaction<Unit, string?> ShowOpenFolderDialog { get; }

    public FilesViewModel()
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
                collection.Add(file.Name);
            }
        }
        Files = collection;
    }
}
