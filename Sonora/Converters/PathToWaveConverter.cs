using Avalonia.Data;
using Avalonia.Data.Converters;
using Sonora.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonora.Converters;

public class PathToWaveConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || value.GetType() != typeof(string) || targetType != typeof(WaveModel))
        {
            return new BindingNotification(new ArgumentException("???"), BindingErrorType.Error);
        }

        return new WaveModel((string)value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
