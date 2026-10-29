using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Data;
using Sonora.Models;

namespace Sonora.Controls;

public class WaveControl : Control {
    public static readonly StyledProperty<Pen> PenProperty =
        AvaloniaProperty.Register<WaveControl, Pen>(
            nameof(Pen),
            defaultValue: new Pen(Brushes.Green)
        );

    Pen Pen {
        get => GetValue(PenProperty);
        set => SetValue(PenProperty, value);
    }

    public static readonly DirectProperty<WaveControl, Wave?> WaveProperty =
        AvaloniaProperty.RegisterDirect<WaveControl, Wave?>(
            nameof(Wave),
            o => o.Wave,
            (o, v) => o.Wave = v,
            defaultBindingMode: BindingMode.OneWay
        );

    Wave? _wave;
    public Wave? Wave {
        get => _wave;
        set => SetAndRaise(WaveProperty, ref _wave, value);
    }

    static WaveControl() {
        AffectsRender<WaveControl>(PenProperty);
        AffectsRender<WaveControl>(WaveProperty);
    }

    public WaveControl() {
    }

    public override void Render(DrawingContext context) {
        var area = new RoundedRect(
            new Rect(4, 4, Bounds.Width - 8, Bounds.Height - 8),
            5
        );
        context.DrawRectangle(null, Pen, area);

        if (Wave is null) {
            return;
        }

        const int margin = 4;
        double mid = Bounds.Height * 0.5;
        double mul = mid - margin;
        double stride = Wave.Frames / (Bounds.Width - margin * 2);
        double x0 = 0;
        double y0 = (1 - Wave[0]) * mul + margin;
        for (double x1 = 1; x1 < Bounds.Width - margin * 2; ++x1) {
            ulong frame = (ulong)(x1 * stride);
            double y1 = (1 - Wave[frame * Wave.Channels]) * mul + margin;
            context.DrawLine(Pen, new Point(x0 + margin, y0),
                             new Point(x1 + margin, y1));
            x0 = x1;
            y0 = y1;
        }
    }
}
