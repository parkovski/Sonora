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
        SetNoteBtn.Click += SetNoteBtn_Click;
    }

    private void SetNoteBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var noteText = Note.Text ?? "A4";
        int scaleDeg;
        int ofs = 0;
        int octave = 4;
        if (noteText.Length < 2 || !(noteText[0] >= 'A' && noteText[0] <= 'G'))
        {
            return;
        }
        switch (noteText[0])
        {
            case 'A': scaleDeg = 9; break;
            case 'B': scaleDeg = 11; break;
            case 'C': scaleDeg = 0; break;
            case 'D': scaleDeg = 2; break;
            case 'E': scaleDeg = 4; break;
            case 'F': scaleDeg = 5; break;
            case 'G': scaleDeg = 7; break;
            default: return;
        }
        ofs = noteText[1] switch { '#' => 1, 'b' => -1, _ => 0 };
        if (ofs != 0)
        {
            if (noteText.Length != 3 || noteText[2] < '0' || noteText[2] > '9')
            {
                return;
            }
            octave = noteText[2] - '0';
        }
        else
        {
            if (noteText[1] < '0' || noteText[1] > '9')
            {
                return;
            }
            octave = noteText[1] - '0';
        }
        HostInterface.Instance.DeviceSetNote(_dev, (byte)(octave * 12 + scaleDeg + ofs));
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