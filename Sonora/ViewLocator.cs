﻿using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Sonora.ViewModels;

namespace Sonora;

public class ViewLocator : IDataTemplate
{
    public bool SupportsRecycling => false;

    public Control? Build(object? data)
    {
        var name = "(null)";
        if (data != null)
        {
            name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);
            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
