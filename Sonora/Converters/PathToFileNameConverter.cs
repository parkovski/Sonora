using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Globalization;
using System.IO;

namespace Sonora.Converters;

public class PathToFileNameConverter : IValueConverter {
    public static readonly PathToFileNameConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter,
                           CultureInfo culture) {
        if (value == null || targetType != typeof(string)) {
            return new BindingNotification(new InvalidCastException(),
                                           BindingErrorType.Error);
        }
        return Path.GetFileName((string)value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter,
                               CultureInfo culture) {
        throw new NotSupportedException();
    }
}
