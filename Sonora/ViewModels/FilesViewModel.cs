using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using ReactiveUI;

namespace Sonora.ViewModels;

public class FilesViewModel : ViewModelBase
{
    private string _directory;

    public string Directory
    {
        get => _directory;
        set => this.RaiseAndSetIfChanged(ref _directory, value);
    }

    public List<string> Files { get; }

    public ICommand ChooseDirectoryCommand { get; }

    public Interaction<Unit, string?> ShowOpenFolderDialog { get; }

    public FilesViewModel()
    {
        _directory = System.Environment.CurrentDirectory;
        Files = new List<string> { "test.wav", "foo.wav" };
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
}
