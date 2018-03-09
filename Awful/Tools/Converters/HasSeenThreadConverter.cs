using Awful.Helpers;
using Awful.Services;
using System;
using System.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Awful.Tools.Converters
{
    public class HasSeenThreadConverter : IValueConverter
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(_elementTheme == ElementTheme.Dark)
            {
                return (bool)value ? WindowsHelpers.GetSolidColorBrush("#FF5C7AB7") : WindowsHelpers.GetSolidColorBrush("#FF232323");
            }
            return (bool)value ? WindowsHelpers.GetSolidColorBrush("#FFC6D8FB") : WindowsHelpers.GetSolidColorBrush("#FFd4e1ee");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
