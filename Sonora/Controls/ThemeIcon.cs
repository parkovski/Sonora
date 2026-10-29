using IOPath = System.IO.Path;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.Platform;

namespace Sonora.Controls;

public class ThemeIcon : UserControl {
    public static readonly StyledProperty<string> PathProperty =
        AvaloniaProperty.Register<ThemeIcon, string>(
            nameof(Path)
        );

    public string Path {
        get => GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    private string? _currentVariant;

    static ThemeIcon() {
        AffectsRender<ThemeIcon>(PathProperty);
    }

    public ThemeIcon() {
        this.Content = new Image();
        this.PropertyChanged += (s, e) => {
            if (e.Property.Name == "Path") {
                SetImageSource(GetImageVariant());
            }
        };
        Application.Current!.ActualThemeVariantChanged += (s, e) => {
            SetImageSource(GetImageVariant());
        };
    }

    private void SetImageSource(string source) {
        if (_currentVariant == source) {
            return;
        }
        _currentVariant = source;
        var stream = AssetLoader.Open(new Uri(source));
        ((Image)this.Content!).Source = new Bitmap(stream);
    }

    private string GetImageVariant() {
        if (Application.Current!.ActualThemeVariant == ThemeVariant.Dark) {
            return GetWhiteVariant();
        }
        return Path;
    }

    private string GetWhiteVariant() {
        string pathWithoutExt = IOPath.GetFileNameWithoutExtension(Path);
        string ext = IOPath.GetExtension(Path);
        int slash = Path.LastIndexOf('/');
        if (OperatingSystem.IsWindows()) {
            int backslash = Path.LastIndexOf('\\');
            if (backslash > slash) {
                slash = backslash;
            }
        }
        string dir = Path.Substring(0, slash + 1);
        return dir + pathWithoutExt + "-white" + ext;
    }
}
