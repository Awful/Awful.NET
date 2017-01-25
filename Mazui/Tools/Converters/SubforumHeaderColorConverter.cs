using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Mazui.Tools.Converters
{
    public class SubforumHeaderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = (bool)value;
            return item
                ? new SolidColorBrush(Colors.DarkGray)
                : new SolidColorBrush(Color.FromArgb(255, 65, 91, 100));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
