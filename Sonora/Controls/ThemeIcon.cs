using System.Collections.Generic;
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

    private static ThemeVariant _currentVariant;

    private static HashSet<ThemeIcon> _activeIcons;

    static ThemeIcon() {
        AffectsRender<ThemeIcon>(PathProperty);

        _currentVariant = Application.Current!.ActualThemeVariant;
        _activeIcons = new();
        Application.Current!.ActualThemeVariantChanged += (s, e) => {
            if (_currentVariant == Application.Current!.ActualThemeVariant) {
                return;
            }
            _currentVariant = Application.Current!.ActualThemeVariant;
            foreach (var icon in _activeIcons) {
                icon.SetImageSource(icon.GetImageVariant());
            }
        };
    }

    public ThemeIcon() {
        this.Content = new Image();
        this.PropertyChanged += (s, e) => {
            if (e.Property.Name == "Path") {
                SetImageSource(GetImageVariant());
            }
        };
        _activeIcons.Add(this);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e) {
        base.OnDetachedFromVisualTree(e);
        _activeIcons.Remove(this);
    }

    private void SetImageSource(string source) {
        var stream = AssetLoader.Open(new Uri(source));
        ((Image)this.Content!).Source = new Bitmap(stream);
    }

    private string GetImageVariant() {
        if (_currentVariant == ThemeVariant.Dark) {
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
