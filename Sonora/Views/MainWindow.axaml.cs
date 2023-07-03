using Avalonia.ReactiveUI;
using Sonora.ViewModels;

namespace Sonora.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    nint _auctx;
    nint _devEnum;
    nint _dev;

    public MainWindow()
    {
        InitializeComponent();

        StartBtn.Click += StartBtn_Click;
        StopBtn.Click += StopBtn_Click;
        SineBtn.Click += (sender, e) => HostInterface.Instance.DeviceSetOsc(_dev, 0);
        SawBtn.Click += (sender, e) => HostInterface.Instance.DeviceSetOsc(_dev, 1);
        SquareBtn.Click += (sender, e) => HostInterface.Instance.DeviceSetOsc(_dev, 2);
        TriangleBtn.Click += (sender, e) => HostInterface.Instance.DeviceSetOsc(_dev, 3);
    }

    private void StartBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_auctx != 0) return;

        ref readonly var host = ref HostInterface.Instance;
        _auctx = host.AudioContextNew();
        _devEnum = host.DeviceEnumeratorNew(_auctx);
        _dev = host.DeviceNew(_auctx, _devEnum, true, 2, 2, 48000);
        host.DeviceStart(_dev);
    }

    private void StopBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_auctx == 0) return;
        ref readonly var host = ref HostInterface.Instance;
        host.DeviceFree(_dev);
        host.DeviceEnumeratorFree(_devEnum);
        host.AudioContextFree(_auctx);
        _auctx = _devEnum = _dev = 0;
    }
}