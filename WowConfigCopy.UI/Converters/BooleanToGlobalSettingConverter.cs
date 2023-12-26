using System;
using System.Globalization;
using System.Windows.Data;

namespace WowConfigCopy.UI.Converters;

public class BooleanToGlobalSettingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isGlobal)
        {
            return isGlobal ? "Global Setting" : "Local Setting";
        }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
