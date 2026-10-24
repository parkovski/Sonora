using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Globalization;
using Sonora.Models;

namespace Sonora.Converters;

public class PathToWaveConverter : IValueConverter {
    public object? Convert(object? value, Type targetType,
                           object? parameter, CultureInfo culture) {
        if (value == null || value.GetType() != typeof(string)
            || targetType != typeof(Wave)) {
            return new BindingNotification(new ArgumentException(
                "Invalid binding"
            ));
        }

        return new Wave((string)value);
    }

    public object? ConvertBack(object? value, Type targetType,
                               object? parameter, CultureInfo culture) {
        throw new NotSupportedException();
    }
}
