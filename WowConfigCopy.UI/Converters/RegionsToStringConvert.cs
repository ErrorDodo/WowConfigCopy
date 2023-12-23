using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.UI.Converters;

public class RegionsToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<RealmModel> realms)
        {
            var regions = realms.Select(r => r.RealmRegion).Distinct();
            return string.Join(", ", regions);
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
