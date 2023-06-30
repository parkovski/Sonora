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
    public static readonly StyledProperty<WaveModel> WaveProperty = AvaloniaProperty.Register<WaveView, WaveModel>(nameof(Wave));

    public WaveModel Wave
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

    public static readonly StyledProperty<DrawMode> ModeProperty = AvaloniaProperty.Register<WaveView, DrawMode>(nameof(Mode), DrawMode.Auto);

    public DrawMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public WaveView()
    {
        InitializeComponent();
    }

    public override void Render(DrawingContext context)
    {
        const int margin = 2;
        double mul = Height / 2 - margin;
        double add = margin;

        var pen = new Pen(Brushes.White);
        context.DrawRectangle(pen, new Rect(0, 0, Width, Height));
        pen = new Pen(Brushes.Aqua);

        double stride = Wave.Frames / Width;
        if (Mode == DrawMode.Average || (Mode == DrawMode.Auto && stride >= 2))
        {
            // Draw an average waveform.
            double x0 = 0;
            double y0 = 0;
            for (double x1 = 0; x1 < Width; x1++)
            {
                double avg = 0;
                ulong frame0 = (ulong)(x0 * stride);
                ulong frame1 = (ulong)(x1 * stride);
                for (ulong frame = frame0; frame < frame1; frame++)
                {
                    avg += Wave[frame * Wave.Channels];
                }
                avg /= (frame1 - frame0);
                double y1 = (avg + 1) * mul + add;
                context.DrawLine(pen, new Point(x0, y0), new Point(x1, y1));
                x0 = x1;
                y0 = y1;
            }
        }
        else
        {
            // Draw a line from previous sample to current sample.
            double x0 = 0;
            double y0 = (Wave[0] + 1) * mul + add;
            for (double x1 = 1; x1 < Width; x1++)
            {
                ulong frame = (ulong)(x1 * stride);
                double y1 = (Wave[frame * Wave.Channels] + 1) * mul + add;
                context.DrawLine(pen, new Point(x0, y0), new Point(x1, y1));
                x0 = x1;
                y0 = y1;
            }
        }
    }
}