using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Sonora.Models;
using Sonora.ViewModels;

namespace Sonora.Views;

public partial class WaveView : UserControl
{
    public static readonly StyledProperty<Wave> WaveProperty =
        AvaloniaProperty.Register<WaveView, Wave>(nameof(Wave));

    public Wave Wave
    {
        get => GetValue(WaveProperty);
        set => SetValue(WaveProperty, value);
    }

    public enum DrawMode
    {
        Auto,
        Samples,
        Average,
    }

    public static readonly StyledProperty<DrawMode> DrawProperty =
        AvaloniaProperty.Register<WaveView, DrawMode>(nameof(Draw), DrawMode.Auto);

    public DrawMode Draw
    {
        get => GetValue(DrawProperty);
        set => SetValue(DrawProperty, value);
    }

    public WaveView()
    {
        InitializeComponent();
    }

    public override void Render(DrawingContext context)
    {
        const int margin = 2;

        var pen = new Pen(Brushes.White);
        context.DrawRectangle(pen, new Rect(0, 0, Width, Height));
        pen = new Pen(Brushes.Aqua);

        double mid = Height * 0.5;
        double mul = mid - margin;
        double stride = Wave.Frames / Width;
        if (Draw == DrawMode.Average || (Draw == DrawMode.Auto && stride >= 10))
        {
            // Draw an average waveform.
            for (double x = 0; x < Width; x++)
            {
                double avg = 0;
                ulong frame0 = (ulong)(x * stride);
                ulong frame1 = (ulong)((x + 1) * stride);
                for (ulong frame = frame0; frame < frame1; frame++)
                {
                    avg += Math.Abs(Wave[frame * Wave.Channels]);
                }
                avg /= (frame1 - frame0);
                double y0 = mid + avg * mul;
                double y1 = mid - avg * mul;
                context.DrawLine(pen, new Point(x, y0), new Point(x, y1));
            }
        }
        else
        {
            // Draw a line from previous sample to current sample.
            double x0 = 0;
            // Scale so that -1 = Height - 2*margin, 0 = Height/2, 1 = margin.
            // [0..2] -> [margin..Height-2*margin]
            double y0 = (1 - Wave[0]) * mul + margin;
            for (double x1 = 1; x1 < Width; x1++)
            {
                ulong frame = (ulong)(x1 * stride);
                double y1 = (1 - Wave[frame * Wave.Channels]) * mul + margin;
                context.DrawLine(pen, new Point(x0, y0), new Point(x1, y1));
                x0 = x1;
                y0 = y1;
            }
        }
    }
}