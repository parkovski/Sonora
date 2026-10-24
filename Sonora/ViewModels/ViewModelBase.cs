using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Sonora.ViewModels;

public interface IViewToViewModelBridge {
    Visual? View { get; set; }
}

public abstract class ViewModelBase : ObservableObject, IViewToViewModelBridge
{
    readonly WeakReference<Visual?> _visualRef = new(default);

    Visual? IViewToViewModelBridge.View {
        get => _visualRef.TryGetTarget(out var target) ? target : default;
        set => _visualRef.SetTarget(value);
    }

    public TopLevel? TopLevel =>
        _visualRef.TryGetTarget(out var view) && view is not null
            ? TopLevel.GetTopLevel(view)
            : default;
}
