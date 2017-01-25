using System;
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
            if (_helper.Read<bool>("TransparentThreadListBackground", true))
            {
                return new SolidColorBrush(Colors.Transparent);
            }
            return (bool)value ? Application.Current.Resources["HasSeenThreadColor"] as SolidColorBrush : Application.Current.Resources["ThreadColor"] as SolidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
