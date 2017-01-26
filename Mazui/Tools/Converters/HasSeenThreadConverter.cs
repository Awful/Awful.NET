using Mazui.Services;
using System;
using System.Linq;
using Template10.Common;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Mazui.Tools.Converters
{
    public class HasSeenThreadConverter : IValueConverter
    {
        readonly Template10.Services.SettingsService.ISettingsHelper _helper;
        public HasSeenThreadConverter()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Can't use Application Resources here, it doesn't refresh them after a theme switch without restarting...
            if (_helper.Read<bool>("TransparentThreadListBackground", false))
            {
                return new SolidColorBrush(Colors.Transparent);
            }
            if(SettingsService.Instance.AppTheme == ApplicationTheme.Dark)
            {
                return (bool)value ? Helpers.GetSolidColorBrush("#FF5C7AB7") : Helpers.GetSolidColorBrush("#FF232323");
            }
            return (bool)value ? Helpers.GetSolidColorBrush("#FFC6D8FB") : Helpers.GetSolidColorBrush("#FFd4e1ee");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
