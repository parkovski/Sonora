using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System.Runtime.InteropServices;
using Sonora.ViewModels;
using Sonora.Views;

namespace Sonora;

public partial class App : Application
{
    [LibraryImport("Kernel32")]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static partial bool AttachConsole(int processId);

    public override void Initialize()
    {
        AttachConsole(-1);
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
