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
        base.OnPointerPressed(e);

        if (sender is not Control control) {
            return;
        }
        Console.WriteLine(control.DataContext!.GetType());
        if (control.DataContext is string item) {
            var data = new DataTransfer();
            var dataItem = new DataTransferItem();
            var file = await TopLevel.GetTopLevel(this)!
                .StorageProvider
                .TryGetFileFromPathAsync(item);

            if (file != null) {
                dataItem.Set(DataFormat.File, file);
                data.Add(dataItem);
                await DragDrop.DoDragDropAsync(e, data, DragDropEffects.Move);
            }
        }
    }
}
