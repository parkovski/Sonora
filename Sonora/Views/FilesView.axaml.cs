using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Sonora.ViewModels;

namespace Sonora.Views;

public partial class FilesView : UserControl
{
    public FilesView()
    {
        InitializeComponent();
        if (DataContext is IViewToViewModelBridge bridge) {
            bridge.View = this;
        }
    }

    async void OnPointerPressed(object? sender, PointerPressedEventArgs e) {
        if (sender is not Control control) {
            return;
        }
        Console.WriteLine(control.DataContext!.GetType());
        if (control.DataContext is string item) {
            var data = new DataTransfer();
            var dataItem = new DataTransferItem();
            //dataItem.Set(DataFormat.Text, item);
            dataItem.Set(
                DataFormat.File,
                await TopLevel.GetTopLevel(this)!
                    .StorageProvider
                    .TryGetFileFromPathAsync(item)
            );
            //dataItem.Set(DataFormat.CreateStringPlatformFormat("audio/wav"),
            //             item);
            data.Add(dataItem);

            await DragDrop.DoDragDropAsync(e, data, DragDropEffects.Move);
        }
    }
}
