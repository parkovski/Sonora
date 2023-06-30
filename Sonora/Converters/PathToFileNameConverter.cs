using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonora.Converters;

public class PathToFileNameConverter : IValueConverter
{
    public static readonly PathToFileNameConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || targetType != typeof(string))
        {
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }
        var name = value.ToString()!;
        var lastSep = name.LastIndexOf('/');
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            var lastSep2 = name.LastIndexOf('\\');
            if (lastSep2 > lastSep)
            {
                lastSep = lastSep2;
            }
        }
        return name.Substring(lastSep + 1);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
