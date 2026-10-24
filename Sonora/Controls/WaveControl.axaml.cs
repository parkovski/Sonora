using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Data;
using Sonora.Models;

namespace Sonora.Controls;

public partial class WaveControl : Control {
    Pen _pen;

    public static readonly DirectProperty<WaveControl, Wave> WaveProperty =
        AvaloniaProperty.RegisterDirect<WaveControl, Wave>(
            nameof(Wave),
            o => o.Wave,
            (o, v) => o.Wave = v,
            defaultBindingMode: BindingMode.OneWay
        );

    Wave _wave;
    public Wave Wave {
        get => _wave;
        set => SetAndRaise(WaveProperty, ref _wave, value);
    }

    public WaveControl() {
        InitializeComponent();
        _pen = new Pen(new SolidColorBrush(Color.FromRgb(0xff, 0xff, 0xff)));
        _wave = new Wave("C:\\Users\\parker\\Documents\\music\\wt\\AKWF-FREE-master\\AKWF\\AKWF_0001\\AKWF_0001.wav");
    }

    public override void Render(DrawingContext context) {
        var area = new RoundedRect(
            new Rect(4, 4, Bounds.Width - 8, Bounds.Height - 8),
            5
        );
        context.DrawRectangle(null, _pen, area);

        const int margin = 4;
        double mid = Bounds.Height * 0.5;
        double mul = mid - margin;
        double stride = _wave.Frames / (Bounds.Width - margin * 2);
        double x0 = 0;
        double y0 = (1 - _wave[0]) * mul + margin;
        for (double x1 = 1; x1 < Bounds.Width - margin * 2; ++x1) {
            ulong frame = (ulong)(x1 * stride);
            double y1 = (1 - _wave[frame * _wave.Channels]) * mul + margin;
            context.DrawLine(_pen, new Point(x0 + margin, y0),
                             new Point(x1 + margin, y1));
            x0 = x1;
            y0 = y1;
        }
    }
}
